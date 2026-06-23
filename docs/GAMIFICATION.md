# Gamification

The reward system is the engine that makes Skuttoo *fun* and keeps kids coming back — without pressure, timers or punishment. In the MVP everything is tracked **client-side** (IndexedDB); it moves server-side with parent accounts (Phase 2).

## Core loop

```
pick a subject → enter its themed map → play a node's 3 questions → earn coins/stars
   → reach ≥4 stars on the node → unlock the next node → earn a badge / extend streak
   → spend coins on rewards → come back tomorrow (streak)
```

## Currencies & signals

- **Stars (3/2/1 per question, graded by attempt):** a quality signal — 3 for a first‑try solve, 2 on the second try, 1 after the third (never 0 once solved). Persisted per question and summed per node; shown on the map.
- **Coins:** earned for solving exercises (and bonuses for streaks/badges); the spendable currency.
- **Badges:** milestone achievements (definitions seeded in DB; earning tracked locally). Examples: *First hops* (complete first level), *Island explorer* (complete an island), *On a roll* (3-day streak), *Coin collector* (N coins).
- **Daily streak:** increments once per calendar day played; gentle, never breaks the experience if missed (just resets the counter). Small coin bonus for keeping it.

## Reward economy (coins sink)

- A simple **reward shop / collection**: spend coins on cosmetic, kid-pleasing items — mascot outfits/accessories, island decorations, sticker collection, new mascot color. Purely cosmetic; nothing pay-to-win, nothing gated behind money.
- Unlockables on the world map (new path stops, decorations) as islands are completed.

## Progression & unlocking

- Each subject is a track of **10 nodes**; a node is **3 questions at one difficulty**. The next node unlocks once the current node earns **≥ 4 stars, OR all 3 questions are solved** (`isLevelPassed`). Doing well lets a child move on quickly; a struggling child who answers all three still progresses — there are **no dead‑ends**. Locked nodes are visible‑but‑dimmed to motivate.
- Difficulty rises **between** nodes; the child's chosen age sets the **starting node** (earlier nodes stay playable but optional — see `CONTENT-MODEL.md`).
- **Track complete = all 10 nodes passed.** Optional warm‑up nodes before the start node count toward 100% but aren't required. Badge definitions and thresholds are **unchanged** for now — revisit when tuning the economy.

## Feedback & encouragement (tone)

- **Correct:** burst of coins/stars, mascot cheers, success sound, short praise (read aloud).
- **Incorrect:** gentle "almost — try again", mascot encourages, no coin loss, no negative sound; the answer is revealed after the **third** try so the child still progresses and learns (and a third‑try solve still earns 1 star).
- **No timers, no leaderboards, no fail states** in the MVP. Competition/pressure is not age-appropriate for 3–9.

## Data (MVP, client-side)

See `DATA-MODEL.md` → *Client-side progress*. The local `Profile` holds `coins`, `stars`, `badgeKeys[]`, `streak`; `ExerciseResult[]` holds per-exercise stars/attempts. The server only computes the **per-attempt reward** (`{ coins, stars }`) and stores nothing about the child in the MVP.

## Server's role

- `POST /api/exercises/{id}/attempt` returns `{ correct, correctChoiceId, reward { coins, stars } }`. Coins are flat (`RewardCoins`); **stars scale by `attemptNumber`** — `max(1, RewardStars − (attempt − 1))` → 3/2/1. The client awards the reward once, on the first correct solve.
- Badge **definitions** are served from the DB; badge **earning** logic runs client-side in MVP (promoted server-side in Phase 2 for cross-device consistency).

## Future (Phase 2+)

- Cloud-synced coins/badges/streaks per child profile.
- Parent-visible progress (not gamified pressure — informative).
- Possibly gentle weekly goals; still no competitive/time pressure.
