import { describe, expect, it, beforeEach } from 'vitest';
import { renderHook, act, waitFor } from '@testing-library/react';
import type { ReactNode } from 'react';
import { useProgress } from './useProgress';
import { ProgressProvider } from '../contexts/ProgressProvider';
import { clearProfile, loadProfile } from '../services/progressStore';
import type { AttemptResult } from '../types/content';

const wrapper = ({ children }: { children: ReactNode }) => <ProgressProvider>{children}</ProgressProvider>;

const correct: AttemptResult = { correct: true, correctChoiceId: 9, reward: { coins: 4, stars: 2 } };

describe('useProgress (IndexedDB persistence)', () => {
  beforeEach(async () => {
    await clearProfile();
  });

  it('starts at zero coins after load', async () => {
    const { result } = renderHook(() => useProgress(), { wrapper });
    await waitFor(() => expect(result.current.loading).toBe(false));
    expect(result.current.profile.coins).toBe(0);
  });

  it('persists awarded coins so they can be read back from the store', async () => {
    const { result } = renderHook(() => useProgress(), { wrapper });
    await waitFor(() => expect(result.current.loading).toBe(false));

    await act(async () => {
      await result.current.recordAttempt({ exerciseId: 42, attemptNumber: 1, result: correct });
    });

    expect(result.current.profile.coins).toBe(4);
    expect(result.current.profile.stars).toBe(2);

    // Read back directly from the IndexedDB-backed store.
    const reloaded = await loadProfile();
    expect(reloaded.coins).toBe(4);
    expect(reloaded.stars).toBe(2);
    expect(reloaded.results).toHaveLength(1);
    expect(reloaded.results[0].exerciseId).toBe(42);
  });

  it('reset wipes progress back to zero', async () => {
    const { result } = renderHook(() => useProgress(), { wrapper });
    await waitFor(() => expect(result.current.loading).toBe(false));

    await act(async () => {
      await result.current.recordAttempt({ exerciseId: 1, attemptNumber: 1, result: correct });
    });
    expect(result.current.profile.coins).toBe(4);

    await act(async () => {
      await result.current.reset();
    });
    expect(result.current.profile.coins).toBe(0);

    const reloaded = await loadProfile();
    expect(reloaded.coins).toBe(0);
  });
});
