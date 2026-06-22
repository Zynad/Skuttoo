import { describe, expect, it } from 'vitest';
import { normalizeProfile } from './normalizeProfile';
import { createDefaultProfile, DEFAULT_MASCOT_COLOR } from '../types/progress';

describe('normalizeProfile', () => {
  it('returns a default profile for non-object input', () => {
    expect(normalizeProfile(null)).toEqual(createDefaultProfile());
    expect(normalizeProfile(undefined)).toEqual(createDefaultProfile());
    expect(normalizeProfile('nope')).toEqual(createDefaultProfile());
  });

  it('upgrades an old-shape profile (no cosmetics fields) to a valid Profile', () => {
    // A profile saved before cosmetics existed: lacks ownedCosmetics + equipped.
    const old = {
      id: 'local',
      name: 'Skutt',
      ageBand: '6-9',
      avatar: 'fox',
      coins: 30,
      stars: 8,
      badgeKeys: ['first-hops'],
      streak: { count: 2, lastPlayedDate: '2026-06-20' },
      results: [{ exerciseId: 1, completed: true, starsEarned: 3, attempts: 1, lastPlayedAt: '2026-06-20T10:00:00.000Z' }],
    };

    const result = normalizeProfile(old);

    expect(result.coins).toBe(30);
    expect(result.stars).toBe(8);
    expect(result.badgeKeys).toEqual(['first-hops']);
    expect(result.ownedCosmetics).toEqual([DEFAULT_MASCOT_COLOR]);
    expect(result.equipped).toEqual({ mascotColor: DEFAULT_MASCOT_COLOR, mascotAccessory: null });
    expect(result.results).toHaveLength(1);
    expect(result.results[0].exerciseId).toBe(1);
  });

  it('is idempotent', () => {
    const once = normalizeProfile({ coins: 5, badgeKeys: ['a', 'a'] });
    const twice = normalizeProfile(once);
    expect(twice).toEqual(once);
  });

  it('always keeps the default mascot colour owned', () => {
    const result = normalizeProfile({ ownedCosmetics: ['mascot-color-forest'] });
    expect(result.ownedCosmetics).toContain(DEFAULT_MASCOT_COLOR);
    expect(result.ownedCosmetics).toContain('mascot-color-forest');
  });

  it('resets an equipped colour that is not owned back to the default', () => {
    const result = normalizeProfile({
      ownedCosmetics: ['mascot-color-forest'],
      equipped: { mascotColor: 'mascot-color-ocean', mascotAccessory: 'accessory-hat' },
    });
    // ocean colour + hat accessory are not owned → fall back.
    expect(result.equipped.mascotColor).toBe(DEFAULT_MASCOT_COLOR);
    expect(result.equipped.mascotAccessory).toBeNull();
  });

  it('keeps an equipped cosmetic that is owned', () => {
    const result = normalizeProfile({
      ownedCosmetics: ['mascot-color-forest', 'accessory-bow'],
      equipped: { mascotColor: 'mascot-color-forest', mascotAccessory: 'accessory-bow' },
    });
    expect(result.equipped).toEqual({ mascotColor: 'mascot-color-forest', mascotAccessory: 'accessory-bow' });
  });

  it('coerces completion sets and drops invalid entries', () => {
    const result = normalizeProfile({
      completedLevelIds: [1, 2, 2, 'x', null],
      completedSubjectKeys: ['math', 'bogus', 'logic'],
    });
    expect(result.completedLevelIds).toEqual([1, 2]);
    expect(result.completedSubjectKeys).toEqual(['math', 'logic']);
  });

  it('drops malformed results and coerces bad scalars', () => {
    const result = normalizeProfile({
      coins: -5,
      stars: 'lots',
      badgeKeys: ['ok', 42, null],
      results: [{ exerciseId: 7, completed: true }, 'garbage', { noId: true }],
    });
    expect(result.coins).toBe(0);
    expect(result.stars).toBe(0);
    expect(result.badgeKeys).toEqual(['ok']);
    expect(result.results).toHaveLength(1);
    expect(result.results[0].exerciseId).toBe(7);
    expect(result.results[0].attempts).toBe(0);
  });
});
