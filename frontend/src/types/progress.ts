/**
 * Client-side progress types (IndexedDB, MVP). The server stores nothing about
 * the child yet; these shape the local store and the future Phase 2 server tables.
 */

import type { SubjectKey } from './content';

export type AgeBand = '3-5' | '6-9';

/** The exact ages a child can pick at onboarding. */
export const MIN_AGE = 3;
export const MAX_AGE = 9;

/** The age band a given exact age belongs to (the behavioural pre-reader/reader split). */
export const ageBandForAge = (age: number): AgeBand => (age <= 5 ? '3-5' : '6-9');

export interface Streak {
  count: number;
  /** ISO date (YYYY-MM-DD) of the last calendar day played. */
  lastPlayedDate: string | null;
}

export interface ExerciseResult {
  exerciseId: number;
  completed: boolean;
  starsEarned: number;
  attempts: number;
  /** ISO timestamp. */
  lastPlayedAt: string;
  /** Owning island/level — lets the map summarise progress without re-fetching content.
   * Optional for backward compatibility with profiles saved before attribution existed. */
  subjectKey?: SubjectKey;
  levelId?: number;
}

/** Cosmetic items the child currently has equipped on the mascot. `null` = nothing equipped. */
export interface Equipped {
  mascotColor: string | null;
  mascotAccessory: string | null;
}

/** Cosmetic id of the free, default-owned mascot colour. */
export const DEFAULT_MASCOT_COLOR = 'mascot-color-default';

export interface Profile {
  id: string;
  name: string;
  /** The child's exact age (3–9), chosen at onboarding. `null` means not onboarded yet. */
  age: number | null;
  /** Behavioural band derived from {@link age} (kept for badge/UI logic that splits at age 6). */
  ageBand: AgeBand;
  avatar: string;
  coins: number;
  stars: number;
  badgeKeys: string[];
  /** Cosmetic ids the child has bought (the default mascot colour is owned for free). */
  ownedCosmetics: string[];
  equipped: Equipped;
  /** Ids of levels seen fully completed — accumulated as islands are visited, for badge facts. */
  completedLevelIds: number[];
  /** Subject keys seen fully completed — accumulated for the island/world badges. */
  completedSubjectKeys: SubjectKey[];
  streak: Streak;
  results: ExerciseResult[];
}

export const createDefaultProfile = (): Profile => ({
  id: 'local',
  name: 'Skutt',
  age: null,
  ageBand: '3-5',
  avatar: 'fox',
  coins: 0,
  stars: 0,
  badgeKeys: [],
  ownedCosmetics: [DEFAULT_MASCOT_COLOR],
  equipped: { mascotColor: DEFAULT_MASCOT_COLOR, mascotAccessory: null },
  completedLevelIds: [],
  completedSubjectKeys: [],
  streak: { count: 0, lastPlayedDate: null },
  results: [],
});
