import { useCallback, useEffect, useState } from 'react';
import { useLanguage } from './useLanguage';
import { speak, stopSpeaking } from '../services/speechService';
import type { Lang } from '../i18n/dictionaries';

/** Options for a single read-aloud, overriding the active UI language when needed. */
export interface SayOptions {
  /** Optional pre-generated clip path. */
  clip?: string | null;
  /** Language to speak in; defaults to the active UI language. */
  lang?: Lang;
}

export interface UseSpeakResult {
  /** True while a read-aloud is in progress. */
  speaking: boolean;
  /**
   * Speak the given text. Pass a clip path or an options object; `lang` lets content
   * (e.g. an English word on the English island) be spoken in its own language while
   * the UI stays in another.
   */
  say: (text: string, clipOrOptions?: string | null | SayOptions) => Promise<void>;
  /** Cancel any in-progress speech. */
  stop: () => void;
}

/** Read-aloud bound to the active language; plays a clip if present, else SpeechSynthesis. */
export function useSpeak(): UseSpeakResult {
  const { lang } = useLanguage();
  const [speaking, setSpeaking] = useState(false);

  const say = useCallback(
    async (text: string, clipOrOptions?: string | null | SayOptions) => {
      const options: SayOptions =
        typeof clipOrOptions === 'object' && clipOrOptions !== null ? clipOrOptions : { clip: clipOrOptions };
      setSpeaking(true);
      try {
        await speak({ text, lang: options.lang ?? lang, clip: options.clip });
      } finally {
        setSpeaking(false);
      }
    },
    [lang],
  );

  const stop = useCallback(() => {
    stopSpeaking();
    setSpeaking(false);
  }, []);

  useEffect(() => () => stopSpeaking(), []);

  return { speaking, say, stop };
}
