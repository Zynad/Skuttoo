import { useCallback, useEffect, useMemo, useRef, useState, type ReactNode } from 'react';
import { createDefaultProfile, type Profile } from '../types/progress';
import type { SubjectKey } from '../types/content';
import type { BadgeDef } from '../types/badges';
import { clearProfile, loadProfile, saveProfile } from '../services/progressStore';
import { badgeApi, FALLBACK_BADGES } from '../services/badgeApi';
import { applyAttempt } from '../utils/progressLogic';
import { awardBadges, factsFromProfile } from '../utils/badgeLogic';
import { applyPurchase, equipCosmetic as equipCosmeticItem } from '../utils/shopLogic';
import { cosmeticById } from '../game/cosmeticsCatalog';
import type { EquippableCategory } from '../types/cosmetics';
import { ProgressContext, type ProgressContextValue, type RecordAttemptArgs, type RecordAttemptOutcome } from './progressContext';

export interface ProgressProviderProps {
  children: ReactNode;
}

export function ProgressProvider({ children }: ProgressProviderProps) {
  const [profile, setProfile] = useState<Profile>(() => createDefaultProfile());
  const [loading, setLoading] = useState(true);
  const [badgeCatalog, setBadgeCatalog] = useState<BadgeDef[]>(FALLBACK_BADGES);

  // Mirror of the latest profile so recordAttempt always computes from the
  // freshest state — avoids a stale closure where two attempts dispatched before
  // a re-render would both read the same stale profile and lose a coin update.
  const profileRef = useRef(profile);
  const setProfileSynced = useCallback((next: Profile) => {
    profileRef.current = next;
    setProfile(next);
  }, []);

  // The catalogue rarely changes, so a ref keeps badge evaluation synchronous.
  const catalogRef = useRef(badgeCatalog);
  const setCatalogSynced = useCallback((next: BadgeDef[]) => {
    catalogRef.current = next;
    setBadgeCatalog(next);
  }, []);

  useEffect(() => {
    let active = true;
    void loadProfile().then((loaded) => {
      if (active) {
        setProfileSynced(loaded);
        setLoading(false);
      }
    });
    return () => {
      active = false;
    };
  }, [setProfileSynced]);

  useEffect(() => {
    let active = true;
    void badgeApi
      .getBadges()
      .then((badges) => {
        if (active && badges.length > 0) {
          setCatalogSynced(badges);
        }
      })
      .catch(() => {
        // Keep the bundled fallback catalogue (offline-friendly).
      });
    return () => {
      active = false;
    };
  }, [setCatalogSynced]);

  const recordAttempt = useCallback(
    async ({ exerciseId, attemptNumber, result, subjectKey, levelId }: RecordAttemptArgs): Promise<RecordAttemptOutcome> => {
      const outcome = applyAttempt(profileRef.current, { exerciseId, attemptNumber, result, subjectKey, levelId });
      // Coin/streak badges can be earned immediately; structural badges sync from the island screen.
      const awarded = awardBadges(outcome.profile, catalogRef.current, factsFromProfile(outcome.profile));
      setProfileSynced(awarded.profile);
      await saveProfile(awarded.profile);
      return { ...outcome, profile: awarded.profile, newBadgeKeys: awarded.newBadgeKeys };
    },
    [setProfileSynced],
  );

  const syncSubjectCompletion = useCallback(
    async (subjectKey: SubjectKey, completedLevelIds: number[], subjectComplete: boolean): Promise<string[]> => {
      const current = profileRef.current;

      const mergedLevels = [...new Set([...current.completedLevelIds, ...completedLevelIds])];
      const mergedSubjects = subjectComplete && !current.completedSubjectKeys.includes(subjectKey)
        ? [...current.completedSubjectKeys, subjectKey]
        : current.completedSubjectKeys;

      const withCompletion: Profile =
        mergedLevels.length !== current.completedLevelIds.length || mergedSubjects !== current.completedSubjectKeys
          ? { ...current, completedLevelIds: mergedLevels, completedSubjectKeys: mergedSubjects }
          : current;

      const awarded = awardBadges(withCompletion, catalogRef.current, factsFromProfile(withCompletion));

      if (awarded.profile !== current) {
        setProfileSynced(awarded.profile);
        await saveProfile(awarded.profile);
      }
      return awarded.newBadgeKeys;
    },
    [setProfileSynced],
  );

  const purchaseCosmetic = useCallback(
    async (itemId: string) => {
      const item = cosmeticById(itemId);
      if (!item) {
        return;
      }
      const next = applyPurchase(profileRef.current, item);
      if (next === profileRef.current) {
        return;
      }
      setProfileSynced(next);
      await saveProfile(next);
    },
    [setProfileSynced],
  );

  const equipCosmetic = useCallback(
    async (category: EquippableCategory, id: string | null) => {
      const next = equipCosmeticItem(profileRef.current, category, id);
      if (next === profileRef.current) {
        return;
      }
      setProfileSynced(next);
      await saveProfile(next);
    },
    [setProfileSynced],
  );

  const reset = useCallback(async () => {
    await clearProfile();
    const fresh = createDefaultProfile();
    setProfileSynced(fresh);
    await saveProfile(fresh);
  }, [setProfileSynced]);

  const value = useMemo<ProgressContextValue>(
    () => ({ profile, loading, badgeCatalog, recordAttempt, syncSubjectCompletion, purchaseCosmetic, equipCosmetic, reset }),
    [profile, loading, badgeCatalog, recordAttempt, syncSubjectCompletion, purchaseCosmetic, equipCosmetic, reset],
  );

  return <ProgressContext.Provider value={value}>{children}</ProgressContext.Provider>;
}
