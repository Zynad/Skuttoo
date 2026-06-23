import {
  ageBandForAge,
  createDefaultProfile,
  DEFAULT_MASCOT_COLOR,
  MAX_AGE,
  MIN_AGE,
  type AgeBand,
  type Equipped,
  type ExerciseResult,
  type Profile,
} from '../types/progress';
import type { SubjectKey } from '../types/content';

const SUBJECT_KEYS: readonly SubjectKey[] = ['math', 'swedish', 'english', 'logic'];

/**
 * Brings a stored profile up to the current {@link Profile} shape. Profiles saved by an
 * earlier version may lack fields added later (e.g. cosmetics) — reading those directly
 * would crash (`profile.ownedCosmetics.includes` on `undefined`). This pure, idempotent
 * function fills missing fields from {@link createDefaultProfile}, keeps any valid stored
 * values, and repairs invariants (the default mascot colour is always owned; an equipped
 * cosmetic that isn't owned falls back to a safe value).
 */
export function normalizeProfile(raw: unknown): Profile {
  const base = createDefaultProfile();
  if (!isRecord(raw)) {
    return base;
  }

  const ownedCosmetics = uniqueStrings([
    DEFAULT_MASCOT_COLOR,
    ...asStringArray(raw.ownedCosmetics),
  ]);

  const coins = asNonNegativeInt(raw.coins, base.coins);
  const stars = asNonNegativeInt(raw.stars, base.stars);
  const results = asResultArray(raw.results);
  const legacyBand: AgeBand = raw.ageBand === '6-9' ? '6-9' : '3-5';
  const age = resolveAge(raw.age, legacyBand, results.length > 0 || coins > 0 || stars > 0);

  return {
    id: asString(raw.id, base.id),
    name: asString(raw.name, base.name),
    age,
    // ageBand is derived from the exact age; for not-yet-onboarded profiles fall back to the legacy value.
    ageBand: age !== null ? ageBandForAge(age) : legacyBand,
    avatar: asString(raw.avatar, base.avatar),
    coins,
    stars,
    badgeKeys: uniqueStrings(asStringArray(raw.badgeKeys)),
    ownedCosmetics,
    equipped: normalizeEquipped(raw.equipped, ownedCosmetics),
    completedLevelIds: [...new Set(asNumberArray(raw.completedLevelIds))],
    completedSubjectKeys: [...new Set(asStringArray(raw.completedSubjectKeys).filter(isSubjectKey))],
    streak: normalizeStreak(raw.streak),
    results,
  };
}

/**
 * Resolves the child's exact age. A valid stored age wins. Otherwise, a profile saved before the
 * `age` field existed but that already shows real progress is migrated by inferring an age from its
 * legacy band ('6-9'→7, '3-5'→4) so returning players are NOT forced back through onboarding. A
 * truly empty profile resolves to `null`, which the onboarding gate treats as first run.
 */
function resolveAge(raw: unknown, legacyBand: AgeBand, hasProgress: boolean): number | null {
  if (typeof raw === 'number' && Number.isFinite(raw)) {
    const age = Math.floor(raw);
    if (age >= MIN_AGE && age <= MAX_AGE) {
      return age;
    }
  }
  if (hasProgress) {
    return legacyBand === '6-9' ? 7 : 4;
  }
  return null;
}

function isSubjectKey(value: string): value is SubjectKey {
  return (SUBJECT_KEYS as readonly string[]).includes(value);
}

function asNumberArray(value: unknown): number[] {
  return Array.isArray(value) ? value.filter((v): v is number => typeof v === 'number' && Number.isFinite(v)) : [];
}

function normalizeEquipped(raw: unknown, ownedCosmetics: string[]): Equipped {
  const equipped: Equipped = { mascotColor: DEFAULT_MASCOT_COLOR, mascotAccessory: null };
  if (isRecord(raw)) {
    if (typeof raw.mascotColor === 'string' && ownedCosmetics.includes(raw.mascotColor)) {
      equipped.mascotColor = raw.mascotColor;
    }
    if (typeof raw.mascotAccessory === 'string' && ownedCosmetics.includes(raw.mascotAccessory)) {
      equipped.mascotAccessory = raw.mascotAccessory;
    }
  }
  return equipped;
}

function normalizeStreak(raw: unknown): Profile['streak'] {
  if (isRecord(raw)) {
    return {
      count: asNonNegativeInt(raw.count, 0),
      lastPlayedDate: typeof raw.lastPlayedDate === 'string' ? raw.lastPlayedDate : null,
    };
  }
  return { count: 0, lastPlayedDate: null };
}

function asResultArray(raw: unknown): ExerciseResult[] {
  if (!Array.isArray(raw)) {
    return [];
  }
  const results: ExerciseResult[] = [];
  for (const entry of raw) {
    if (isRecord(entry) && typeof entry.exerciseId === 'number') {
      results.push({
        exerciseId: entry.exerciseId,
        completed: entry.completed === true,
        starsEarned: asNonNegativeInt(entry.starsEarned, 0),
        attempts: asNonNegativeInt(entry.attempts, 0),
        lastPlayedAt: typeof entry.lastPlayedAt === 'string' ? entry.lastPlayedAt : '',
        ...(typeof entry.subjectKey === 'string' ? { subjectKey: entry.subjectKey as ExerciseResult['subjectKey'] } : {}),
        ...(typeof entry.levelId === 'number' ? { levelId: entry.levelId } : {}),
      });
    }
  }
  return results;
}

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === 'object' && value !== null;
}

function asString(value: unknown, fallback: string): string {
  return typeof value === 'string' && value.length > 0 ? value : fallback;
}

function asNonNegativeInt(value: unknown, fallback: number): number {
  return typeof value === 'number' && Number.isFinite(value) && value >= 0 ? Math.floor(value) : fallback;
}

function asStringArray(value: unknown): string[] {
  return Array.isArray(value) ? value.filter((v): v is string => typeof v === 'string') : [];
}

function uniqueStrings(values: string[]): string[] {
  return [...new Set(values)];
}
