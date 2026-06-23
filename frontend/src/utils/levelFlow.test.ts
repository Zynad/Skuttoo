import { describe, expect, it } from 'vitest';
import { firstUnsolvedExerciseId, nextExerciseId } from './levelFlow';
import type { ExerciseSummary } from '../types/content';
import type { ExerciseResult } from '../types/progress';

const ex = (id: number, displayOrder: number): ExerciseSummary => ({
  id,
  levelId: 1,
  displayOrder,
  type: 'countObjects',
});

const solved = (exerciseId: number): ExerciseResult => ({
  exerciseId,
  completed: true,
  starsEarned: 3,
  attempts: 1,
  lastPlayedAt: '2026-06-22T00:00:00.000Z',
});

// Deliberately out of order to prove sorting by displayOrder.
const exercises = [ex(12, 3), ex(10, 1), ex(11, 2)];

describe('firstUnsolvedExerciseId', () => {
  it('opens the first exercise when none are solved', () => {
    expect(firstUnsolvedExerciseId(exercises, [])).toBe(10);
  });

  it('resumes at the first not-yet-solved exercise', () => {
    expect(firstUnsolvedExerciseId(exercises, [solved(10)])).toBe(11);
    expect(firstUnsolvedExerciseId(exercises, [solved(10), solved(11)])).toBe(12);
  });

  it('falls back to the first exercise when all are solved (replay)', () => {
    expect(firstUnsolvedExerciseId(exercises, [solved(10), solved(11), solved(12)])).toBe(10);
  });

  it('returns null for a node with no exercises', () => {
    expect(firstUnsolvedExerciseId([], [])).toBeNull();
  });
});

describe('nextExerciseId', () => {
  it('returns the next exercise by display order', () => {
    expect(nextExerciseId(exercises, 10)).toBe(11);
    expect(nextExerciseId(exercises, 11)).toBe(12);
  });

  it('returns null after the last exercise (back to the map)', () => {
    expect(nextExerciseId(exercises, 12)).toBeNull();
  });

  it('returns null for an unknown exercise id', () => {
    expect(nextExerciseId(exercises, 999)).toBeNull();
  });
});
