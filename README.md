# Skuttoo 🦘

A playful, mobile-first learning app for kids **3–9**. Hop with a friendly mascot between **subject islands** — Math, Swedish, English and Logic — solving bite-sized challenges and earning coins, badges and streaks. Bilingual (Swedish + English), audio-first for pre-readers, installable as a PWA and playable offline.

> Swedish brand: **Skuttö** ("Hoppön"). Code/identifiers use the ASCII form **Skuttoo**.

## Status

Early development. Current milestone: **Phase 0** — planning docs, runnable skeleton, and one end-to-end vertical slice (a counting exercise). See [`docs/ROADMAP.md`](docs/ROADMAP.md).

## Tech

React 19 + Vite + TypeScript (frontend) · .NET 10 + ASP.NET Core + EF Core/SQLite (backend) · Tailwind v4 · Azure Neural TTS · Playwright/Vitest/xUnit. One app: the backend serves the built frontend from `wwwroot`.

## Quick start

Prerequisites: **.NET 10 SDK**, **Node 20+**, (optional) **Docker**.

```bash
# 1. Backend API (terminal A)
cd backend
dotnet watch --project src/Skuttoo.Api        # serves /api and /docs

# 2. Frontend (terminal B)
cd frontend
npm install
npm run dev                                    # http://localhost:5173, proxies /api

# …or run the whole stack in one container
docker compose up --build
```

The SQLite database is created and seeded automatically on first run (Development).

## Tests

```bash
cd backend  && dotnet test          # xUnit unit + integration
cd frontend && npm test             # Vitest unit
cd frontend && npm run e2e          # Playwright E2E (mobile viewport)
```

## Configuration & secrets

This is a **public repo — no secrets are committed.** `appsettings.json` contains placeholders only. Provide real values locally via `dotnet user-secrets`, and in production via Azure App Service settings. The Azure Speech key is used **server-side only**. See [`CLAUDE.md`](CLAUDE.md) and [`.env.example`](.env.example).

## Documentation

| Doc | Contents |
|---|---|
| [CLAUDE.md](CLAUDE.md) | Conventions & rules (read first) |
| [docs/ROADMAP.md](docs/ROADMAP.md) | Phased plan (sub-phases 1.1, 1.2 …) |
| [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) | System design & data flow |
| [docs/DATA-MODEL.md](docs/DATA-MODEL.md) | Entities, ERD, migrations |
| [docs/CONTENT-MODEL.md](docs/CONTENT-MODEL.md) | Subjects, exercises, bilingual content, audio |
| [docs/DESIGN.md](docs/DESIGN.md) | UX, mascot, age-adaptation, accessibility |
| [docs/GAMIFICATION.md](docs/GAMIFICATION.md) | Map, coins, badges, streaks, rewards |
| [docs/TESTING.md](docs/TESTING.md) | Test strategy (unit, integration, E2E) |
| [docs/DECISIONS.md](docs/DECISIONS.md) | Decision log (ADR) |

## License

TBD.
