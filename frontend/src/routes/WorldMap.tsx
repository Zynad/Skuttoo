import { useNavigate } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useProgress } from '../hooks/useProgress';
import { CoinsBadge } from '../components/CoinsBadge';
import { LanguageToggle } from '../components/LanguageToggle';
import { ThemeToggle } from '../components/ThemeToggle';
import { IslandTile } from '../components/IslandTile';
import { MascotBubble } from '../components/MascotBubble';
import { SkyBackground } from '../components/SkyBackground';
import { ISLAND_ORDER, islandThemes, type IslandTheme } from '../utils/islandTheme';

/** Home screen: the playful world map of subject islands. */
export function WorldMap() {
  const navigate = useNavigate();
  const t = useT();
  const { profile } = useProgress();

  const enterIsland = (theme: IslandTheme) => void navigate(`/island/${theme.key}`);

  return (
    <div className="min-h-full">
      <SkyBackground />

      <header className="flex items-center gap-2 px-4 pb-2 pt-[max(0.75rem,env(safe-area-inset-top))]">
        <CoinsBadge coins={profile.coins} stars={profile.stars} />
        <div className="ml-auto flex items-center gap-2">
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
          <MascotBubble message={t('worldmap.mascotGreeting')} state="happy" withAudio mascotSize={96} />
        </div>

        <section className="mt-7 grid grid-cols-1 gap-4 sm:grid-cols-2" data-testid="island-grid">
          {ISLAND_ORDER.map((key, index) => (
            <IslandTile key={key} theme={islandThemes[key]} onEnter={enterIsland} index={index} />
          ))}
        </section>
      </main>
    </div>
  );
}
