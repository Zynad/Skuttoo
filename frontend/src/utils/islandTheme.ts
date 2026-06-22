import type { SubjectKey } from '../types/content';
import type { TranslationKey } from '../i18n/dictionaries';

/**
 * Static per-island presentation derived from semantic tokens. Components read
 * CSS custom properties via these var() strings — never raw hex. `motif` is a
 * playful emoji placeholder until final mascot/island art lands.
 */
export interface IslandTheme {
  key: SubjectKey;
  /** CSS var() reference to the island base colour token. */
  color: string;
  colorSoft: string;
  colorAccent: string;
  motif: string;
  nameKey: TranslationKey;
  descKey: TranslationKey;
}

export const ISLAND_ORDER: SubjectKey[] = ['math', 'swedish', 'english', 'logic'];

export const islandThemes: Record<SubjectKey, IslandTheme> = {
  math: {
    key: 'math',
    color: 'var(--island-math)',
    colorSoft: 'var(--island-math-soft)',
    colorAccent: 'var(--island-math-accent)',
    motif: '🚀',
    nameKey: 'island.math.name',
    descKey: 'island.math.desc',
  },
  swedish: {
    key: 'swedish',
    color: 'var(--island-swedish)',
    colorSoft: 'var(--island-swedish-soft)',
    colorAccent: 'var(--island-swedish-accent)',
    motif: '🌲',
    nameKey: 'island.swedish.name',
    descKey: 'island.swedish.desc',
  },
  english: {
    key: 'english',
    color: 'var(--island-english)',
    colorSoft: 'var(--island-english-soft)',
    colorAccent: 'var(--island-english-accent)',
    motif: '✈️',
    nameKey: 'island.english.name',
    descKey: 'island.english.desc',
  },
  logic: {
    key: 'logic',
    color: 'var(--island-logic)',
    colorSoft: 'var(--island-logic-soft)',
    colorAccent: 'var(--island-logic-accent)',
    motif: '🦜',
    nameKey: 'island.logic.name',
    descKey: 'island.logic.desc',
  },
};

export const isSubjectKey = (value: string): value is SubjectKey =>
  value === 'math' || value === 'swedish' || value === 'english' || value === 'logic';
