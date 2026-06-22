import { test, expect } from '@playwright/test';
import { stubSpeech } from './helpers';

/**
 * Reduced-motion respect (sub-phase 1.8). With the OS preference set, the global CSS block
 * collapses every animation to ~0 so nothing loops or moves. Run by the human integrator.
 */

test.use({ reducedMotion: 'reduce' });

test.describe('reduced motion', () => {
  test.beforeEach(async ({ page }) => {
    await stubSpeech(page);
  });

  test('neutralises the mascot animation when the child prefers reduced motion', async ({ page }) => {
    await page.goto('/');
    const mascot = page.getByTestId('mascot').first();
    await expect(mascot).toBeVisible();

    // Normally the mascot bobs/hops for ~1–3s; reduced motion collapses it to ~0.001ms.
    const duration = await mascot.evaluate((el) => getComputedStyle(el).animationDuration);
    expect(parseFloat(duration)).toBeLessThan(0.01);
  });
});
