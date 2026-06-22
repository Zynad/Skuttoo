import { test, expect, type Page } from '@playwright/test';

/**
 * The English island must teach in English even when the app's UI language is Swedish.
 *
 * Flow (mobile viewport, Swedish UI by default): open / -> English island -> first level's
 * "listen and pick" exercise -> the instruction/helper are Swedish, but the answer words are
 * English (apple, not äpple) -> read-aloud is invoked -> choosing "apple" earns a reward.
 *
 * Requires the backend on :5080 seeded with the reworked English content (instruction in the
 * UI language + English target word + English answer labels). Run by the human integrator.
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

test.describe('English island teaches in English', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('shows Swedish instructions but English words, and rewards the right pick', async ({ page }) => {
    await page.goto('/');

    await page.getByTestId('island-english').click();
    await expect(page).toHaveURL(/\/island\/english$/);

    // Enter the first level -> the listen-and-pick exercise.
    await page.getByTestId('progress-path').getByRole('button').first().click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('choices')).toBeVisible();

    // Instruction + helper stay in Swedish (the UI language).
    await expect(page.getByText(/Lyssna och välj/)).toBeVisible();
    await expect(page.getByText('Välj rätt svar')).toBeVisible();

    // The answer words are English — the bug fix.
    const choices = page.getByTestId('choices');
    await expect(choices.getByRole('button', { name: 'apple', exact: true })).toBeVisible();
    await expect(choices.getByRole('button', { name: 'äpple', exact: true })).toHaveCount(0);

    // Read-aloud is wired (audio is stubbed; we assert it was invoked).
    await page.getByTestId('audio-button').first().click();
    await expect
      .poll(() => page.evaluate(() => (window as unknown as { __spoke: string[] }).__spoke.length))
      .toBeGreaterThan(0);

    // Choosing the English word "apple" is correct -> reward.
    await choices.getByRole('button', { name: 'apple', exact: true }).click();
    await expect(page.getByTestId('reward-burst')).toBeVisible();
  });
});
