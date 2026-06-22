import { useCallback, useEffect, useMemo, useRef, useState, type ReactNode } from 'react';
import { createDefaultProfile, type Profile } from '../types/progress';
import { clearProfile, loadProfile, saveProfile } from '../services/progressStore';
import { applyAttempt, type ApplyAttemptOutcome } from '../utils/progressLogic';
import { ProgressContext, type ProgressContextValue, type RecordAttemptArgs } from './progressContext';

export interface ProgressProviderProps {
  children: ReactNode;
}

export function ProgressProvider({ children }: ProgressProviderProps) {
  const [profile, setProfile] = useState<Profile>(() => createDefaultProfile());
  const [loading, setLoading] = useState(true);

  // Mirror of the latest profile so recordAttempt always computes from the
  // freshest state — avoids a stale closure where two attempts dispatched before
  // a re-render would both read the same stale profile and lose a coin update.
  const profileRef = useRef(profile);
  const setProfileSynced = useCallback((next: Profile) => {
    profileRef.current = next;
    setProfile(next);
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

  const recordAttempt = useCallback(
    async ({ exerciseId, attemptNumber, result, subjectKey, levelId }: RecordAttemptArgs): Promise<ApplyAttemptOutcome> => {
      const outcome = applyAttempt(profileRef.current, { exerciseId, attemptNumber, result, subjectKey, levelId });
      setProfileSynced(outcome.profile);
      await saveProfile(outcome.profile);
      return outcome;
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
    () => ({ profile, loading, recordAttempt, reset }),
    [profile, loading, recordAttempt, reset],
  );

  return <ProgressContext.Provider value={value}>{children}</ProgressContext.Provider>;
}
