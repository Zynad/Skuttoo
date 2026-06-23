import { useMemo } from 'react';
import { Choice, type ChoiceStatus } from '../../components/Choice';
import type { AttemptRequest, Exercise } from '../../types/content';
import type { Lang } from '../../i18n/dictionaries';
import type { FlowPhase } from '../../game/useExerciseFlow';
import { shuffle } from '../../utils/shuffle';

export interface MultipleChoiceExerciseProps {
  exercise: Exercise;
  /** Language the answer labels render in (target language on a language island). */
  contentLang: Lang;
  phase: FlowPhase;
  selectedChoiceId: number | null;
  correctChoiceId: number | null;
  disabled: boolean;
  onSubmit: (payload: AttemptRequest) => void;
}

/** The classic "pick one answer" exercise — tap a choice to submit it. */
export function MultipleChoiceExercise({
  exercise,
  contentLang,
  phase,
  selectedChoiceId,
  correctChoiceId,
  disabled,
  onSubmit,
}: MultipleChoiceExerciseProps) {
  const resolved = phase === 'correct' || phase === 'revealed';

  const statusFor = (choiceId: number): ChoiceStatus => {
    if (correctChoiceId === choiceId && resolved) {
      return phase === 'correct' ? 'correct' : 'revealed';
    }
    if (phase === 'wrong' && selectedChoiceId === choiceId) {
      return 'wrong';
    }
    if (resolved) {
      return 'dimmed';
    }
    return 'idle';
  };

  // Shuffle once per exercise so the correct answer isn't always in the same slot, but stays put
  // while the child is choosing.
  const choices = useMemo(() => shuffle(exercise.choices), [exercise.choices]);

  return (
    <section className="grid grid-cols-2 gap-3" data-testid="choices">
      {choices.map((choice, index) => (
        <Choice
          key={choice.id}
          choice={choice}
          lang={contentLang}
          index={index}
          status={statusFor(choice.id)}
          disabled={disabled}
          onSelect={(c) => onSubmit({ choiceId: c.id })}
        />
      ))}
    </section>
  );
}
