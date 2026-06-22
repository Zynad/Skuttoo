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

### ADR-014 — Strong testing incl. Playwright E2E
**Decision:** Unit + integration (backend), unit (frontend), and **Playwright E2E** in a mobile viewport, all gating CI.
**Why:** Quality matters; E2E protects the actual child-facing flows. See `TESTING.md`.

### ADR-015 — Incremental delivery in fine-grained sub-phases
**Decision:** Work in small sub-phases (1.1, 1.2, …) with review/checkpoints between them.
**Why:** Controls scope and token spend; each step is shippable and testable. See `ROADMAP.md`.
