import { test, expect, type Page } from '@playwright/test';
import { ensureOnboarded } from './helpers';

/**
 * World-map navigation & progress (mobile viewport) for phase 1.6.
 *
 * The home screen is a winding trail of island nodes. Solving an exercise should be reflected
 * back on the map: the island node shows the stars earned there. Also exercises the framer-motion
 * route transitions — navigation across map → island → exercise → map must stay reliable.
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

function islandStars(page: Page, key: string): Promise<number> {
  return page
    .getByTestId(`island-${key}-stars`)
    .innerText()
    .then((text) => Number(text.replace(/\D+/g, '')) || 0);
}

test.describe('world map', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
    await ensureOnboarded(page);
  });

  test('shows the four island stops and guides the child with the mascot', async ({ page }) => {
    await page.goto('/');

    await expect(page.getByTestId('subject-grid')).toBeVisible();
    for (const key of ['math', 'swedish', 'english', 'logic']) {
      await expect(page.getByTestId(`island-${key}`)).toBeVisible();
    }
    // Skutt greets/guides from a speech bubble.
    await expect(page.getByTestId('mascot-bubble')).toBeVisible();
  });

  test('solving an exercise is reflected as stars on the island node', async ({ page }) => {
    await page.goto('/');
    expect(await islandStars(page, 'math')).toBe(0);

    // Map → Math island → first level → counting exercise (transitions in between).
    await page.getByTestId('island-math').click();
    await expect(page).toHaveURL(/\/island\/math$/);

    await page.getByTestId('progress-path').getByRole('button').first().click();
    await expect(page).toHaveURL(/\/exercise\/\d+$/);
    await expect(page.getByTestId('choices')).toBeVisible();

    // First counting exercise: the answer is 3.
    await page.getByTestId('choices').getByRole('button', { name: '3', exact: true }).click();
    await expect(page.getByTestId('reward-burst')).toBeVisible();

    // Back to the map — the Math node now shows the stars earned there.
    await page.goto('/');
    await expect(page.getByTestId('island-math')).toBeVisible();
    await expect.poll(() => islandStars(page, 'math')).toBeGreaterThan(0);
  });
});
