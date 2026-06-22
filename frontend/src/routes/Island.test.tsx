import { describe, expect, it, vi, beforeEach } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import { Routes, Route } from 'react-router-dom';
import { Island } from './Island';
import { renderWithProviders } from '../test/renderWithProviders';
import { contentApi } from '../services/contentApi';
import { clearProfile, saveProfile } from '../services/progressStore';
import { createDefaultProfile } from '../types/progress';
import type { SubjectDetail } from '../types/content';

const mathSubject: SubjectDetail = {
  id: 1,
  key: 'math',
  name: { sv: 'Matte', en: 'Math' },
  description: { sv: 'Räkna i rymden.', en: 'Count in space.' },
  themeKey: 'space',
  displayOrder: 1,
  contentLanguage: null,
  levels: [
    {
      id: 1,
      subjectId: 1,
      displayOrder: 1,
      title: { sv: 'Räkna', en: 'Counting' },
      difficultyTier: 1,
      ageMin: 3,
      ageMax: 6,
      exerciseIds: [10, 11],
    },
    {
      id: 2,
      subjectId: 1,
      displayOrder: 2,
      title: { sv: 'Tal', en: 'Numbers' },
      difficultyTier: 2,
      ageMin: 5,
      ageMax: 8,
      exerciseIds: [20],
    },
  ],
};

const renderIsland = () =>
  renderWithProviders(
    <Routes>
      <Route path="/island/:key" element={<Island />} />
    </Routes>,
    { route: '/island/math', lang: 'sv' },
  );

describe('Island progress path', () => {
  beforeEach(async () => {
    await clearProfile();
    vi.restoreAllMocks();
    vi.spyOn(contentApi, 'getSubject').mockResolvedValue(mathSubject);
    vi.stubGlobal(
      'fetch',
      vi.fn(() => Promise.resolve({ ok: false } as Response)),
    );
  });

  it('marks the first level current and the rest available with no progress', async () => {
    renderIsland();

    await waitFor(() => expect(screen.getByTestId('level-1')).toHaveAttribute('data-state', 'current'));
    expect(screen.getByTestId('level-2')).toHaveAttribute('data-state', 'available');
  });

  it('lights a completed level and moves current to the next one', async () => {
    await saveProfile({
      ...createDefaultProfile(),
      results: [10, 11].map((exerciseId) => ({
        exerciseId,
        completed: true,
        starsEarned: 3,
        attempts: 1,
        lastPlayedAt: '2026-06-22T00:00:00.000Z',
        subjectKey: 'math' as const,
        levelId: 1,
      })),
    });

    renderIsland();

    await waitFor(() => expect(screen.getByTestId('level-1')).toHaveAttribute('data-state', 'completed'));
    expect(screen.getByTestId('level-2')).toHaveAttribute('data-state', 'current');
  });
});
