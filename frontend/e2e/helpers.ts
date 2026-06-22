import { type Page } from '@playwright/test';

/** Capture SpeechSynthesis usage without producing sound (asserts read-aloud is invoked). */
export async function stubSpeech(page: Page): Promise<void> {
  await page.addInitScript(() => {
    const w = window as unknown as { __spoke: string[] };
    w.__spoke = [];
    window.speechSynthesis.speak = (utterance: SpeechSynthesisUtterance) => {
      w.__spoke.push(utterance.text);
    };
    window.speechSynthesis.cancel = () => undefined;
  });
}

/** Reads the numeric coin total from the top-bar coins badge. */
export function coinsValue(page: Page): Promise<number> {
  return page
    .getByTestId('coins')
    .innerText()
    .then((text) => Number(text.replace(/\D+/g, '')) || 0);
}

/** Reads the stars shown on an island node on the world map. */
export function islandStars(page: Page, key: string): Promise<number> {
  return page
    .getByTestId(`island-${key}-stars`)
    .innerText()
    .then((text) => Number(text.replace(/\D+/g, '')) || 0);
}

interface SeedLevel {
  id: number;
  displayOrder: number;
  exerciseIds: number[];
  title: { sv: string; en: string };
}

/**
 * Merges a patch into the locally stored profile (IndexedDB record "local") and reloads so the
 * app picks it up. Used to put the child in a known progress state — e.g. to unlock a level that
 * progressive locking (1.7) would otherwise hide, without playing through every prior level.
 */
export async function seedProfile(page: Page, patch: Record<string, unknown>): Promise<void> {
  await page.goto('/');
  await page.evaluate(
    (seed) =>
      new Promise<void>((resolve, reject) => {
        const open = indexedDB.open('skuttoo', 1);
        open.onupgradeneeded = () => {
          const db = open.result;
          if (!db.objectStoreNames.contains('profile')) {
            db.createObjectStore('profile', { keyPath: 'id' });
          }
        };
        open.onsuccess = () => {
          const db = open.result;
          const tx = db.transaction('profile', 'readwrite');
          const store = tx.objectStore('profile');
          const get = store.get('local');
          get.onsuccess = () => {
            const existing = (get.result as Record<string, unknown>) ?? { id: 'local' };
            store.put({ ...existing, ...seed, id: 'local' });
          };
          tx.oncomplete = () => {
            db.close();
            resolve();
          };
          tx.onerror = () => reject(tx.error);
        };
        open.onerror = () => reject(open.error);
      }),
    patch,
  );
  await page.reload();
}

async function levelsOf(page: Page, subjectKey: string): Promise<SeedLevel[]> {
  const response = await page.request.get(`/api/subjects/${subjectKey}`);
  const subject = (await response.json()) as { levels: SeedLevel[] };
  return [...subject.levels].sort((a, b) => a.displayOrder - b.displayOrder);
}

function completedResults(subjectKey: string, levels: SeedLevel[]) {
  return levels.flatMap((level) =>
    level.exerciseIds.map((exerciseId) => ({
      exerciseId,
      completed: true,
      starsEarned: 3,
      attempts: 1,
      lastPlayedAt: '2026-06-22T00:00:00.000Z',
      subjectKey,
      levelId: level.id,
    })),
  );
}

/** Completes every level before the one whose title matches `titlePattern`, so it becomes playable. */
export async function unlockBeforeTitle(page: Page, subjectKey: string, titlePattern: RegExp): Promise<void> {
  const levels = await levelsOf(page, subjectKey);
  const target = levels.find((l) => titlePattern.test(l.title.sv) || titlePattern.test(l.title.en));
  if (!target) {
    throw new Error(`No level matching ${titlePattern} on island ${subjectKey}`);
  }
  const prior = levels.filter((l) => l.displayOrder < target.displayOrder);
  await seedProfile(page, { results: completedResults(subjectKey, prior) });
}

/** Completes the first `index` levels (by display order) so the level at that index becomes playable. */
export async function unlockBeforeIndex(page: Page, subjectKey: string, index: number): Promise<void> {
  const levels = await levelsOf(page, subjectKey);
  await seedProfile(page, { results: completedResults(subjectKey, levels.slice(0, index)) });
}

/** Marks an island's level (by display order, default the first) fully completed. */
export async function completeLevel(page: Page, subjectKey: string, levelOrder = 1): Promise<void> {
  const levels = await levelsOf(page, subjectKey);
  const target = levels.find((l) => l.displayOrder === levelOrder) ?? levels[0];
  await seedProfile(page, { results: completedResults(subjectKey, [target]) });
}
