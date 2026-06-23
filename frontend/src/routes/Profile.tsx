import { useState } from 'react';
import { useT } from '../i18n/useT';
import { useProgress } from '../hooks/useProgress';
import { TopBar } from '../components/TopBar';
import { SkyBackground } from '../components/SkyBackground';
import { Card } from '../components/Card';
import { Button } from '../components/Button';
import { AgePicker } from '../components/AgePicker';
import { MascotBubble } from '../components/MascotBubble';
import { StreakBadge } from '../components/StreakBadge';
import { BadgeGrid } from '../components/BadgeGrid';
import { ShopSection } from '../components/ShopSection';

/** Simple profile screen summarising local progress. */
export function Profile() {
  const t = useT();
  const { profile, setAge } = useProgress();
  const [editingAge, setEditingAge] = useState(false);
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
          <div className="flex items-center justify-center gap-3">
            <StreakBadge count={profile.streak.count} size="lg" />
            <p data-testid="profile-streak">{t('profile.streakLine', { count: profile.streak.count })}</p>
          </div>
        </Card>

        <Card className="mt-4">
          {editingAge ? (
            <AgePicker
              initialAge={profile.age}
              onConfirm={(age) => {
                void setAge(age);
                setEditingAge(false);
              }}
            />
          ) : (
            <div className="flex flex-wrap items-center justify-center gap-4">
              <p data-testid="profile-age" className="font-display text-lg font-bold text-[var(--color-text)]">
                {profile.age !== null ? t('profile.ageLine', { age: profile.age }) : ''}
              </p>
              <Button variant="secondary" data-testid="change-age" onClick={() => setEditingAge(true)}>
                {t('profile.changeAge')}
              </Button>
            </div>
          )}
        </Card>

        <section className="mt-8">
          <h2 className="text-center font-display text-xl font-extrabold text-[var(--color-text-soft)]">
            {t('badges.title')}
          </h2>
          <div className="mt-4">
            <BadgeGrid earnedKeys={profile.badgeKeys} />
          </div>
        </section>

        <div className="mt-10">
          <ShopSection />
        </div>
      </main>
    </div>
  );
}
