import type { CSSProperties, HTMLAttributes, ReactNode } from 'react';

export interface CardProps extends HTMLAttributes<HTMLDivElement> {
  children: ReactNode;
  style?: CSSProperties;
}

/** Soft, rounded "clay" surface used to group content. */
export function Card({ children, className = '', style, ...rest }: CardProps) {
  return (
    <div
      className={`rounded-[1.75rem] bg-[var(--color-surface-raised)] shadow-[var(--shadow-soft)] p-5 ${className}`.trim()}
      style={style}
      {...rest}
    >
      {children}
    </div>
  );
}
