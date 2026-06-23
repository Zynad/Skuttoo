import { describe, expect, it, vi } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { AgePicker } from './AgePicker';
import { renderWithProviders } from '../test/renderWithProviders';

describe('AgePicker', () => {
  it('renders a big option for every age 3–9', () => {
    renderWithProviders(<AgePicker onConfirm={() => undefined} />);
    for (let age = 3; age <= 9; age += 1) {
      expect(screen.getByTestId(`age-option-${age}`)).toBeInTheDocument();
    }
  });

  it('requires a selection, then confirms with the chosen age', async () => {
    const onConfirm = vi.fn();
    renderWithProviders(<AgePicker onConfirm={onConfirm} />);

    expect(screen.getByTestId('age-confirm')).toBeDisabled();
    await userEvent.click(screen.getByTestId('age-option-7'));
    expect(screen.getByTestId('age-confirm')).toBeEnabled();

    await userEvent.click(screen.getByTestId('age-confirm'));
    expect(onConfirm).toHaveBeenCalledWith(7);
  });

  it('reads the age aloud when an option is tapped', async () => {
    const speakSpy = vi.spyOn(window.speechSynthesis, 'speak');
    renderWithProviders(<AgePicker onConfirm={() => undefined} />);
    await userEvent.click(screen.getByTestId('age-option-5'));
    await waitFor(() => expect(speakSpy).toHaveBeenCalled());
  });

  it('pre-selects an initial age (used when changing age later)', () => {
    renderWithProviders(<AgePicker initialAge={6} onConfirm={() => undefined} />);
    expect(screen.getByTestId('age-option-6')).toHaveAttribute('data-selected', 'true');
    expect(screen.getByTestId('age-confirm')).toBeEnabled();
  });
});
