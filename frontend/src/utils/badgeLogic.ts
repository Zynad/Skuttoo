import type { BadgeDef } from '../types/badges';
import type { Profile } from '../types/progress';

/** The facts a badge's criteria are checked against. */
export interface BadgeFacts {
  coins: number;
  /** Current daily-streak length. */
  streak: number;
  /** Number of levels seen fully completed. */
  completedLevels: number;
  /** Number of subjects (islands) seen fully completed. */
  completedSubjects: number;
}

/** Whether a single badge's criteria are met by the given facts. */
export function badgeEarned(badge: BadgeDef, facts: BadgeFacts): boolean {
  switch (badge.criteriaType) {
    case 'completeLevel':
      return facts.completedLevels >= badge.criteriaValue;
    case 'completeSubject':
      return facts.completedSubjects >= badge.criteriaValue;
    case 'streak':
      return facts.streak >= badge.criteriaValue;
    case 'coinTotal':
      return facts.coins >= badge.criteriaValue;
    default:
      return false;
  }
}

/** Derives the always-available facts (coins, streak) from the profile. */
export function factsFromProfile(profile: Profile, structural?: Partial<Pick<BadgeFacts, 'completedLevels' | 'completedSubjects'>>): BadgeFacts {
  return {
    coins: profile.coins,
    streak: profile.streak.count,
    completedLevels: structural?.completedLevels ?? profile.completedLevelIds.length,
    completedSubjects: structural?.completedSubjects ?? profile.completedSubjectKeys.length,
  };
}

export interface AwardBadgesResult {
  profile: Profile;
  /** Badge keys newly earned by this evaluation (empty if none). */
  newBadgeKeys: string[];
}

/**
 * Awards any badges whose criteria are now met and that the child does not already hold.
 * Pure: returns a new Profile (only when something changed), never mutates the input.
 */
export function awardBadges(profile: Profile, catalog: BadgeDef[], facts: BadgeFacts): AwardBadgesResult {
  const owned = new Set(profile.badgeKeys);
  const newBadgeKeys = catalog.filter((badge) => !owned.has(badge.key) && badgeEarned(badge, facts)).map((b) => b.key);

  if (newBadgeKeys.length === 0) {
    return { profile, newBadgeKeys };
  }

  return { profile: { ...profile, badgeKeys: [...profile.badgeKeys, ...newBadgeKeys] }, newBadgeKeys };
}
