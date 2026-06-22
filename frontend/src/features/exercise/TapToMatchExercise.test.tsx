import { describe, expect, it, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { TapToMatchExercise } from './TapToMatchExercise';
import type { AttemptRequest, Exercise } from '../../types/content';

const matchExercise: Exercise = {
  id: 300,
  levelId: 2,
  displayOrder: 1,
  type: 'tapToMatch',
  prompt: { sv: 'Para ihop.', en: 'Match.' },
  promptAudio: { sv: null, en: null },
  target: null,
  targetAudio: null,
  imageRef: null,
  subjectKey: 'english',
  contentLanguage: 'en',
  buckets: [],
  choices: [
    { id: 1, displayOrder: 1, label: { sv: 'äpple', en: 'apple' }, imageRef: null, audio: null },
    { id: 2, displayOrder: 2, label: { sv: '', en: '' }, imageRef: 'assets/img/apple.svg', audio: null },
    { id: 3, displayOrder: 3, label: { sv: 'banan', en: 'banana' }, imageRef: null, audio: null },
    { id: 4, displayOrder: 4, label: { sv: '', en: '' }, imageRef: 'assets/img/banana.svg', audio: null },
  ],
};

describe('TapToMatchExercise', () => {
  it('renders item labels in the content language', () => {
    render(
      <TapToMatchExercise
        exercise={matchExercise}
        contentLang="en"
        phase="answering"
        correctPlacements={null}
        disabled={false}
        onSubmit={vi.fn()}
      />,
    );
    expect(screen.getByTestId('match-item-1')).toHaveTextContent('apple');
    expect(screen.queryByText('äpple')).not.toBeInTheDocument();
  });

  it('pairs tapped items and submits placements once all are paired', async () => {
    const onSubmit = vi.fn<(payload: AttemptRequest) => void>();
    render(
      <TapToMatchExercise
        exercise={matchExercise}
        contentLang="en"
        phase="answering"
        correctPlacements={null}
        disabled={false}
        onSubmit={onSubmit}
      />,
    );

    await userEvent.click(screen.getByTestId('match-item-1'));
    expect(screen.getByTestId('match-item-1')).toHaveAttribute('data-status', 'selected');

    await userEvent.click(screen.getByTestId('match-item-2'));
    expect(screen.getByTestId('match-item-1')).toHaveAttribute('data-status', 'matched');
    expect(screen.getByTestId('match-item-2')).toHaveAttribute('data-status', 'matched');

    await userEvent.click(screen.getByTestId('match-item-3'));
    await userEvent.click(screen.getByTestId('match-item-4'));

    await waitFor(() => expect(onSubmit).toHaveBeenCalledTimes(1));
    const payload = onSubmit.mock.calls[0][0];
    expect(payload.placements).toHaveLength(4);
    const byItem = Object.fromEntries((payload.placements ?? []).map((p) => [p.itemId, p.targetKey]));
    expect(byItem[1]).toBe(byItem[2]);
    expect(byItem[3]).toBe(byItem[4]);
    expect(byItem[1]).not.toBe(byItem[3]);
  });
});
