import { describe, expect, it } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import { renderWithProviders } from '../test/renderWithProviders';
import { BadgeGrid } from './BadgeGrid';

describe('BadgeGrid', () => {
  it('renders the catalogue with earned badges lit and the rest locked', async () => {
    renderWithProviders(<BadgeGrid earnedKeys={['first-hops']} />);

    // The fallback catalogue renders immediately (no network needed).
    await waitFor(() => expect(screen.getByTestId('badge-first-hops')).toBeInTheDocument());

    expect(screen.getByTestId('badge-first-hops')).toHaveAttribute('data-earned', 'true');
    expect(screen.getByTestId('badge-coin-master')).toHaveAttribute('data-earned', 'false');
    expect(screen.getByTestId('profile-badges')).toBeInTheDocument();
  });
});
