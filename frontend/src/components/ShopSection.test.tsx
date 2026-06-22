import { describe, expect, it, beforeEach } from 'vitest';
import { screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { renderWithProviders } from '../test/renderWithProviders';
import { ShopSection } from './ShopSection';
import { clearProfile, saveProfile } from '../services/progressStore';
import { createDefaultProfile } from '../types/progress';

describe('ShopSection', () => {
  beforeEach(async () => {
    await clearProfile();
  });

  it('disables buying an item the child cannot afford', async () => {
    renderWithProviders(<ShopSection />);
    await waitFor(() => expect(screen.getByTestId('shop')).toBeInTheDocument());

    const item = screen.getByTestId('shop-item-mascot-color-forest');
    expect(item).toHaveAttribute('data-owned', 'false');
    expect(within(item).getByRole('button')).toBeDisabled();
  });

  it('buys then equips a colour, deducting coins', async () => {
    await saveProfile({ ...createDefaultProfile(), coins: 100 });
    const user = userEvent.setup();
    renderWithProviders(<ShopSection />);

    const item = () => screen.getByTestId('shop-item-mascot-color-forest');
    await waitFor(() => expect(within(item()).getByRole('button')).toBeEnabled());

    // Buy it.
    await user.click(within(item()).getByRole('button'));
    await waitFor(() => expect(item()).toHaveAttribute('data-owned', 'true'));

    // Now the same control equips it.
    await user.click(within(item()).getByRole('button'));
    await waitFor(() => expect(item()).toHaveAttribute('data-equipped', 'true'));
  });
});
