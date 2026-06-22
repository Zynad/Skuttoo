import type { ReactElement, ReactNode } from 'react';
import { render, type RenderOptions } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { ThemeProvider } from '../contexts/ThemeProvider';
import { LanguageProvider } from '../contexts/LanguageProvider';
import { ProgressProvider } from '../contexts/ProgressProvider';
import type { Lang } from '../i18n/dictionaries';

export interface ProvidersOptions extends Omit<RenderOptions, 'wrapper'> {
  lang?: Lang;
  route?: string;
}

/** Renders a component wrapped in all app providers + a memory router. */
export function renderWithProviders(ui: ReactElement, { lang = 'sv', route = '/', ...options }: ProvidersOptions = {}) {
  const Wrapper = ({ children }: { children: ReactNode }) => (
    <ThemeProvider>
      <LanguageProvider initialLang={lang}>
        <ProgressProvider>
          <MemoryRouter initialEntries={[route]}>{children}</MemoryRouter>
        </ProgressProvider>
      </LanguageProvider>
    </ThemeProvider>
  );

  return render(ui, { wrapper: Wrapper, ...options });
}
