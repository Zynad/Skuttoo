import { test, expect } from '@playwright/test';
import { stubSpeech, coinsValue, completeLevel, seedProfile } from './helpers';

/**
 * Gamification slice (mobile viewport) for phase 1.7: badges, the daily streak, and the reward shop.
 *
 * Requires the backend on :5080 (proxied via /api) and seeded. Run by the human integrator.
 * Audio is stubbed.
 */

test.describe('gamification', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('earns a badge for completing a level and the shop purchase persists', async ({ page }) => {
    // Put the child in a state where finishing Math level 1 earns "first hops", with coins to spend.
    await completeLevel(page, 'math', 1);
    await seedProfile(page, { coins: 100 });

    // Visiting the island evaluates the structural badges → a celebration appears.
    await page.goto('/island/math');
    await expect(page.getByTestId('badge-celebration')).toBeVisible();

    // The badge gallery on the profile shows it as earned.
    await page.goto('/profile');
    await expect(page.getByTestId('badge-first-hops')).toHaveAttribute('data-earned', 'true');

    // Buy a cheap sticker from the shop; coins go down and it is now owned.
    const coinsBefore = await coinsValue(page);
    const sticker = page.getByTestId('shop-item-sticker-star');
    await sticker.scrollIntoViewIfNeeded();
    await sticker.getByRole('button').click();
    await expect(sticker).toHaveAttribute('data-owned', 'true');
    await expect.poll(() => coinsValue(page)).toBeLessThan(coinsBefore);

    // Both the badge and the purchase survive a reload (IndexedDB).
    await page.reload();
    await expect(page.getByTestId('badge-first-hops')).toHaveAttribute('data-earned', 'true');
    await expect(page.getByTestId('shop-item-sticker-star')).toHaveAttribute('data-owned', 'true');
  });

  test('shows the daily streak after the first play of the day', async ({ page }) => {
    await page.goto('/');
    await expect(page.getByTestId('streak-badge')).toHaveCount(0); // hidden at zero

    await page.getByTestId('island-math').click();
    await page.getByTestId('progress-path').getByRole('button').first().click();
    await expect(page.getByTestId('choices')).toBeVisible();
    await page.getByTestId('choices').getByRole('button', { name: '3', exact: true }).click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    // Playing today starts the streak, so the 🔥 badge now appears.
    await expect(page.getByTestId('streak-badge')).toBeVisible();
  });
});
