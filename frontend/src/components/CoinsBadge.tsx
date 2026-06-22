import { useT } from '../i18n/useT';

export interface CoinsBadgeProps {
  coins: number;
  stars: number;
  className?: string;
}

/** Compact coins + stars indicator for the top bar. */
export function CoinsBadge({ coins, stars, className = '' }: CoinsBadgeProps) {
  const t = useT();
  return (
    <div
      className={`flex items-center gap-3 rounded-full bg-[var(--color-surface-raised)] px-3 py-1.5 shadow-[var(--shadow-soft)] ${className}`.trim()}
      data-testid="coins-badge"
    >
      <span className="flex items-center gap-1.5" data-testid="coins" aria-label={t('coins.aria', { count: coins })}>
        <span
          aria-hidden="true"
          className="grid h-6 w-6 place-items-center rounded-full bg-[var(--color-coin)] text-sm shadow-[inset_0_-2px_0_var(--color-coin-edge)]"
        >
          ●
        </span>
        <span className="font-display text-lg font-extrabold tabular-nums text-[var(--color-text)]">{coins}</span>
      </span>
      <span className="flex items-center gap-1.5" data-testid="stars" aria-label={t('stars.aria', { count: stars })}>
        <span aria-hidden="true" className="text-lg" style={{ color: 'var(--color-star)' }}>
          ★
        </span>
        <span className="font-display text-lg font-extrabold tabular-nums text-[var(--color-text)]">{stars}</span>
      </span>
    </div>
  );
}
