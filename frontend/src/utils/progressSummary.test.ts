import { describe, expect, it } from 'vitest';
import { isLevelCompleted, islandProgress, levelStates, nextIsland } from './progressSummary';
import type { Level } from '../types/content';
import type { ExerciseResult } from '../types/progress';

const level = (id: number, exerciseIds: number[]): Level => ({
  id,
  subjectId: 1,
  displayOrder: id,
  title: { sv: `Nivå ${id}`, en: `Level ${id}` },
  difficultyTier: 1,
  ageMin: 3,
  ageMax: 9,
  exerciseIds,
});

const result = (exerciseId: number, completed: boolean, extra: Partial<ExerciseResult> = {}): ExerciseResult => ({
  exerciseId,
  completed,
  starsEarned: completed ? 3 : 0,
  attempts: 1,
  lastPlayedAt: '2026-06-22T00:00:00.000Z',
  ...extra,
});

describe('isLevelCompleted', () => {
  it('is true only when every exercise in the level is solved', () => {
    expect(isLevelCompleted(level(1, [10, 11]), [result(10, true), result(11, true)])).toBe(true);
    expect(isLevelCompleted(level(1, [10, 11]), [result(10, true)])).toBe(false);
  });

  it('is false for a level with no exercises', () => {
    expect(isLevelCompleted(level(1, []), [])).toBe(false);
  });
});

describe('levelStates', () => {
  const levels = [level(1, [10, 11]), level(2, [20, 21]), level(3, [30])];

  it('makes the current level and the next one playable, and locks the rest', () => {
    expect(levelStates(levels, [])).toEqual(['current', 'available', 'locked']);
  });

  it('lights completed levels and moves current + available forward', () => {
    const results = [result(10, true), result(11, true)];
    expect(levelStates(levels, results)).toEqual(['completed', 'current', 'available']);
  });

  it('marks every level completed when all exercises are solved (no locks)', () => {
    const results = [10, 11, 20, 21, 30].map((id) => result(id, true));
    expect(levelStates(levels, results)).toEqual(['completed', 'completed', 'completed']);
  });

  it('keeps levels beyond the one-step lookahead locked even if a later one was solved', () => {
    const results = [result(20, true), result(21, true)]; // second level done, first not
    expect(levelStates(levels, results)).toEqual(['current', 'completed', 'locked']);
  });
});

describe('islandProgress', () => {
  it('sums stars and counts solved exercises attributed to the island', () => {
    const results = [
      result(1, true, { subjectKey: 'math', starsEarned: 3 }),
      result(2, true, { subjectKey: 'math', starsEarned: 2 }),
      result(3, false, { subjectKey: 'math' }),
      result(4, true, { subjectKey: 'logic', starsEarned: 3 }),
    ];
    expect(islandProgress('math', results)).toEqual({ starsEarned: 5, exercisesDone: 2, started: true });
    expect(islandProgress('swedish', results)).toEqual({ starsEarned: 0, exercisesDone: 0, started: false });
  });
});

describe('nextIsland', () => {
  it('points to the first island when nothing is played', () => {
    expect(nextIsland([])).toBe('math');
  });

  it('points to the first not-yet-started island', () => {
    const results = [result(1, true, { subjectKey: 'math' })];
    expect(nextIsland(results)).toBe('swedish');
  });

  it('points to the least-progressed island once all are started', () => {
    const results = [
      result(1, true, { subjectKey: 'math' }),
      result(2, true, { subjectKey: 'math' }),
      result(3, true, { subjectKey: 'swedish' }),
      result(4, true, { subjectKey: 'english' }),
      result(5, true, { subjectKey: 'logic' }),
      result(6, true, { subjectKey: 'logic' }),
    ];
    // swedish and english each have 1 done (fewest); swedish is earlier in order.
    expect(nextIsland(results)).toBe('swedish');
  });
});
