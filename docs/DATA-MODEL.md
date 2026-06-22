# Data model

The **server database stores content only** (subjects, levels, exercises, choices, badge definitions). In the MVP, **child progress lives client-side** (IndexedDB) because play is anonymous. Progress is promoted to the server in Phase 2.

## Value objects (owned, stored as JSON columns)

```
LocalizedText  { string Sv; string En; }          // every user-facing string
LocalizedAudio { string? Sv; string? En; }         // relative paths to pre-generated audio, e.g. "assets/audio/sv/ex-101.mp3"
```

Content responses always include both locales so the client switches language offline.

## Entities (server, EF Core → SQLite)

### Subject  (an "island")
| Field | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| Key | enum `SubjectKey` | `Math, Swedish, English, Logic` (unique) |
| Name | LocalizedText | |
| Description | LocalizedText | short, read-aloud intro |
| ThemeKey | string | drives the island theme/colours (`--island-math`, …) |
| DisplayOrder | int | order on the world map |

### Level  (a stage/path stop on an island)
| Field | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| SubjectId | int (FK→Subject) | |
| DisplayOrder | int | order within the island |
| Title | LocalizedText | |
| DifficultyTier | int | 1..n (rough progression) |
| AgeMin / AgeMax | int | suggested age band (3..9) |

### Exercise
| Field | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| LevelId | int (FK→Level) | |
| DisplayOrder | int | |
| Type | enum `ExerciseType` | see below |
| Prompt | LocalizedText | the question |
| PromptAudio | LocalizedAudio | read-aloud prompt |
| ImageRef | string? | optional illustration |
| RewardCoins | int | coins for first correct solve |
| RewardStars | int | 1–3 stars |

### Choice  (answer option)
| Field | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| ExerciseId | int (FK→Exercise) | |
| DisplayOrder | int | |
| Label | LocalizedText | text (may be empty for image-only) |
| ImageRef | string? | for image choices (colors/shapes) |
| Audio | LocalizedAudio? | optional |
| IsCorrect | bool | **never serialized to the client** in normal content endpoints |

### Badge  (definition only; earning is client-side in MVP)
| Field | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| Key | string | unique, e.g. `math-first-island` |
| Name | LocalizedText | |
| Description | LocalizedText | |
| IconRef | string | |
| CriteriaType | enum | `CompleteLevel, CompleteSubject, Streak, CoinTotal` |
| CriteriaValue | int | threshold / target id |

### ERD

```
Subject 1───* Level 1───* Exercise 1───* Choice
Badge (standalone definitions)
```

`ExerciseType` (extensible): `CountObjects, NumberRecognition, SimpleAddition, ShapeMatch, ColorMatch, PatternNext, LetterSound, WordImageMatch, ListenPickWord`.

## Client-side progress (IndexedDB, MVP)

Not in the server DB yet — shapes the local store and the Phase 2 server tables:

```
Profile         { id, name, ageBand: '3-5'|'6-9', avatar, coins, stars, badgeKeys[], streak:{count,lastPlayedDate} }
ExerciseResult  { exerciseId, completed, starsEarned, attempts, lastPlayedAt }
```

## Migrations & seeding

- **Code-first migrations** in `Skuttoo.Infrastructure/Migrations`. In Development the app `Migrate()`s on startup; in prod migrations run on deploy/startup (guarded).
- **Seeding** is idempotent (upsert by stable keys) and lives in `Infrastructure/Seeding`. Seed data is authored in C# or JSON resources and covers all four subjects (at least one exercise each) for the slice.
- Keep migrations **backwards-compatible** (additive / expand-then-contract) so deploys are safe.

## PostgreSQL path (Phase 4)

EF Core keeps data access provider-agnostic. To move: swap `UseSqlite` → `UseNpgsql`, regenerate migrations (or maintain a provider-neutral model), and point the connection string at Azure Database for PostgreSQL. JSON columns for `LocalizedText`/`LocalizedAudio` map cleanly to `jsonb`.
