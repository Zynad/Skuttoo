import type { Level } from '../types/content';
import type { Lang } from '../i18n/dictionaries';
import type { IslandTheme } from '../utils/islandTheme';
import { useT } from '../i18n/useT';

export type LevelState = 'completed' | 'current' | 'locked';

export interface ProgressPathProps {
  levels: Level[];
  theme: IslandTheme;
  lang: Lang;
  /** Resolves the play state of each level. */
  stateOf: (level: Level, index: number) => LevelState;
  onSelectLevel: (level: Level) => void;
}

/** A vertical path of level "stops": completed = lit, current = highlighted, locked = dimmed. */
export function ProgressPath({ levels, theme, lang, stateOf, onSelectLevel }: ProgressPathProps) {
  const t = useT();

  return (
    <ol className="relative mx-auto flex max-w-md list-none flex-col gap-4 p-0" data-testid="progress-path">
      {levels.map((level, index) => {
        const state = stateOf(level, index);
        const locked = state === 'locked';
        const completed = state === 'completed';

        return (
          <li key={level.id} className="flex items-center gap-4">
            <span
              aria-hidden="true"
              className="grid h-12 w-12 shrink-0 place-items-center rounded-full font-display text-xl font-extrabold text-white"
              style={{
                background: locked ? 'var(--color-border)' : theme.color,
                opacity: locked ? 0.6 : 1,
              }}
            >
              {completed ? '★' : index + 1}
            </span>
            <button
              type="button"
              data-testid={`level-${level.id}`}
              data-state={state}
              disabled={locked}
              onClick={() => onSelectLevel(level)}
              className={
                `flex flex-1 items-center justify-between rounded-[1.4rem] border-2 px-4 py-3 text-left ` +
                `shadow-[var(--shadow-soft)] transition-transform enabled:active:scale-[0.98] ` +
                `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 ` +
                `focus-visible:ring-[color:var(--color-primary)] disabled:cursor-default disabled:opacity-50`
              }
              style={{
                borderColor: state === 'current' ? theme.color : 'var(--color-border)',
                background:
                  state === 'current'
                    ? `color-mix(in srgb, ${theme.colorSoft} 80%, var(--color-surface-raised))`
                    : 'var(--color-surface-raised)',
              }}
            >
              <span>
                <span className="block font-display text-lg font-extrabold text-[var(--color-text)]">
                  {level.title[lang]}
                </span>
                <span className="block text-sm font-semibold text-[var(--color-text-soft)]">
                  {t('island.level', { n: level.displayOrder })}
                </span>
              </span>
              <span className="font-display text-base font-bold" style={{ color: locked ? undefined : theme.color }}>
                {locked ? t('island.locked') : t('island.startLevel')}
              </span>
            </button>
          </li>
        );
      })}
    </ol>
  );
}
