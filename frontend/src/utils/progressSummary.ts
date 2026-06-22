import type { Level, SubjectKey } from '../types/content';
import type { ExerciseResult } from '../types/progress';
import { ISLAND_ORDER } from './islandTheme';

/** Play state of a level on the island path. Matches ProgressPath's LevelState. */
export type PlayState = 'completed' | 'current' | 'available' | 'locked';

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
 * Resolves the play state of each level (in display order): completed levels are lit, the
 * first not-yet-completed level is the current one, the very next level is available (a one-step
 * lookahead so a child isn't hard-blocked), and anything further ahead is locked (visible but
 * dimmed). Islands themselves are never locked — only levels within an island. (Sub-phase 1.7.)
 */
export function levelStates(levels: Level[], results: ExerciseResult[]): PlayState[] {
  const completed = levels.map((level) => isLevelCompleted(level, results));
  const currentIndex = completed.findIndex((done) => !done);
  return levels.map((_, index) => {
    if (completed[index]) {
      return 'completed';
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
