import { ageBandForAge } from '../types/progress';

export interface AgeUiDefaults {
  /** Auto-play the instruction audio when an exercise loads (pre-readers rely on it). */
  audioAutoplay: boolean;
  /** Suggested maximum number of answer choices (content already respects this per node). */
  maxChoices: number;
  /** How prominently to lean on written text vs pictures + audio. */
  textDensity: 'low' | 'normal';
}

/**
 * UI behaviour defaults derived from the child's exact age. Younger children (pre-readers, ≤5) get
 * fewer choices and low text density; readers (≥6) get more text. Audio auto-play stays on for every
 * age — Skuttoo is audio-first by design — but the flag gives a single seam to tune later. Derived,
 * never stored, so changing age updates behaviour everywhere at once. A `null` age (not onboarded)
 * uses the youngest, most forgiving defaults.
 */
export function uiDefaultsForAge(age: number | null): AgeUiDefaults {
  if (age === null || ageBandForAge(age) === '3-5') {
    return { audioAutoplay: true, maxChoices: 3, textDensity: 'low' };
  }
  return { audioAutoplay: true, maxChoices: 4, textDensity: 'normal' };
}
