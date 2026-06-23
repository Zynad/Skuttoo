import type { ExerciseSummary } from '../types/content';
import type { ExerciseResult } from '../types/progress';

const ordered = (exercises: ExerciseSummary[]): ExerciseSummary[] =>
  [...exercises].sort((a, b) => a.displayOrder - b.displayOrder);

/**
 * The exercise to open when entering a level (node): the first not-yet-solved one — so a partially
 * finished node resumes where the child left off — falling back to the first exercise when every
 * exercise is already solved (a replay). A node has 3–4 exercises; they're played in sequence and
 * the node only counts as complete once all of them are solved.
 */
export function firstUnsolvedExerciseId(exercises: ExerciseSummary[], results: ExerciseResult[]): number | null {
  const sorted = ordered(exercises);
  if (sorted.length === 0) {
    return null;
  }
  const solved = new Set(results.filter((r) => r.completed).map((r) => r.exerciseId));
  const next = sorted.find((e) => !solved.has(e.id));
  return (next ?? sorted[0]).id;
}

/** The next exercise in the node after the given one (by display order), or null if it was the last. */
export function nextExerciseId(exercises: ExerciseSummary[], currentId: number): number | null {
  const sorted = ordered(exercises);
  const index = sorted.findIndex((e) => e.id === currentId);
  if (index === -1 || index === sorted.length - 1) {
    return null;
  }
  return sorted[index + 1].id;
}
