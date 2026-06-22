import type { Choice as ChoiceModel } from '../types/content';
import type { Lang } from '../i18n/dictionaries';
import { imageUrl } from '../utils/assets';

export type ChoiceStatus = 'idle' | 'correct' | 'wrong' | 'revealed' | 'dimmed';

export interface ChoiceProps {
  choice: ChoiceModel;
  lang: Lang;
  status?: ChoiceStatus;
  disabled?: boolean;
  index: number;
  onSelect: (choice: ChoiceModel) => void;
}

const statusRing: Record<ChoiceStatus, string> = {
  idle: 'border-[var(--color-border)] bg-[var(--color-surface-raised)]',
  correct: 'border-[var(--color-success)] bg-[var(--color-success-soft)] animate-pop',
  wrong: 'border-[var(--color-warn)] bg-[var(--color-warn-soft)] animate-shake',
  revealed: 'border-[var(--color-success)] bg-[var(--color-success-soft)]',
  dimmed: 'border-[var(--color-border)] bg-[var(--color-surface-raised)] opacity-50',
};

/** A big, tappable answer button — image-led with optional text, child-friendly. */
export function Choice({ choice, lang, status = 'idle', disabled = false, index, onSelect }: ChoiceProps) {
  const label = choice.label[lang];
  const img = imageUrl(choice.imageRef);

  return (
    <button
      type="button"
      data-testid={`choice-${choice.id}`}
      data-choice-id={choice.id}
      data-status={status}
      disabled={disabled}
      onClick={() => onSelect(choice)}
      aria-label={label || `Val ${index + 1}`}
      className={
        `flex min-h-[96px] flex-col items-center justify-center gap-2 rounded-[1.5rem] border-4 p-4 ` +
        `shadow-[var(--shadow-soft)] transition-transform duration-150 ` +
        `enabled:active:scale-95 enabled:hover:-translate-y-0.5 focus-visible:outline-none ` +
        `focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] ` +
        `disabled:cursor-default ${statusRing[status]}`
      }
    >
      {img && <img src={img} alt="" aria-hidden="true" className="h-16 w-16 object-contain pointer-events-none" />}
      {label && <span className="font-display text-2xl font-extrabold text-[var(--color-text)]">{label}</span>}
      {status === 'correct' || status === 'revealed' ? (
        <span aria-hidden="true" className="text-xl">
          ✅
        </span>
      ) : null}
    </button>
  );
}
