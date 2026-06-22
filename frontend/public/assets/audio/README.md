# Pre-generated audio

This folder will hold pre-generated Azure Neural TTS clips, organised by locale:

```
assets/audio/sv/<key>.mp3
assets/audio/en/<key>.mp3
```

Clips are produced by the backend TTS generator (server-side; the Azure Speech
key is NEVER in the frontend) and committed as static files.

Until clips exist, the client falls back to the browser `SpeechSynthesis` API in
the active language — see `src/services/speechService.ts`. No clip is required
for development.
