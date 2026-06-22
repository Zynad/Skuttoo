import { useCallback, useState } from 'react';
import type { Choice, Exercise } from '../types/content';
import { contentApi, type ContentApi } from '../services/contentApi';
import { useProgress } from '../hooks/useProgress';

export type FlowPhase = 'answering' | 'correct' | 'wrong' | 'revealed';

const REVEAL_AFTER_ATTEMPTS = 2;

export interface ExerciseFlowState {
  phase: FlowPhase;
  /** 1-based number of the attempt currently being made / just made. */
  attemptNumber: number;
  /** Id of the chosen choice for the latest attempt (for styling). */
  selectedChoiceId: number | null;
  /** Id of the correct choice once known (correct answer or reveal). */
  correctChoiceId: number | null;
  /** Coins awarded on the (first) correct solve. */
  awardedCoins: number;
  /** Stars awarded on the (first) correct solve. */
  awardedStars: number;
  submitting: boolean;
}

export interface UseExerciseFlowResult extends ExerciseFlowState {
  /** Submit a choice; updates phase, progress and rewards. */
  choose: (choice: Choice) => Promise<void>;
}

/**
 * Drives a single exercise: submits attempts to the API, records progress, and
 * manages the gentle retry / reveal-after-two-tries behaviour. No punishment on
 * a wrong answer; rewards are awarded by useProgress on the first correct solve.
 */
export function useExerciseFlow(exercise: Exercise, api: ContentApi = contentApi): UseExerciseFlowResult {
  const { recordAttempt } = useProgress();
  const [state, setState] = useState<ExerciseFlowState>({
    phase: 'answering',
    attemptNumber: 0,
    selectedChoiceId: null,
    correctChoiceId: null,
    awardedCoins: 0,
    awardedStars: 0,
    submitting: false,
  });

  const choose = useCallback(
    async (choice: Choice) => {
      // Ignore taps once the exercise is resolved or while submitting.
      setState((prev) => {
        if (prev.phase === 'correct' || prev.phase === 'revealed' || prev.submitting) {
          return prev;
        }
        return { ...prev, submitting: true, selectedChoiceId: choice.id };
      });

      const attemptNumber = state.attemptNumber + 1;

      try {
        const result = await api.submitAttempt(exercise.id, { choiceId: choice.id, attemptNumber });
        const outcome = await recordAttempt({ exerciseId: exercise.id, attemptNumber, result });

        setState((prev) => {
          if (result.correct) {
            return {
              ...prev,
              phase: 'correct',
              attemptNumber,
              correctChoiceId: result.correctChoiceId,
              awardedCoins: outcome.awardedCoins,
              awardedStars: outcome.awardedStars,
              submitting: false,
            };
          }
          const reveal = attemptNumber >= REVEAL_AFTER_ATTEMPTS;
          return {
            ...prev,
            phase: reveal ? 'revealed' : 'wrong',
            attemptNumber,
            correctChoiceId: reveal ? result.correctChoiceId : null,
            submitting: false,
          };
        });
      } catch {
        // Network/other failure: let the child try again, no punishment.
        setState((prev) => ({ ...prev, submitting: false, selectedChoiceId: null }));
      }
    },
    [api, exercise.id, recordAttempt, state.attemptNumber],
  );

  return { ...state, choose };
}
