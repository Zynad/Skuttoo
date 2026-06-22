import { describe, expect, it, vi } from 'vitest';
import { screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { IslandNode } from './IslandNode';
import { renderWithProviders } from '../test/renderWithProviders';
import { islandThemes } from '../utils/islandTheme';

describe('IslandNode', () => {
  it('renders the themed island with its name, star progress and accessible label', () => {
    renderWithProviders(
      <IslandNode
        theme={islandThemes.math}
        progress={{ starsEarned: 5, exercisesDone: 2, started: true }}
        isNext={false}
        side="left"
        index={0}
        onEnter={vi.fn()}
      />,
      { lang: 'sv' },
    );

    const node = screen.getByTestId('island-math');
    expect(node).toHaveAccessibleName('Åk till Matte');
    expect(node).toHaveTextContent('Matte');
    expect(screen.getByTestId('island-math-stars')).toHaveTextContent('5');
  });

  it('calls onEnter with the theme when tapped', async () => {
    const onEnter = vi.fn();
    renderWithProviders(
      <IslandNode
        theme={islandThemes.logic}
        progress={{ starsEarned: 0, exercisesDone: 0, started: false }}
        isNext={false}
        side="right"
        index={3}
        onEnter={onEnter}
      />,
      { lang: 'sv' },
    );

    await userEvent.click(screen.getByTestId('island-logic'));
    expect(onEnter).toHaveBeenCalledWith(islandThemes.logic);
  });

  it('places Skutt at the recommended next stop', () => {
    renderWithProviders(
      <IslandNode
        theme={islandThemes.swedish}
        progress={{ starsEarned: 0, exercisesDone: 0, started: false }}
        isNext
        side="left"
        index={1}
        onEnter={vi.fn()}
      />,
      { lang: 'sv' },
    );

    expect(screen.getByTestId('island-swedish')).toHaveAttribute('data-next', 'true');
    expect(screen.getByTestId('mascot')).toBeInTheDocument();
  });
});
