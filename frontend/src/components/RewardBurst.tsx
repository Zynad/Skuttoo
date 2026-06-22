import type { CSSProperties } from 'react';
import { useT } from '../i18n/useT';

export interface RewardBurstProps {
  coins: number;
  stars: number;
}

interface Particle {
  id: number;
  symbol: string;
  color: string;
  left: number;
  delay: number;
  spin: number;
}

const buildParticles = (coins: number, stars: number): Particle[] => {
  const particles: Particle[] = [];
  const coinCount = Math.min(coins, 8);
  const starCount = Math.min(stars, 5);
  let id = 0;
  for (let i = 0; i < coinCount; i += 1) {
    particles.push({
      id: id++,
      symbol: '●',
      color: 'var(--color-coin)',
      left: 12 + (i / Math.max(coinCount, 1)) * 70,
      delay: i * 0.06,
      spin: (i % 2 === 0 ? 1 : -1) * (15 + i * 8),
    });
  }
  for (let i = 0; i < starCount; i += 1) {
    particles.push({
      id: id++,
      symbol: '★',
      color: 'var(--color-star)',
      left: 22 + (i / Math.max(starCount, 1)) * 56,
      delay: 0.1 + i * 0.08,
      spin: (i % 2 === 0 ? -1 : 1) * (20 + i * 6),
    });
  }
  return particles;
};

/**
 * Celebratory burst of coins and stars on a correct answer. Decorative — the
 * actual amounts are announced separately for assistive tech / tests.
 */
export function RewardBurst({ coins, stars }: RewardBurstProps) {
  const t = useT();
  const particles = buildParticles(coins, stars);

  return (
    <div
      className="pointer-events-none absolute inset-0 z-20 overflow-hidden"
      data-testid="reward-burst"
      data-coins={coins}
      data-stars={stars}
    >
      <span className="sr-only" role="status">
        {t('reward.earnedCoins', { coins })} {t('reward.earnedStars', { stars })}
      </span>
      {particles.map((p) => (
        <span
          key={p.id}
          aria-hidden="true"
          className="absolute bottom-1/3 text-3xl animate-float-up"
          style={
            {
              left: `${p.left}%`,
              color: p.color,
              animationDelay: `${p.delay}s`,
              '--spin': `${p.spin}deg`,
            } as CSSProperties
          }
        >
          {p.symbol}
        </span>
      ))}
    </div>
  );
}
