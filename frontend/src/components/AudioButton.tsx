import { useT } from '../i18n/useT';
import { useSpeak } from '../hooks/useSpeak';

export interface AudioButtonProps {
  /** Text spoken by the SpeechSynthesis fallback. */
  text: string;
  /** Optional pre-generated clip path; played when the file exists. */
  clip?: string | null;
  /** Visible label shown next to the icon (defaults to "Read aloud"). */
  label?: string;
  size?: 'md' | 'lg';
  className?: string;
}

/**
 * Round read-aloud button. Plays the pre-generated clip when present, otherwise
 * falls back to the browser SpeechSynthesis in the active language via useSpeak.
 */
export function AudioButton({ text, clip, label, size = 'md', className = '' }: AudioButtonProps) {
  const t = useT();
  const { say, speaking } = useSpeak();
  const visibleLabel = label ?? t('exercise.readAloud');
  const dimension = size === 'lg' ? 'h-16 w-16 text-3xl' : 'h-12 w-12 text-xl';

  return (
    <button
      type="button"
      data-testid="audio-button"
      aria-label={visibleLabel}
      aria-pressed={speaking}
      onClick={() => void say(text, clip)}
      className={
        `inline-flex shrink-0 items-center justify-center rounded-full text-white ` +
        `bg-[var(--color-accent)] shadow-[var(--shadow-soft)] border-b-4 border-[var(--color-warn)] ` +
        `transition-transform duration-150 active:scale-90 focus-visible:outline-none ` +
        `focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] ` +
        `${dimension} ${speaking ? 'animate-pop' : ''} ${className}`.trim()
      }
    >
      <span aria-hidden="true">{speaking ? '🔊' : '🔈'}</span>
    </button>
  );
}
