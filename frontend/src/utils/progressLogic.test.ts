import { describe, expect, it } from 'vitest';
import { applyAttempt, bumpStreak, todayIso, STREAK_BONUS_CAP } from './progressLogic';
import { createDefaultProfile, type Profile } from '../types/progress';
import type { AttemptResult } from '../types/content';

const correct: AttemptResult = { correct: true, correctChoiceId: 2, reward: { coins: 5, stars: 3 } };
const wrong: AttemptResult = { correct: false, correctChoiceId: 2, reward: { coins: 0, stars: 0 } };

const playedOn = (count: number, lastPlayedDate: string): Profile => ({
  ...createDefaultProfile(),
  streak: { count, lastPlayedDate },
});

describe('applyAttempt', () => {
  it('awards coins and stars on the first correct solve', () => {
    const outcome = applyAttempt(createDefaultProfile(), { exerciseId: 1, attemptNumber: 1, result: correct });
    expect(outcome.firstSolve).toBe(true);
    expect(outcome.awardedCoins).toBe(5);
    expect(outcome.awardedStars).toBe(3);
    expect(outcome.profile.coins).toBe(5);
    expect(outcome.profile.stars).toBe(3);
  });

  it('does not award again on a repeat solve of the same exercise', () => {
    const first = applyAttempt(createDefaultProfile(), { exerciseId: 1, attemptNumber: 1, result: correct });
    const second = applyAttempt(first.profile, { exerciseId: 1, attemptNumber: 1, result: correct });
    expect(second.firstSolve).toBe(false);
    expect(second.awardedCoins).toBe(0);
    expect(second.profile.coins).toBe(5);
  });

  it('records a wrong attempt without awarding or punishing', () => {
    const outcome = applyAttempt(createDefaultProfile(), { exerciseId: 1, attemptNumber: 1, result: wrong });
    expect(outcome.awardedCoins).toBe(0);
    expect(outcome.profile.coins).toBe(0);
    expect(outcome.profile.results[0].attempts).toBe(1);
    expect(outcome.profile.results[0].completed).toBe(false);
  });

  it('does not mutate the input profile', () => {
    const profile = createDefaultProfile();
    applyAttempt(profile, { exerciseId: 1, attemptNumber: 1, result: correct });
    expect(profile.coins).toBe(0);
    expect(profile.results).toHaveLength(0);
  });
});

describe('bumpStreak', () => {
  it('continues the streak on consecutive days', () => {
    const base = { ...createDefaultProfile(), streak: { count: 2, lastPlayedDate: '2026-06-20' } };
    const next = bumpStreak(base, '2026-06-21');
    expect(next.streak.count).toBe(3);
  });

  it('resets the streak after a missed day', () => {
    const base = { ...createDefaultProfile(), streak: { count: 4, lastPlayedDate: '2026-06-18' } };
    const next = bumpStreak(base, '2026-06-21');
    expect(next.streak.count).toBe(1);
  });

  it('does not double-count the same day', () => {
    const base = { ...createDefaultProfile(), streak: { count: 1, lastPlayedDate: '2026-06-21' } };
    const next = bumpStreak(base, '2026-06-21');
    expect(next.streak.count).toBe(1);
  });

  it('starts a streak at 1 on the first play', () => {
    const next = bumpStreak(createDefaultProfile(), '2026-06-21');
    expect(next.streak.count).toBe(1);
    expect(next.streak.lastPlayedDate).toBe('2026-06-21');
  });
});

describe('applyAttempt — daily streak bonus', () => {
  const today = new Date('2026-06-21T09:00:00');

  it('awards no bonus on the very first play ever', () => {
    const outcome = applyAttempt(createDefaultProfile(), { exerciseId: 1, attemptNumber: 1, result: correct, now: today });
    expect(outcome.streakBonusCoins).toBe(0);
    expect(outcome.newStreakCount).toBe(1);
    expect(outcome.profile.coins).toBe(5); // reward only
  });

  it('awards a bonus equal to the streak count when returning on a consecutive new day', () => {
    const base = playedOn(2, '2026-06-20');
    const outcome = applyAttempt(base, { exerciseId: 1, attemptNumber: 1, result: correct, now: today });
    expect(outcome.newStreakCount).toBe(3);
    expect(outcome.streakBonusCoins).toBe(3);
    expect(outcome.profile.coins).toBe(5 + 3); // reward + bonus
  });

  it('does not award a bonus on a second attempt the same day', () => {
    const base = playedOn(3, '2026-06-21');
    const outcome = applyAttempt(base, { exerciseId: 2, attemptNumber: 1, result: correct, now: today });
    expect(outcome.streakBonusCoins).toBe(0);
    expect(outcome.profile.coins).toBe(5);
  });

  it('caps the bonus at STREAK_BONUS_CAP', () => {
    const base = playedOn(15, '2026-06-20');
    const outcome = applyAttempt(base, { exerciseId: 1, attemptNumber: 1, result: correct, now: today });
    expect(outcome.newStreakCount).toBe(16);
    expect(outcome.streakBonusCoins).toBe(STREAK_BONUS_CAP);
  });

  it('grants the bonus even when the first attempt of the day is wrong', () => {
    const base = playedOn(1, '2026-06-20');
    const outcome = applyAttempt(base, { exerciseId: 1, attemptNumber: 1, result: wrong, now: today });
    expect(outcome.awardedCoins).toBe(0);
    expect(outcome.streakBonusCoins).toBe(2);
    expect(outcome.profile.coins).toBe(2); // bonus only, no reward for a wrong answer
  });

  it('resets the streak after a missed day and gives a small bonus', () => {
    const base = playedOn(5, '2026-06-18'); // gap before 2026-06-21
    const outcome = applyAttempt(base, { exerciseId: 1, attemptNumber: 1, result: correct, now: today });
    expect(outcome.newStreakCount).toBe(1);
    expect(outcome.streakBonusCoins).toBe(1);
  });
});

describe('todayIso', () => {
  it('formats a date as YYYY-MM-DD', () => {
    expect(todayIso(new Date('2026-01-05T10:00:00'))).toBe('2026-01-05');
  });
});
