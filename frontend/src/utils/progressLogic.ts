import type { AttemptResult, SubjectKey } from '../types/content';
import type { Profile } from '../types/progress';

/** Local-date (YYYY-MM-DD) helper for streak tracking. */
export function todayIso(now: Date = new Date()): string {
  const y = now.getFullYear();
  const m = String(now.getMonth() + 1).padStart(2, '0');
  const d = String(now.getDate()).padStart(2, '0');
  return `${y}-${m}-${d}`;
}

function daysBetween(fromIso: string, toIso: string): number {
  const from = new Date(`${fromIso}T00:00:00`).getTime();
  const to = new Date(`${toIso}T00:00:00`).getTime();
  return Math.round((to - from) / 86_400_000);
}

/** Advances the daily streak for a play on `today`. Resets if a day was missed. */
export function bumpStreak(profile: Profile, today: string = todayIso()): Profile {
  const { lastPlayedDate, count } = profile.streak;
  if (lastPlayedDate === today) {
    return profile;
  }
  let nextCount = 1;
  if (lastPlayedDate) {
    nextCount = daysBetween(lastPlayedDate, today) === 1 ? count + 1 : 1;
  }
  return { ...profile, streak: { count: nextCount, lastPlayedDate: today } };
}

export interface ApplyAttemptOptions {
  exerciseId: number;
  attemptNumber: number;
  result: AttemptResult;
  /** Owning island/level, stored on the result for map-level progress (optional). */
  subjectKey?: SubjectKey;
  levelId?: number;
  now?: Date;
}

export interface ApplyAttemptOutcome {
  profile: Profile;
  /** Coins newly awarded by this attempt (0 if already solved / wrong). */
  awardedCoins: number;
  /** Stars newly awarded by this attempt. */
  awardedStars: number;
  /** True when this is the first correct solve of the exercise. */
  firstSolve: boolean;
}

/**
 * Applies an attempt result to the profile. Coins/stars are awarded only on the
 * FIRST correct solve of an exercise. Always records the attempt and bumps the
 * daily streak. Pure: returns a new Profile, never mutates the input.
 */
export function applyAttempt(profile: Profile, options: ApplyAttemptOptions): ApplyAttemptOutcome {
  const { exerciseId, attemptNumber, result, subjectKey, levelId, now } = options;
  const today = todayIso(now);
  const timestamp = (now ?? new Date()).toISOString();

  const existing = profile.results.find((r) => r.exerciseId === exerciseId);
  const alreadyCompleted = existing?.completed ?? false;
  const firstSolve = result.correct && !alreadyCompleted;

  const awardedCoins = firstSolve ? result.reward.coins : 0;
  const awardedStars = firstSolve ? result.reward.stars : 0;

  const updatedResult = {
    exerciseId,
    completed: alreadyCompleted || result.correct,
    starsEarned: Math.max(existing?.starsEarned ?? 0, firstSolve ? result.reward.stars : 0),
    attempts: (existing?.attempts ?? 0) + 1,
    lastPlayedAt: timestamp,
    // Keep any previously recorded attribution; fill it in from this attempt when available.
    subjectKey: existing?.subjectKey ?? subjectKey,
    levelId: existing?.levelId ?? levelId,
  };

  const results = existing
    ? profile.results.map((r) => (r.exerciseId === exerciseId ? updatedResult : r))
    : [...profile.results, updatedResult];

  const withResult: Profile = {
    ...profile,
    coins: profile.coins + awardedCoins,
    stars: profile.stars + awardedStars,
    results,
  };

  const finalProfile = bumpStreak(withResult, today);

  // attemptNumber is recorded implicitly via attempts; kept for API symmetry.
  void attemptNumber;

  return { profile: finalProfile, awardedCoins, awardedStars, firstSolve };
}
