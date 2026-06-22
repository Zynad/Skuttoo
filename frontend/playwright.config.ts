import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright config for Skuttoo E2E. Mobile-first: the default project runs in a
 * Pixel 5 viewport. The dev server is started automatically against the Vite app
 * on http://localhost:5173 (which proxies /api to the .NET backend on :5080).
 *
 * NOTE: the counting flow needs the backend running and seeded; the human
 * integrator runs these once the API is live.
 */
export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: process.env.CI ? 'github' : 'list',
  use: {
    baseURL: 'http://localhost:5173',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
  },
  projects: [
    {
      name: 'mobile-chrome',
      use: { ...devices['Pixel 5'] },
    },
  ],
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:5173',
    reuseExistingServer: !process.env.CI,
    timeout: 120_000,
  },
});
