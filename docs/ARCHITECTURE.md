# Architecture

## Overview

Skuttoo is **one deployable unit**: an ASP.NET Core (.NET 10) app that exposes a JSON API under `/api` and serves the built React SPA from `wwwroot`. This keeps cost near zero (a single Azure Web App on an existing plan), avoids CORS, and simplifies deployment.

```
        ┌──────────────────────────── Browser / installed PWA (mobile-first) ────────────────────────────┐
        │  React 19 SPA (Vite build)                                                                      │
        │   routes: world map → island → exercise → profile                                               │
        │   state: React context/hooks · content cache + progress in IndexedDB (offline)                  │
        │   audio: pre-generated clip  ──fallback──▶  browser SpeechSynthesis                              │
        └───────────────▲───────────────────────────────────────────────┬───────────────────────────────┘
                        │ static assets (HTML/JS/CSS/audio)              │ /api/* (JSON, both locales)
                        │                                                ▼
        ┌───────────────┴───────────────────────────────────────────────────────────────────────────────┐
        │  ASP.NET Core (.NET 10)  —  Skuttoo.Api                                                          │
        │   StaticFiles + SPA fallback   |   Controllers /api/*   |   /docs (Scalar)   |   /health/*       │
        │        Application (use-case services)  ──▶  Domain (entities, value objects)                    │
        │        Infrastructure: EF Core DbContext + repositories (SQLite),  TTS generator, seeding        │
        └───────────────────────────────────────────────┬───────────────────────────────────────────────┘
                                                         ▼
                                          SQLite file  (/home/data/app.db in prod)
```

## Projects & dependencies

- **Skuttoo.Domain** — entities, enums, value objects (`LocalizedText`, `LocalizedAudio`). No external dependencies.
- **Skuttoo.Application** — use-case services (`ISubjectService`, `IExerciseService`, …), repository **interfaces**, DTOs, mapping. Depends on Domain.
- **Skuttoo.Infrastructure** — EF Core `SkuttooDbContext`, repository implementations, content seeding, the TTS generator. Depends on Application + Domain.
- **Skuttoo.Api** — controllers, `Program.cs`, middleware (error handling, correlation id), OpenAPI, static-file/SPA hosting, DI composition. Depends on Application + Infrastructure.
- **Skuttoo.Tests** — xUnit unit + integration.

`Add­Application()` and `AddInfrastructure(config)` extension methods compose DI; `Program.cs` wires them, the middleware pipeline, and `MapControllers()` + SPA fallback.

## Request flow (exercise attempt)

1. Client `POST /api/exercises/{id}/attempt` with `{ choiceId }`.
2. `ExercisesController` → `IExerciseService.EvaluateAttempt(id, choiceId, ct)`.
3. Service loads the exercise via `IExerciseRepository`, checks the choice, computes the reward.
4. Returns a DTO `{ correct, correctChoiceId, reward { coins, stars } }` (Mapperly maps entities→DTOs).
5. Client shows feedback/animation and updates **local** progress (IndexedDB). MVP does not persist progress server-side.

## Bilingual content delivery

Content endpoints return **both** locales (`{ sv, en }`) for every text and audio reference, so the client switches language instantly and works offline. The client picks the active locale; it never round-trips to translate.

## Offline & PWA

- App shell precached by the service worker (Workbox via `vite-plugin-pwa`).
- On entering an island, its content + audio are cached (runtime caching / explicit "download" later).
- Progress and coins live in IndexedDB; the app is fully playable offline once content is cached. (Cloud sync arrives in Phase 2.)

## Audio (TTS) pipeline

- A small generator (console tool / `Skuttoo.Infrastructure` command) reads seed content and calls **Azure Neural TTS** to produce `sv` and `en` audio files into `frontend/public/assets/audio` (committed; not secret).
- The key comes from user-secrets/env — **never** the frontend.
- Runtime cost is $0 (static files). If a clip is missing, the client falls back to `SpeechSynthesis`, so the app works without any Azure key during development.

## Hosting & deployment

- One Linux Web App on the existing App Service plan (`plan-TTT-*`); `linuxFxVersion DOTNETCORE|10.0`.
- SQLite file in persistent `/home/data`. No managed DB, no container registry, no separate frontend host.
- CI/CD: GitHub Actions + OIDC → build frontend → copy `dist` into `Skuttoo.Api/wwwroot` → `dotnet publish` → `az webapp deploy`. See [DECISIONS.md](DECISIONS.md) and the root `infra/`.

## Scaling path (later)

EF Core abstracts the DB → swap SQLite for **PostgreSQL** (Phase 4) with a provider change + migration. Add caching/CDN for media; extract shared TS contracts for a native client.
