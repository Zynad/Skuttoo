# Testing strategy

Testing is a first-class requirement, on both frontend and backend, including end-to-end with Playwright. A change is **done only when its tests are written and green**.

## The pyramid

```
        ▲  few    E2E (Playwright)        real user flows, mobile viewport
       ███        Integration (backend)   API + EF/SQLite via WebApplicationFactory
      █████       Component (frontend)     React components/hooks via Testing Library
     ███████ many Unit (both)              pure logic, services, utils
```

Push logic down to fast unit tests; use integration/E2E for the seams and the flows that matter to a child.

## Backend

**Stack:** xUnit · NSubstitute (mocking) · Shouldly (assertions) · `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory`). One project: `backend/tests/Skuttoo.Tests`.

- **Unit** — application services and pure domain logic (e.g. answer validation, reward calculation, localization resolution). Dependencies faked with NSubstitute; assertions with Shouldly. No database, no HTTP.
- **Integration** — spin up the API with `WebApplicationFactory<Program>`, point EF Core at a **dedicated SQLite database** (a temp file or `Data Source=:memory:` kept open), seed known content, then exercise real endpoints:
  - `GET /api/subjects` returns the seeded islands in both locales.
  - `GET /api/exercises/{id}` returns prompt/choices/audio refs and **does not** leak which choice is correct.
  - `POST /api/exercises/{id}/attempt` returns `correct`, `correctChoiceId` and the `reward` for both a right and a wrong answer.
  - `/health/ready` returns healthy.
- **Conventions:** test names describe behaviour (`Attempt_with_correct_choice_returns_reward`). Arrange–Act–Assert. A shared fixture builds the factory + seeds; tests are isolated (fresh data per class). Make `Program` visible to tests via `public partial class Program {}`.
- **Run:** `cd backend && dotnet test`. Coverage via `coverlet.collector`.

## Frontend

**Stack:** Vitest · @testing-library/react · @testing-library/user-event · jsdom.

- **Component/hook tests** — render a component, interact with `userEvent`, assert on accessible output. Examples: `AudioButton` calls the speak service; an exercise renders its choices and reports the chosen one; the reward animation shows after a correct answer; `useProgress` persists to (mocked) IndexedDB.
- **i18n parity test** — assert `sv.json` and `en.json` have **identical key sets** (fails CI if a translation is missing).
- **Network** — mock `BaseApi`/axios; never hit a real backend in unit tests.
- **Conventions:** query by role/label/test id, not implementation details. Co-locate as `Component.test.tsx`. Avoid snapshot-only tests.
- **Run:** `cd frontend && npm test` (watch: `npm run test:watch`).

## End-to-end (Playwright)

**Stack:** `@playwright/test`, specs in `frontend/e2e`, config `frontend/playwright.config.ts`. Runs against the production-like build (frontend served by the .NET app) so it mirrors deployment.

- **Mobile-first:** default project uses a phone viewport (e.g. Pixel 5 / iPhone 12); add a desktop project as secondary.
- **Core MVP flows:**
  - Onboarding → world map renders with islands.
  - Enter Math island → a counting exercise appears; tapping the read-aloud button triggers audio (assert the audio/speech call).
  - Answer correctly → reward (coins/stars) shows; answer wrongly → gentle retry, no punishment.
  - Progress persists after reload; coins increased.
  - Offline: with the service worker active, a downloaded island still plays (added in sub-phase 1.9).
  - Language toggle swaps UI between Swedish and English.
- **Locators:** stable `data-testid` for game elements; otherwise role/label. **Web-first assertions** (`await expect(locator).toBeVisible()`) — never fixed `waitForTimeout` except a documented ≤500ms debounce.
- **Isolation:** each spec is independent; reset local storage/IndexedDB between tests. Audio is stubbed/inspected, not actually heard, in CI.
- **Run:** `cd frontend && npm run e2e` (UI mode: `npm run e2e:ui`).

## In CI (GitHub Actions)

On every push/PR: `dotnet build` + `dotnet test`; `npm ci` + `npm run lint` + `npm test`; build the app and run `npm run e2e` (Playwright browsers installed in CI). The pipeline blocks merge/deploy on any red. No secrets needed — TTS is stubbed and the DB is ephemeral SQLite in tests.

## What we deliberately don't do (yet)

- No load/performance testing until Phase 4.
- No visual-regression snapshots in MVP (revisit if the UI stabilizes).
- No testing of third-party internals (Azure Speech) — we test our wrapper and fallback, with the provider mocked.
