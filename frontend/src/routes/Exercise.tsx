import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useLanguage } from '../hooks/useLanguage';
import { useAsync } from '../hooks/useAsync';
import { useSpeak } from '../hooks/useSpeak';
import { contentApi } from '../services/contentApi';
import { TopBar } from '../components/TopBar';
import { SkyBackground } from '../components/SkyBackground';
import { Card } from '../components/Card';
import { AudioButton } from '../components/AudioButton';
import { Choice } from '../components/Choice';
import { Button } from '../components/Button';
import { RewardBurst } from '../components/RewardBurst';
import { MascotBubble } from '../components/MascotBubble';
import { LoadingState } from '../components/LoadingState';
import { ErrorState } from '../components/ErrorState';
import { useExerciseFlow } from '../game/useExerciseFlow';
import { successSound } from '../game/successSound';
import { imageUrl } from '../utils/assets';
import type { ChoiceStatus } from '../components/Choice';
import type { Exercise as ExerciseModel } from '../types/content';

interface ExerciseViewProps {
  exercise: ExerciseModel;
}

/** The interactive exercise once the data is loaded. */
function ExerciseView({ exercise }: ExerciseViewProps) {
  const t = useT();
  const { lang } = useLanguage();
  const navigate = useNavigate();
  const { say } = useSpeak();
  const flow = useExerciseFlow(exercise);

  const promptText = exercise.prompt[lang];
  const promptClip = exercise.promptAudio[lang];
  const img = imageUrl(exercise.imageRef);

  // Auto read the prompt aloud on first show (audio-first for pre-readers).
  useEffect(() => {
    void say(promptText, promptClip);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [exercise.id]);

  // Celebratory sound + spoken praise on a correct answer.
  useEffect(() => {
    if (flow.phase === 'correct') {
      successSound();
      void say(t('exercise.correct'));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [flow.phase]);

  const resolved = flow.phase === 'correct' || flow.phase === 'revealed';

  const statusFor = (choiceId: number): ChoiceStatus => {
    if (flow.correctChoiceId === choiceId && resolved) {
      return flow.phase === 'correct' ? 'correct' : 'revealed';
    }
    if (flow.phase === 'wrong' && flow.selectedChoiceId === choiceId) {
      return 'wrong';
    }
    if (resolved) {
      return 'dimmed';
    }
    return 'idle';
  };

  const mascotState =
    flow.phase === 'correct' ? 'happy' : flow.phase === 'wrong' || flow.phase === 'revealed' ? 'encouraging' : 'idle';

  const feedback =
    flow.phase === 'correct'
      ? t('exercise.correct')
      : flow.phase === 'revealed'
        ? t('exercise.revealed')
        : flow.phase === 'wrong'
          ? t('exercise.tryAgain')
          : null;

  const sortedChoices = [...exercise.choices].sort((a, b) => a.displayOrder - b.displayOrder);

  return (
    <main className="relative mx-auto w-full max-w-2xl px-4 pb-24">
      {flow.phase === 'correct' && <RewardBurst coins={flow.awardedCoins} stars={flow.awardedStars} />}

      <Card className="mt-2 text-center">
        <div className="flex items-center justify-center gap-3">
          <h1 className="font-display text-2xl font-extrabold text-[var(--color-text)]">{promptText}</h1>
          <AudioButton text={promptText} clip={promptClip} size="lg" />
        </div>
        {img && (
          <img
            src={img}
            alt=""
            data-testid="exercise-image"
            className="mx-auto mt-4 h-40 w-full max-w-xs object-contain"
          />
        )}
      </Card>

      <p className="mt-4 text-center font-display font-bold text-[var(--color-text-soft)]">
        {t('exercise.chooseAnswer')}
      </p>

      <section className="mt-3 grid grid-cols-2 gap-3" data-testid="choices">
        {sortedChoices.map((choice, index) => (
          <Choice
            key={choice.id}
            choice={choice}
            lang={lang}
            index={index}
            status={statusFor(choice.id)}
            disabled={flow.submitting || resolved}
            onSelect={(c) => void flow.choose(c)}
          />
        ))}
      </section>

      {feedback && (
        <div className="mt-6" data-testid="feedback" data-phase={flow.phase}>
          <MascotBubble message={feedback} state={mascotState} />
        </div>
      )}

      {resolved && (
        <div className="mt-6 flex justify-center">
          <Button size="lg" data-testid="continue-button" onClick={() => void navigate(-1)}>
            {t('exercise.continue')} →
          </Button>
        </div>
      )}
    </main>
  );
}

/** Route wrapper: loads the exercise by id, then renders the interactive view. */
export function Exercise() {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const t = useT();
  const exerciseId = Number(id);

  const {
    data: exercise,
    loading,
    error,
    reload,
  } = useAsync(() => {
    if (!Number.isFinite(exerciseId)) {
      return Promise.reject(new Error('invalid exercise id'));
    }
    return contentApi.getExercise(exerciseId);
  }, [exerciseId]);

  return (
    <div className="min-h-full">
      <SkyBackground />
      <TopBar showBack showLanguage={false} />
      {loading && <LoadingState />}
      {Boolean(error) && !loading && (
        <ErrorState message={t('exercise.error')} onRetry={reload} retryLabel={t('exercise.retry')} />
      )}
      {exercise && !loading && <ExerciseView key={exercise.id} exercise={exercise} />}
      {!exercise && !loading && !error && (
        <ErrorState message={t('exercise.error')} onRetry={() => void navigate('/')} retryLabel={t('nav.home')} />
      )}
    </div>
  );
}
