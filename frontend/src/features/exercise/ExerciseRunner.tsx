import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useT } from '../../i18n/useT';
import { useLanguage } from '../../hooks/useLanguage';
import { useSpeak } from '../../hooks/useSpeak';
import { contentLangFor } from '../../i18n/contentLang';
import { interactionFor } from '../../game/interactionKind';
import { useExerciseFlow } from '../../game/useExerciseFlow';
import { successSound } from '../../game/successSound';
import { imageUrl } from '../../utils/assets';
import { Card } from '../../components/Card';
import { Button } from '../../components/Button';
import { AudioButton } from '../../components/AudioButton';
import { RewardBurst } from '../../components/RewardBurst';
import { MascotBubble } from '../../components/MascotBubble';
import { BadgeCelebration } from '../../components/BadgeCelebration';
import { MultipleChoiceExercise } from './MultipleChoiceExercise';
import { TapToMatchExercise } from './TapToMatchExercise';
import { DragToBucketExercise } from './DragToBucketExercise';
import type { Exercise as ExerciseModel } from '../../types/content';

export interface ExerciseRunnerProps {
  exercise: ExerciseModel;
}

/**
 * Shared exercise chrome (instruction, audio, feedback, reward) that dispatches to the
 * right interaction renderer. Instructions/praise stay in the UI language; the taught word
 * and answer labels render in the subject's content language.
 */
export function ExerciseRunner({ exercise }: ExerciseRunnerProps) {
  const t = useT();
  const { lang: uiLang } = useLanguage();
  const navigate = useNavigate();
  const { say } = useSpeak();
  const flow = useExerciseFlow(exercise);

  const contentLang = contentLangFor(exercise.contentLanguage, uiLang);
  const kind = interactionFor(exercise.type);

  const promptText = exercise.prompt[uiLang];
  const promptClip = exercise.promptAudio[uiLang];
  const targetText = exercise.target ? exercise.target[contentLang] : null;
  const targetClip = exercise.targetAudio ? exercise.targetAudio[contentLang] : null;
  const img = imageUrl(exercise.imageRef);

  // Auto read-aloud on first show: the taught word (content language) when present,
  // otherwise the instruction (UI language). Audio-first for pre-readers.
  useEffect(() => {
    if (targetText) {
      void say(targetText, { clip: targetClip, lang: contentLang });
    } else {
      void say(promptText, { clip: promptClip, lang: uiLang });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [exercise.id]);

  // Celebratory sound + spoken praise (UI language) on a correct answer.
  useEffect(() => {
    if (flow.phase === 'correct') {
      successSound();
      void say(t('exercise.correct'));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [flow.phase]);

  const resolved = flow.phase === 'correct' || flow.phase === 'revealed';
  const disabled = flow.submitting || resolved;

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

  const helper =
    kind === 'tap-to-match'
      ? t('exercise.matchPairs')
      : kind === 'drag-to-bucket'
        ? t('exercise.dragToBuckets')
        : t('exercise.chooseAnswer');

  return (
    <main className="relative mx-auto w-full max-w-2xl px-4 pb-24">
      {flow.phase === 'correct' && <RewardBurst coins={flow.awardedCoins} stars={flow.awardedStars} />}
      <BadgeCelebration badgeKeys={flow.newBadgeKeys} />

      <Card className="mt-2 text-center">
        <div className="flex items-center justify-center gap-3">
          <h1 className="font-display text-2xl font-extrabold text-[var(--color-text)]">{promptText}</h1>
          <AudioButton text={promptText} clip={promptClip} lang={uiLang} size={targetText ? 'md' : 'lg'} />
        </div>
        {targetText && (
          <div className="mt-4 flex justify-center">
            <AudioButton
              text={targetText}
              clip={targetClip}
              lang={contentLang}
              label={t('exercise.listenWord')}
              size="lg"
            />
          </div>
        )}
        {img && (
          <img
            src={img}
            alt=""
            data-testid="exercise-image"
            className="mx-auto mt-4 h-40 w-full max-w-xs object-contain"
          />
        )}
      </Card>

      <p className="mt-4 text-center font-display font-bold text-[var(--color-text-soft)]">{helper}</p>

      {flow.streakBonusCoins > 0 && (
        <p
          className="mt-2 text-center font-display font-bold text-[var(--color-coin-edge)]"
          role="status"
          data-testid="streak-bonus"
        >
          🔥 {t('streak.bonus', { coins: flow.streakBonusCoins })}
        </p>
      )}

      <div className="mt-3">
        {kind === 'multiple-choice' && (
          <MultipleChoiceExercise
            exercise={exercise}
            contentLang={contentLang}
            phase={flow.phase}
            selectedChoiceId={flow.selectedChoiceId}
            correctChoiceId={flow.correctChoiceId}
            disabled={disabled}
            onSubmit={(payload) => void flow.submit(payload)}
          />
        )}
        {kind === 'tap-to-match' && (
          <TapToMatchExercise
            exercise={exercise}
            contentLang={contentLang}
            phase={flow.phase}
            correctPlacements={flow.correctPlacements}
            disabled={disabled}
            onSubmit={(payload) => void flow.submit(payload)}
          />
        )}
        {kind === 'drag-to-bucket' && (
          <DragToBucketExercise
            exercise={exercise}
            contentLang={contentLang}
            phase={flow.phase}
            correctPlacements={flow.correctPlacements}
            disabled={disabled}
            onSubmit={(payload) => void flow.submit(payload)}
          />
        )}
      </div>

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
