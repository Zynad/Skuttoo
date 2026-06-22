import { createContext } from 'react';
import type { AttemptResult, SubjectKey } from '../types/content';
import type { Profile } from '../types/progress';
import type { BadgeDef } from '../types/badges';
import type { EquippableCategory } from '../types/cosmetics';
import type { ApplyAttemptOutcome } from '../utils/progressLogic';

export interface RecordAttemptArgs {
  exerciseId: number;
  attemptNumber: number;
  result: AttemptResult;
  /** Owning island/level, recorded for map-level progress (optional). */
  subjectKey?: SubjectKey;
  levelId?: number;
}

/** What an attempt produced, plus any badges newly earned by it (coin/streak badges). */
export interface RecordAttemptOutcome extends ApplyAttemptOutcome {
  newBadgeKeys: string[];
}

export interface ProgressContextValue {
  profile: Profile;
  loading: boolean;
  /** The badge catalogue (from the API, or a bundled fallback when offline). */
  badgeCatalog: BadgeDef[];
  /** Applies an attempt, persists, awards coin/streak badges, and returns what was awarded. */
  recordAttempt: (args: RecordAttemptArgs) => Promise<RecordAttemptOutcome>;
  /**
   * Records a subject's level/subject completion (computed where content is loaded) so the
   * structural badges can be evaluated, and returns any newly earned badge keys.
   */
  syncSubjectCompletion: (subjectKey: SubjectKey, completedLevelIds: number[], subjectComplete: boolean) => Promise<string[]>;
  /** Buys a cosmetic (deducts coins, adds to owned). No-op if unaffordable / already owned. */
  purchaseCosmetic: (itemId: string) => Promise<void>;
  /** Equips (id) or un-equips (null) a cosmetic in the given slot. */
  equipCosmetic: (category: EquippableCategory, id: string | null) => Promise<void>;
  /** Resets progress to a fresh profile (used by profile screen / tests). */
  reset: () => Promise<void>;
}

export const ProgressContext = createContext<ProgressContextValue | undefined>(undefined);
