# Roadmap

Work is split into **small, shippable, testable sub-phases**. Each sub-phase has a clear *Done when* so we can stop, review and control scope/token spend between steps. Ship a sub-phase, run its tests, then decide the next one.

Legend: ✅ done · 🔧 in progress · ⬜ todo.

---

## Phase 0 — Foundation (skeleton + one vertical slice)

| # | Sub-phase | Done when |
|---|---|---|
| 0.1 ✅ | **Planning docs** | `CLAUDE.md`, `README.md` and `docs/*` exist and agree on stack, conventions, data model and test strategy. |
| 0.2 ✅ | **Backend skeleton + tests** | `Skuttoo.sln` builds; Api serves `/health` and `/docs`; EF Core/SQLite wired with an initial migration; `dotnet test` green (a smoke integration test via `WebApplicationFactory`). |
| 0.3 ✅ | **Frontend skeleton + tests** | Vite/React/TS app builds and lints; Tailwind tokens + i18n (sv/en) + `BaseApi` in place; a placeholder world map renders; `npm test` green (incl. sv/en key-parity test). |
| 0.4 ✅ | **Vertical slice + E2E** | One counting exercise works end-to-end: seeded content → `GET /api/exercises/{id}` + `POST .../attempt` → exercise screen with TTS read-aloud, answer, reward animation, local progress. Backend integration test + Playwright mobile E2E green. |
| 0.5 ✅ | **Infra, Docker, CI/CD, secrets** | `Dockerfile` + `compose.yaml` run the full app; `infra/deploy.bicep` adds a second web app to the existing plan; `.github/workflows/deploy.yml` (OIDC) builds frontend→wwwroot→publish→deploy; `.gitignore`/`.env.example`/secret-scan in place; repo verified secret-free. |

**Exit criterion for Phase 0:** a kid can open the app on a phone, do a counting exercise read aloud, get a reward, and progress survives reload — all running locally and deployable with no secrets in the repo.

---

## Phase 1 — MVP (the four islands, fun and complete)

Each island sub-phase reuses the same engine and adds its exercise types + seeded content (bilingual + audio) + tests.

| # | Sub-phase | Done when |
|---|---|---|
| 1.1 ✅ | **Content engine & exercise framework** | Generic exercise runner supporting multiple exercise types (multiple-choice, tap-to-match, drag-to-bucket), difficulty/levels, seeded from DB; per-type unit + E2E. Also introduced **content language** (`Subject.ContentLanguage`) so language islands teach in the target language while instructions stay in the child's UI language. |
| 1.2 ✅ | **Math island** | Counting, number recognition, simple addition, shapes; 4 levels (tiers 1–3) spanning ages 3–9; seeded bilingual content with audio path references (real Azure TTS clips deferred to 1.8 — `SpeechSynthesis` fallback meanwhile); backend integration tests + Playwright E2E for the addition & shape flows. |
| 1.3 ⬜ | **Logic & shapes/colors island** | Sorting, patterns, colors, shapes — image-only, works for non-readers (3–5); tests. |
| 1.4 ⬜ | **Swedish island** | Letter sounds, picture↔word matching, first reading; leans on TTS; tests. |
| 1.5 ⬜ | **English island** | Listen-and-pick words/phrases; uses the bilingual setup; tests. |
| 1.6 ⬜ | **World map & navigation** | Mascot-guided map of islands with progress path; per-island themes; route + transitions; tests. |
| 1.7 ⬜ | **Gamification** | Coins, stars, badges, daily streak, simple reward shop / unlockables; all client-side for MVP; tests. |
| 1.8 ⬜ | **Mascot & audio polish** | Mascot reactions/animations, full pre-generated TTS for all content, `prefers-reduced-motion` respected; tests. |
| 1.9 ⬜ | **PWA & offline** | Installable; service worker caches app shell + content + audio; offline play of downloaded islands; tests. |
| 1.10 ⬜ | **Age-adaptation & onboarding** | Pick age band (3–5 / 6–9); UI text/density and audio-first behaviour adapt; lightweight first-run flow; tests. |
| 1.11 ⬜ | **Accessibility & polish pass** | Contrast, focus order, screen-reader labels, large tap targets verified; performance budget on mobile. |

**Exit criterion for MVP:** all four islands have enough quality content across difficulty levels, the map + gamification + audio make it genuinely fun, it installs and plays offline, and the full test suite (unit + integration + E2E) is green.

---

## Phase 2 — Parent accounts & cloud progress

| # | Sub-phase | Done when |
|---|---|---|
| 2.1 ⬜ | **Auth foundation** | ASP.NET Core Identity + JWT (self-contained); register/login parent; secrets via App Service settings; tests. |
| 2.2 ⬜ | **Child profiles** | A parent creates/edits child profiles (name, avatar, age band); tests. |
| 2.3 ⬜ | **Cloud progress sync** | Local progress model promoted to server; sync + conflict resolution; offline still works; tests. |
| 2.4 ⬜ | **Parent insight** | Simple dashboard: time, subjects, progress per child; tests. |
| 2.5 ⬜ | **GDPR & consent** | Data export/delete, consent flow, privacy copy; tests. |

---

## Phase 3 — Admin CMS

| # | Sub-phase | Done when |
|---|---|---|
| 3.1 ⬜ | **Content schema & admin auth** | Admin role; content versioning/draft model. |
| 3.2 ⬜ | **Authoring UI** | Create/edit islands, levels, exercises, choices, media in-app. |
| 3.3 ⬜ | **TTS generation in CMS** | Trigger Azure TTS generation for new content from the admin UI. |
| 3.4 ⬜ | **Publish workflow** | Draft → review → publish; preview. |
| 3.5 ⬜ | **(Optional) AI-assisted expansion** | Generate exercise variants with an LLM, **human-reviewed** before publish. |

---

## Phase 4 — Scale & native

| # | Sub-phase | Done when |
|---|---|---|
| 4.1 ⬜ | **SQLite → PostgreSQL** | Swap EF provider; migrate; load test. |
| 4.2 ⬜ | **Caching/CDN** | Cache content + serve audio/media from Blob/CDN. |
| 4.3 ⬜ | **Shared types package** | Extract TS types/contracts for reuse by a native client. |
| 4.4 ⬜ | **Native Android app** | React Native or MAUI client sharing the API; store release. |

---

## Working agreement

- One sub-phase at a time; finish its tests before moving on.
- Keep PRs small and focused; update the relevant `docs/*` when behaviour changes.
- Pause for review between sub-phases to keep scope and token spend in check.
