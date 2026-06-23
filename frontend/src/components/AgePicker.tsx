import { useState } from 'react';
import { useT } from '../i18n/useT';
import { useSpeak } from '../hooks/useSpeak';
import { MAX_AGE, MIN_AGE } from '../types/progress';
import { Button } from './Button';

const AGES = Array.from({ length: MAX_AGE - MIN_AGE + 1 }, (_, i) => MIN_AGE + i);

export interface AgePickerProps {
  /** Pre-selected age (e.g. when changing age from the profile). */
  initialAge?: number | null;
  /** Called with the confirmed age. */
  onConfirm: (age: number) => void;
}

/**
 * Big, audio-first age picker (3–9). Tapping a number selects it and reads it aloud (so a
 * pre-reader or a parent hears the choice); a separate confirm button commits — a second
 * deliberate step so a stray tap doesn't lock in the wrong age.
 */
export function AgePicker({ initialAge = null, onConfirm }: AgePickerProps) {
  const t = useT();
  const { say } = useSpeak();
  const [selected, setSelected] = useState<number | null>(initialAge);

  const pick = (age: number) => {
    setSelected(age);
    void say(t('onboarding.ageYears', { age }));
  };

  return (
    <div>
      <div role="radiogroup" aria-label={t('onboarding.pickAge')} className="grid grid-cols-3 gap-3 sm:grid-cols-4">
        {AGES.map((age) => {
          const isSelected = selected === age;
          return (
            <button
              key={age}
              type="button"
              role="radio"
              aria-checked={isSelected}
              aria-label={t('onboarding.ageAria', { age })}
              data-testid={`age-option-${age}`}
              data-selected={isSelected}
              onClick={() => pick(age)}
              className={
                `grid aspect-square min-h-[72px] place-items-center rounded-[1.6rem] border-4 ` +
                `font-display text-4xl font-extrabold text-[var(--color-text)] shadow-[var(--shadow-clay)] ` +
                `transition-transform active:scale-95 focus-visible:outline-none focus-visible:ring-4 ` +
                `focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)]`
              }
              style={{
                borderColor: isSelected ? 'var(--color-primary)' : 'var(--color-border)',
                background: isSelected
                  ? 'color-mix(in srgb, var(--color-primary-soft) 80%, var(--color-surface-raised))'
                  : 'var(--color-surface-raised)',
              }}
            >
              {age}
            </button>
          );
        })}
      </div>

      <div className="mt-6 flex justify-center">
        <Button
          size="lg"
          data-testid="age-confirm"
          disabled={selected === null}
          onClick={() => selected !== null && onConfirm(selected)}
        >
          {t('onboarding.confirm')}
        </Button>
      </div>
    </div>
  );
}
