import { describe, expect, it } from 'vitest';
import { interactionFor } from './interactionKind';
import type { ExerciseType } from '../types/content';

describe('interactionFor', () => {
  it('maps the generic interaction types', () => {
    expect(interactionFor('tapToMatch')).toBe('tap-to-match');
    expect(interactionFor('dragToBucket')).toBe('drag-to-bucket');
  });

  it('treats every content type as multiple-choice', () => {
    const single: ExerciseType[] = [
      'countObjects',
      'numberRecognition',
      'simpleAddition',
      'shapeMatch',
      'colorMatch',
      'patternNext',
      'letterSound',
      'wordImageMatch',
      'listenPickWord',
    ];
    for (const type of single) {
      expect(interactionFor(type)).toBe('multiple-choice');
    }
  });
});
