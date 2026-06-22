# Design

Skuttoo must feel **playful, warm and effortless** for a child holding a phone. Design decisions are made for a 3-year-old first, then scaled up for a 9-year-old.

## Principles

1. **Audio-first.** A pre-reader can do everything by listening and tapping. Every screen has a clear read-aloud; instructions never depend on reading.
2. **One thing at a time.** Big, uncluttered screens; a single obvious action; minimal text.
3. **Huge, forgiving tap targets.** Min 44×44px (prefer larger), generous spacing, no tiny controls, no precise gestures required for the youngest.
4. **Always encouraging.** Mistakes are gentle and reassuring; success is celebrated. No timers, no losing, no shaming.
5. **Delight through motion & sound** — but respect `prefers-reduced-motion` and keep it from being distracting.
6. **Mobile-first, thumb-friendly.** Primary actions reachable at the bottom; portrait orientation is the default.

## The mascot

A friendly character — **Skutt** — that *hops* (skutt/skutta) between islands and guides the child: greets them, reads instructions, cheers on success, gently encourages after a miss. The mascot is the consistent, comforting presence across the app and the face of the brand. (Final character design TBD; keep it abstract, cute, animal-like, expressive with simple states: idle, talking, happy, encouraging.)

## World map & islands

- The home screen is a **world map** with the four subject **islands**, the mascot, and a sense of a journey/path.
- Each island has its **own theme** (color + motif): e.g. Math = space, Logic = jungle, Swedish = forest, English = travel. Themes are expressed via semantic tokens (`--island-math`, …), not ad-hoc colors.
- Within an island, a **path of levels** shows progress (completed = lit up / starred, next = highlighted, locked = dimmed).

## Visual language & tokens

- **Semantic design tokens** (CSS custom properties) drive everything; components never hardcode hex. Token groups:
  - Brand/UI: `--color-primary`, `--color-surface`, `--color-text`, `--color-success`, `--color-warn`, radii, spacing scale.
  - Per island: `--island-{math|logic|swedish|english}` (+ accent/soft variants).
  - Reward: coin/star/badge colors.
- Rounded, soft shapes; bright but not harsh; high contrast for readability. Large, friendly type (system font stack to start; a rounded display font later).
- **Dark mode** supported through tokens.
- A small, documented set of reusable components: `Button`, `Card`, `Choice`, `AudioButton`, `MascotBubble`, `RewardBurst`, `ProgressPath`, `IslandTile`.

## Voice & tone (copy)

- Short, kind, concrete. Speak to the child ("Tryck på rätt antal äpplen!" / "Tap the right number of apples!").
- Encouraging on errors ("Nästan! Försök igen." / "Almost! Try again."). Never negative.
- Same warmth in both Swedish and English; translations are adapted, not literal, when needed.

## Age-adaptation

| | 3–5 (pre-readers) | 6–9 (readers) |
|---|---|---|
| Reading | none required; auto read-aloud | text + optional audio |
| Choices | 2–3, large, image-led | 3–4, may be text |
| Complexity | recognition/matching | multi-step (addition, reading) |
| Feedback | immediate, very visual | visual + light scorekeeping |

Age band is chosen at onboarding (sub-phase 1.10) and adjusts defaults; content is tagged by band.

## Accessibility

- Semantic HTML; every control labelled; logical focus order; visible `:focus-visible`.
- Don't rely on color alone (pair with shape/icon/label) — important for the color/shape exercises.
- Respect `prefers-reduced-motion`; provide captions/text alongside audio.
- Target WCAG AA contrast for text and interactive elements.

## Motion

- Micro-interactions: button press, mascot hops between islands, reward burst (coins/stars), level unlock.
- Lightweight (CSS/transform-based; a small motion library only if needed). Everything degrades gracefully with reduced motion.

> Concrete palette, type scale and mascot art are finalized when building the frontend (sub-phases 0.3/1.x), applying the design tokens defined here. This doc sets the direction and the token structure.
