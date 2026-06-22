import { useCallback } from 'react';
import { useLanguage } from '../hooks/useLanguage';
import { dictionaries, type TranslationKey } from './dictionaries';

/** Replaces {placeholder} tokens with provided values. */
export type TParams = Record<string, string | number>;

const interpolate = (template: string, params?: TParams): string => {
  if (!params) {
    return template;
  }
  return template.replace(/\{(\w+)\}/g, (match, name: string) => {
    const value = params[name];
    return value === undefined ? match : String(value);
  });
};

export type TFunction = (key: TranslationKey, params?: TParams) => string;

/** Returns a translate function bound to the active language. */
export function useT(): TFunction {
  const { lang } = useLanguage();
  return useCallback(
    (key: TranslationKey, params?: TParams) => {
      const dict = dictionaries[lang];
      const template = dict[key] ?? key;
      return interpolate(template, params);
    },
    [lang],
  );
}
