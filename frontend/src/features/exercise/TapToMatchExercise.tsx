import { useEffect, useRef, useState } from 'react';
import type { AttemptRequest, Exercise, Placement } from '../../types/content';
import type { Lang } from '../../i18n/dictionaries';
import type { FlowPhase } from '../../game/useExerciseFlow';
import { imageUrl } from '../../utils/assets';

export interface TapToMatchExerciseProps {
  exercise: Exercise;
  contentLang: Lang;
  phase: FlowPhase;
  correctPlacements: Placement[] | null;
  disabled: boolean;
  onSubmit: (payload: AttemptRequest) => void;
}

type ItemStatus = 'idle' | 'selected' | 'matched';

const ring: Record<ItemStatus, string> = {
  idle: 'border-[var(--color-border)] bg-[var(--color-surface-raised)]',
  selected: 'border-[var(--color-primary)] bg-[var(--color-primary-soft)] animate-pop',
  matched: 'border-[var(--color-success)] bg-[var(--color-success-soft)]',
};

/**
 * Tap two items that belong together to pair them. When every item is paired the
 * attempt is submitted automatically. The server decides correctness from the items'
 * hidden group keys; the client only reports its partition of the items.
 */
export function TapToMatchExercise({
  exercise,
  contentLang,
  phase,
  correctPlacements,
  disabled,
  onSubmit,
}: TapToMatchExerciseProps) {
  const items = [...exercise.choices].sort((a, b) => a.displayOrder - b.displayOrder);
  const [pairKeyByItem, setPairKeyByItem] = useState<Record<number, string>>({});
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const submittedRef = useRef(false);

  const resolved = phase === 'correct' || phase === 'revealed';

  // After a wrong attempt, clear the pairings so the child can try again.
  useEffect(() => {
    if (phase === 'wrong') {
      setPairKeyByItem({});
      setSelectedId(null);
      submittedRef.current = false;
    }
  }, [phase]);

  // Submit once every item has been paired.
  useEffect(() => {
    const allPaired = items.length > 0 && items.every((i) => pairKeyByItem[i.id] !== undefined);
    if (allPaired && !resolved && !disabled && !submittedRef.current) {
      submittedRef.current = true;
      onSubmit({ placements: items.map((i) => ({ itemId: i.id, targetKey: pairKeyByItem[i.id] })) });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pairKeyByItem, resolved, disabled]);

  const handleTap = (id: number) => {
    if (disabled || resolved || pairKeyByItem[id] !== undefined) {
      return;
    }
    if (selectedId === null) {
      setSelectedId(id);
      return;
    }
    if (selectedId === id) {
      setSelectedId(null);
      return;
    }
    const pairKey = `pair-${Object.keys(pairKeyByItem).length / 2}`;
    setPairKeyByItem((prev) => ({ ...prev, [selectedId]: pairKey, [id]: pairKey }));
    setSelectedId(null);
  };

  // On reveal, colour items by the correct grouping rather than the child's wrong attempt.
  const groupByItem: Record<number, string> = {};
  if (phase === 'revealed' && correctPlacements) {
    for (const p of correctPlacements) {
      groupByItem[p.itemId] = p.targetKey;
    }
  } else {
    Object.assign(groupByItem, pairKeyByItem);
  }
  const groupKeys = Array.from(new Set(Object.values(groupByItem)));

  const statusOf = (id: number): ItemStatus => {
    if (groupByItem[id] !== undefined) {
      return 'matched';
    }
    return selectedId === id ? 'selected' : 'idle';
  };

  return (
    <section className="grid grid-cols-2 gap-3" data-testid="match-grid">
      {items.map((item, index) => {
        const status = statusOf(item.id);
        const img = imageUrl(item.imageRef);
        const label = item.label[contentLang];
        const group = groupByItem[item.id];
        const badge = group === undefined ? null : groupKeys.indexOf(group) + 1;
        return (
          <button
            key={item.id}
            type="button"
            data-testid={`match-item-${item.id}`}
            data-status={status}
            disabled={disabled || status === 'matched'}
            aria-pressed={status === 'selected'}
            aria-label={label || `Val ${index + 1}`}
            onClick={() => handleTap(item.id)}
            className={
              `relative flex min-h-[96px] flex-col items-center justify-center gap-2 rounded-[1.5rem] border-4 p-4 ` +
              `shadow-[var(--shadow-soft)] transition-transform duration-150 enabled:active:scale-95 ` +
              `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 ` +
              `focus-visible:ring-[color:var(--color-primary)] disabled:cursor-default ${ring[status]}`
            }
          >
            {img && <img src={img} alt="" aria-hidden="true" className="h-16 w-16 object-contain pointer-events-none" />}
            {label && <span className="font-display text-2xl font-extrabold text-[var(--color-text)]">{label}</span>}
            {badge !== null && (
              <span
                aria-hidden="true"
                className="absolute right-2 top-2 flex h-7 w-7 items-center justify-center rounded-full bg-[var(--color-success)] text-sm font-bold text-white"
              >
                {badge}
              </span>
            )}
          </button>
        );
      })}
    </section>
  );
}
