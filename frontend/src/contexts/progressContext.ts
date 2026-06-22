import { createContext } from 'react';
import type { AttemptResult, SubjectKey } from '../types/content';
import type { Profile } from '../types/progress';
import type { ApplyAttemptOutcome } from '../utils/progressLogic';

export interface RecordAttemptArgs {
  exerciseId: number;
  attemptNumber: number;
  result: AttemptResult;
  /** Owning island/level, recorded for map-level progress (optional). */
  subjectKey?: SubjectKey;
  levelId?: number;
}

export interface ProgressContextValue {
  profile: Profile;
  loading: boolean;
  /** Applies an attempt, persists, and returns what was awarded. */
  recordAttempt: (args: RecordAttemptArgs) => Promise<ApplyAttemptOutcome>;
  /** Resets progress to a fresh profile (used by profile screen / tests). */
  reset: () => Promise<void>;
}

export const ProgressContext = createContext<ProgressContextValue | undefined>(undefined);
