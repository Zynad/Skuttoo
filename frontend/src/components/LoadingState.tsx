import { useT } from '../i18n/useT';
import { Mascot } from '../game/Mascot';

export interface LoadingStateProps {
  message?: string;
}

/** Friendly full-bleed loading indicator with a bobbing mascot. */
export function LoadingState({ message }: LoadingStateProps) {
  const t = useT();
  return (
    <div className="flex flex-col items-center justify-center gap-3 py-16" role="status" data-testid="loading">
      <Mascot state="idle" size={88} />
      <p className="font-display text-lg font-bold text-[var(--color-text-soft)]">{message ?? t('exercise.loading')}</p>
    </div>
  );
}
