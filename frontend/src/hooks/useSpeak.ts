import { useCallback, useEffect, useState } from 'react';
import { useLanguage } from './useLanguage';
import { speak, stopSpeaking } from '../services/speechService';

export interface UseSpeakResult {
  /** True while a read-aloud is in progress. */
  speaking: boolean;
  /** Speak the given text, optionally backed by a pre-generated clip path. */
  say: (text: string, clip?: string | null) => Promise<void>;
  /** Cancel any in-progress speech. */
  stop: () => void;
}

/** Read-aloud bound to the active language; plays a clip if present, else SpeechSynthesis. */
export function useSpeak(): UseSpeakResult {
  const { lang } = useLanguage();
  const [speaking, setSpeaking] = useState(false);

  const say = useCallback(
    async (text: string, clip?: string | null) => {
      setSpeaking(true);
      try {
        await speak({ text, lang, clip });
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
