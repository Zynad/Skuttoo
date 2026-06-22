import { useCallback, useState } from 'react';
import type { AttemptRequest, Exercise, Placement } from '../types/content';
import { contentApi, type ContentApi } from '../services/contentApi';
import { useProgress } from '../hooks/useProgress';

export type FlowPhase = 'answering' | 'correct' | 'wrong' | 'revealed';

const REVEAL_AFTER_ATTEMPTS = 2;

export interface ExerciseFlowState {
  phase: FlowPhase;
  /** 1-based number of the attempt currently being made / just made. */
  attemptNumber: number;
  /** Id of the chosen choice for the latest single-choice attempt (for styling). */
  selectedChoiceId: number | null;
  /** Id of the correct choice once known (correct answer or reveal); single-choice only. */
  correctChoiceId: number | null;
  /** The correct mapping once known (correct answer or reveal); generic types only. */
  correctPlacements: Placement[] | null;
  /** Coins awarded on the (first) correct solve. */
  awardedCoins: number;
  /** Stars awarded on the (first) correct solve. */
  awardedStars: number;
  /** Coins given for the first play of a new day (0 unless triggered by this exercise). */
  streakBonusCoins: number;
  /** Badge keys newly earned by this exercise (coin/streak badges), for a celebration. */
  newBadgeKeys: string[];
  submitting: boolean;
}

export interface UseExerciseFlowResult extends ExerciseFlowState {
  /** Submit an attempt (single choice or placements); updates phase, progress and rewards. */
  submit: (payload: AttemptRequest) => Promise<void>;
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
    correctPlacements: null,
    awardedCoins: 0,
    awardedStars: 0,
    streakBonusCoins: 0,
    newBadgeKeys: [],
    submitting: false,
  });

  const submit = useCallback(
    async (payload: AttemptRequest) => {
      // Ignore attempts once the exercise is resolved or while submitting.
      setState((prev) => {
        if (prev.phase === 'correct' || prev.phase === 'revealed' || prev.submitting) {
          return prev;
        }
        return { ...prev, submitting: true, selectedChoiceId: payload.choiceId ?? null };
      });

      const attemptNumber = state.attemptNumber + 1;

      try {
        const result = await api.submitAttempt(exercise.id, { ...payload, attemptNumber });
        const outcome = await recordAttempt({
          exerciseId: exercise.id,
          attemptNumber,
          result,
          subjectKey: exercise.subjectKey,
          levelId: exercise.levelId,
        });

        setState((prev) => {
          if (result.correct) {
            return {
              ...prev,
              phase: 'correct',
              attemptNumber,
              correctChoiceId: result.correctChoiceId,
              correctPlacements: result.correctPlacements ?? null,
              awardedCoins: outcome.awardedCoins,
              awardedStars: outcome.awardedStars,
              streakBonusCoins: prev.streakBonusCoins || outcome.streakBonusCoins,
              newBadgeKeys: outcome.newBadgeKeys.length ? outcome.newBadgeKeys : prev.newBadgeKeys,
              submitting: false,
            };
          }
          const reveal = attemptNumber >= REVEAL_AFTER_ATTEMPTS;
          return {
            ...prev,
            phase: reveal ? 'revealed' : 'wrong',
            attemptNumber,
            correctChoiceId: reveal ? result.correctChoiceId : null,
            correctPlacements: reveal ? (result.correctPlacements ?? null) : null,
            streakBonusCoins: prev.streakBonusCoins || outcome.streakBonusCoins,
            newBadgeKeys: outcome.newBadgeKeys.length ? outcome.newBadgeKeys : prev.newBadgeKeys,
            submitting: false,
          };
        });
      } catch {
        // Network/other failure: let the child try again, no punishment.
        setState((prev) => ({ ...prev, submitting: false, selectedChoiceId: null }));
      }
    },
    [api, exercise.id, exercise.subjectKey, exercise.levelId, recordAttempt, state.attemptNumber],
  );

  return { ...state, submit };
}
