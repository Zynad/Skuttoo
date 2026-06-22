import { useCallback } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useLanguage } from '../hooks/useLanguage';
import { useProgress } from '../hooks/useProgress';
import { useAsync } from '../hooks/useAsync';
import { contentApi } from '../services/contentApi';
import { TopBar } from '../components/TopBar';
import { SkyBackground } from '../components/SkyBackground';
import { MascotBubble } from '../components/MascotBubble';
import { ProgressPath } from '../components/ProgressPath';
import { Button } from '../components/Button';
import { LoadingState } from '../components/LoadingState';
import { ErrorState } from '../components/ErrorState';
import { isSubjectKey, islandThemes } from '../utils/islandTheme';
import { levelStates } from '../utils/progressSummary';
import type { Level } from '../types/content';

/** Subject island: shows the level path. Entering a level opens its first exercise. */
export function Island() {
  const { key = '' } = useParams();
  const navigate = useNavigate();
  const t = useT();
  const { lang } = useLanguage();
  const { profile } = useProgress();

  const validKey = isSubjectKey(key) ? key : null;

  const {
    data: subject,
    loading,
    error,
    reload,
  } = useAsync(() => {
    if (!validKey) {
      return Promise.reject(new Error('unknown island'));
    }
    return contentApi.getSubject(validKey);
  }, [validKey]);

  const openLevel = useCallback(
    async (level: Level) => {
      const detail = await contentApi.getLevel(level.id);
      const first = [...detail.exercises].sort((a, b) => a.displayOrder - b.displayOrder)[0];
      if (first) {
        void navigate(`/exercise/${first.id}`);
      }
    },
    [navigate],
  );

  if (!validKey) {
    return (
      <div className="min-h-full">
        <SkyBackground />
        <TopBar showBack />
        <ErrorState message={t('notfound.title')} onRetry={() => void navigate('/')} retryLabel={t('notfound.back')} />
      </div>
    );
  }

  const theme = islandThemes[validKey];

  // Real progress: completed levels are lit, the first unsolved one is current, the rest available.
  // Everything stays playable — progressive locking/unlocking arrives with gamification (1.7).
  const sortedLevels = subject ? [...subject.levels].sort((a, b) => a.displayOrder - b.displayOrder) : [];
  const states = levelStates(sortedLevels, profile.results);

  return (
    <div className="min-h-full">
      <SkyBackground />
      <TopBar showBack />

      <main className="mx-auto w-full max-w-2xl px-4 pb-16">
        {loading && <LoadingState />}
        {Boolean(error) && !loading && (
          <ErrorState message={t('exercise.error')} onRetry={reload} retryLabel={t('exercise.retry')} />
        )}

        {subject && !loading && (
          <>
            <div className="mt-2 text-center">
              <span aria-hidden="true" className="text-5xl">
                {theme.motif}
              </span>
              <h1 className="font-display text-4xl font-extrabold" style={{ color: theme.color }}>
                {subject.name[lang]}
              </h1>
            </div>

            <div className="mt-4">
              <MascotBubble message={subject.description[lang]} state="talking" withAudio />
            </div>

            <h2 className="mt-7 text-center font-display text-xl font-extrabold text-[var(--color-text-soft)]">
              {t('island.levelsTitle')}
            </h2>

            <div className="mt-4">
              {subject.levels.length > 0 ? (
                <ProgressPath
                  levels={sortedLevels}
                  theme={theme}
                  lang={lang}
                  stateOf={(_level, index) => states[index]}
                  onSelectLevel={(level) => void openLevel(level)}
                />
              ) : (
                <p className="text-center font-semibold text-[var(--color-text-soft)]">{t('island.locked')}</p>
              )}
            </div>

            <div className="mt-8 text-center">
              <Button variant="ghost" onClick={() => void navigate('/')}>
                ← {t('nav.home')}
              </Button>
            </div>

            <p className="sr-only">{t('stars.aria', { count: profile.stars })}</p>
          </>
        )}
      </main>
    </div>
  );
}
