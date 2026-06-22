import type { IslandTheme } from '../utils/islandTheme';
import { useT } from '../i18n/useT';
import { Mascot } from '../game/Mascot';
import type { IslandProgress } from '../utils/progressSummary';

export interface IslandNodeProps {
  theme: IslandTheme;
  progress: IslandProgress;
  /** True for the island Skutt is nudging the child toward (gets a ring + the mascot). */
  isNext: boolean;
  /** Which side of the trail the node sits on (mirrors the layout). */
  side: 'left' | 'right';
  /** Stagger index for the page-load reveal. */
  index: number;
  onEnter: (theme: IslandTheme) => void;
}

/** A themed island stop along the world-map trail. Colour comes from island tokens. */
export function IslandNode({ theme, progress, isNext, side, index, onEnter }: IslandNodeProps) {
  const t = useT();
  const name = t(theme.nameKey);

  return (
    <div className={`flex items-center gap-3 ${side === 'right' ? 'flex-row-reverse' : ''}`}>
      <button
        type="button"
        data-testid={`island-${theme.key}`}
        data-next={isNext ? 'true' : undefined}
        onClick={() => onEnter(theme)}
        aria-label={t('worldmap.enterIsland', { island: name })}
        className={
          `group relative flex flex-col items-center gap-1 rounded-[2rem] border-4 px-5 py-4 ` +
          `shadow-[var(--shadow-clay)] transition-transform duration-200 active:scale-95 hover:-translate-y-1 ` +
          `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 ` +
          `focus-visible:ring-[color:var(--color-primary)] animate-rise`
        }
        style={{
          background: `color-mix(in srgb, ${theme.colorSoft} 88%, var(--color-surface-raised))`,
          borderColor: theme.color,
          animationDelay: `${index * 90}ms`,
        }}
      >
        {isNext && (
          <span
            aria-hidden="true"
            className="absolute -inset-2 rounded-[2.6rem] border-4"
            style={{ borderColor: theme.color, opacity: 0.45 }}
          />
        )}
        <span
          aria-hidden="true"
          className="grid h-16 w-16 place-items-center rounded-full text-3xl shadow-[var(--shadow-soft)] transition-transform duration-300 group-hover:scale-110 group-hover:animate-hop"
          style={{ background: theme.color }}
        >
          {theme.motif}
        </span>
        <span className="font-display text-xl font-extrabold" style={{ color: theme.color }}>
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

      {/* Skutt stands at the recommended next stop. */}
      {isNext && <Mascot state="happy" size={64} className="shrink-0" />}
    </div>
  );
}
