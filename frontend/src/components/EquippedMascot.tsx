import { Mascot, type MascotProps } from '../game/Mascot';
import { useProgress } from '../hooks/useProgress';
import { cosmeticById } from '../game/cosmeticsCatalog';

export type EquippedMascotProps = Omit<MascotProps, 'fill' | 'fillSoft' | 'accessory'>;

/**
 * Renders Skutt with the child's currently equipped colour + accessory applied. A thin wrapper
 * so every mascot in the app reflects cosmetics without each call site repeating the lookup.
 */
export function EquippedMascot(props: EquippedMascotProps) {
  const { profile } = useProgress();
  const colour = cosmeticById(profile.equipped.mascotColor);
  const accessory = cosmeticById(profile.equipped.mascotAccessory);

  return (
    <Mascot
      {...props}
      fill={colour?.swatch}
      fillSoft={colour?.swatchSoft}
      accessory={accessory?.preview}
    />
  );
}
