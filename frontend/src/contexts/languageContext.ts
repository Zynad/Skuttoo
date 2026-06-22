import { createContext } from 'react';
import type { Lang } from '../i18n/dictionaries';

export interface LanguageContextValue {
  lang: Lang;
  setLang: (lang: Lang) => void;
  toggle: () => void;
}

export const LanguageContext = createContext<LanguageContextValue | undefined>(undefined);
