import { useNavigate } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useProgress } from '../hooks/useProgress';
import { CoinsBadge } from '../components/CoinsBadge';
import { LanguageToggle } from '../components/LanguageToggle';
import { ThemeToggle } from '../components/ThemeToggle';
import { IslandNode } from '../components/IslandNode';
import { MascotBubble } from '../components/MascotBubble';
import { SkyBackground } from '../components/SkyBackground';
import { ISLAND_ORDER, islandThemes, type IslandTheme } from '../utils/islandTheme';
import { islandProgress, nextIsland } from '../utils/progressSummary';

/** Home screen: the playful world map — a winding trail of subject islands, guided by Skutt. */
export function WorldMap() {
  const navigate = useNavigate();
  const t = useT();
  const { profile } = useProgress();

  const enterIsland = (theme: IslandTheme) => void navigate(`/island/${theme.key}`);

  const next = nextIsland(profile.results);
  const nextName = t(islandThemes[next].nameKey);
  const started = profile.results.length > 0;
  const guidance = started ? t('worldmap.next', { island: nextName }) : t('worldmap.mascotGreeting');

  return (
    <div className="min-h-full">
      <SkyBackground />

      <header className="flex items-center gap-2 px-4 pb-2 pt-[max(0.75rem,env(safe-area-inset-top))]">
        <CoinsBadge coins={profile.coins} stars={profile.stars} />
        <div className="ml-auto flex items-center gap-2">
          <button
            type="button"
            data-testid="profile-link"
            onClick={() => void navigate('/profile')}
            aria-label={t('nav.profile')}
            className="grid h-11 w-11 place-items-center rounded-full bg-[var(--color-surface-raised)] text-xl shadow-[var(--shadow-soft)] transition-transform active:scale-90"
          >
            <span aria-hidden="true">🦊</span>
          </button>
          <LanguageToggle />
          <ThemeToggle />
        </div>
      </header>

      <main className="mx-auto w-full max-w-2xl px-4 pb-16">
        <div className="pt-2 text-center">
          <h1 className="font-display text-5xl font-extrabold tracking-tight text-[var(--color-primary)] drop-shadow-sm">
            {t('worldmap.title')}
          </h1>
          <p className="mt-1 font-display text-lg font-bold text-[var(--color-text-soft)]">{t('worldmap.subtitle')}</p>
        </div>

        <div className="mt-5">
          <MascotBubble message={guidance} state="happy" withAudio mascotSize={96} />
        </div>

        {/* The trail: a winding path connecting the island stops, alternating sides. */}
        <div className="relative mt-8" data-testid="island-trail">
          <svg
            aria-hidden="true"
            className="absolute inset-0 h-full w-full"
            viewBox="0 0 100 100"
            preserveAspectRatio="none"
          >
            <path
              d="M28 11 C28 26 72 22 72 37 C72 52 28 48 28 63 C28 78 72 74 72 89"
              fill="none"
              stroke="var(--color-primary-soft)"
              strokeWidth={7}
              strokeLinecap="round"
              opacity={0.85}
            />
          </svg>

          <ol className="relative flex list-none flex-col gap-8 p-0">
            {ISLAND_ORDER.map((key, index) => (
              <li key={key} className={`w-[72%] ${index % 2 === 1 ? 'self-end' : 'self-start'}`}>
                <IslandNode
                  theme={islandThemes[key]}
                  progress={islandProgress(key, profile.results)}
                  isNext={key === next}
                  side={index % 2 === 1 ? 'right' : 'left'}
                  index={index}
                  onEnter={enterIsland}
                />
              </li>
            ))}
          </ol>
        </div>
      </main>
    </div>
  );
}
