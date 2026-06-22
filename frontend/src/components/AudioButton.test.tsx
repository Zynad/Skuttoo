import { describe, expect, it, vi, beforeEach } from 'vitest';
import { screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { AudioButton } from './AudioButton';
import { renderWithProviders } from '../test/renderWithProviders';

describe('AudioButton', () => {
  beforeEach(() => {
    // No pre-generated clip exists → HEAD check must report "not found".
    vi.stubGlobal(
      'fetch',
      vi.fn(() => Promise.resolve({ ok: false } as Response)),
    );
  });

  it('renders with an accessible read-aloud label', () => {
    renderWithProviders(<AudioButton text="Tre äpplen" />, { lang: 'sv' });
    expect(screen.getByTestId('audio-button')).toHaveAccessibleName('Läs upp');
  });

  it('falls back to SpeechSynthesis in the active language when no clip exists', async () => {
    const speakSpy = vi.spyOn(window.speechSynthesis, 'speak');
    renderWithProviders(<AudioButton text="Three apples" />, { lang: 'en' });

    await userEvent.click(screen.getByTestId('audio-button'));

    expect(speakSpy).toHaveBeenCalledTimes(1);
    const utterance = speakSpy.mock.calls[0][0];
    expect(utterance.text).toBe('Three apples');
    expect(utterance.lang).toBe('en-US');
  });

  it('uses the Swedish voice locale when language is Swedish', async () => {
    const speakSpy = vi.spyOn(window.speechSynthesis, 'speak');
    renderWithProviders(<AudioButton text="Tre äpplen" />, { lang: 'sv' });

    await userEvent.click(screen.getByTestId('audio-button'));

    const utterance = speakSpy.mock.calls[0][0];
    expect(utterance.lang).toBe('sv-SE');
  });
});
