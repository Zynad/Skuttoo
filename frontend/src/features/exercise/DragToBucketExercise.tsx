import { useEffect, useMemo, useRef, useState } from 'react';
import type { AttemptRequest, Exercise, Placement } from '../../types/content';
import type { Lang } from '../../i18n/dictionaries';
import type { FlowPhase } from '../../game/useExerciseFlow';
import { imageUrl } from '../../utils/assets';
import { shuffle } from '../../utils/shuffle';

export interface DragToBucketExerciseProps {
  exercise: Exercise;
  contentLang: Lang;
  phase: FlowPhase;
  correctPlacements: Placement[] | null;
  disabled: boolean;
  onSubmit: (payload: AttemptRequest) => void;
}

/**
 * Sort items into buckets by tapping. Tap-to-place is the primary interaction (reliable on
 * touch and accessible): tap an item to pick it up, then tap a bucket to drop it there.
 * When every item is placed the attempt is submitted automatically.
 */
export function DragToBucketExercise({
  exercise,
  contentLang,
  phase,
  correctPlacements,
  disabled,
  onSubmit,
}: DragToBucketExerciseProps) {
  // Shuffle the draggable items once per exercise so they aren't always in the same spot; buckets
  // keep their authored order (they're labelled categories).
  const items = useMemo(() => shuffle(exercise.choices), [exercise.choices]);
  const buckets = [...exercise.buckets].sort((a, b) => a.displayOrder - b.displayOrder);
  const [placedByItem, setPlacedByItem] = useState<Record<number, string>>({});
  const [pickedId, setPickedId] = useState<number | null>(null);
  const submittedRef = useRef(false);

  const resolved = phase === 'correct' || phase === 'revealed';

  useEffect(() => {
    if (phase === 'wrong') {
      setPlacedByItem({});
      setPickedId(null);
      submittedRef.current = false;
    }
  }, [phase]);

  useEffect(() => {
    const allPlaced = items.length > 0 && items.every((i) => placedByItem[i.id] !== undefined);
    if (allPlaced && !resolved && !disabled && !submittedRef.current) {
      submittedRef.current = true;
      onSubmit({ placements: items.map((i) => ({ itemId: i.id, targetKey: placedByItem[i.id] })) });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [placedByItem, resolved, disabled]);

  // On reveal, show the correct arrangement instead of the child's wrong one.
  const placement: Record<number, string> = {};
  if (phase === 'revealed' && correctPlacements) {
    for (const p of correctPlacements) {
      placement[p.itemId] = p.targetKey;
    }
  } else {
    Object.assign(placement, placedByItem);
  }

  const pickItem = (id: number) => {
    if (disabled || resolved) {
      return;
    }
    if (placedByItem[id] !== undefined) {
      // Take it back out of its bucket and hold it.
      setPlacedByItem((prev) => {
        const next = { ...prev };
        delete next[id];
        return next;
      });
      setPickedId(id);
      return;
    }
    setPickedId((prev) => (prev === id ? null : id));
  };

  const dropInBucket = (key: string) => {
    if (disabled || resolved || pickedId === null) {
      return;
    }
    setPlacedByItem((prev) => ({ ...prev, [pickedId]: key }));
    setPickedId(null);
  };

  const renderItem = (id: number) => {
    const item = items.find((i) => i.id === id);
    if (!item) {
      return null;
    }
    const img = imageUrl(item.imageRef);
    const label = item.label[contentLang];
    const picked = pickedId === id;
    const placed = placement[id] !== undefined;
    return (
      <button
        key={id}
        type="button"
        data-testid={`drag-item-${id}`}
        data-picked={picked}
        data-placed={placed}
        disabled={disabled}
        aria-pressed={picked}
        aria-label={label || `Val ${id}`}
        onClick={() => pickItem(id)}
        className={
          `flex min-h-[72px] min-w-[72px] flex-col items-center justify-center gap-1 rounded-2xl border-4 p-2 ` +
          `shadow-[var(--shadow-soft)] transition-transform duration-150 enabled:active:scale-95 ` +
          `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] ` +
          (picked
            ? 'border-[var(--color-primary)] bg-[var(--color-primary-soft)] animate-pop'
            : 'border-[var(--color-border)] bg-[var(--color-surface-raised)]')
        }
      >
        {img && <img src={img} alt="" aria-hidden="true" className="h-12 w-12 object-contain pointer-events-none" />}
        {label && <span className="font-display text-lg font-extrabold text-[var(--color-text)]">{label}</span>}
      </button>
    );
  };

  const unplaced = items.filter((i) => placement[i.id] === undefined).map((i) => i.id);
  const droppable = pickedId !== null && !disabled && !resolved;

  return (
    <div className="space-y-4">
      <section
        data-testid="bucket-tray"
        className="flex min-h-[88px] flex-wrap items-center justify-center gap-2 rounded-2xl border-4 border-dashed border-[var(--color-border)] bg-[var(--color-surface-sunk)] p-3"
      >
        {unplaced.length > 0 ? (
          unplaced.map((id) => renderItem(id))
        ) : (
          <span aria-hidden="true" className="text-3xl">
            ✨
          </span>
        )}
      </section>

      <section className="grid grid-cols-2 gap-3">
        {buckets.map((bucket) => {
          const placedHere = items.filter((i) => placement[i.id] === bucket.key).map((i) => i.id);
          const img = imageUrl(bucket.imageRef);
          return (
            <div
              key={bucket.key}
              className="flex flex-col gap-2 rounded-[1.5rem] border-4 border-[var(--color-border)] bg-[var(--color-surface)] p-3"
            >
              <button
                type="button"
                data-testid={`bucket-${bucket.key}`}
                disabled={disabled || pickedId === null}
                aria-label={bucket.label[contentLang]}
                onClick={() => dropInBucket(bucket.key)}
                className={
                  `flex items-center justify-center gap-2 rounded-2xl border-4 py-3 font-display text-lg font-extrabold ` +
                  `text-[var(--color-text)] transition-transform duration-150 enabled:active:scale-95 focus-visible:outline-none ` +
                  `focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] ` +
                  (droppable
                    ? 'border-[var(--color-primary)] bg-[var(--color-primary-soft)]'
                    : 'border-[var(--color-border)] bg-[var(--color-surface-raised)]')
                }
              >
                {img && <img src={img} alt="" aria-hidden="true" className="h-8 w-8 object-contain" />}
                {bucket.label[contentLang]}
              </button>
              <div className="flex min-h-[60px] flex-wrap items-center justify-center gap-1">
                {placedHere.map((id) => renderItem(id))}
              </div>
            </div>
          );
        })}
      </section>
    </div>
  );
}
