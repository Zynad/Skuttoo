import { useTheme } from '../hooks/useTheme';
import { useT } from '../i18n/useT';

export interface ThemeToggleProps {
  className?: string;
}

/** Round light/dark switch. */
export function ThemeToggle({ className = '' }: ThemeToggleProps) {
  const { theme, toggle } = useTheme();
  const t = useT();
  return (
    <button
      type="button"
      data-testid="theme-toggle"
      aria-label={t('theme.toggle')}
      aria-pressed={theme === 'dark'}
      onClick={toggle}
      className={
        `grid h-11 w-11 place-items-center rounded-full bg-[var(--color-surface-raised)] text-xl ` +
        `shadow-[var(--shadow-soft)] transition-transform active:scale-90 ${className}`.trim()
      }
    >
      <span aria-hidden="true">{theme === 'dark' ? '🌙' : '☀️'}</span>
    </button>
  );
}
