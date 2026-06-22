/**
 * Plays a short, cheerful success chime using the Web Audio API — no audio asset
 * required, works offline. A gentle ascending arpeggio. Safely no-ops when the
 * Web Audio API is unavailable (e.g. jsdom) or blocked.
 */
export function successSound(): void {
  type AudioCtor = typeof AudioContext;
  const w = window as unknown as { AudioContext?: AudioCtor; webkitAudioContext?: AudioCtor };
  const Ctor = w.AudioContext ?? w.webkitAudioContext;
  if (!Ctor) {
    return;
  }

  try {
    const ctx = new Ctor();
    const now = ctx.currentTime;
    // C5, E5, G5 — a happy major triad.
    const notes = [523.25, 659.25, 783.99];

    notes.forEach((freq, i) => {
      const osc = ctx.createOscillator();
      const gain = ctx.createGain();
      osc.type = 'triangle';
      osc.frequency.value = freq;
      const start = now + i * 0.12;
      const end = start + 0.18;
      gain.gain.setValueAtTime(0.0001, start);
      gain.gain.exponentialRampToValueAtTime(0.25, start + 0.02);
      gain.gain.exponentialRampToValueAtTime(0.0001, end);
      osc.connect(gain).connect(ctx.destination);
      osc.start(start);
      osc.stop(end + 0.02);
    });

    window.setTimeout(() => void ctx.close(), 700);
  } catch {
    // ignore — sound is a nice-to-have
  }
}
