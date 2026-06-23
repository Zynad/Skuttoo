import { useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useSpeak } from '../hooks/useSpeak';
import { useProgress } from '../hooks/useProgress';
import { SkyBackground } from '../components/SkyBackground';
import { MascotBubble } from '../components/MascotBubble';
import { AgePicker } from '../components/AgePicker';
import { LoadingState } from '../components/LoadingState';

/** First-run screen: Skutt greets the child and they pick their exact age (audio-first). */
export function Onboarding() {
  const t = useT();
  const navigate = useNavigate();
  const { say } = useSpeak();
  const { profile, loading, setAge } = useProgress();

  // Read the greeting + question aloud once the profile has resolved (audio-first for pre-readers).
  useEffect(() => {
    if (!loading && profile.age === null) {
      void say(t('onboarding.intro'));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [loading]);

  if (loading) {
    return (
      <div className="min-h-full">
        <SkyBackground />
        <LoadingState />
      </div>
    );
  }

  // Already onboarded (e.g. navigating to /onboarding directly): send them home.
  if (profile.age !== null) {
    return <Navigate to="/" replace />;
  }

  const confirm = async (age: number) => {
    await setAge(age);
    void navigate('/', { replace: true });
  };

  return (
    <div className="min-h-full" data-testid="onboarding">
      <SkyBackground />
      <main className="mx-auto flex w-full max-w-2xl flex-col px-4 pb-16 pt-[max(1rem,env(safe-area-inset-top))]">
        <h1 className="mt-4 text-center font-display text-4xl font-extrabold text-[var(--color-primary)]">
          {t('onboarding.title')}
        </h1>
        <div className="mt-5">
          <MascotBubble message={t('onboarding.intro')} state="happy" withAudio mascotSize={96} />
        </div>
        <div className="mt-8">
          <AgePicker onConfirm={(age) => void confirm(age)} />
        </div>
      </main>
    </div>
  );
}
