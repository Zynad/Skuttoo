import { describe, expect, it } from 'vitest';
import { screen } from '@testing-library/react';
import { renderWithProviders } from '../test/renderWithProviders';
import { Mascot, type MascotState } from './Mascot';

const states: MascotState[] = ['idle', 'talking', 'happy', 'encouraging', 'celebrate'];

describe('Mascot', () => {
  it('renders each state with its data-state and an accessible label', () => {
    for (const state of states) {
      const { unmount } = renderWithProviders(<Mascot state={state} />);
      const el = screen.getByTestId('mascot');
      expect(el).toHaveAttribute('data-state', state);
      expect(el.getAttribute('aria-label')).toBeTruthy();
      unmount();
    }
  });

  it('applies the matching animation class per state', () => {
    renderWithProviders(<Mascot state="celebrate" />);
    expect(screen.getByTestId('mascot').className).toContain('animate-celebrate');
  });

  it('renders an equipped accessory overlay when provided', () => {
    renderWithProviders(<Mascot state="idle" accessory="🎩" />);
    expect(screen.getByTestId('mascot-accessory')).toHaveTextContent('🎩');
  });

  it('uses the chosen fur colour for the body', () => {
    renderWithProviders(<Mascot state="idle" fill="var(--island-math)" />);
    const head = screen.getByTestId('mascot').querySelector('circle[r="38"]');
    expect(head).toHaveAttribute('fill', 'var(--island-math)');
  });
});
