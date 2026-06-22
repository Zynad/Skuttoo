import { describe, expect, it, beforeEach, vi } from 'vitest';
import { screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { WorldMap } from './WorldMap';
import { renderWithProviders } from '../test/renderWithProviders';
import { clearProfile } from '../services/progressStore';

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
});
