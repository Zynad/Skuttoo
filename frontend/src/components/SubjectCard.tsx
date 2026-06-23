import type { IslandTheme } from '../utils/islandTheme';
import type { IslandProgress } from '../utils/progressSummary';
import { useT } from '../i18n/useT';

export interface SubjectCardProps {
  theme: IslandTheme;
  progress: IslandProgress;
  onEnter: (theme: IslandTheme) => void;
}

/** A big themed subject tile on the hub — one of the four choices. Colour comes from island tokens. */
export function SubjectCard({ theme, progress, onEnter }: SubjectCardProps) {
  const t = useT();
  const name = t(theme.nameKey);

  return (
    <button
      type="button"
      data-testid={`island-${theme.key}`}
      onClick={() => onEnter(theme)}
      aria-label={t('worldmap.enterIsland', { island: name })}
      className={
        `group flex flex-col items-center gap-2 rounded-[2rem] border-4 px-4 py-6 ` +
        `shadow-[var(--shadow-clay)] transition-transform duration-200 hover:-translate-y-1 active:scale-95 ` +
        `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] animate-rise`
      }
      style={{
        background: `color-mix(in srgb, ${theme.colorSoft} 88%, var(--color-surface-raised))`,
        borderColor: theme.color,
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
      <span
        className="inline-flex items-center gap-1 text-sm font-bold text-[var(--color-text-soft)]"
        data-testid={`island-${theme.key}-stars`}
      >
        <span aria-hidden="true" style={{ color: 'var(--color-star)' }}>
          ★
        </span>
        {progress.starsEarned}
      </span>
    </button>
  );
}
