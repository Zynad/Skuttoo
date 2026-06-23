import type { Level, SubjectKey } from '../types/content';
import type { ExerciseResult } from '../types/progress';
import { ISLAND_ORDER } from './islandTheme';

/** Play state of a level on the island path. Matches ProgressPath's LevelState. */
export type PlayState = 'completed' | 'current' | 'available' | 'locked' | 'optional';

const completedExerciseIds = (results: ExerciseResult[]): Set<number> =>
  new Set(results.filter((r) => r.completed).map((r) => r.exerciseId));

/** A level is completed once every one of its exercises has been solved. */
export function isLevelCompleted(level: Level, results: ExerciseResult[]): boolean {
  if (level.exerciseIds.length === 0) {
    return false;
  }
  const done = completedExerciseIds(results);
  return level.exerciseIds.every((id) => done.has(id));
}

/**
 * Index of the level an exact age should start on: the first level (in display order) whose
 * suggested age range still reaches the child's age (`ageMax >= age`) — i.e. the earliest node
 * that isn't too easy. Younger children start at the first node; older children skip the easiest
 * ones. A `null` age (not onboarded) starts at the first node. Relies on the seed invariant that
 * `ageMax` is non-decreasing along the path.
 */
export function startNodeForAge(levels: Level[], age: number | null): number {
  if (levels.length === 0 || age === null) {
    return 0;
  }
  const index = levels.findIndex((level) => level.ageMax >= age);
  return index === -1 ? levels.length - 1 : index;
}

/**
 * Resolves the play state of each level (in display order). Completed levels are lit. Nodes before
 * the age-appropriate start node (see {@link startNodeForAge}) are `optional` — playable warm-ups
 * that never block progress. From the start onward, the first not-yet-completed node is `current`,
 * the very next is `available` (a one-step lookahead so a child isn't hard-blocked), and anything
 * further ahead is `locked` (visible but dimmed). Islands themselves are never locked — only levels
 * within an island. (Sub-phases 1.7 + exact-age onboarding.)
 */
export function levelStates(levels: Level[], results: ExerciseResult[], age: number | null): PlayState[] {
  const completed = levels.map((level) => isLevelCompleted(level, results));
  const start = startNodeForAge(levels, age);

  // Current = first not-yet-completed node at or after the start; optional warm-ups don't block it.
  let currentIndex = -1;
  for (let i = start; i < levels.length; i += 1) {
    if (!completed[i]) {
      currentIndex = i;
      break;
    }
  }

  return levels.map((_, index) => {
    if (completed[index]) {
      return 'completed';
    }
    if (index < start) {
      return 'optional';
    }
    if (index === currentIndex) {
      return 'current';
    }
    return index === currentIndex + 1 ? 'available' : 'locked';
  });
}

export interface IslandProgress {
  /** Stars earned across the island's solved exercises. */
  starsEarned: number;
  /** Number of solved exercises on the island. */
  exercisesDone: number;
  /** Whether the child has attempted anything on the island. */
  started: boolean;
}

/** Per-island progress, summed from attributed results (no content re-fetch needed). */
export function islandProgress(subjectKey: SubjectKey, results: ExerciseResult[]): IslandProgress {
  const mine = results.filter((r) => r.subjectKey === subjectKey);
  const done = mine.filter((r) => r.completed);
  return {
    starsEarned: done.reduce((sum, r) => sum + (r.starsEarned ?? 0), 0),
    exercisesDone: done.length,
    started: mine.length > 0,
  };
}

/**
 * The island to nudge the child toward: the first not-yet-started island in order,
 * otherwise the least-progressed one (ties resolve to the earliest in order).
 */
export function nextIsland(results: ExerciseResult[], order: readonly SubjectKey[] = ISLAND_ORDER): SubjectKey {
  const progress = order.map((key) => ({ key, ...islandProgress(key, results) }));
  const unstarted = progress.find((p) => !p.started);
  if (unstarted) {
    return unstarted.key;
  }
  return progress.reduce((best, p) => (p.exercisesDone < best.exercisesDone ? p : best)).key;
}
