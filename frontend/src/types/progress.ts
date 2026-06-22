/**
 * Client-side progress types (IndexedDB, MVP). The server stores nothing about
 * the child yet; these shape the local store and the future Phase 2 server tables.
 */

export type AgeBand = '3-5' | '6-9';

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
}

export interface Profile {
  id: string;
  name: string;
  ageBand: AgeBand;
  avatar: string;
  coins: number;
  stars: number;
  badgeKeys: string[];
  streak: Streak;
  results: ExerciseResult[];
}

export const createDefaultProfile = (): Profile => ({
  id: 'local',
  name: 'Skutt',
  ageBand: '3-5',
  avatar: 'fox',
  coins: 0,
  stars: 0,
  badgeKeys: [],
  streak: { count: 0, lastPlayedDate: null },
  results: [],
});
