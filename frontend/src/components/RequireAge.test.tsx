import { describe, expect, it, beforeEach } from 'vitest';
import { screen, waitFor } from '@testing-library/react';
import { Route, Routes } from 'react-router-dom';
import { RequireAge } from './RequireAge';
import { renderWithProviders } from '../test/renderWithProviders';
import { clearProfile, saveProfile } from '../services/progressStore';
import { createDefaultProfile } from '../types/progress';

const renderGate = () =>
  renderWithProviders(
    <Routes>
      <Route
        path="/"
        element={
          <RequireAge>
            <div data-testid="home">HOME</div>
          </RequireAge>
        }
      />
      <Route path="/onboarding" element={<div data-testid="onboarding-stub">ONBOARD</div>} />
    </Routes>,
    { route: '/' },
  );

describe('RequireAge', () => {
  beforeEach(async () => {
    await clearProfile();
  });

  it('redirects to onboarding when no age is set', async () => {
    renderGate();
    await waitFor(() => expect(screen.getByTestId('onboarding-stub')).toBeInTheDocument());
    expect(screen.queryByTestId('home')).not.toBeInTheDocument();
  });

  it('renders its children once an age is set', async () => {
    await saveProfile({ ...createDefaultProfile(), age: 6 });
    renderGate();
    await waitFor(() => expect(screen.getByTestId('home')).toBeInTheDocument());
    expect(screen.queryByTestId('onboarding-stub')).not.toBeInTheDocument();
  });
});
