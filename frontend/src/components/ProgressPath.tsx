import type { Level } from '../types/content';
import type { Lang } from '../i18n/dictionaries';
import type { IslandTheme } from '../utils/islandTheme';
import { useT } from '../i18n/useT';

export type LevelState = 'completed' | 'current' | 'available' | 'locked' | 'optional';

export interface ProgressPathProps {
  levels: Level[];
  theme: IslandTheme;
  lang: Lang;
  /** Resolves the play state of each level. */
  stateOf: (level: Level, index: number) => LevelState;
  onSelectLevel: (level: Level) => void;
}

/**
 * A winding trail of themed "island" nodes for one subject (planets / glades / destinations /
 * temples) — nodes alternate sides along a dotted, theme-tinted path. completed = lit + starred,
 * current = highlighted ring, available = playable, optional = muted warm-up, locked = dimmed.
 */
export function ProgressPath({ levels, theme, lang, stateOf, onSelectLevel }: ProgressPathProps) {
  const t = useT();

  return (
    <div className="relative" data-testid="progress-path">
      {/* Dotted trail down the middle, tinted by the subject theme. */}
      <div
        aria-hidden="true"
        className="absolute bottom-6 left-1/2 top-6 w-1 -translate-x-1/2 rounded-full border-l-4 border-dashed"
        style={{ borderColor: theme.colorAccent, opacity: 0.6 }}
      />

      <ol className="relative flex list-none flex-col gap-5 p-0">
        {levels.map((level, index) => {
          const state = stateOf(level, index);
          const locked = state === 'locked';
          const completed = state === 'completed';
          const current = state === 'current';
          const optional = state === 'optional';
          const side = index % 2 === 0 ? 'left' : 'right';
          const emblemBg = locked ? 'var(--color-border)' : optional ? theme.colorAccent : theme.color;

          return (
            <li key={level.id} className={`w-[88%] sm:w-[74%] ${side === 'right' ? 'self-end' : 'self-start'}`}>
              <button
                type="button"
                data-testid={`level-${level.id}`}
                data-state={state}
                disabled={locked}
                onClick={() => onSelectLevel(level)}
                aria-label={`${level.title[lang]} — ${t(theme.metaphorNounKey, { n: level.displayOrder })}`}
                className={
                  `group flex w-full items-center gap-3 rounded-[1.8rem] border-4 p-3 text-left ` +
                  `shadow-[var(--shadow-clay)] transition-transform duration-150 enabled:hover:-translate-y-0.5 enabled:active:scale-[0.97] ` +
                  `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 ` +
                  `focus-visible:ring-[color:var(--color-primary)] disabled:cursor-default`
                }
                style={{
                  borderColor: current ? theme.color : 'var(--color-border)',
                  background: current
                    ? `color-mix(in srgb, ${theme.colorSoft} 80%, var(--color-surface-raised))`
                    : 'var(--color-surface-raised)',
                  opacity: locked ? 0.55 : optional ? 0.85 : 1,
                }}
              >
                {/* The themed "island" emblem (planet / glade / destination / temple). */}
                <span
                  aria-hidden="true"
                  className={
                    `grid h-16 w-16 shrink-0 place-items-center rounded-full font-display text-2xl font-extrabold text-white ` +
                    `shadow-[var(--shadow-soft)] ${current ? 'group-hover:animate-hop' : ''}`
                  }
                  style={{ background: emblemBg }}
                >
                  {locked ? '🔒' : completed ? '★' : level.displayOrder}
                </span>

                <span className="min-w-0 flex-1">
                  <span className="block text-sm font-bold uppercase tracking-wide" style={{ color: theme.color }}>
                    {t(theme.metaphorNounKey, { n: level.displayOrder })}
                  </span>
                  <span className="block truncate font-display text-lg font-extrabold text-[var(--color-text)]">
                    {level.title[lang]}
                  </span>
                  <span className="block text-sm font-bold text-[var(--color-text-soft)]">
                    {locked ? t('island.locked') : optional ? t('track.optional') : t('island.startLevel')}
                  </span>
                </span>

                {/* A themed motif accent to make each track feel like its own world. */}
                <span aria-hidden="true" className="shrink-0 text-2xl opacity-80">
                  {theme.motif}
                </span>
              </button>
            </li>
          );
        })}
      </ol>
    </div>
  );
}
