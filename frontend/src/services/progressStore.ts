import { openDB, type IDBPDatabase } from 'idb';
import { createDefaultProfile, type Profile } from '../types/progress';

/**
 * Tiny IndexedDB-backed store for the local child Profile (anonymous, MVP).
 * Falls back to localStorage when IndexedDB is unavailable. A single record
 * keyed by the profile id ("local") holds the whole profile.
 */

const DB_NAME = 'skuttoo';
const DB_VERSION = 1;
const STORE = 'profile';
const PROFILE_ID = 'local';
const LS_KEY = 'skuttoo.profile';

let dbPromise: Promise<IDBPDatabase> | null = null;

const hasIndexedDb = (): boolean => typeof indexedDB !== 'undefined';

function getDb(): Promise<IDBPDatabase> {
  if (!dbPromise) {
    dbPromise = openDB(DB_NAME, DB_VERSION, {
      upgrade(db) {
        if (!db.objectStoreNames.contains(STORE)) {
          db.createObjectStore(STORE, { keyPath: 'id' });
        }
      },
    });
  }
  return dbPromise;
}

function readFromLocalStorage(): Profile | null {
  if (typeof localStorage === 'undefined') {
    return null;
  }
  const raw = localStorage.getItem(LS_KEY);
  if (!raw) {
    return null;
  }
  try {
    return JSON.parse(raw) as Profile;
  } catch {
    return null;
  }
}

function writeToLocalStorage(profile: Profile): void {
  if (typeof localStorage !== 'undefined') {
    localStorage.setItem(LS_KEY, JSON.stringify(profile));
  }
}

/** Loads the stored profile, creating a default one on first run. */
export async function loadProfile(): Promise<Profile> {
  if (hasIndexedDb()) {
    try {
      const db = await getDb();
      const stored = (await db.get(STORE, PROFILE_ID)) as Profile | undefined;
      if (stored) {
        return stored;
      }
      const fresh = createDefaultProfile();
      await db.put(STORE, fresh);
      return fresh;
    } catch {
      // fall back to localStorage below
    }
  }

  const fromLs = readFromLocalStorage();
  if (fromLs) {
    return fromLs;
  }
  const fresh = createDefaultProfile();
  writeToLocalStorage(fresh);
  return fresh;
}

/** Persists the full profile. */
export async function saveProfile(profile: Profile): Promise<void> {
  if (hasIndexedDb()) {
    try {
      const db = await getDb();
      await db.put(STORE, profile);
      return;
    } catch {
      // fall back to localStorage below
    }
  }
  writeToLocalStorage(profile);
}

/** Test/utility helper — wipes stored progress. */
export async function clearProfile(): Promise<void> {
  if (hasIndexedDb()) {
    try {
      const db = await getDb();
      await db.delete(STORE, PROFILE_ID);
    } catch {
      /* ignore */
    }
  }
  if (typeof localStorage !== 'undefined') {
    localStorage.removeItem(LS_KEY);
  }
}
