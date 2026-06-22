# Pre-generated audio

This folder will hold pre-generated Azure Neural TTS clips, organised by locale:

```
assets/audio/sv/<key>.mp3
assets/audio/en/<key>.mp3
```

Clips are produced by the **`Skuttoo.AudioGen`** tool (`backend/tools/Skuttoo.AudioGen`),
which plans every clip from the seed content and synthesizes it with Azure Neural TTS
(Swedish `sv-SE-SofieNeural`, English `en-US-AnaNeural`). The Azure Speech key is NEVER in
the frontend; the generated `.mp3` files are committed here as static assets. To (re)generate:

```bash
# Set the key/region (never committed), then run the tool from the repo root:
#   $env:Speech__Key = "..."; $env:Speech__Region = "swedencentral"
dotnet run --project backend/tools/Skuttoo.AudioGen -- --out ./frontend/public
```

Until clips exist, the client falls back to the browser `SpeechSynthesis` API in
the active language — see `src/services/speechService.ts`. No clip is required
for development.
