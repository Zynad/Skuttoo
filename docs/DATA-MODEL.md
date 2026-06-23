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
| ContentLanguage | enum `Language?` | `Sv`/`En`, or null. The language this island teaches (English→`En`, Swedish→`Sv`); null = follow the child's UI language (Math, Logic). |
| Name | LocalizedText | |
| Description | LocalizedText | short, read-aloud intro |
| ThemeKey | string | drives the island theme/colours (`--island-math`, …) **and the per-track node metaphor** (planets/glades/destinations/temples), which is derived **client-side** from this key (`islandTheme.ts`) — there is no separate DB field for it |
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
| Prompt | LocalizedText | the **instruction**, shown/spoken in the UI language |
| PromptAudio | LocalizedAudio | read-aloud instruction (UI language) |
| Target | LocalizedText? | the **taught word** (language islands), rendered/spoken in the content language; null otherwise |
| TargetAudio | LocalizedAudio? | audio for the taught word (the "listen" stimulus), content language |
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
| GroupKey | string? | **never serialized** — tap-to-match: two choices sharing a key form a pair; drag-to-bucket: the correct `Bucket.Key` |

### Bucket  (drop target for drag-to-bucket exercises)
| Field | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| ExerciseId | int (FK→Exercise) | |
| DisplayOrder | int | |
| Key | string | stable key choices reference via `Choice.GroupKey` (safe to serialize) |
| Label | LocalizedText | |
| ImageRef | string? | optional |

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
                          Exercise 1───* Bucket   (drag-to-bucket only)
Badge (standalone definitions)
```

`ExerciseType` (extensible): `CountObjects, NumberRecognition, SimpleAddition, ShapeMatch, ColorMatch, PatternNext, LetterSound, WordImageMatch, ListenPickWord, TapToMatch, DragToBucket`. The content types all evaluate as single-correct-choice; `TapToMatch`/`DragToBucket` are the generic interaction types. The attempt endpoint accepts `{ choiceId }` (single choice) **or** `{ placements: [{ itemId, targetKey }] }` (match/bucket) and returns `{ correct, correctChoiceId?, reward, correctPlacements? }`.

## Client-side progress (IndexedDB, MVP)

Not in the server DB yet — shapes the local store and the Phase 2 server tables:

```
Profile         { id, name, age: number|null, avatar, coins, stars, badgeKeys[], streak:{count,lastPlayedDate} }
ExerciseResult  { exerciseId, completed, starsEarned, attempts, lastPlayedAt }
```

- **`age`** is the exact age (3–9); `null` = not yet onboarded (the app redirects to `/onboarding`). It is **client-side only** — there is **no server schema change for age** (the MVP stays anonymous, ADR-007). The exact age sets the per-subject starting node (`startNodeForAge`); earlier nodes remain optional warm-ups.
- **`ageBand`** (`'3-5'|'6-9'`) is no longer stored — it is **derived** from `age` (≤5 → `3-5`, else `6-9`) and drives behavioural defaults only.
- **Legacy migration:** `normalizeProfile` migrates a pre-`age` save **with progress** by inferring an age from its legacy band (`'6-9'` → 7, `'3-5'` → 4) so returning players aren't bounced to onboarding; an empty old save stays `age: null`.

## Migrations & seeding

- **Code-first migrations** in `Skuttoo.Infrastructure/Migrations`. In Development the app `Migrate()`s on startup; in prod migrations run on deploy/startup (guarded).
- **Seeding** is idempotent (upsert by stable keys) and lives in `Infrastructure/Seeding`. Seed data is authored in C# or JSON resources and covers all four subjects (at least one exercise each) for the slice.
- The content-depth expansion (sub-phase 1.12) is **append-only**: new nodes/exercises were added without changing or reordering existing `Level` ids, so the client `completedLevelIds` references stay valid — no EF migration was needed.
- Keep migrations **backwards-compatible** (additive / expand-then-contract) so deploys are safe.

## PostgreSQL path (Phase 4)

EF Core keeps data access provider-agnostic. To move: swap `UseSqlite` → `UseNpgsql`, regenerate migrations (or maintain a provider-neutral model), and point the connection string at Azure Database for PostgreSQL. JSON columns for `LocalizedText`/`LocalizedAudio` map cleanly to `jsonb`.
