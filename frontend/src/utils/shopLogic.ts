import type { CosmeticItem, EquippableCategory } from '../types/cosmetics';
import type { Profile } from '../types/progress';

/** Whether the child has enough coins for an item. */
export function canAfford(profile: Profile, item: CosmeticItem): boolean {
  return profile.coins >= item.cost;
}

/** Whether the child already owns an item. */
export function isOwned(profile: Profile, item: CosmeticItem): boolean {
  return profile.ownedCosmetics.includes(item.id);
}

/**
 * Buys a cosmetic: deducts coins and adds it to the owned set. No-op (returns the same
 * profile) if it is already owned or unaffordable. Pure — never mutates the input.
 */
export function applyPurchase(profile: Profile, item: CosmeticItem): Profile {
  if (isOwned(profile, item) || !canAfford(profile, item)) {
    return profile;
  }
  return {
    ...profile,
    coins: profile.coins - item.cost,
    ownedCosmetics: [...profile.ownedCosmetics, item.id],
  };
}

/**
 * Equips (or, with `id === null`, un-equips) a cosmetic in the given slot. Requires the
 * item to be owned. Pure — never mutates the input.
 */
export function equipCosmetic(profile: Profile, category: EquippableCategory, id: string | null): Profile {
  if (id !== null && !profile.ownedCosmetics.includes(id)) {
    return profile;
  }
  const key = category === 'mascotColor' ? 'mascotColor' : 'mascotAccessory';
  if (profile.equipped[key] === id) {
    return profile;
  }
  return { ...profile, equipped: { ...profile.equipped, [key]: id } };
}
