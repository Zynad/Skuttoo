import { describe, expect, it, beforeEach, vi } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Route, Routes } from 'react-router-dom';
import { Onboarding } from './Onboarding';
import { renderWithProviders } from '../test/renderWithProviders';
import { clearProfile, loadProfile } from '../services/progressStore';

const renderOnboarding = () =>
  renderWithProviders(
    <Routes>
      <Route path="/onboarding" element={<Onboarding />} />
      <Route path="/" element={<div data-testid="hub">HUB</div>} />
    </Routes>,
    { route: '/onboarding' },
  );

describe('Onboarding', () => {
  beforeEach(async () => {
    await clearProfile();
  });

  it('shows the age picker and reads the greeting aloud', async () => {
    const speakSpy = vi.spyOn(window.speechSynthesis, 'speak');
    renderOnboarding();
    await waitFor(() => expect(screen.getByTestId('onboarding')).toBeInTheDocument());
    expect(screen.getByTestId('age-option-3')).toBeInTheDocument();
    await waitFor(() => expect(speakSpy).toHaveBeenCalled());
  });

  it('persists the chosen age and continues to the hub', async () => {
    renderOnboarding();
    await waitFor(() => expect(screen.getByTestId('onboarding')).toBeInTheDocument());

    await userEvent.click(screen.getByTestId('age-option-7'));
    await userEvent.click(screen.getByTestId('age-confirm'));

    await waitFor(() => expect(screen.getByTestId('hub')).toBeInTheDocument());
    const saved = await loadProfile();
    expect(saved.age).toBe(7);
  });
});
