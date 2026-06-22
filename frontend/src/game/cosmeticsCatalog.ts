import type { CosmeticItem } from '../types/cosmetics';
import { DEFAULT_MASCOT_COLOR } from '../types/progress';

/**
 * The authored cosmetic catalogue (client-side; cosmetics carry no server state in the MVP).
 * Colours reuse the island colour tokens so they respect dark mode automatically — no raw hex.
 * All items are purely cosmetic and bought with coins (ADR-009: no pay-to-win, no gating).
 */
export const COSMETICS: CosmeticItem[] = [
  // Mascot colours (equippable). The default is owned for free.
  { id: DEFAULT_MASCOT_COLOR, category: 'mascotColor', cost: 0, nameKey: 'cosmetic.mascot-color-default.name', swatch: 'var(--color-primary)', swatchSoft: 'var(--color-primary-soft)' },
  { id: 'mascot-color-blueberry', category: 'mascotColor', cost: 20, nameKey: 'cosmetic.mascot-color-blueberry.name', swatch: 'var(--island-math)', swatchSoft: 'var(--island-math-soft)' },
  { id: 'mascot-color-forest', category: 'mascotColor', cost: 20, nameKey: 'cosmetic.mascot-color-forest.name', swatch: 'var(--island-swedish)', swatchSoft: 'var(--island-swedish-soft)' },
  { id: 'mascot-color-ocean', category: 'mascotColor', cost: 20, nameKey: 'cosmetic.mascot-color-ocean.name', swatch: 'var(--island-english)', swatchSoft: 'var(--island-english-soft)' },
  { id: 'mascot-color-amber', category: 'mascotColor', cost: 20, nameKey: 'cosmetic.mascot-color-amber.name', swatch: 'var(--island-logic)', swatchSoft: 'var(--island-logic-soft)' },

  // Mascot accessories (equippable, emoji overlay).
  { id: 'accessory-hat', category: 'mascotAccessory', cost: 25, nameKey: 'cosmetic.accessory-hat.name', preview: '🎩' },
  { id: 'accessory-bow', category: 'mascotAccessory', cost: 25, nameKey: 'cosmetic.accessory-bow.name', preview: '🎀' },
  { id: 'accessory-glasses', category: 'mascotAccessory', cost: 25, nameKey: 'cosmetic.accessory-glasses.name', preview: '🕶️' },

  // Stickers (collectibles).
  { id: 'sticker-star', category: 'sticker', cost: 15, nameKey: 'cosmetic.sticker-star.name', preview: '⭐' },
  { id: 'sticker-rocket', category: 'sticker', cost: 15, nameKey: 'cosmetic.sticker-rocket.name', preview: '🚀' },
  { id: 'sticker-rainbow', category: 'sticker', cost: 15, nameKey: 'cosmetic.sticker-rainbow.name', preview: '🌈' },

  // Map decorations (collectibles).
  { id: 'decoration-balloons', category: 'mapDecoration', cost: 30, nameKey: 'cosmetic.decoration-balloons.name', preview: '🎈' },
  { id: 'decoration-flowers', category: 'mapDecoration', cost: 30, nameKey: 'cosmetic.decoration-flowers.name', preview: '🌸' },
];

/** Look up a cosmetic by id. */
export const cosmeticById = (id: string | null): CosmeticItem | undefined =>
  id === null ? undefined : COSMETICS.find((c) => c.id === id);

/** Display order of the categories in the shop. */
export const COSMETIC_CATEGORY_ORDER: CosmeticItem['category'][] = [
  'mascotColor',
  'mascotAccessory',
  'sticker',
  'mapDecoration',
];
