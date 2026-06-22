import type { IslandTheme } from '../utils/islandTheme';
import { useT } from '../i18n/useT';

export interface IslandTileProps {
  theme: IslandTheme;
  /** Stars collected on this island, for a tiny progress hint. */
  earnedStars?: number;
  onEnter: (theme: IslandTheme) => void;
  /** Stagger index for the page-load reveal. */
  index?: number;
}

/** A themed, tappable island on the world map. Color comes from island tokens. */
export function IslandTile({ theme, earnedStars = 0, onEnter, index = 0 }: IslandTileProps) {
  const t = useT();
  const name = t(theme.nameKey);

  return (
    <button
      type="button"
      data-testid={`island-${theme.key}`}
      onClick={() => onEnter(theme)}
      aria-label={t('worldmap.enterIsland', { island: name })}
      className={
        `group relative flex w-full flex-col items-center gap-2 rounded-[2rem] border-4 p-5 text-center ` +
        `shadow-[var(--shadow-clay)] transition-transform duration-200 ` +
        `active:scale-95 hover:-translate-y-1 focus-visible:outline-none ` +
        `focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] animate-rise`
      }
      style={{
        background: `color-mix(in srgb, ${theme.colorSoft} 88%, var(--color-surface-raised))`,
        borderColor: theme.color,
        animationDelay: `${index * 90}ms`,
      }}
    >
      <span
        aria-hidden="true"
        className="grid h-20 w-20 place-items-center rounded-full text-4xl shadow-[var(--shadow-soft)] transition-transform duration-300 group-hover:scale-110 group-hover:animate-hop"
        style={{ background: theme.color }}
      >
        {theme.motif}
      </span>
      <span className="font-display text-2xl font-extrabold" style={{ color: theme.color }}>
        {name}
      </span>
      <span className="text-sm font-semibold text-[var(--color-text-soft)]">{t(theme.descKey)}</span>
      {earnedStars > 0 && (
        <span className="mt-1 inline-flex items-center gap-1 rounded-full bg-[var(--color-surface-raised)] px-2 py-0.5 text-sm font-bold">
          <span aria-hidden="true" style={{ color: 'var(--color-star)' }}>
            ★
          </span>
          {earnedStars}
        </span>
      )}
    </button>
  );
}
