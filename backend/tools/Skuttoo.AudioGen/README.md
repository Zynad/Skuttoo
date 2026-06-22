# Skuttoo.AudioGen

Author-time tool that pre-generates the Azure Neural TTS clips for all seed content (ADR-010).
It is **deliberately not part of `Skuttoo.slnx`** so the Azure Speech SDK and its native libraries
stay out of the deployed web app and the test build.

## What it does

1. Plans the clips from the seed content (`ClipPlanner.PlanFromSeed()` in `Skuttoo.Infrastructure`):
   every exercise prompt, taught word (target) and answer-choice label, in both `sv` and `en`,
   deduplicated by `(locale, path)`.
2. Synthesizes each with Azure Neural TTS (Swedish `sv-SE-SofieNeural`, English `en-US-AnaNeural`)
   and writes a 24 kHz mono mp3 to the matching path under the output root, e.g.
   `frontend/public/assets/audio/sv/math-count-apples.mp3`.
3. Idempotent: existing files are skipped (use `--force` to regenerate).

## Secrets (never committed)

Provide an Azure Cognitive Services **Speech** key + region via environment variables:

```powershell
$env:Speech__Key = "<your-speech-key>"
$env:Speech__Region = "swedencentral"   # your resource's region
```

## Run

```bash
# From the repo root. Writes into frontend/public by default.
dotnet run --project backend/tools/Skuttoo.AudioGen -- --out ./frontend/public

# Force-regenerate everything:
dotnet run --project backend/tools/Skuttoo.AudioGen -- --out ./frontend/public --force
```

Without a key/region set, it prints the planned clip count and exits without writing (a safe dry run).

The generated mp3s are committed to this public repo (they are not secret); the key is not.
At runtime the app plays a clip when present and otherwise falls back to browser `SpeechSynthesis`.
