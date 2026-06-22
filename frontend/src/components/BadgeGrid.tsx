import { useT } from '../i18n/useT';
import { useLanguage } from '../hooks/useLanguage';
import { useProgress } from '../hooks/useProgress';

export interface BadgeGridProps {
  /** Keys the child has earned; the rest render locked/dimmed. */
  earnedKeys: string[];
}

/** The badge gallery: every catalogue badge, earned ones lit, the rest dimmed. */
export function BadgeGrid({ earnedKeys }: BadgeGridProps) {
  const t = useT();
  const { lang } = useLanguage();
  const { badgeCatalog } = useProgress();
  const earned = new Set(earnedKeys);

  return (
    <div data-testid="profile-badges" className="grid grid-cols-2 gap-3 sm:grid-cols-4">
      {badgeCatalog.map((badge) => {
        const isEarned = earned.has(badge.key);
        return (
          <div
            key={badge.key}
            data-testid={`badge-${badge.key}`}
            data-earned={isEarned}
            title={badge.description[lang]}
            className={`flex flex-col items-center gap-1 rounded-2xl bg-[var(--color-surface-raised)] p-3 text-center shadow-[var(--shadow-soft)] ${
              isEarned ? '' : 'opacity-40 grayscale'
            }`.trim()}
          >
            <span className="text-3xl" aria-hidden="true">
              {badge.iconRef}
            </span>
            <span className="font-display text-xs font-bold text-[var(--color-text)]">{badge.name[lang]}</span>
            <span className="sr-only">{isEarned ? t('badges.earned') : t('badges.locked')}</span>
          </div>
        );
      })}
    </div>
  );
}
