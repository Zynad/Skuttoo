import { useT } from '../i18n/useT';
import { useProgress } from '../hooks/useProgress';
import { TopBar } from '../components/TopBar';
import { SkyBackground } from '../components/SkyBackground';
import { Card } from '../components/Card';
import { MascotBubble } from '../components/MascotBubble';

/** Simple profile screen summarising local progress. */
export function Profile() {
  const t = useT();
  const { profile } = useProgress();
  const hasProgress = profile.coins > 0 || profile.stars > 0 || profile.results.length > 0;

  return (
    <div className="min-h-full">
      <SkyBackground />
      <TopBar showBack />

      <main className="mx-auto w-full max-w-xl px-4 pb-16">
        <h1 className="mt-2 text-center font-display text-3xl font-extrabold text-[var(--color-primary)]">
          {t('profile.title')}
        </h1>

        <div className="mt-5">
          <MascotBubble
            message={hasProgress ? t('profile.coinsLine', { coins: profile.coins }) : t('profile.noProgress')}
            state={hasProgress ? 'happy' : 'idle'}
          />
        </div>

        <Card className="mt-6 space-y-3 text-center font-display text-lg font-bold text-[var(--color-text)]">
          <p data-testid="profile-coins">{t('profile.coinsLine', { coins: profile.coins })}</p>
          <p data-testid="profile-stars">{t('profile.starsLine', { stars: profile.stars })}</p>
          <p data-testid="profile-streak">{t('profile.streakLine', { count: profile.streak.count })}</p>
        </Card>
      </main>
    </div>
  );
}
