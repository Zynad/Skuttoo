import { describe, expect, it, beforeEach, vi } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { WorldMap } from './WorldMap';
import { renderWithProviders } from '../test/renderWithProviders';
import { clearProfile, saveProfile } from '../services/progressStore';
import { createDefaultProfile } from '../types/progress';

describe('WorldMap', () => {
  beforeEach(async () => {
    await clearProfile();
    vi.stubGlobal(
      'fetch',
      vi.fn(() => Promise.resolve({ ok: false } as Response)),
    );
  });

  it('renders all four subject islands', () => {
    renderWithProviders(<WorldMap />);
    expect(screen.getByTestId('island-math')).toBeInTheDocument();
    expect(screen.getByTestId('island-swedish')).toBeInTheDocument();
    expect(screen.getByTestId('island-english')).toBeInTheDocument();
    expect(screen.getByTestId('island-logic')).toBeInTheDocument();
  });

  it('shows the coins/stars indicator', () => {
    renderWithProviders(<WorldMap />);
    expect(screen.getByTestId('coins-badge')).toBeInTheDocument();
  });

  it('switches UI text when toggling the language', async () => {
    renderWithProviders(<WorldMap />, { lang: 'sv' });
    expect(screen.getByTestId('island-math')).toHaveTextContent('Matte');

    await userEvent.click(screen.getByTestId('lang-en'));
    expect(screen.getByTestId('island-math')).toHaveTextContent('Math');
  });

  it('shows per-island star progress and guides the child to the next island', async () => {
    await saveProfile({
      ...createDefaultProfile(),
      coins: 10,
      stars: 3,
      results: [
        {
          exerciseId: 1,
          completed: true,
          starsEarned: 3,
          attempts: 1,
          lastPlayedAt: '2026-06-22T00:00:00.000Z',
          subjectKey: 'math',
          levelId: 1,
        },
      ],
    });

    renderWithProviders(<WorldMap />, { lang: 'sv' });

    // The Math node reflects the earned stars once the stored profile loads.
    await waitFor(() => expect(screen.getByTestId('island-math-stars')).toHaveTextContent('3'));

    // Math is started, so Skutt nudges the child to the first un-started island (Svenska).
    expect(screen.getByTestId('mascot-bubble')).toHaveTextContent('Svenska');
  });
});
