/**
 * API contract types. JSON from the backend is camelCase and these mirror it EXACTLY.
 * Content endpoints return BOTH locales so the client can switch language offline.
 * `IsCorrect` is never serialized to the client — it is intentionally absent here.
 */

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
  | 'listenPickWord';

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
}

export interface Level {
  id: number;
  subjectId: number;
  displayOrder: number;
  title: Loc;
  difficultyTier: number;
  ageMin: number;
  ageMax: number;
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

export interface LevelDetail extends Level {
  exercises: ExerciseSummary[];
}

export interface Choice {
  id: number;
  displayOrder: number;
  label: Loc;
  imageRef: string | null;
  audio: LocAudio | null;
}

export interface Exercise {
  id: number;
  levelId: number;
  displayOrder: number;
  type: ExerciseType;
  prompt: Loc;
  promptAudio: LocAudio;
  imageRef: string | null;
  choices: Choice[];
}

export interface AttemptRequest {
  choiceId: number;
  attemptNumber?: number;
}

export interface Reward {
  coins: number;
  stars: number;
}

export interface AttemptResult {
  correct: boolean;
  correctChoiceId: number;
  reward: Reward;
}
