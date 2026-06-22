import { describe, expect, it, vi, beforeEach } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Exercise } from './Exercise';
import { renderWithProviders } from '../test/renderWithProviders';
import { contentApi } from '../services/contentApi';
import { clearProfile } from '../services/progressStore';
import type { Exercise as ExerciseModel } from '../types/content';

const mockExercise: ExerciseModel = {
  id: 101,
  levelId: 1,
  displayOrder: 1,
  type: 'countObjects',
  prompt: { sv: 'Hur många äpplen?', en: 'How many apples?' },
  promptAudio: { sv: null, en: null },
  target: null,
  targetAudio: null,
  imageRef: 'assets/img/apples-3.svg',
  subjectKey: 'math',
  contentLanguage: null,
  choices: [
    { id: 1, displayOrder: 1, label: { sv: '2', en: '2' }, imageRef: null, audio: null },
    { id: 2, displayOrder: 2, label: { sv: '3', en: '3' }, imageRef: null, audio: null },
    { id: 3, displayOrder: 3, label: { sv: '4', en: '4' }, imageRef: null, audio: null },
  ],
  buckets: [],
};

describe('Exercise route', () => {
  beforeEach(async () => {
    await clearProfile();
    vi.restoreAllMocks();
    vi.spyOn(contentApi, 'getExercise').mockResolvedValue(mockExercise);
    // No pre-generated clip → HEAD lookups fail, SpeechSynthesis fallback used.
    vi.stubGlobal(
      'fetch',
      vi.fn(() => Promise.resolve({ ok: false } as Response)),
    );
  });

  it('renders the localized prompt and all choices from the mocked exercise', async () => {
    renderWithProviders(<Exercise />, { route: '/exercise/101' });

    expect(await screen.findByText('Hur många äpplen?')).toBeInTheDocument();
    expect(screen.getByTestId('choice-1')).toHaveTextContent('2');
    expect(screen.getByTestId('choice-2')).toHaveTextContent('3');
    expect(screen.getByTestId('choice-3')).toHaveTextContent('4');
  });

  it('reports the chosen choice to the attempt API', async () => {
    const submitSpy = vi
      .spyOn(contentApi, 'submitAttempt')
      .mockResolvedValue({ correct: true, correctChoiceId: 2, reward: { coins: 5, stars: 3 } });

    renderWithProviders(<Exercise />, { route: '/exercise/101' });
    await screen.findByText('Hur många äpplen?');

    await userEvent.click(screen.getByTestId('choice-2'));

    await waitFor(() => expect(submitSpy).toHaveBeenCalledTimes(1));
    const [exerciseId, request] = submitSpy.mock.calls[0];
    expect(exerciseId).toBe(101);
    expect(request.choiceId).toBe(2);
  });

  it('shows the reward burst and praise after a correct answer', async () => {
    vi.spyOn(contentApi, 'submitAttempt').mockResolvedValue({
      correct: true,
      correctChoiceId: 2,
      reward: { coins: 5, stars: 3 },
    });

    renderWithProviders(<Exercise />, { route: '/exercise/101' });
    await screen.findByText('Hur många äpplen?');

    await userEvent.click(screen.getByTestId('choice-2'));

    const burst = await screen.findByTestId('reward-burst');
    expect(burst).toHaveAttribute('data-coins', '5');
    expect(burst).toHaveAttribute('data-stars', '3');
    expect(screen.getByTestId('feedback')).toHaveAttribute('data-phase', 'correct');
    expect(screen.getByTestId('continue-button')).toBeInTheDocument();
  });

  it('shows a gentle retry (no reward) after a wrong answer', async () => {
    vi.spyOn(contentApi, 'submitAttempt').mockResolvedValue({
      correct: false,
      correctChoiceId: 2,
      reward: { coins: 0, stars: 0 },
    });

    renderWithProviders(<Exercise />, { route: '/exercise/101' });
    await screen.findByText('Hur många äpplen?');

    await userEvent.click(screen.getByTestId('choice-1'));

    const feedback = await screen.findByTestId('feedback');
    expect(feedback).toHaveAttribute('data-phase', 'wrong');
    expect(screen.queryByTestId('reward-burst')).not.toBeInTheDocument();
    // Choices remain tappable for another try.
    expect(screen.getByTestId('choice-2')).toBeEnabled();
  });

  it('reveals the correct answer after two wrong tries', async () => {
    vi.spyOn(contentApi, 'submitAttempt').mockResolvedValue({
      correct: false,
      correctChoiceId: 2,
      reward: { coins: 0, stars: 0 },
    });

    renderWithProviders(<Exercise />, { route: '/exercise/101' });
    await screen.findByText('Hur många äpplen?');

    await userEvent.click(screen.getByTestId('choice-1'));
    await waitFor(() => expect(screen.getByTestId('feedback')).toHaveAttribute('data-phase', 'wrong'));
    await userEvent.click(screen.getByTestId('choice-3'));

    await waitFor(() => expect(screen.getByTestId('feedback')).toHaveAttribute('data-phase', 'revealed'));
    expect(screen.getByTestId('choice-2')).toHaveAttribute('data-status', 'revealed');
  });
});
