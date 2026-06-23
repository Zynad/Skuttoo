# Decision log (ADR)

Lightweight record of the key decisions and *why*, so future changes are informed. Newest decisions can be appended; supersede rather than delete.

Status: all **Accepted** (2026-06-21) unless noted.

---

### ADR-001 — Single repo, frontend served by the backend
**Decision:** One public GitHub repo with `/backend` + `/frontend`; the .NET app serves the built React SPA from `wwwroot`.
**Why:** Simplest for a solo project; one deploy, one origin (no CORS), no separate frontend hosting → near-zero cost. **Trade-off:** frontend and backend version together (fine at this scale).

### ADR-002 — Stack: React 19 + Vite + TS (strict) / .NET 10 + Controllers
**Decision:** Match the conventions used across `D:\TBRepos` (Vite/React/TS strict; .NET Controllers, EF Core, Mapperly, xUnit) but stay self-contained.
**Why:** Familiar, proven, consistent with the author's other work; no private Triggerbee dependencies so the repo can be public.

### ADR-003 — SQLite first, PostgreSQL later
**Decision:** EF Core on SQLite for the MVP; documented migration path to PostgreSQL.
**Why:** Zero DB infrastructure/cost; a file on the App Service persistent disk. EF makes the later swap cheap. **Trade-off:** SQLite concurrency limits — acceptable for low-traffic MVP; revisit in Phase 4.

### ADR-004 — Public repo ⇒ no secrets in Git (hard rule)
**Decision:** `appsettings.json` holds placeholders only; secrets via user-secrets (local) and App Service settings (prod). Frontend gets only public `VITE_*` vars; the Azure Speech key is server-side only. Strict `.gitignore` + a secret-scan backstop.
**Why:** The repo is public. **Note:** the sibling `D:\AlexRepos\API` commits secrets in `appsettings.json` — that pattern is explicitly **not** reused here.

### ADR-005 — CI/CD: GitHub Actions + OIDC
**Decision:** Deploy from GitHub Actions using federated (OIDC) login to Azure — passwordless, no stored credentials.
**Why:** Natural for a public GitHub repo; keeps secrets out of the repo and workflow. **Alternative considered:** Azure DevOps (as the API repo) — rejected to avoid a second system and stored secrets.

### ADR-006 — Reuse the existing Azure App Service plan
**Decision:** Deploy as a second Web App on the existing `plan-TTT-*` Linux plan; no Azure Container Registry.
**Why:** Marginal compute cost ≈ 0; ACR would add ~$5/mo. Docker is used for **local** dev only; production deploys published code via `az webapp deploy`.

### ADR-007 — MVP is anonymous with local progress
**Decision:** No login in the MVP; progress stored client-side (IndexedDB). Parent accounts + child profiles + cloud sync come in Phase 2 (ASP.NET Core Identity + JWT).
**Why:** Fastest path to a fun, usable product; GDPR-friendly (no child PII); auth is real work better done once the core is proven.

### ADR-008 — Bilingual (Swedish + English) from day one
**Decision:** Full bilingual UI + content. Content APIs return both locales; the client switches instantly (offline-friendly). Frontend i18n via simple JSON dictionaries with an enforced sv/en key-parity test.
**Why:** The app teaches English; bilingual is core, not an add-on. Returning both locales avoids translation round-trips and suits offline play.

### ADR-009 — Heavy gamification, no pressure
**Decision:** World map, mascot, coins, stars, badges, streaks, cosmetic reward shop. No timers, leaderboards or fail states.
**Why:** Strong intrinsic/extrinsic motivation that's age-appropriate for 3–9; competition/time pressure is not.

### ADR-010 — Audio via pre-generated Azure Neural TTS, with fallback
**Decision:** Generate `sv`/`en` audio files from seed content using Azure Neural TTS; commit the files; fall back to browser `SpeechSynthesis` when a clip is missing.
**Why:** Warm, consistent voice at $0 runtime cost and offline-capable; the fallback means the app works without any Azure key during development. **Trade-off:** asset management for generated audio (acceptable; not secret).

### ADR-011 — Installable PWA with offline; native app later
**Decision:** Ship a mobile-first installable PWA with offline play (service worker caches shell + content + audio). A native Android app (React Native or MAUI) is a later goal, sharing the API/types.
**Why:** No app-store friction to start; reach kids on phones/tablets immediately; keep a path to native.

### ADR-012 — Brand name: Skuttoo / Skuttö
**Decision:** Brand is **Skuttoo** (ASCII, global) with **Skuttö** ("Hoppön") as the Swedish spelling; code uses `Skuttoo`. From *skutt/skutta* (hop) → a mascot hopping between subject islands.
**Why:** Researched as collision-free in edtech/kids (unlike Klurio, Kluro, Mojo, Zooba, Pippo, Klippo…); ASCII form gives a clean `.com` and works internationally, ö-form reads beautifully in Swedish. **To do before launch:** WHOIS for `skuttoo.com`/`.se` and PRV/EUIPO trademark check (Nice classes 9 & 41).

### ADR-013 — MVP age band 3–9; subjects Math, Swedish, English, Logic
**Decision:** Target ages 3–9 first (pre-readers + early readers); four subjects in the MVP.
**Why:** Matches the author's own children (3 and 6) and a coherent design space; audio-first covers the youngest. Older ages (up to 15) come after the engine is proven.
**Amended by ADR-016** (onboarding asks for an exact age rather than a 3–5 / 6–9 band).

### ADR-014 — Strong testing incl. Playwright E2E
**Decision:** Unit + integration (backend), unit (frontend), and **Playwright E2E** in a mobile viewport, all gating CI.
**Why:** Quality matters; E2E protects the actual child-facing flows. See `TESTING.md`.

### ADR-015 — Incremental delivery in fine-grained sub-phases
**Decision:** Work in small sub-phases (1.1, 1.2, …) with review/checkpoints between them.
**Why:** Controls scope and token spend; each step is shippable and testable. See `ROADMAP.md`.

### ADR-016 — Exact-age onboarding (supersedes the age-band idea)
**Decision:** First-run onboarding asks for an **exact age** (3–9) instead of a 3–5 / 6–9 band — superseding the planned band picker and **amending ADR-013**. Age is **client-side only** (the MVP stays anonymous, ADR-007): the local `Profile` stores `age: number|null` and `ageBand` becomes a derived value (≤5 → `3-5`, else `6-9`). The exact age sets the per-subject **starting node** (`startNodeForAge`); the derived band drives the UI default split (pre-reader ≤5 vs reader ≥6). Earlier nodes stay visible and playable as **optional warm-ups** (never hard-locked).
**Why:** A single exact age is simpler for a parent than a band and gives finer placement without any backend change; keeping earlier nodes optional means a misjudged age never blocks or bores the child. Local storage keeps it anonymous and GDPR-friendly.

### ADR-017 — Per-subject themed tracks with a data-driven node metaphor
**Decision:** The world map is a **hub**; entering a subject opens its **own themed map**. Each track's node "metaphor noun" (planets / glades / destinations / temples) is **derived client-side** from the existing `Subject.ThemeKey` (`islandTheme.ts`) — **no new DB field and no migration**.
**Why:** Gives each island a distinct, child-legible identity and a richer sense of journey, while reusing data we already have; deriving on the client keeps the content schema and seed untouched.

### ADR-018 — Double the content depth for ages 6–9
**Decision:** Roughly double the per-track content (Math 5 → 10 nodes; Logic/Swedish/English 5 → 9) via an **append-only** seed — no `Level` ids changed, no EF migration. **No new `ExerciseType`**: `SimpleAddition` is the single-choice arithmetic type and now also covers subtraction, teens, two-digit addition (incl. carry) and comparison.
**Why:** The MVP was thin for older children; appending nodes adds genuinely harder material without invalidating client `completedLevelIds` or growing the type system. Reusing `SimpleAddition` keeps the engine unchanged (everything still grades as single-correct-choice).

### ADR-019 — Node shape, star gate and graded stars (gameplay refinement)
**Decision:** A **node** is a small set of **3 questions at one difficulty**; difficulty rises **between** nodes, and every subject has **10 nodes** (so the four tracks are uniform — ADR-018's 9-node tracks are extended to 10). **Stars are graded by attempt** (1st correct = 3, 2nd = 2, 3rd = 1; never below 1; reveal after the 3rd miss) — server-side in `ExerciseService`, awarded once on the first solve. The next node **unlocks at ≥ 4 stars on the current node OR when all its questions are solved** (`isLevelPassed`) — the "all solved" fallback prevents any dead‑end given 3 questions can total as few as 3 stars. A node's questions are **played in sequence** (resuming on the first unsolved). Answer/match **positions are shuffled** per exercise so the correct answer isn't always in the same slot. The home screen is a **plain 4‑choice hub** (no "next subject" nudge); each subject opens its **own themed node map** (planets/glades/destinations/temples).
**Why:** Short same‑difficulty nodes give a clear sense of progress; graded stars + a star gate reward doing well without punishing a struggling child (the fallback guarantees progress); shuffling removes a positional "tell"; the hub/themed‑map split makes each subject feel like its own world and stops Skutt from appearing tied to one subject. All client‑side except the star grading; no schema change.
