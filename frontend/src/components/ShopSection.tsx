import { useT } from '../i18n/useT';
import { useProgress } from '../hooks/useProgress';
import { Button } from './Button';
import { COSMETICS, COSMETIC_CATEGORY_ORDER } from '../game/cosmeticsCatalog';
import { canAfford, isOwned } from '../utils/shopLogic';
import type { CosmeticCategory, CosmeticItem } from '../types/cosmetics';

const categoryNameKey = {
  mascotColor: 'shop.category.mascotColor',
  mascotAccessory: 'shop.category.mascotAccessory',
  sticker: 'shop.category.sticker',
  mapDecoration: 'shop.category.mapDecoration',
} as const;

const isEquippable = (category: CosmeticCategory): category is 'mascotColor' | 'mascotAccessory' =>
  category === 'mascotColor' || category === 'mascotAccessory';

/** The reward shop: buy cosmetics with coins, equip colours/accessories on Skutt. */
export function ShopSection() {
  const t = useT();
  const { profile, purchaseCosmetic, equipCosmetic } = useProgress();

  const equippedId = (category: 'mascotColor' | 'mascotAccessory'): string | null =>
    category === 'mascotColor' ? profile.equipped.mascotColor : profile.equipped.mascotAccessory;

  const renderItem = (item: CosmeticItem) => {
    const owned = isOwned(profile, item);
    const equippable = isEquippable(item.category);
    const equipped = isEquippable(item.category) && equippedId(item.category) === item.id;

    return (
      <div
        key={item.id}
        data-testid={`shop-item-${item.id}`}
        data-owned={owned}
        data-equipped={equipped}
        className="flex flex-col items-center gap-2 rounded-2xl bg-[var(--color-surface-raised)] p-3 text-center shadow-[var(--shadow-soft)]"
      >
        {item.category === 'mascotColor' ? (
          <span
            aria-hidden="true"
            className="h-10 w-10 rounded-full border-2 border-[var(--color-surface)]"
            style={{ backgroundColor: item.swatch }}
          />
        ) : (
          <span aria-hidden="true" className="text-3xl">
            {item.preview}
          </span>
        )}

        <span className="font-display text-sm font-bold text-[var(--color-text)]">{t(item.nameKey)}</span>

        {!owned ? (
          <Button
            size="md"
            variant="primary"
            disabled={!canAfford(profile, item)}
            onClick={() => void purchaseCosmetic(item.id)}
          >
            {canAfford(profile, item) ? `${t('shop.buy')} · ${t('shop.cost', { coins: item.cost })}` : t('shop.cantAfford')}
          </Button>
        ) : equippable ? (
          <Button
            size="md"
            variant={equipped ? 'ghost' : 'secondary'}
            onClick={() =>
              void equipCosmetic(
                item.category as 'mascotColor' | 'mascotAccessory',
                equipped && item.category === 'mascotAccessory' ? null : item.id,
              )
            }
          >
            {equipped ? `✓ ${t('shop.equipped')}` : t('shop.equip')}
          </Button>
        ) : (
          <span className="font-display text-xs font-bold text-[var(--color-text-soft)]">✓ {t('shop.owned')}</span>
        )}
      </div>
    );
  };

  return (
    <section data-testid="shop">
      <h2 className="text-center font-display text-xl font-extrabold text-[var(--color-text-soft)]">{t('shop.title')}</h2>
      {COSMETIC_CATEGORY_ORDER.map((category) => (
        <div key={category} className="mt-4">
          <h3 className="font-display text-sm font-bold text-[var(--color-text-soft)]">{t(categoryNameKey[category])}</h3>
          <div className="mt-2 grid grid-cols-2 gap-3 sm:grid-cols-4">
            {COSMETICS.filter((c) => c.category === category).map(renderItem)}
          </div>
        </div>
      ))}
    </section>
  );
}
