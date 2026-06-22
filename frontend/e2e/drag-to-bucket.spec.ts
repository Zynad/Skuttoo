import { test, expect, type Page } from '@playwright/test';
import { unlockBeforeIndex } from './helpers';

/**
 * Drag-to-bucket exercise via tap-to-place (mobile viewport): open / -> English island ->
 * third level -> sort each word into the right box -> earn a reward.
 *
 * Requires the backend on :5080 seeded so the English island's third level's first exercise
 * is a drag-to-bucket sorting words into Fruit / Vehicle. Run by the human integrator.
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

test.describe('drag-to-bucket exercise', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('child sorts the words into the correct buckets and earns a reward', async ({ page }) => {
    // The sorting level is the third stop; complete the earlier ones so it is unlocked.
    await unlockBeforeIndex(page, 'english', 2);
    const startingCoins = await coinsValue(page);

    await page.getByTestId('island-english').click();
    // Third stop on the path -> the sorting level.
    await page.getByTestId('progress-path').getByRole('button').nth(2).click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('bucket-tray')).toBeVisible();

    const tray = page.getByTestId('bucket-tray');

    // Tap an item to pick it up, then tap its bucket to drop it.
    await tray.getByRole('button', { name: 'apple', exact: true }).click();
    await page.getByTestId('bucket-fruit').click();

    await tray.getByRole('button', { name: 'banana', exact: true }).click();
    await page.getByTestId('bucket-fruit').click();

    await tray.getByRole('button', { name: 'car', exact: true }).click();
    await page.getByTestId('bucket-vehicle').click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);
  });
});
