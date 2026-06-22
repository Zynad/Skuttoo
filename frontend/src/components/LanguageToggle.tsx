import { useLanguage } from '../hooks/useLanguage';
import { useT } from '../i18n/useT';
import type { Lang } from '../i18n/dictionaries';

export interface LanguageToggleProps {
  className?: string;
}

const flags: Record<Lang, string> = { sv: '🇸🇪', en: '🇬🇧' };

/** Pill toggle between Swedish and English. */
export function LanguageToggle({ className = '' }: LanguageToggleProps) {
  const { lang, setLang } = useLanguage();
  const t = useT();

  return (
    <div
      role="group"
      aria-label={t('lang.toggle')}
      data-testid="language-toggle"
      className={`flex items-center rounded-full bg-[var(--color-surface-raised)] p-1 shadow-[var(--shadow-soft)] ${className}`.trim()}
    >
      {(['sv', 'en'] as const).map((option) => {
        const active = lang === option;
        return (
          <button
            key={option}
            type="button"
            data-testid={`lang-${option}`}
            aria-pressed={active}
            onClick={() => setLang(option)}
            className={
              `flex min-h-[40px] items-center gap-1.5 rounded-full px-3 font-display text-base font-bold transition-colors ` +
              `focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] ` +
              (active
                ? 'bg-[var(--color-primary)] text-white shadow-[var(--shadow-soft)]'
                : 'text-[var(--color-text-soft)]')
            }
          >
            <span aria-hidden="true">{flags[option]}</span>
            <span>{t(option === 'sv' ? 'lang.sv' : 'lang.en')}</span>
          </button>
        );
      })}
    </div>
  );
}
