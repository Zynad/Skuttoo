import { test, expect, type Page } from '@playwright/test';

/**
 * Logic island slices (mobile viewport) for phase 1.3.
 *
 * Covers the image-only types added this phase: ShapeMatch (level "Former") and PatternNext
 * (level "Mönster"), both reusing the multiple-choice renderer. Logic follows the UI language
 * (default Swedish), so shape answers carry Swedish labels ("Cirkel") and the pattern choices
 * are image-only swatches selected by their image. Audio is stubbed.
 *
 * Requires the backend on :5080 (proxied via /api) and seeded.
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

test.describe('logic island slices', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('child taps the matching shape and earns a reward, which persists', async ({ page }) => {
    await page.goto('/');
    await expect(page.getByTestId('island-logic')).toBeVisible();

    const startingCoins = await coinsValue(page);

    await page.getByTestId('island-logic').click();
    await expect(page).toHaveURL(/\/island\/logic$/);

    // Enter the Shapes level ("Former") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Former|Shapes/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('choices')).toBeVisible();

    // First shapes exercise asks to tap the circle ("Cirkel" in Swedish).
    await page.getByTestId('choices').getByRole('button', { name: 'Cirkel', exact: true }).click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);

    const earnedCoins = await coinsValue(page);

    // Progress persists across a reload (IndexedDB).
    await page.reload();
    await expect.poll(() => coinsValue(page)).toBe(earnedCoins);
  });

  test('child picks the next item in a colour pattern and earns a reward', async ({ page }) => {
    await page.goto('/');
    await page.getByTestId('island-logic').click();
    await expect(page).toHaveURL(/\/island\/logic$/);

    const startingCoins = await coinsValue(page);

    // Enter the Patterns level ("Mönster") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Mönster|Patterns/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);

    const choices = page.getByTestId('choices');
    await expect(choices).toBeVisible();

    // The pattern is red, blue, red, blue, ? — the next item is the red swatch (image-only choice).
    await choices
      .locator('button')
      .filter({ has: page.locator('img[src*="color-red"]') })
      .click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);
  });
});
