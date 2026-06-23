# Roadmap

Work is split into **small, shippable, testable sub-phases**. Each sub-phase has a clear *Done when* so we can stop, review and control scope/token spend between steps. Ship a sub-phase, run its tests, then decide the next one.

Legend: ✅ done · 🔧 in progress · ⬜ todo.

---

## Phase 0 — Foundation (skeleton + one vertical slice)

| # | Sub-phase | Done when |
|---|---|---|
| 0.1 ✅ | **Planning docs** | `CLAUDE.md`, `README.md` and `docs/*` exist and agree on stack, conventions, data model and test strategy. |
| 0.2 ✅ | **Backend skeleton + tests** | `Skuttoo.slnx` builds; Api serves `/health` and `/docs`; EF Core/SQLite wired with an initial migration; `dotnet test` green (a smoke integration test via `WebApplicationFactory`). |
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
| 1.2 ✅ | **Math island** | Counting, number recognition, simple addition, shapes; 5 levels (tiers 1–3) spanning ages 3–9; seeded bilingual content with audio path references (real Azure TTS clips deferred to 1.8 — `SpeechSynthesis` fallback meanwhile); backend integration tests + Playwright E2E for the addition & shape flows. (Enriched alongside 1.3–1.5.) |
| 1.3 ✅ | **Logic & shapes/colors island** | Colors, sorting, shapes and patterns across 5 levels — image-only, works for non-readers (3–6). `PatternNext` reuses single-choice (the sequence is a composite `pattern-*.svg`, choices are the candidate next swatches). Seed-integrity + integration tests + Playwright E2E (shape match, pattern next). |
| 1.4 ✅ | **Swedish island** | Picture↔word matching, letter sounds, tap-to-match and first reading across 5 levels (`ContentLanguage = Sv`); `LetterSound` reuses single-choice (hear the word, pick the starting letter). Integration tests + Playwright E2E (letter sound, first reading). |
| 1.5 ✅ | **English island** | Listen-and-pick words, matching, sorting and short phrases ("three apples") across 5 levels (`ContentLanguage = En`); phrases reuse the existing `apples-N` pictures. Integration tests + Playwright E2E (phrase pick, listen-and-pick word). |
| 1.6 ✅ | **World map & navigation** | Illustrated winding trail of island nodes (per-island themes + earned-star progress) with Skutt guiding the child to the next stop; the island path reflects real completion (completed/current/available, no locks — locking stays in 1.7); framer-motion route transitions honouring `prefers-reduced-motion`. Backend exposes per-level `exerciseIds`; progress is attributed by island/level. Unit + integration + Playwright E2E (map progress + transitions). |
| 1.7 ✅ | **Gamification** | Coins, stars, a scaling daily-streak bonus (with a 🔥 streak badge), DB-seeded **badges** (`GET /api/badges`, earned client-side via a pure `awardBadges`) shown in a profile gallery with an on-earn celebration, and a **reward shop** of cosmetics (mascot colours + accessories shown on Skutt, plus sticker/decoration collectibles) bought with coins. Progressive **level locking** (current + next playable, rest dimmed; islands stay open). All client-side; a `normalizeProfile` migration keeps older saves valid. Unit + integration + Playwright E2E. |
| 1.8 ⬜ | **Mascot & audio polish** | **Done:** richer Skutt reactions (talking/encouraging/celebrate motions + a celebrate state on badge earn), equipped cosmetics shown on the mascot, an audited `prefers-reduced-motion` (global CSS neutralises every animation; Playwright spec), and the full **TTS pipeline** — a standalone `Skuttoo.AudioGen` tool (`ClipPlanner` + `AudioGenerator` over an `ISpeechSynthesizer`, Azure-free + unit-tested; the planner found & fixed a real seed clip-path conflict; plans 113 clips). **Remaining (manual, one-time):** run the tool with an Azure Speech key to generate + commit the `.mp3`s, then the `audio-clip` E2E plays a real clip. New 1.10/1.12 content also relies on the `SpeechSynthesis` fallback until this clip generation is run. |
| 1.9 ⬜ | **PWA & offline** | Installable; service worker caches app shell + content + audio; offline play of downloaded islands; tests. |
| 1.10 ✅ | **Exact-age onboarding** | First-run is gated: until an age is set the app redirects to `/onboarding`, an audio-first age picker with big **3–9** buttons (Skutt reads the greeting). A `RequireAge` wrapper guards `/`, `/island/:key`, `/exercise/:id`, `/profile` and honours a loading flag so returning players never flash to onboarding. Age is **client-side only** (MVP stays anonymous — ADR-007); the local `Profile` gains `age: number \| null` (`ageBand` kept but **derived**: ≤5 → `3-5`, else `6-9`). The exact age sets the **starting node** per subject (`startNodeForAge` = first node whose `AgeMax ≥ age`; earlier nodes stay visible & playable as optional warm-ups, never hard-locked — relies on a non-decreasing `AgeMax` ladder, now test-enforced). `normalizeProfile` migrates a pre-age save **with** progress by inferring an age from its legacy band; age is changeable later from the Profile screen. Unit + integration + Playwright E2E. |
| 1.11 ✅ | **Per-subject themed tracks & reachable profile** | The world map is a **hub**; entering a subject opens its **own themed map**. Each track's node "metaphor noun" is derived client-side from `Subject.ThemeKey` (no backend field, no DB migration) via `islandTheme.ts`: Math (space) = *Planet {n}*, Swedish (forest) = *Glänta/Glade {n}*, English (travel) = *Resmål/Destination {n}*, Logic (jungle) = *Tempel/Temple {n}*. The previously-orphaned **profile** screen is now reachable from a profile button in the TopBar and the hub header. Unit + integration + Playwright E2E. |
| 1.12 ✅ | **Doubled content depth** | Append-only seed (no `Level` ids changed, no EF migration): Math 5 → **10** nodes, Logic/Swedish/English 5 → **9** nodes. New nodes add genuinely harder content for 6–9 (Math: teens, subtraction, skip-counting, bigger/smaller, two-digit addition incl. carry; Swedish: more letter sounds, blends, longer words, short sentences; English: more vocabulary, colours, food/animals, phrase-building; Logic: odd-one-out, harder patterns, order-by-size, what's-missing). **No new `ExerciseType`** — `SimpleAddition` is reused as the single-choice arithmetic type (now also subtraction/teens/two-digit/comparison). Existing Math nodes' `AgeMin/AgeMax` lightly retuned into a monotonic ladder; no node reordered or removed. New audio uses the `SpeechSynthesis` fallback until 1.8's clip generation runs. Seed-integrity + integration + Playwright E2E. |
| 1.13 ✅ | **Gameplay & content refinement** | Reworked each track to a uniform **10 nodes of 3 same-difficulty questions** (Logic/Swedish/English brought 9 → 10), difficulty rising between nodes. **Graded stars** (3/2/1 by attempt; reveal after the 3rd try) and a **star gate** — the next node unlocks at **≥ 4 stars, or when all 3 questions are solved** (`isLevelPassed`; the "all solved" fallback means no dead-ends). A node's questions are now **played in sequence** (resuming on the first unsolved — fixes a bug where multi-question nodes never completed, so progress was stuck). **Answer/match positions are shuffled** per exercise. The hub became a **plain 4-choice grid** (no single-subject nudge) and each subject a **themed node map** (planets/glades/destinations/temples). See **ADR-019**. Unit + integration + Playwright E2E. |
| 1.14 ⬜ | **Accessibility & polish pass** | Contrast, focus order, screen-reader labels, large tap targets verified; performance budget on mobile. |

> **Redesign note:** sub-phases 1.10–1.13 (exact-age onboarding, per-subject themed tracks + reachable profile, doubled content depth, and the gameplay/content refinement) shipped together as one cohesive redesign phase.

**Exit criterion for MVP:** all four islands have enough quality content across difficulty levels — including genuinely challenging material for 6–9 — reached through a hub of per-subject **themed tracks**, the map + gamification + audio make it genuinely fun, it installs and plays offline, and the full test suite (unit + integration + E2E) is green.

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
