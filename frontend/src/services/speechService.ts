import type { Lang } from '../i18n/dictionaries';

/**
 * Audio playback for read-aloud. Strategy:
 *   1. If a pre-generated clip path is provided AND the file exists, play it.
 *   2. Otherwise fall back to the browser SpeechSynthesis API in the active language.
 * Pre-generated clips don't exist yet in development, so the fallback is the norm.
 */

export interface SpeakOptions {
  /** Localized text spoken by the fallback synthesizer. */
  text: string;
  /** Active language — picks the clip and the synthesizer voice/locale. */
  lang: Lang;
  /** Optional relative path to a pre-generated clip (e.g. "assets/audio/sv/ex-1.mp3"). */
  clip?: string | null;
}

const localeFor = (lang: Lang): string => (lang === 'sv' ? 'sv-SE' : 'en-US');

const toUrl = (clip: string): string => (clip.startsWith('/') ? clip : `/${clip}`);

/** Resolves true only if the clip URL returns a successful response. */
async function clipExists(url: string): Promise<boolean> {
  if (typeof fetch !== 'function') {
    return false;
  }
  try {
    const res = await fetch(url, { method: 'HEAD' });
    return res.ok;
  } catch {
    return false;
  }
}

function playClip(url: string): Promise<void> {
  return new Promise<void>((resolve, reject) => {
    const audio = new Audio(url);
    audio.addEventListener('ended', () => resolve(), { once: true });
    audio.addEventListener('error', () => reject(new Error('clip failed to play')), { once: true });
    void audio.play().catch(reject);
  });
}

function speakWithSynthesis(text: string, lang: Lang): void {
  if (typeof window === 'undefined' || !('speechSynthesis' in window)) {
    return;
  }
  const synth = window.speechSynthesis;
  synth.cancel();
  const utterance = new SpeechSynthesisUtterance(text);
  utterance.lang = localeFor(lang);
  utterance.rate = 0.95;
  utterance.pitch = 1.1;
  synth.speak(utterance);
}

/**
 * Speaks the prompt: pre-generated clip when available, else SpeechSynthesis.
 * Returns the channel actually used (useful for tests/telemetry).
 */
export async function speak({ text, lang, clip }: SpeakOptions): Promise<'clip' | 'synthesis'> {
  if (clip) {
    const url = toUrl(clip);
    if (await clipExists(url)) {
      try {
        await playClip(url);
        return 'clip';
      } catch {
        // fall through to synthesis
      }
    }
  }
  speakWithSynthesis(text, lang);
  return 'synthesis';
}

/** Stops any in-progress synthesized speech. */
export function stopSpeaking(): void {
  if (typeof window !== 'undefined' && 'speechSynthesis' in window) {
    window.speechSynthesis.cancel();
  }
}
