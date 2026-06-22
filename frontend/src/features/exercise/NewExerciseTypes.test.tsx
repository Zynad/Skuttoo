import { describe, expect, it, vi, beforeEach } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { ExerciseRunner } from './ExerciseRunner';
import { renderWithProviders } from '../../test/renderWithProviders';
import { contentApi } from '../../services/contentApi';
import { clearProfile } from '../../services/progressStore';
import type { Exercise as ExerciseModel } from '../../types/content';

/**
 * The exercise types added in phases 1.3–1.5 (PatternNext for Logic, LetterSound for Swedish) reuse
 * the multiple-choice renderer. These tests verify they render and report the chosen choice — proving
 * the "content-only, no new engine" approach works end to end on the client.
 */

// Logic: a colour pattern. The sequence is the exercise image; choices are image-only swatches.
const patternExercise: ExerciseModel = {
  id: 700,
  levelId: 90,
  displayOrder: 1,
  type: 'patternNext',
  prompt: { sv: 'Vad kommer sen i mönstret?', en: 'What comes next in the pattern?' },
  promptAudio: { sv: null, en: null },
  target: null,
  targetAudio: null,
  imageRef: 'assets/img/pattern-color-rb.svg',
  subjectKey: 'logic',
  contentLanguage: null,
  buckets: [],
  choices: [
    { id: 1, displayOrder: 1, label: { sv: '', en: '' }, imageRef: 'assets/img/color-red.svg', audio: null },
    { id: 2, displayOrder: 2, label: { sv: '', en: '' }, imageRef: 'assets/img/color-blue.svg', audio: null },
    { id: 3, displayOrder: 3, label: { sv: '', en: '' }, imageRef: 'assets/img/color-green.svg', audio: null },
  ],
};

// Swedish: a letter-sound exercise. The spoken stimulus is the word; choices are letters (text).
const letterExercise: ExerciseModel = {
  id: 701,
  levelId: 91,
  displayOrder: 1,
  type: 'letterSound',
  prompt: { sv: 'Vilken bokstav börjar ordet på?', en: 'Which letter does the word start with?' },
  promptAudio: { sv: null, en: null },
  target: { sv: 'sol', en: 'sol' },
  targetAudio: { sv: null, en: null },
  imageRef: 'assets/img/sun.svg',
  subjectKey: 'swedish',
  contentLanguage: 'sv',
  buckets: [],
  choices: [
    { id: 11, displayOrder: 1, label: { sv: 'S', en: 'S' }, imageRef: null, audio: null },
    { id: 12, displayOrder: 2, label: { sv: 'M', en: 'M' }, imageRef: null, audio: null },
    { id: 13, displayOrder: 3, label: { sv: 'B', en: 'B' }, imageRef: null, audio: null },
  ],
};

describe('new exercise types reuse the multiple-choice renderer', () => {
  beforeEach(async () => {
    await clearProfile();
    vi.restoreAllMocks();
    // No pre-generated clip → HEAD lookups fail, SpeechSynthesis fallback used.
    vi.stubGlobal(
      'fetch',
      vi.fn(() => Promise.resolve({ ok: false } as Response)),
    );
  });

  it('PatternNext shows the sequence image and image-only choices, and submits the tapped swatch', async () => {
    const submitSpy = vi
      .spyOn(contentApi, 'submitAttempt')
      .mockResolvedValue({ correct: true, correctChoiceId: 1, reward: { coins: 10, stars: 3 } });

    renderWithProviders(<ExerciseRunner exercise={patternExercise} />, { lang: 'sv' });

    // The pattern sequence is conveyed by the exercise image.
    const image = screen.getByTestId('exercise-image');
    expect(image).toHaveAttribute('src', expect.stringContaining('pattern-color-rb'));

    // Three image-only choices render through the choices grid.
    const choices = screen.getByTestId('choices');
    expect(choices).toBeInTheDocument();
    expect(screen.getByTestId('choice-1')).toBeInTheDocument();
    expect(screen.getByTestId('choice-3')).toBeInTheDocument();

    await userEvent.click(screen.getByTestId('choice-1'));

    await waitFor(() => expect(submitSpy).toHaveBeenCalledTimes(1));
    const [exerciseId, request] = submitSpy.mock.calls[0];
    expect(exerciseId).toBe(700);
    expect(request.choiceId).toBe(1);
  });

  it('LetterSound shows the instruction and letter choices, and submits the tapped letter', async () => {
    const submitSpy = vi
      .spyOn(contentApi, 'submitAttempt')
      .mockResolvedValue({ correct: true, correctChoiceId: 11, reward: { coins: 10, stars: 3 } });

    renderWithProviders(<ExerciseRunner exercise={letterExercise} />, { lang: 'sv' });

    // Instruction stays in the UI language.
    expect(screen.getByText('Vilken bokstav börjar ordet på?')).toBeInTheDocument();

    // Letters render as tappable text choices.
    expect(screen.getByTestId('choice-11')).toHaveTextContent('S');
    expect(screen.getByTestId('choice-12')).toHaveTextContent('M');

    await userEvent.click(screen.getByTestId('choice-11'));

    await waitFor(() => expect(submitSpy).toHaveBeenCalledTimes(1));
    const [exerciseId, request] = submitSpy.mock.calls[0];
    expect(exerciseId).toBe(701);
    expect(request.choiceId).toBe(11);
  });
});
