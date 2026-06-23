# Gamification

The reward system is the engine that makes Skuttoo *fun* and keeps kids coming back — without pressure, timers or punishment. In the MVP everything is tracked **client-side** (IndexedDB); it moves server-side with parent accounts (Phase 2).

## Core loop

```
pick island → do exercise → instant positive feedback → earn coins/stars
   → fill the level → unlock the next stop → earn a badge / extend streak
   → spend coins on rewards → come back tomorrow (streak)
```

## Currencies & signals

- **Stars (1–3 per exercise/level):** quality signal — how well it went on first tries. Persisted per exercise; shown on the island path.
- **Coins:** earned for solving exercises (and bonuses for streaks/badges); the spendable currency.
- **Badges:** milestone achievements (definitions seeded in DB; earning tracked locally). Examples: *First hops* (complete first level), *Island explorer* (complete an island), *On a roll* (3-day streak), *Coin collector* (N coins).
- **Daily streak:** increments once per calendar day played; gentle, never breaks the experience if missed (just resets the counter). Small coin bonus for keeping it.

## Reward economy (coins sink)

- A simple **reward shop / collection**: spend coins on cosmetic, kid-pleasing items — mascot outfits/accessories, island decorations, sticker collection, new mascot color. Purely cosmetic; nothing pay-to-win, nothing gated behind money.
- Unlockables on the world map (new path stops, decorations) as islands are completed.

## Progression & unlocking

- Islands and levels unlock as earlier ones are completed (with some freedom so a child isn't stuck). Locked stops are visible-but-dimmed to motivate.
- Difficulty rises along an island's path; completing tiers earns more stars/coins.
- **Island/track complete = all nodes.** The optional **warm-up nodes** before a child's start node (skipped by older children) still **count toward 100%** but are **not required to start** playing. Badge definitions and thresholds are **unchanged** for this redesign phase — revisit when tuning the economy.

## Feedback & encouragement (tone)

- **Correct:** burst of coins/stars, mascot cheers, success sound, short praise (read aloud).
- **Incorrect:** gentle "almost — try again", mascot encourages, no coin loss, no negative sound; reveal the right answer after a couple of tries so the child still progresses and learns.
- **No timers, no leaderboards, no fail states** in the MVP. Competition/pressure is not age-appropriate for 3–9.

## Data (MVP, client-side)

See `DATA-MODEL.md` → *Client-side progress*. The local `Profile` holds `coins`, `stars`, `badgeKeys[]`, `streak`; `ExerciseResult[]` holds per-exercise stars/attempts. The server only computes the **per-attempt reward** (`{ coins, stars }`) and stores nothing about the child in the MVP.

## Server's role

- `POST /api/exercises/{id}/attempt` returns `{ correct, correctChoiceId, reward { coins, stars } }`. Reward values come from the exercise definition (`RewardCoins`, `RewardStars`) and simple rules (e.g. fewer stars after multiple wrong tries).
- Badge **definitions** are served from the DB; badge **earning** logic runs client-side in MVP (promoted server-side in Phase 2 for cross-device consistency).

## Future (Phase 2+)

- Cloud-synced coins/badges/streaks per child profile.
- Parent-visible progress (not gamified pressure — informative).
- Possibly gentle weekly goals; still no competitive/time pressure.
