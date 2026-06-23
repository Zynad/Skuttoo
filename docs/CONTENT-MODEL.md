# Content model

How pedagogical content is structured, authored, localized and voiced. Content is **hand-authored and seeded** in the MVP (we keep full control); an admin CMS comes in Phase 3.

## Hierarchy

```
Subject (island)  →  Level (difficulty stop)  →  Exercise  →  Choice(s)
```

A child travels along a subject's themed track of **Levels** (the per-track node "stops"; easy → harder), each containing a handful of short **Exercises**. Difficulty is expressed both by `DifficultyTier` and a suggested `AgeMin/AgeMax` so the world adapts to a 3-year-old vs a 9-year-old.

**Node counts (per track):** Math has **10** nodes; Swedish, English and Logic each have **9**. The deeper Math/6–9 content is genuinely harder (teens, subtraction, skip-counting, comparison, two-digit addition with carry).

## Subjects & exercise types (MVP)

| Island | Theme idea | Node metaphor (sv / en) | Exercise types | Age focus |
|---|---|---|---|---|
| **Math** (Matematik) | space/numbers | *Planet {n}* | `CountObjects`, `NumberRecognition`, `SimpleAddition`, `ShapeMatch` | 3–9, scaling |
| **Logic & shapes/colors** (Logik) | jungle/sorting | *Tempel / Temple {n}* | `ColorMatch`, `ShapeMatch`, `PatternNext` (image-only) | 3–6 (non-readers), now reaching 7–9 |
| **Swedish** (Svenska) | letters/forest | *Glänta / Glade {n}* | `LetterSound`, `WordImageMatch`, early reading | 4–9 |
| **English** (Engelska) | travel/words | *Resmål / Destination {n}* | `ListenPickWord`, `WordImageMatch` (listen & pick) | 5–9 |

The **Logic** island is deliberately image- and audio-only so the youngest can play before they can read.

Each subject opens its **own themed map** (the world map is the hub). The node metaphor noun above is **derived client-side from `Subject.ThemeKey`** (via `islandTheme.ts`) — there is no backend field and no DB migration for it.

**`SimpleAddition` is the single-choice arithmetic type.** Beyond simple addition it now also covers subtraction, teens, two-digit addition (incl. carry) and bigger/smaller comparison — all graded as single-correct-choice. **No new `ExerciseType` was added** for the deeper content.

## Age-adaptation principles

- **3–5 (pre-readers):** no required reading; big pictures; everything auto-read-aloud; 2–3 large choices; forgiving.
- **6–9 (readers):** text + audio optional; more choices; multi-step (e.g. addition); higher tiers unlock.
- Onboarding asks for an **exact age** (3–9; sub-phase 1.10), not just a band. The exact age sets the **starting node** per subject — `startNodeForAge` is the first node (by display order) whose `AgeMax ≥ the child's age`, so an older child skips the easiest stops. **Earlier nodes stay visible and playable as optional warm-ups** (never hard-locked). The derived `ageBand` (≤5 vs ≥6) still drives behavioural defaults: auto-play audio, choice count, text density. This relies on `AgeMax` being non-decreasing along each track (a seed invariant, test-enforced).

## Bilingual authoring

- Every user-facing field is a `LocalizedText { sv, en }`; both must be filled. A test enforces sv/en key parity on the frontend i18n dictionaries; seed content must include both languages.
- Some content is **language-specific by nature**: the Swedish island teaches Swedish letters/sounds; the English island teaches English words. This is modelled with a per-subject **content language** (`Subject.ContentLanguage`): the exercise **instruction** (`Exercise.Prompt`) renders in the child's UI language, while the **taught word** (`Exercise.Target` + `TargetAudio`) and the **answer labels** render in the content language. So a Swedish child on the English island gets Swedish instructions while hearing/seeing English words; Math/Logic leave `ContentLanguage` null and follow the UI language.
- **Interaction types:** beyond single-choice exercises, the engine supports **tap-to-match** (pair items sharing a hidden `Choice.GroupKey`) and **drag-to-bucket** (place each item into the `Bucket` its `GroupKey` names). Correctness is always evaluated server-side; the grouping keys are never serialized.
- **Content types reuse single-choice.** Every pedagogical type except the two generic interaction types grades as single-choice (one `Choice.IsCorrect`), so they need no engine code — only seeded content. Two worth calling out:
  - **`PatternNext`** (Logic): the sequence is a **composite image** (`Exercise.ImageRef` → `pattern-*.svg`, e.g. 🔴🔵🔴🔵❓) and the choices are the candidate next items (image-only swatches). Image-only, non-reader friendly.
  - **`LetterSound`** (Swedish): the spoken stimulus is the word (`Exercise.Target` + `TargetAudio`, with a picture in `ImageRef`); the choices are **letters as text labels** (no assets). The child hears the word and picks its starting letter.
- Keep copy short, warm and concrete (see `DESIGN.md` voice & tone).

## Audio

- Each `Exercise.PromptAudio` and (optionally) each `Choice.Audio` references pre-generated TTS files per locale: `assets/audio/{sv|en}/{key}.mp3`.
- Generated once by the TTS tool (Azure Neural voices: a friendly Swedish `sv-SE` voice and an `en-US`/`en-GB` voice). Files are committed (not secret).
- Missing file → client falls back to `SpeechSynthesis` in the active language, so authoring can proceed before audio is generated.

## Visuals

- Images are small flat **SVGs** under `frontend/public/assets/img`, referenced by relative path (`ImageRef`).
- **Colors, shapes and pattern sequences** are hand-authored geometric SVGs (clean, precise, token-matched fills); pattern sequences are single composite files (`pattern-*.svg`).
- **Object/word pictures** (cat, dog, house, fish…) are tiny **emoji-based SVGs** (one emoji glyph per concept) — broad, recognizable vocabulary at near-zero cost. These are placeholders to be replaced by real illustrations in the art/polish pass (1.8); emoji styling varies slightly by platform.

## Authoring & seeding workflow (MVP)

1. Author content as structured data (C# seed builders or JSON resources) under `Skuttoo.Infrastructure/Seeding`, keyed by stable string keys (e.g. `math.l1.count-apples`).
2. Run the **TTS generator** to produce audio for any new prompts/choices (skips existing files).
3. App seeding upserts content idempotently on startup (Development) / deploy.
4. Add/extend tests: an integration test asserts the seeded island/exercise is served correctly in both locales.

## Quality bar for content

- Pedagogically sound and age-appropriate; no trick questions for the youngest.
- Encouraging on mistakes (gentle retry, never punitive — see GAMIFICATION).
- Culturally neutral, inclusive imagery; accessible color choices (don't rely on color alone — pair with shape/label).

## Content packs & offline

Content is grouped per island so it can be cached/"downloaded" for offline play (sub-phase 1.9). A future `GET /api/content/pack/{subjectKey}` can return a full island bundle (including answers) for client-side validation offline; the MVP slice validates server-side.
