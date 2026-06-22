/**
 * API contract types. JSON from the backend is camelCase and these mirror it EXACTLY.
 * Content endpoints return BOTH locales so the client can switch language offline.
 * `IsCorrect` / `GroupKey` are never serialized to the client — they are intentionally absent here.
 */

import type { Lang } from '../i18n/dictionaries';

export type SubjectKey = 'math' | 'swedish' | 'english' | 'logic';

export type ExerciseType =
  | 'countObjects'
  | 'numberRecognition'
  | 'simpleAddition'
  | 'shapeMatch'
  | 'colorMatch'
  | 'patternNext'
  | 'letterSound'
  | 'wordImageMatch'
  | 'listenPickWord'
  | 'tapToMatch'
  | 'dragToBucket';

/** Every user-facing string carries both locales. */
export interface Loc {
  sv: string;
  en: string;
}

/** Relative paths to pre-generated audio; null when no clip authored. */
export interface LocAudio {
  sv: string | null;
  en: string | null;
}

export interface Subject {
  id: number;
  key: SubjectKey;
  name: Loc;
  description: Loc;
  themeKey: string;
  displayOrder: number;
  /** Language this island teaches; null = follow the UI language (Math, Logic). */
  contentLanguage: Lang | null;
}

export interface Level {
  id: number;
  subjectId: number;
  displayOrder: number;
  title: Loc;
  difficultyTier: number;
  ageMin: number;
  ageMax: number;
  /** Ids of this level's exercises, so the client can show real per-level progress. */
  exerciseIds: number[];
}

export interface SubjectDetail extends Subject {
  levels: Level[];
}

export interface ExerciseSummary {
  id: number;
  levelId: number;
  displayOrder: number;
  type: ExerciseType;
}

/** A level with full exercise summaries (from GET /levels/{id}); no separate exerciseIds field. */
export interface LevelDetail extends Omit<Level, 'exerciseIds'> {
  exercises: ExerciseSummary[];
}

export interface Choice {
  id: number;
  displayOrder: number;
  label: Loc;
  imageRef: string | null;
  audio: LocAudio | null;
}

/** A drop target for drag-to-bucket exercises. */
export interface Bucket {
  id: number;
  displayOrder: number;
  key: string;
  label: Loc;
  imageRef: string | null;
}

export interface Exercise {
  id: number;
  levelId: number;
  displayOrder: number;
  type: ExerciseType;
  /** The instruction, shown/spoken in the UI language. */
  prompt: Loc;
  promptAudio: LocAudio;
  /** The taught word (language islands), rendered/spoken in the content language. */
  target: Loc | null;
  targetAudio: LocAudio | null;
  imageRef: string | null;
  subjectKey: SubjectKey;
  /** The owning subject's content language; null = follow the UI language. */
  contentLanguage: Lang | null;
  choices: Choice[];
  /** Drop targets for drag-to-bucket exercises (empty otherwise). */
  buckets: Bucket[];
}

/** One placed item. drag-to-bucket: targetKey is the bucket key. tap-to-match: a client pair id. */
export interface Placement {
  itemId: number;
  targetKey: string;
}

export interface AttemptRequest {
  choiceId?: number;
  placements?: Placement[];
  attemptNumber?: number;
}

export interface Reward {
  coins: number;
  stars: number;
}

export interface AttemptResult {
  correct: boolean;
  correctChoiceId: number | null;
  reward: Reward;
  /** The correct mapping for the reveal (generic types); null/absent for single-choice. */
  correctPlacements?: Placement[] | null;
}
