import { test, expect, type Page } from '@playwright/test';

/**
 * Counting-exercise vertical slice (mobile viewport).
 *
 * Flow: open / -> world map with islands -> enter Math -> reach the first level's
 * counting exercise -> tap read-aloud -> choose "3" -> reward shows + coins
 * increase -> progress persists after reload.
 *
 * Requires the backend running on :5080 (proxied via /api) and seeded so that
 * Math's first level's first exercise is a "count to 3" question with a choice
 * labelled "3". The human integrator runs this once the API is live; it is not
 * expected to pass in an environment without the backend.
 *
 * Audio is stubbed (we assert SpeechSynthesis is INVOKED, never actually heard).
 */

async function stubSpeech(page: Page): Promise<void> {
  await page.addInitScript(() => {
    const w = window as unknown as { __spoke: string[] };
    w.__spoke = [];
    // Capture SpeechSynthesis usage without producing sound.
    window.speechSynthesis.speak = (utterance: SpeechSynthesisUtterance) => {
      w.__spoke.push(utterance.text);
    };
    window.speechSynthesis.cancel = () => undefined;
  });
}

function coinsValue(page: Page): Promise<number> {
  return page
    .getByTestId('coins')
    .innerText()
    .then((text) => Number(text.replace(/\D+/g, '')) || 0);
}

test.describe('counting exercise slice', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('child counts to three, earns a reward, and progress persists', async ({ page }) => {
    await page.goto('/');

    // World map renders with all four islands.
    await expect(page.getByTestId('island-math')).toBeVisible();
    await expect(page.getByTestId('island-swedish')).toBeVisible();
    await expect(page.getByTestId('island-english')).toBeVisible();
    await expect(page.getByTestId('island-logic')).toBeVisible();

    const startingCoins = await coinsValue(page);

    // Enter the Math island.
    await page.getByTestId('island-math').click();
    await expect(page).toHaveURL(/\/island\/math$/);

    // Start the first level on the path -> lands on the counting exercise.
    await page.getByTestId('progress-path').getByRole('button').first().click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('choices')).toBeVisible();

    // Tap read-aloud (audio is stubbed; we assert it was invoked).
    await page.getByTestId('audio-button').first().click();
    await expect
      .poll(() => page.evaluate(() => (window as unknown as { __spoke: string[] }).__spoke.length))
      .toBeGreaterThan(0);

    // Choose "3".
    await page.getByTestId('choices').getByRole('button', { name: '3', exact: true }).click();

    // Reward burst appears and coins increase.
    const burst = page.getByTestId('reward-burst');
    await expect(burst).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);

    const earnedCoins = await coinsValue(page);

    // Progress persists across a reload (IndexedDB).
    await page.reload();
    await expect.poll(() => coinsValue(page)).toBe(earnedCoins);
  });

  test('a wrong answer is gently handled with no coin loss', async ({ page }) => {
    await page.goto('/');
    await page.getByTestId('island-math').click();
    await page.getByTestId('progress-path').getByRole('button').first().click();
    await expect(page.getByTestId('choices')).toBeVisible();

    const startingCoins = await coinsValue(page);

    // Pick a wrong number (anything that isn't "3").
    await page.getByTestId('choices').getByRole('button', { name: '2', exact: true }).click();

    // Gentle retry feedback, no reward, coins unchanged.
    await expect(page.getByTestId('feedback')).toHaveAttribute('data-phase', 'wrong');
    await expect(page.getByTestId('reward-burst')).toHaveCount(0);
    expect(await coinsValue(page)).toBe(startingCoins);
  });
});
