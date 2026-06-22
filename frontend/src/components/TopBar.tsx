import { useNavigate } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useProgress } from '../hooks/useProgress';
import { CoinsBadge } from './CoinsBadge';
import { LanguageToggle } from './LanguageToggle';
import { ThemeToggle } from './ThemeToggle';

export interface TopBarProps {
  /** Shows a back button that returns to the previous screen. */
  showBack?: boolean;
  /** Shows the language toggle (hidden on small in-exercise bars). */
  showLanguage?: boolean;
}

/** Sticky top bar with optional back, coins/stars, language + theme toggles. */
export function TopBar({ showBack = false, showLanguage = true }: TopBarProps) {
  const navigate = useNavigate();
  const t = useT();
  const { profile } = useProgress();

  return (
    <header className="sticky top-0 z-30 flex items-center gap-2 px-4 pb-2 pt-[max(0.75rem,env(safe-area-inset-top))]">
      {showBack ? (
        <button
          type="button"
          data-testid="back-button"
          onClick={() => void navigate(-1)}
          aria-label={t('nav.back')}
          className="grid h-11 w-11 place-items-center rounded-full bg-[var(--color-surface-raised)] text-xl shadow-[var(--shadow-soft)] transition-transform active:scale-90"
        >
          <span aria-hidden="true">←</span>
        </button>
      ) : (
        <button
          type="button"
          data-testid="home-button"
          onClick={() => void navigate('/')}
          aria-label={t('nav.home')}
          className="grid h-11 w-11 place-items-center rounded-full bg-[var(--color-surface-raised)] text-xl shadow-[var(--shadow-soft)] transition-transform active:scale-90"
        >
          <span aria-hidden="true">🏝️</span>
        </button>
      )}

      <CoinsBadge coins={profile.coins} stars={profile.stars} />

      <div className="ml-auto flex items-center gap-2">
        {showLanguage && <LanguageToggle />}
        <ThemeToggle />
      </div>
    </header>
  );
}
