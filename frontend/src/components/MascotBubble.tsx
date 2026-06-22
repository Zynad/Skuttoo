import type { ReactNode } from 'react';
import type { MascotState } from '../game/Mascot';
import { EquippedMascot } from './EquippedMascot';
import { AudioButton } from './AudioButton';

export interface MascotBubbleProps {
  /** Speech-bubble message (already localized). */
  message: string;
  state?: MascotState;
  /** When true, shows a read-aloud button for the message. */
  withAudio?: boolean;
  /** Optional pre-generated clip for the message. */
  clip?: string | null;
  mascotSize?: number;
  children?: ReactNode;
}

/** Skutt with a speech bubble — the app's voice and emotional anchor. */
export function MascotBubble({
  message,
  state = 'talking',
  withAudio = false,
  clip,
  mascotSize = 88,
  children,
}: MascotBubbleProps) {
  return (
    <div className="flex items-end gap-3" data-testid="mascot-bubble">
      <EquippedMascot state={state} size={mascotSize} />
      <div className="relative max-w-[18rem] flex-1">
        <div className="rounded-[1.5rem] rounded-bl-md bg-[var(--color-surface-raised)] px-4 py-3 shadow-[var(--shadow-soft)]">
          <p className="m-0 font-display text-lg font-bold leading-snug text-[var(--color-text)]">{message}</p>
          {children}
          {withAudio && (
            <div className="mt-2">
              <AudioButton text={message} clip={clip} />
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
