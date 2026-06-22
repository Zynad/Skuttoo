import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import type { Lang } from '../i18n/dictionaries';
import { LanguageContext, type LanguageContextValue } from './languageContext';

const STORAGE_KEY = 'skuttoo.lang';

const readInitialLang = (): Lang => {
  if (typeof localStorage !== 'undefined') {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (stored === 'sv' || stored === 'en') {
      return stored;
    }
  }
  return 'sv';
};

export interface LanguageProviderProps {
  children: ReactNode;
  /** Override the starting language (used in tests). */
  initialLang?: Lang;
}

export function LanguageProvider({ children, initialLang }: LanguageProviderProps) {
  const [lang, setLangState] = useState<Lang>(() => initialLang ?? readInitialLang());

  useEffect(() => {
    if (typeof document !== 'undefined') {
      document.documentElement.lang = lang;
    }
    if (typeof localStorage !== 'undefined') {
      localStorage.setItem(STORAGE_KEY, lang);
    }
  }, [lang]);

  const setLang = useCallback((next: Lang) => setLangState(next), []);
  const toggle = useCallback(() => setLangState((current) => (current === 'sv' ? 'en' : 'sv')), []);

  const value = useMemo<LanguageContextValue>(() => ({ lang, setLang, toggle }), [lang, setLang, toggle]);

  return <LanguageContext.Provider value={value}>{children}</LanguageContext.Provider>;
}
