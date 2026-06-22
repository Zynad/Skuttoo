import { describe, expect, it } from 'vitest';
import sv from './sv.json';
import en from './en.json';

describe('i18n dictionaries', () => {
  it('sv.json and en.json have identical key sets', () => {
    const svKeys = Object.keys(sv).sort();
    const enKeys = Object.keys(en).sort();

    const missingInEn = svKeys.filter((k) => !enKeys.includes(k));
    const missingInSv = enKeys.filter((k) => !svKeys.includes(k));

    expect(missingInEn, `keys missing in en.json: ${missingInEn.join(', ')}`).toEqual([]);
    expect(missingInSv, `keys missing in sv.json: ${missingInSv.join(', ')}`).toEqual([]);
    expect(svKeys).toEqual(enKeys);
  });

  it('has no empty translation values', () => {
    for (const [key, value] of Object.entries(sv)) {
      expect(value, `sv.${key} is empty`).not.toBe('');
    }
    for (const [key, value] of Object.entries(en)) {
      expect(value, `en.${key} is empty`).not.toBe('');
    }
  });
});
