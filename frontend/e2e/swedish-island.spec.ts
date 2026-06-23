import { test, expect, type Page } from '@playwright/test';
import { ensureOnboarded, unlockBeforeTitle } from './helpers';

/**
 * Swedish island slices (mobile viewport) for phase 1.4.
 *
 * Covers the literacy types added this phase: LetterSound (level "Bokstavsljud") and first reading
 * (level "Första läsningen"), both reusing the multiple-choice renderer. Swedish is a content-language
 * island: instructions stay in the UI language (default Swedish) while the taught word + picture
 * answers are Swedish. Audio is stubbed; we also assert read-aloud is invoked.
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

test.describe('swedish island slices', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
    await ensureOnboarded(page);
  });

  test('child hears a word and picks its first letter, earning a reward that persists', async ({ page }) => {
    // Letter sounds is a later node now; unlock the earlier ones so it's playable.
    await unlockBeforeTitle(page, 'swedish', /Bokstavsljud|Letter sounds/);
    await expect(page.getByTestId('island-swedish')).toBeVisible();

    const startingCoins = await coinsValue(page);

    await page.getByTestId('island-swedish').click();
    await expect(page).toHaveURL(/\/island\/swedish$/);

    // Enter the Letter sounds level ("Bokstavsljud") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Bokstavsljud|Letter sounds/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('choices')).toBeVisible();

    // Read-aloud is wired (audio is stubbed; we assert it was invoked).
    await expect
      .poll(() => page.evaluate(() => (window as unknown as { __spoke: string[] }).__spoke.length))
      .toBeGreaterThan(0);

    // "sol" starts with S.
    await page.getByTestId('choices').getByRole('button', { name: 'S', exact: true }).click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);

    const earnedCoins = await coinsValue(page);
    await page.reload();
    await expect.poll(() => coinsValue(page)).toBe(earnedCoins);
  });

  test('child reads a word and picks the matching picture', async ({ page }) => {
    // "Read the word" is a later node; complete the earlier ones so it is unlocked.
    await unlockBeforeTitle(page, 'swedish', /Läs ordet|Read the word/);
    await page.getByTestId('island-swedish').click();
    await expect(page).toHaveURL(/\/island\/swedish$/);

    const startingCoins = await coinsValue(page);

    // Enter the "Read the word" level ("Läs ordet") on the path.
    await page
      .getByTestId('progress-path')
      .getByRole('button', { name: /Läs ordet|Read the word/ })
      .click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);

    const choices = page.getByTestId('choices');
    await expect(choices).toBeVisible();

    // The word is "blomma" — pick the flower picture (image-only choice).
    await choices
      .locator('button')
      .filter({ has: page.locator('img[src*="pic-flower"]') })
      .click();

    await expect(page.getByTestId('reward-burst')).toBeVisible();
    await expect.poll(() => coinsValue(page)).toBeGreaterThan(startingCoins);
  });
});
