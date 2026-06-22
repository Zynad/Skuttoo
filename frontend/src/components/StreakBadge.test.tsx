import { describe, expect, it } from 'vitest';
import { screen } from '@testing-library/react';
import { renderWithProviders } from '../test/renderWithProviders';
import { StreakBadge } from './StreakBadge';

describe('StreakBadge', () => {
  it('shows the day count and an accessible label', () => {
    renderWithProviders(<StreakBadge count={4} />);
    const badge = screen.getByTestId('streak-badge');
    expect(badge).toHaveTextContent('4');
    expect(badge).toHaveAttribute('data-count', '4');
    expect(badge).toHaveAttribute('aria-label', expect.stringContaining('4'));
  });

  it('renders a larger variant without changing the count', () => {
    renderWithProviders(<StreakBadge count={7} size="lg" />);
    expect(screen.getByTestId('streak-badge')).toHaveTextContent('7');
  });
});
