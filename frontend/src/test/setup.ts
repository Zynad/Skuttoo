import '@testing-library/jest-dom/vitest';
import { afterEach, beforeEach, vi } from 'vitest';
import { cleanup } from '@testing-library/react';
import 'fake-indexeddb/auto';

// ProgressProvider fetches the badge catalogue on mount; in tests resolve it instantly with
// the bundled fallback so no component test makes a real (failing) network request.
vi.mock('../services/badgeApi', async (importOriginal) => {
  const actual = await importOriginal<typeof import('../services/badgeApi')>();
  return {
    ...actual,
    badgeApi: { getBadges: () => Promise.resolve(actual.FALLBACK_BADGES) },
  };
});

// jsdom lacks SpeechSynthesis; provide a minimal spyable stub.
class FakeUtterance {
  text: string;
  lang = '';
  rate = 1;
  pitch = 1;
  constructor(text: string) {
    this.text = text;
  }
}

beforeEach(() => {
  vi.stubGlobal('SpeechSynthesisUtterance', FakeUtterance);
  vi.stubGlobal('speechSynthesis', {
    speak: vi.fn(),
    cancel: vi.fn(),
    getVoices: vi.fn(() => []),
  });
  // jsdom has no matchMedia; default to light theme, no reduced motion.
  vi.stubGlobal(
    'matchMedia',
    vi.fn((query: string) => ({
      matches: false,
      media: query,
      onchange: null,
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
      addListener: vi.fn(),
      removeListener: vi.fn(),
      dispatchEvent: vi.fn(),
    })),
  );
});

afterEach(() => {
  cleanup();
  vi.restoreAllMocks();
  localStorage.clear();
});
