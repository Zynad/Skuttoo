import type { TranslationKey } from '../i18n/dictionaries';

/** Cosmetic kinds sold in the reward shop. Colours + accessories are equipped on the
 * mascot; stickers + map decorations are collectibles the child owns. */
export type CosmeticCategory = 'mascotColor' | 'mascotAccessory' | 'sticker' | 'mapDecoration';

/** The equippable subset of cosmetic categories. */
export type EquippableCategory = 'mascotColor' | 'mascotAccessory';

export interface CosmeticItem {
  id: string;
  category: CosmeticCategory;
  /** Price in coins (0 = owned for free). */
  cost: number;
  nameKey: TranslationKey;
  /** mascotColor: the main fill token (a CSS var, never raw hex). */
  swatch?: string;
  /** mascotColor: the soft-accent fill token. */
  swatchSoft?: string;
  /** accessories / stickers / decorations: an emoji preview (and overlay for accessories). */
  preview?: string;
}
