import { describe, expect, it, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { DragToBucketExercise } from './DragToBucketExercise';
import type { AttemptRequest, Exercise } from '../../types/content';

const bucketExercise: Exercise = {
  id: 400,
  levelId: 2,
  displayOrder: 2,
  type: 'dragToBucket',
  prompt: { sv: 'Sortera.', en: 'Sort.' },
  promptAudio: { sv: null, en: null },
  target: null,
  targetAudio: null,
  imageRef: null,
  subjectKey: 'english',
  contentLanguage: 'en',
  buckets: [
    { id: 10, displayOrder: 1, key: 'fruit', label: { sv: 'Frukt', en: 'Fruit' }, imageRef: null },
    { id: 11, displayOrder: 2, key: 'vehicle', label: { sv: 'Fordon', en: 'Vehicle' }, imageRef: null },
  ],
  choices: [
    { id: 1, displayOrder: 1, label: { sv: 'äpple', en: 'apple' }, imageRef: null, audio: null },
    { id: 2, displayOrder: 2, label: { sv: 'banan', en: 'banana' }, imageRef: null, audio: null },
    { id: 3, displayOrder: 3, label: { sv: 'bil', en: 'car' }, imageRef: null, audio: null },
  ],
};

describe('DragToBucketExercise', () => {
  it('renders bucket and item labels in the content language', () => {
    render(
      <DragToBucketExercise
        exercise={bucketExercise}
        contentLang="en"
        phase="answering"
        correctPlacements={null}
        disabled={false}
        onSubmit={vi.fn()}
      />,
    );
    expect(screen.getByTestId('bucket-fruit')).toHaveTextContent('Fruit');
    expect(screen.getByTestId('bucket-vehicle')).toHaveTextContent('Vehicle');
    expect(screen.getByTestId('drag-item-1')).toHaveTextContent('apple');
  });

  it('places each tapped item into a bucket and submits when all are placed', async () => {
    const onSubmit = vi.fn<(payload: AttemptRequest) => void>();
    render(
      <DragToBucketExercise
        exercise={bucketExercise}
        contentLang="en"
        phase="answering"
        correctPlacements={null}
        disabled={false}
        onSubmit={onSubmit}
      />,
    );

    await userEvent.click(screen.getByTestId('drag-item-1'));
    expect(screen.getByTestId('drag-item-1')).toHaveAttribute('data-picked', 'true');
    await userEvent.click(screen.getByTestId('bucket-fruit'));
    expect(screen.getByTestId('drag-item-1')).toHaveAttribute('data-placed', 'true');

    await userEvent.click(screen.getByTestId('drag-item-2'));
    await userEvent.click(screen.getByTestId('bucket-fruit'));

    await userEvent.click(screen.getByTestId('drag-item-3'));
    await userEvent.click(screen.getByTestId('bucket-vehicle'));

    await waitFor(() => expect(onSubmit).toHaveBeenCalledTimes(1));
    const payload = onSubmit.mock.calls[0][0];
    expect(payload.placements).toHaveLength(3);
    const byItem = Object.fromEntries((payload.placements ?? []).map((p) => [p.itemId, p.targetKey]));
    expect(byItem[1]).toBe('fruit');
    expect(byItem[2]).toBe('fruit');
    expect(byItem[3]).toBe('vehicle');
  });
});
