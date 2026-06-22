import { describe, expect, it } from 'vitest';
import { applyPurchase, canAfford, equipCosmetic, isOwned } from './shopLogic';
import { createDefaultProfile, DEFAULT_MASCOT_COLOR, type Profile } from '../types/progress';
import type { CosmeticItem } from '../types/cosmetics';

const colour: CosmeticItem = {
  id: 'mascot-color-forest',
  category: 'mascotColor',
  cost: 20,
  nameKey: 'cosmetic.mascot-color-forest.name',
  swatch: 'var(--island-swedish)',
};

const richProfile = (): Profile => ({ ...createDefaultProfile(), coins: 100 });

describe('canAfford / isOwned', () => {
  it('reports affordability against the coin balance', () => {
    expect(canAfford({ ...createDefaultProfile(), coins: 20 }, colour)).toBe(true);
    expect(canAfford({ ...createDefaultProfile(), coins: 19 }, colour)).toBe(false);
  });

  it('reports the default colour as owned out of the box', () => {
    const profile = createDefaultProfile();
    expect(profile.ownedCosmetics).toContain(DEFAULT_MASCOT_COLOR);
    expect(isOwned(profile, colour)).toBe(false);
  });
});

describe('applyPurchase', () => {
  it('deducts coins and adds the item to owned', () => {
    const next = applyPurchase(richProfile(), colour);
    expect(next.coins).toBe(80);
    expect(next.ownedCosmetics).toContain('mascot-color-forest');
  });

  it('is a no-op when unaffordable', () => {
    const poor = { ...createDefaultProfile(), coins: 5 };
    expect(applyPurchase(poor, colour)).toBe(poor);
  });

  it('is a no-op when already owned', () => {
    const owner = { ...richProfile(), ownedCosmetics: [DEFAULT_MASCOT_COLOR, 'mascot-color-forest'] };
    expect(applyPurchase(owner, colour)).toBe(owner);
  });

  it('does not mutate the input', () => {
    const profile = richProfile();
    applyPurchase(profile, colour);
    expect(profile.coins).toBe(100);
    expect(profile.ownedCosmetics).toEqual([DEFAULT_MASCOT_COLOR]);
  });
});

describe('equipCosmetic', () => {
  it('equips an owned colour', () => {
    const owner = { ...richProfile(), ownedCosmetics: [DEFAULT_MASCOT_COLOR, 'mascot-color-forest'] };
    const next = equipCosmetic(owner, 'mascotColor', 'mascot-color-forest');
    expect(next.equipped.mascotColor).toBe('mascot-color-forest');
  });

  it('refuses to equip an unowned item', () => {
    const profile = createDefaultProfile();
    expect(equipCosmetic(profile, 'mascotColor', 'mascot-color-forest')).toBe(profile);
  });

  it('equips and un-equips an accessory', () => {
    const owner = { ...richProfile(), ownedCosmetics: [DEFAULT_MASCOT_COLOR, 'accessory-hat'] };
    const withHat = equipCosmetic(owner, 'mascotAccessory', 'accessory-hat');
    expect(withHat.equipped.mascotAccessory).toBe('accessory-hat');
    const without = equipCosmetic(withHat, 'mascotAccessory', null);
    expect(without.equipped.mascotAccessory).toBeNull();
  });

  it('is a no-op when already equipped', () => {
    const owner = { ...richProfile(), ownedCosmetics: [DEFAULT_MASCOT_COLOR, 'accessory-hat'], equipped: { mascotColor: DEFAULT_MASCOT_COLOR, mascotAccessory: 'accessory-hat' } };
    expect(equipCosmetic(owner, 'mascotAccessory', 'accessory-hat')).toBe(owner);
  });

  it('does not mutate the input', () => {
    const owner = { ...richProfile(), ownedCosmetics: [DEFAULT_MASCOT_COLOR, 'mascot-color-forest'] };
    equipCosmetic(owner, 'mascotColor', 'mascot-color-forest');
    expect(owner.equipped.mascotColor).toBe(DEFAULT_MASCOT_COLOR);
  });
});
