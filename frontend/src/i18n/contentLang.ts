import type { Lang } from './dictionaries';

/**
 * Resolves the language the exercise CONTENT (taught word, answer labels, their audio) should
 * use. A language island sets its content language (English → 'en'); everything else follows
 * the child's UI language. Instructions, helper text and praise always use the UI language.
 */
export function contentLangFor(contentLanguage: Lang | null, uiLang: Lang): Lang {
  return contentLanguage ?? uiLang;
}
