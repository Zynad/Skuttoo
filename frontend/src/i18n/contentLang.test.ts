import { describe, expect, it } from 'vitest';
import { contentLangFor } from './contentLang';

describe('contentLangFor', () => {
  it('uses the content language when the island sets one', () => {
    expect(contentLangFor('en', 'sv')).toBe('en');
    expect(contentLangFor('sv', 'en')).toBe('sv');
  });

  it('falls back to the UI language when the island has none', () => {
    expect(contentLangFor(null, 'sv')).toBe('sv');
    expect(contentLangFor(null, 'en')).toBe('en');
  });
});
