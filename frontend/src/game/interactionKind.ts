import type { ExerciseType } from '../types/content';

/** How an exercise is played, regardless of its pedagogical content type. */
export type InteractionKind = 'multiple-choice' | 'tap-to-match' | 'drag-to-bucket';

/**
 * Maps an exercise type to the renderer that drives it. The many content types
 * (count, color, listen-pick, …) are all single-choice; only the two generic
 * interaction types get their own renderers.
 */
export function interactionFor(type: ExerciseType): InteractionKind {
  switch (type) {
    case 'tapToMatch':
      return 'tap-to-match';
    case 'dragToBucket':
      return 'drag-to-bucket';
    default:
      return 'multiple-choice';
  }
}
