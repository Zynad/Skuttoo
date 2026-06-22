import { useT } from '../i18n/useT';

export type MascotState = 'idle' | 'talking' | 'happy' | 'encouraging' | 'celebrate';

export interface MascotProps {
  state?: MascotState;
  /** Pixel size of the square mascot. */
  size?: number;
  className?: string;
  /** Main fur colour (a CSS var token). Defaults to the brand primary. */
  fill?: string;
  /** Soft-accent colour (a CSS var token) for the inner ears. */
  fillSoft?: string;
  /** Optional accessory emoji rendered over the head (hat/bow/glasses). */
  accessory?: string;
}

const stateLabelKey = {
  idle: 'mascot.idle',
  talking: 'mascot.talking',
  happy: 'mascot.happy',
  encouraging: 'mascot.encouraging',
  celebrate: 'mascot.celebrate',
} as const;

/** CSS animation class per state. All are neutralised under prefers-reduced-motion (index.css). */
const stateAnimation: Record<MascotState, string> = {
  idle: 'animate-bob',
  talking: 'animate-talk',
  happy: 'animate-hop',
  encouraging: 'animate-wave',
  celebrate: 'animate-celebrate',
};

/**
 * Skutt — the friendly hopping fox-ish mascot. A self-contained SVG placeholder
 * with simple expressive states (idle/talking/happy/encouraging). No external art.
 */
export function Mascot({
  state = 'idle',
  size = 96,
  className = '',
  fill = 'var(--color-primary)',
  fillSoft = 'var(--color-primary-soft)',
  accessory,
}: MascotProps) {
  const t = useT();
  const animation = stateAnimation[state];

  // Eyes/mouth adapt to the emotional state.
  const happy = state === 'happy' || state === 'celebrate';
  const encouraging = state === 'encouraging';
  const mouth = happy ? 'M40 64 Q60 86 80 64' : encouraging ? 'M44 70 Q60 80 76 70' : 'M46 70 Q60 78 74 70';

  return (
    <span
      className={`relative inline-block ${animation} ${className}`.trim()}
      role="img"
      aria-label={t(stateLabelKey[state])}
      data-testid="mascot"
      data-state={state}
    >
      <svg width={size} height={size} viewBox="0 0 120 120" fill="none" xmlns="http://www.w3.org/2000/svg">
        {/* shadow */}
        <ellipse cx="60" cy="110" rx="30" ry="6" fill="rgb(0 0 0 / 0.12)" />
        {/* ears */}
        <path d="M30 40 L24 8 L52 30 Z" fill={fill} />
        <path d="M90 40 L96 8 L68 30 Z" fill={fill} />
        <path d="M34 34 L31 18 L46 30 Z" fill={fillSoft} />
        <path d="M86 34 L89 18 L74 30 Z" fill={fillSoft} />
        {/* head */}
        <circle cx="60" cy="62" r="38" fill={fill} />
        {/* cheeks / muzzle */}
        <ellipse cx="60" cy="74" rx="26" ry="20" fill="var(--color-surface-raised)" />
        {/* eyes */}
        <circle cx="46" cy="56" r={happy ? 4 : 5} fill="var(--color-text)" />
        <circle cx="74" cy="56" r={happy ? 4 : 5} fill="var(--color-text)" />
        <circle cx="47.5" cy="54.5" r="1.6" fill="#fff" />
        <circle cx="75.5" cy="54.5" r="1.6" fill="#fff" />
        {/* nose */}
        <circle cx="60" cy="68" r="4.5" fill="var(--color-text)" />
        {/* mouth */}
        <path d={mouth} stroke="var(--color-text)" strokeWidth="3.5" strokeLinecap="round" fill="none" />
        {/* encouraging: a little raised paw */}
        {encouraging && <circle cx="98" cy="78" r="9" fill={fill} />}
      </svg>
      {accessory && (
        <span
          aria-hidden="true"
          data-testid="mascot-accessory"
          className="pointer-events-none absolute left-1/2 top-0 -translate-x-1/2"
          style={{ fontSize: size * 0.34, lineHeight: 1 }}
        >
          {accessory}
        </span>
      )}
    </span>
  );
}
