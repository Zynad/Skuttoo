import { test, expect, type Page } from '@playwright/test';

/**
 * English island slices (mobile viewport) for phase 1.5.
 *
 * Covers the listen-and-pick content added this phase: a "Listen and pick" word level and a
 * "Short phrases" level (countable phrases like "three apples", depicted with the apples-N
 * pictures). English is a content-language island: instructions stay in the UI language
 * (default Swedish) while the taught word/phrase and picture answers are English. Audio is stubbed.
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

test.describe('english island slices', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('child listens to a short phrase and picks the right picture, earning a reward that persists', async ({
    page,
  }) => {
    await page.goto('/');
    await expect(page.getByTestId('island-english')).toBeVisible();

    const startingCoins = await coinsValue(page);

    await page.getByTestId('island-english').click();
    await expect(page).toHaveURL(/\/island\/english$/);

    // Enter the Short phrases level ("Korta fraser") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Korta fraser|Short phrases/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);

    const choices = page.getByTestId('choices');
    await expect(choices).toBeVisible();

    // Read-aloud is wired (audio is stubbed; we assert it was invoked).
    await expect
      .poll(() => page.evaluate(() => (window as unknown as { __spoke: string[] }).__spoke.length))
      .toBeGreaterThan(0);

    // The phrase is "three apples" — pick the picture of three apples (image-only choice).
    await choices
      .locator('button')
      .filter({ has: page.locator('img[src*="apples-3"]') })
      .click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);

    const earnedCoins = await coinsValue(page);
    await page.reload();
    await expect.poll(() => coinsValue(page)).toBe(earnedCoins);
  });

  test('child listens and picks an English word, keeping Swedish instructions', async ({ page }) => {
    await page.goto('/');
    await page.getByTestId('island-english').click();
    await expect(page).toHaveURL(/\/island\/english$/);

    const startingCoins = await coinsValue(page);

    // Enter the "Listen and pick" level ("Lyssna och välj") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Lyssna och välj|Listen and pick/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);

    const choices = page.getByTestId('choices');
    await expect(choices).toBeVisible();

    // Instruction stays in Swedish (the UI language); the answer words are English.
    await expect(page.getByText(/Lyssna och välj/)).toBeVisible();
    await expect(choices.getByRole('button', { name: 'fish', exact: true })).toBeVisible();

    // "fish" is the correct word.
    await choices.getByRole('button', { name: 'fish', exact: true }).click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);
  });
});
