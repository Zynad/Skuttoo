import type { ButtonHTMLAttributes, ReactNode } from 'react';

export type ButtonVariant = 'primary' | 'secondary' | 'ghost';
export type ButtonSize = 'md' | 'lg';

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  size?: ButtonSize;
  children: ReactNode;
}

const base =
  'inline-flex items-center justify-center gap-2 font-display font-extrabold rounded-[1.4rem] ' +
  'transition-transform duration-150 active:scale-95 select-none ' +
  'focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-offset-2 focus-visible:ring-[color:var(--color-primary)] ' +
  'disabled:opacity-50 disabled:active:scale-100 disabled:cursor-not-allowed';

const variants: Record<ButtonVariant, string> = {
  primary:
    'text-white bg-[var(--color-primary)] shadow-[var(--shadow-clay)] ' +
    'border-b-4 border-[var(--color-primary-strong)] hover:brightness-105',
  secondary:
    'text-[var(--color-text)] bg-[var(--color-surface-raised)] shadow-[var(--shadow-soft)] ' +
    'border-2 border-[var(--color-border)] hover:bg-[var(--color-surface-sunk)]',
  ghost: 'text-[var(--color-text)] bg-transparent hover:bg-[var(--color-surface-sunk)]',
};

// Always meets the 44px minimum tap target.
const sizes: Record<ButtonSize, string> = {
  md: 'min-h-[48px] px-5 text-lg',
  lg: 'min-h-[60px] px-7 text-2xl',
};

export function Button({ variant = 'primary', size = 'md', className = '', children, type, ...rest }: ButtonProps) {
  return (
    <button
      type={type ?? 'button'}
      className={`${base} ${variants[variant]} ${sizes[size]} ${className}`.trim()}
      {...rest}
    >
      {children}
    </button>
  );
}
