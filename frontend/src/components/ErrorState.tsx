import { Mascot } from '../game/Mascot';
import { Button } from './Button';

export interface ErrorStateProps {
  message: string;
  onRetry?: () => void;
  retryLabel?: string;
}

/** Gentle error panel with the mascot and an optional retry action. */
export function ErrorState({ message, onRetry, retryLabel }: ErrorStateProps) {
  return (
    <div className="flex flex-col items-center justify-center gap-4 py-16 text-center" role="alert" data-testid="error">
      <Mascot state="encouraging" size={88} />
      <p className="font-display text-xl font-bold text-[var(--color-text)]">{message}</p>
      {onRetry && retryLabel && (
        <Button variant="secondary" onClick={onRetry}>
          {retryLabel}
        </Button>
      )}
    </div>
  );
}
