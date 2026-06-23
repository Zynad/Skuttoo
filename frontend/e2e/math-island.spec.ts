import { test, expect, type Page } from '@playwright/test';
import { unlockBeforeTitle } from './helpers';

/**
 * Math island vertical slices (mobile viewport) for phase 1.2.
 *
 * Covers the two exercise types added this phase that reuse the multiple-choice
 * renderer: SimpleAddition (level "Plus") and ShapeMatch (level "Former").
 *
 * Flow per test: open / -> Math island -> pick the target level on the path ->
 * exercise appears -> tap read-aloud (audio stubbed) -> answer correctly ->
 * reward shows + coins increase -> progress persists after reload.
 *
 * Requires the backend running on :5080 (proxied via /api) and seeded. Default
 * UI language is Swedish, and Math follows the UI language, so answer labels are
 * Swedish ("Cirkel"). Audio is stubbed (we assert SpeechSynthesis is INVOKED).
 */

async function stubSpeech(page: Page): Promise<void> {
  await page.addInitScript(() => {
    const w = window as unknown as { __spoke: string[] };
    w.__spoke = [];
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

test.describe('math island slices', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('child solves a simple addition, earns a reward, and progress persists', async ({ page }) => {
    // Addition is a later level; complete the earlier ones so it is unlocked.
    await unlockBeforeTitle(page, 'math', /Plus|Addition/);
    await expect(page.getByTestId('island-math')).toBeVisible();

    const startingCoins = await coinsValue(page);

    await page.getByTestId('island-math').click();
    await expect(page).toHaveURL(/\/island\/math$/);

    // Enter the Addition level ("Plus") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Plus|Addition/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('choices')).toBeVisible();

    // Tap read-aloud (audio is stubbed; we assert it was invoked).
    await page.getByTestId('audio-button').first().click();
    await expect
      .poll(() => page.evaluate(() => (window as unknown as { __spoke: string[] }).__spoke.length))
      .toBeGreaterThan(0);

    // 2 + 1 = 3.
    await page.getByTestId('choices').getByRole('button', { name: '3', exact: true }).click();

    const burst = page.getByTestId('reward-burst');
    await expect(burst).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);

    const earnedCoins = await coinsValue(page);

    // Progress persists across a reload (IndexedDB).
    await page.reload();
    await expect.poll(() => coinsValue(page)).toBe(earnedCoins);
  });

  // (Shapes moved to the Logic island in the 10×3 rebuild — see logic-island.spec.)
});
