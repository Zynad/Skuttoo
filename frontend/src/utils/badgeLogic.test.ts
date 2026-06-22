import { describe, expect, it } from 'vitest';
import { awardBadges, badgeEarned, factsFromProfile, type BadgeFacts } from './badgeLogic';
import { createDefaultProfile } from '../types/progress';
import type { BadgeDef } from '../types/badges';

const loc = { sv: '', en: '' };
const def = (key: string, criteriaType: BadgeDef['criteriaType'], criteriaValue: number, id: number): BadgeDef => ({
  id,
  key,
  iconRef: '★',
  criteriaType,
  criteriaValue,
  name: loc,
  description: loc,
});

const catalog: BadgeDef[] = [
  def('lvl1', 'completeLevel', 1, 1),
  def('lvl5', 'completeLevel', 5, 2),
  def('sub1', 'completeSubject', 1, 3),
  def('streak3', 'streak', 3, 4),
  def('coin50', 'coinTotal', 50, 5),
];

const noFacts: BadgeFacts = { coins: 0, streak: 0, completedLevels: 0, completedSubjects: 0 };

describe('badgeEarned', () => {
  it('checks each criteria type against the matching fact', () => {
    expect(badgeEarned(def('x', 'completeLevel', 5, 0), { ...noFacts, completedLevels: 5 })).toBe(true);
    expect(badgeEarned(def('x', 'completeLevel', 5, 0), { ...noFacts, completedLevels: 4 })).toBe(false);
    expect(badgeEarned(def('x', 'completeSubject', 1, 0), { ...noFacts, completedSubjects: 1 })).toBe(true);
    expect(badgeEarned(def('x', 'streak', 3, 0), { ...noFacts, streak: 3 })).toBe(true);
    expect(badgeEarned(def('x', 'coinTotal', 50, 0), { ...noFacts, coins: 49 })).toBe(false);
  });
});

describe('awardBadges', () => {
  it('awards only the badges whose criteria are newly met', () => {
    const profile = { ...createDefaultProfile(), coins: 50, streak: { count: 3, lastPlayedDate: '2026-06-21' } };
    const { profile: next, newBadgeKeys } = awardBadges(profile, catalog, factsFromProfile(profile));
    expect(newBadgeKeys.sort()).toEqual(['coin50', 'streak3']);
    expect(next.badgeKeys.sort()).toEqual(['coin50', 'streak3']);
  });

  it('does not re-award a badge already held', () => {
    const profile = { ...createDefaultProfile(), coins: 50, badgeKeys: ['coin50'] };
    const { newBadgeKeys } = awardBadges(profile, catalog, factsFromProfile(profile));
    expect(newBadgeKeys).not.toContain('coin50');
  });

  it('returns the same profile reference when nothing is newly earned', () => {
    const profile = createDefaultProfile();
    const result = awardBadges(profile, catalog, factsFromProfile(profile));
    expect(result.profile).toBe(profile);
    expect(result.newBadgeKeys).toEqual([]);
  });

  it('does not mutate the input profile', () => {
    const profile = { ...createDefaultProfile(), coins: 50 };
    awardBadges(profile, catalog, factsFromProfile(profile));
    expect(profile.badgeKeys).toEqual([]);
  });

  it('awards structural badges from completion counts', () => {
    const profile = { ...createDefaultProfile(), completedLevelIds: [1, 2, 3, 4, 5], completedSubjectKeys: ['math' as const] };
    const { newBadgeKeys } = awardBadges(profile, catalog, factsFromProfile(profile));
    expect(newBadgeKeys).toContain('lvl1');
    expect(newBadgeKeys).toContain('lvl5');
    expect(newBadgeKeys).toContain('sub1');
  });
});

describe('factsFromProfile', () => {
  it('derives coins/streak from the profile and counts from the persisted sets', () => {
    const profile = {
      ...createDefaultProfile(),
      coins: 12,
      streak: { count: 4, lastPlayedDate: '2026-06-21' },
      completedLevelIds: [1, 2],
      completedSubjectKeys: ['math' as const],
    };
    expect(factsFromProfile(profile)).toEqual({ coins: 12, streak: 4, completedLevels: 2, completedSubjects: 1 });
  });

  it('lets callers override the structural counts', () => {
    const profile = createDefaultProfile();
    expect(factsFromProfile(profile, { completedLevels: 9 }).completedLevels).toBe(9);
  });
});
