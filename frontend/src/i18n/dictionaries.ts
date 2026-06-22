import sv from './sv.json';
import en from './en.json';

export type Lang = 'sv' | 'en';

/** A flat key -> string dictionary. sv is the source of truth for the key set. */
export type Dictionary = Record<string, string>;

export const dictionaries: Record<Lang, Dictionary> = {
  sv: sv,
  en: en,
};

/** Keys available across both dictionaries (typed from the Swedish source). */
export type TranslationKey = keyof typeof sv;
