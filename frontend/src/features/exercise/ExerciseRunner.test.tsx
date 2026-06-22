import { describe, expect, it } from 'vitest';
import { screen } from '@testing-library/react';
import { ExerciseRunner } from './ExerciseRunner';
import { renderWithProviders } from '../../test/renderWithProviders';
import type { Exercise } from '../../types/content';

/** English island exercise: instruction in the UI language, answer words in English. */
const englishExercise: Exercise = {
  id: 500,
  levelId: 2,
  displayOrder: 1,
  type: 'wordImageMatch',
  prompt: { sv: 'Vilken bild är en banan?', en: 'Which picture is a banana?' },
  promptAudio: { sv: null, en: null },
  target: { sv: 'banana', en: 'banana' },
  targetAudio: { sv: null, en: null },
  imageRef: null,
  subjectKey: 'english',
  contentLanguage: 'en',
  buckets: [],
  choices: [
    { id: 1, displayOrder: 1, label: { sv: 'äpple', en: 'apple' }, imageRef: null, audio: null },
    { id: 2, displayOrder: 2, label: { sv: 'banan', en: 'banana' }, imageRef: null, audio: null },
  ],
};

const matchExercise: Exercise = { ...englishExercise, id: 501, type: 'tapToMatch', target: null };

const bucketExercise: Exercise = {
  ...englishExercise,
  id: 502,
  type: 'dragToBucket',
  target: null,
  buckets: [{ id: 1, displayOrder: 1, key: 'fruit', label: { sv: 'Frukt', en: 'Fruit' }, imageRef: null }],
};

describe('ExerciseRunner', () => {
  it('shows English answer words while keeping Swedish instructions and helper text', () => {
    renderWithProviders(<ExerciseRunner exercise={englishExercise} />, { lang: 'sv' });

    // Instruction + helper stay in the UI language (Swedish).
    expect(screen.getByText('Vilken bild är en banan?')).toBeInTheDocument();
    expect(screen.getByText('Välj rätt svar')).toBeInTheDocument();

    // Answer labels render in the content language (English) — the bug fix.
    expect(screen.getByTestId('choice-1')).toHaveTextContent('apple');
    expect(screen.getByTestId('choice-2')).toHaveTextContent('banana');
    expect(screen.queryByText('äpple')).not.toBeInTheDocument();
  });

  it('dispatches to the tap-to-match renderer', () => {
    renderWithProviders(<ExerciseRunner exercise={matchExercise} />, { lang: 'sv' });
    expect(screen.getByTestId('match-grid')).toBeInTheDocument();
  });

  it('dispatches to the drag-to-bucket renderer', () => {
    renderWithProviders(<ExerciseRunner exercise={bucketExercise} />, { lang: 'sv' });
    expect(screen.getByTestId('bucket-tray')).toBeInTheDocument();
  });
});
