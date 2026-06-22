import { useT } from '../i18n/useT';
import { useLanguage } from '../hooks/useLanguage';
import { useProgress } from '../hooks/useProgress';
import { Mascot } from '../game/Mascot';

export interface BadgeCelebrationProps {
  /** Keys of badges just earned; nothing renders when empty. */
  badgeKeys: string[];
}

/** A gentle, non-blocking toast announcing newly earned badges (with Skutt cheering). */
export function BadgeCelebration({ badgeKeys }: BadgeCelebrationProps) {
  const t = useT();
  const { lang } = useLanguage();
  const { badgeCatalog } = useProgress();

  if (badgeKeys.length === 0) {
    return null;
  }
  const earned = badgeCatalog.filter((b) => badgeKeys.includes(b.key));
  if (earned.length === 0) {
    return null;
  }

  return (
    <div
      className="pointer-events-none fixed inset-x-0 top-20 z-40 flex justify-center px-4"
      data-testid="badge-celebration"
      role="status"
    >
      <div className="flex items-center gap-3 rounded-3xl bg-[var(--color-surface-raised)] px-5 py-3 shadow-[var(--shadow-soft)]">
        <Mascot state="celebrate" size={56} />
        <div className="text-left">
          <p className="font-display text-sm font-bold text-[var(--color-text-soft)]">{t('badges.newlyEarned')}</p>
          {earned.map((b) => (
            <p key={b.key} className="font-display text-lg font-extrabold text-[var(--color-text)]">
              <span aria-hidden="true">{b.iconRef} </span>
              {b.name[lang]}
            </p>
          ))}
        </div>
      </div>
    </div>
  );
}
