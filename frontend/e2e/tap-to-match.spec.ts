import { test, expect, type Page } from '@playwright/test';
import { ensureOnboarded, unlockBeforeIndex } from './helpers';

/**
 * Tap-to-match exercise (mobile viewport): open / -> English island -> second level ->
 * pair each word with its picture -> earn a reward; progress persists after reload.
 *
 * Requires the backend on :5080 seeded so the English island's second level's first exercise
 * is a tap-to-match of word↔picture (apple, banana). Run by the human integrator.
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

test.describe('tap-to-match exercise', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
    await ensureOnboarded(page);
  });

  test('child matches every word to its picture and earns a reward', async ({ page }) => {
    // "Match words" is the third node; unlock the earlier ones so it's playable.
    await unlockBeforeIndex(page, 'english', 2);
    const startingCoins = await coinsValue(page);

    await page.getByTestId('island-english').click();
    // Third stop on the path -> the matching level.
    await page.getByTestId('progress-path').getByRole('button').nth(2).click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);

    const grid = page.getByTestId('match-grid');
    await expect(grid).toBeVisible();

    // Pair each English word with its matching picture.
    await grid.getByRole('button', { name: 'apple', exact: true }).click();
    await grid.locator('button').filter({ has: page.locator('img[src*="apple"]') }).click();
    await grid.getByRole('button', { name: 'banana', exact: true }).click();
    await grid.locator('button').filter({ has: page.locator('img[src*="banana"]') }).click();

    // Reward shows and coins increase; progress persists across a reload.
    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);

    const earned = await coinsValue(page);
    await page.reload();
    await expect.poll(() => coinsValue(page)).toBe(earned);
  });
});
