# CLAUDE.md — Skuttoo

Guidance for AI agents and developers working in this repo. Read this first. It is the **single source of truth for conventions**; the `docs/` folder holds the deeper design.

## What this is

**Skuttoo** (Swedish spelling **Skuttö**, "Hoppön") is a playful, mobile-first educational web app for children **3–9 years old**. Children hop with a friendly **mascot** between **subject islands** (Math, Swedish, English, Logic) on a world map, solving bite-sized exercises and earning coins, badges and streaks.

- **Bilingual** Swedish + English (UI and content) from day one.
- **Mobile-first**, installable **PWA** with offline play.
- **Audio-first**: pre-readers (age 3) must be able to do everything by listening — every instruction can be read aloud (pre-generated Azure Neural TTS, with browser `SpeechSynthesis` fallback).
- MVP saves progress **locally** (anonymous, no login). Parent accounts come later.

The brand name in the UI is **Skuttö** for Swedish and **Skuttoo** for English. Code, namespaces, packages and identifiers always use the ASCII form **`Skuttoo`**.

## Tech stack

| Layer | Choice |
|---|---|
| Frontend | Vite · React 19 · TypeScript (strict) · React Router v7 · Tailwind CSS v4 + semantic design tokens · Axios (via `BaseApi`) · PWA (service worker + manifest) |
| i18n | Plain JSON dictionaries (`sv.json`, `en.json`) + a tiny `useT()` hook. No heavyweight i18n lib. |
| Backend | .NET 10 · ASP.NET Core **Controllers** · EF Core + **SQLite** · **Riok.Mapperly** · repository pattern |
| API docs | `Microsoft.AspNetCore.OpenApi` + Scalar UI at `/docs` |
| Audio | Azure Cognitive Services Speech (Neural), pre-generated to files; `SpeechSynthesis` fallback |
| Tests | Backend: xUnit + NSubstitute + Shouldly + `WebApplicationFactory`. Frontend: Vitest + Testing Library. E2E: **Playwright** (mobile viewport). |
| Hosting | One Azure App Service (Linux) serving API **and** the React build from `wwwroot`. SQLite file on persistent `/home`. CI/CD via GitHub Actions + OIDC. |

## Repository layout

```
/                      root: CLAUDE.md, README.md, compose.yaml, Dockerfile, .editorconfig, .gitignore, .env.example
/docs                  planning & design docs (see below)
/.github/workflows     GitHub Actions (deploy.yml — OIDC, no secrets)
/infra                 deploy.bicep + parameters (second web app on existing plan)
/backend
  Skuttoo.slnx
  /src
    Skuttoo.Api            Controllers, Program.cs, Config/appsettings*.json, wwwroot
    Skuttoo.Application    feature services (the use-cases)
    Skuttoo.Domain         entities, enums, value objects — no dependencies
    Skuttoo.Infrastructure EF Core DbContext, repositories, TTS, seeding
  /tests
    Skuttoo.Tests          xUnit (unit + WebApplicationFactory integration)
/frontend
  package.json, vite.config.ts, tsconfig*.json, eslint.config.js, .prettierrc, playwright.config.ts
  /public                  manifest.webmanifest, icons, generated audio
  /src
    main.tsx, App.tsx, index.css
    /routes                world map, subject island, exercise, profile
    /components            reusable UI (Button, Card, AudioButton, …)
    /features              per subject / per exercise type
    /game                  world map, rewards, mascot, progress
    /hooks /contexts /services (BaseApi) /utils /types
    /i18n                  sv.json, en.json, useT
    /styles                tokens + per-island themes
  /e2e                     Playwright specs
```

Dependency direction (backend): `Api → Application → Domain`, `Infrastructure → Domain` (and `Application` depends on Infrastructure abstractions via interfaces defined in Application/Domain). `Domain` depends on nothing.

## Golden rules

1. **PUBLIC REPO — NEVER COMMIT SECRETS.** This repo is public on GitHub. No API keys, connection strings with credentials, tokens, or `.pubxml`. See **Secrets** below. When in doubt, leave it out and use a placeholder.
2. **Tests are not optional.** Every feature ships with tests at the right level (see **Testing**). Don't mark work done while tests fail.
3. **Bilingual + audio by default.** Every new user-facing string exists in both `sv` and `en`, and anything a 3-year-old must understand has audio.
4. **Mobile-first.** Design and test at a phone viewport first (≥360px wide), large tap targets (min 44×44px), then scale up.
5. **Keep it self-contained.** No private Triggerbee NuGet feeds or libraries. Public packages only.
6. **Match the conventions below** — read the surrounding code and mirror it. Small, focused changes.

## Backend conventions (.NET 10)

- **Controllers**, not Minimal APIs. Group by feature. Route prefix `/api`. Annotate actions with `[ProducesResponseType(...)]`.
- `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>` in every csproj.
- **Primary constructors** for DI: `public sealed class ExerciseService(IExerciseRepository repo) : IExerciseService`.
- Private fields `_camelCase`. Prefer `var` when the type is apparent, explicit type for built-ins (`int`, `string`).
- **Async everywhere**; `await ...ConfigureAwait(false)` in `Application`/`Infrastructure` library code. No `.Result`/`.Wait()`. Pass `CancellationToken` through.
- **EF Core + SQLite.** DbContext in `Infrastructure`. Repository interfaces in `Application` (or `Domain`), implementations in `Infrastructure`. Code-first migrations in `Infrastructure/Migrations`, applied on startup in Development.
- **Localization model:** user-facing text is a value object `LocalizedText { string Sv; string En; }` stored as a **JSON column**. Audio is `LocalizedAudio { string? Sv; string? En; }` (relative asset paths). Content API responses return **both** locales so the client can switch language offline.
- **Mapping:** Riok.Mapperly (`[Mapper] partial class` in `Api` or `Application/Mapping`). No AutoMapper.
- **DI registration** is explicit (`services.AddScoped<IFoo, Foo>()`), grouped in `AddApplication()` / `AddInfrastructure()` extension methods. No assembly scanning.
- **Errors:** throw domain exceptions (e.g. `NotFoundException`); a single error-handling middleware maps them to `ProblemDetails`. Never leak stack traces in production.
- **No** MediatR/CQRS, **no** FluentValidation (validate in the service/endpoint), **no** Serilog (use `ILogger<T>`; Application Insights optional later).
- **Health:** `/health/live` and `/health/ready`.
- **Config:** `appsettings.json` (committed, placeholders only) + `appsettings.Development.json` (gitignored) + env vars + user-secrets. Bind via the Options pattern.

## Frontend conventions

- **Functional components** only, **named exports**, props `interface` declared above the component. One component per file, `PascalCase.tsx`. Utilities `camelCase.ts`. No `index.ts` barrels. **No path aliases** — relative imports.
- TypeScript **strict**; no `any` (use `unknown`); ESLint flat config; Prettier (single quotes, 2 spaces, `printWidth: 120`). `npm run lint` must pass with no warnings.
- **State:** React `useState`/`useContext`/`useReducer`. No Redux/Zustand. Server data via the `BaseApi` axios wrapper + small typed service classes; cache content in memory/IndexedDB for offline.
- **Styling:** Tailwind v4 utilities + **semantic design tokens** (CSS custom properties like `--color-primary`, `--island-math`), never raw hex in components. Dark mode supported via tokens. Big rounded playful components, large tap targets.
- **i18n:** never hardcode user text. Use `t('key')`; keys exist in both `sv.json` and `en.json` (keep them in sync — a test enforces this).
- **Audio:** use the shared `AudioButton`/`useSpeak` — play the pre-generated clip if present, else fall back to `SpeechSynthesis` in the current language.
- **Accessibility:** semantic HTML, labelled controls, visible `:focus-visible`, respect `prefers-reduced-motion`.

## Testing (required — see docs/TESTING.md)

- **Backend unit:** services and pure logic with xUnit + NSubstitute + Shouldly.
- **Backend integration:** API endpoints via `WebApplicationFactory` against a SQLite database (seeded), asserting status + payload + rewards.
- **Frontend unit:** components/hooks with Vitest + Testing Library (render, interact, assert). Plus a test that `sv.json`/`en.json` have identical key sets.
- **E2E (Playwright):** real user flows in a **mobile viewport** — e.g. open app → enter Math island → answer a counting exercise → see reward → progress persists after reload. Tests use `data-testid` for stable locators and web-first assertions (no fixed `waitForTimeout`).
- A change is "done" only when `dotnet test`, `npm test`, `npm run lint`, and the relevant Playwright specs are green.

## Secrets (public repo)

- `appsettings.json` holds **placeholders only** (`"Speech": { "Key": "" }`). Real values: `dotnet user-secrets` locally, **App Service application settings** in prod.
- Frontend: only `VITE_*` vars that are safe to be public (they end up in the client bundle). **The Azure Speech key is server-side only** — never in frontend code or env.
- Gitignored: `appsettings.Development.json`, `appsettings.*.local.json`, `.env*` (except `.env.example`), `*.db`/`*.db-*`, `*.pubxml`, `*.PublishSettings`, `wwwroot` build output.
- CI uses **GitHub Actions OIDC** (federated, passwordless) — no secrets stored in the repo or workflow files.

## Commands

```bash
# Backend
cd backend
dotnet build
dotnet test
dotnet watch --project src/Skuttoo.Api        # http://localhost:5173/api, /docs

# Frontend
cd frontend
npm install
npm run dev        # http://localhost:5173 (proxies /api to backend)
npm run build      # tsc + vite build -> dist
npm run lint
npm test           # Vitest
npm run e2e        # Playwright

# Full stack locally
docker compose up --build
```

## Git workflow

- Public GitHub repo. Default branch `main`. Feature branches → PR → `main`. `main` deploys to Azure via GitHub Actions.
- Conventional, present-tense commit subjects (e.g. `add counting exercise endpoint`). Don't commit unless asked; never push secrets.
- Before committing: `dotnet test`, `npm run lint && npm test` pass, and `git status`/`git diff` reviewed for accidental secrets.

## Docs map

`docs/ROADMAP.md` (phased plan, sub-phases 1.1…), `docs/TESTING.md` (test strategy), `docs/ARCHITECTURE.md`, `docs/DATA-MODEL.md`, `docs/CONTENT-MODEL.md`, `docs/DESIGN.md`, `docs/GAMIFICATION.md`, `docs/DECISIONS.md` (ADR log).
