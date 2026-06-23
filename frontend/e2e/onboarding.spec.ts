import { test, expect, type Page } from '@playwright/test';
import { stubSpeech } from './helpers';

/**
 * Exact-age onboarding, themed tracks and a reachable profile (this redesign phase).
 *
 * Flow (mobile viewport): a first run is gated by an age picker; the chosen age sets the starting
 * node per subject (older children skip the easiest, image-only nodes) and persists; the profile is
 * reachable from the hub and the age can be changed.
 *
 * Requires the backend on :5080 (proxied via /api) and seeded. Run by the human integrator.
 */

/** Drives the first-run flow: lands on the gate, picks an age, confirms, and waits for the hub. */
async function onboard(page: Page, age: number): Promise<void> {
  await page.goto('/');
  await expect(page.getByTestId('onboarding')).toBeVisible();
  await page.getByTestId(`age-option-${age}`).click();
  await page.getByTestId('age-confirm').click();
  await expect(page).toHaveURL('/');
  await expect(page.getByTestId('island-math')).toBeVisible();
}

test.describe('exact-age onboarding', () => {
  test.beforeEach(async ({ page, context }) => {
    await context.clearCookies();
    await stubSpeech(page);
  });

  test('first run is gated by the age picker and reads it aloud before any play', async ({ page }) => {
    await page.goto('/');

    await expect(page).toHaveURL(/\/onboarding$/);
    await expect(page.getByTestId('onboarding')).toBeVisible();
    // The hub is not reachable until an age is chosen.
    await expect(page.getByTestId('island-trail')).toHaveCount(0);
    // The greeting is read aloud (audio-first for pre-readers).
    await expect
      .poll(() => page.evaluate(() => (window as unknown as { __spoke: string[] }).__spoke.length))
      .toBeGreaterThan(0);
  });

  test('the chosen age persists — onboarding is skipped on the next visit', async ({ page }) => {
    await onboard(page, 6);
    await page.reload();
    await expect(page).toHaveURL('/');
    await expect(page.getByTestId('island-math')).toBeVisible();
  });

  test('a 9-year-old skips the easiest planets and sees the space metaphor', async ({ page }) => {
    await onboard(page, 9);

    await page.getByTestId('island-math').click();
    await expect(page).toHaveURL(/\/island\/math$/);
    await expect(page.getByTestId('progress-path')).toBeVisible();

    // The track uses the theme metaphor noun ("Planet") rather than a generic "level".
    await expect(page.getByText(/Planet/).first()).toBeVisible();
    // The easiest early nodes are optional warm-ups for an older child.
    await expect(page.locator('[data-state="optional"]').first()).toBeVisible();
  });

  test('a 3-year-old starts on the very first node with nothing optional', async ({ page }) => {
    await onboard(page, 3);

    await page.getByTestId('island-math').click();
    await expect(page.getByTestId('progress-path')).toBeVisible();
    await expect(page.locator('[data-state="optional"]')).toHaveCount(0);
    await expect(page.locator('[data-state="current"]').first()).toBeVisible();
  });

  test('the profile is reachable from the hub and the age can be changed', async ({ page }) => {
    await onboard(page, 7);

    await page.getByTestId('profile-link').click();
    await expect(page).toHaveURL(/\/profile$/);
    await expect(page.getByTestId('profile-age')).toContainText('7');

    // Change the age via the inline picker on the profile.
    await page.getByTestId('change-age').click();
    await page.getByTestId('age-option-5').click();
    await page.getByTestId('age-confirm').click();
    await expect(page.getByTestId('profile-age')).toContainText('5');

    // The new age persists across a reload.
    await page.reload();
    await expect(page.getByTestId('profile-age')).toContainText('5');
  });
});
