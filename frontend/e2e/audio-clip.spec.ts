import { test, expect } from '@playwright/test';
import { ensureOnboarded, stubSpeech } from './helpers';

/**
 * Pre-generated audio (sub-phase 1.8). The read-aloud is clip-first: the app probes the committed
 * mp3 path before falling back to SpeechSynthesis. This asserts the clip URL is actually requested
 * when an exercise loads. Run by the human integrator against the seeded backend; once the clips
 * are generated and committed, the request also resolves 200 and the clip plays.
 */

test.describe('pre-generated audio clips', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
    await ensureOnboarded(page);
  });

  test('requests a pre-generated clip for the exercise (clip-first strategy)', async ({ page }) => {
    await page.goto('/');
    await page.getByTestId('island-math').click();
    await expect(page).toHaveURL(/\/island\/math$/);

    // Arm the wait before the exercise mounts; its auto read-aloud probes the clip URL.
    const clipRequest = page.waitForRequest(/\/assets\/audio\/.*\.mp3$/);
    await page.getByTestId('progress-path').getByRole('button').first().click();
    await expect(page.getByTestId('choices')).toBeVisible();

    await clipRequest;
  });
});
