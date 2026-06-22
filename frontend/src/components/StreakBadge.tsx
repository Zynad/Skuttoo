import { useT } from '../i18n/useT';

export interface StreakBadgeProps {
  count: number;
  /** Larger variant for the profile screen. */
  size?: 'sm' | 'lg';
  className?: string;
}

/** Compact daily-streak indicator (🔥 + day count), mirroring CoinsBadge. */
export function StreakBadge({ count, size = 'sm', className = '' }: StreakBadgeProps) {
  const t = useT();
  const big = size === 'lg';
  return (
    <div
      className={`flex items-center gap-1.5 rounded-full bg-[var(--color-surface-raised)] ${
        big ? 'px-4 py-2' : 'px-3 py-1.5'
      } shadow-[var(--shadow-soft)] ${className}`.trim()}
      data-testid="streak-badge"
      data-count={count}
      aria-label={t('streak.aria', { count })}
    >
      <span aria-hidden="true" className={big ? 'text-2xl' : 'text-lg'}>
        🔥
      </span>
      <span className={`font-display font-extrabold tabular-nums text-[var(--color-text)] ${big ? 'text-2xl' : 'text-lg'}`}>
        {count}
      </span>
    </div>
  );
}
