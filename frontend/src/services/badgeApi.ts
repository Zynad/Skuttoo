import { BaseApi, baseApi } from './baseApi';
import type { BadgeDef } from '../types/badges';

/**
 * Typed client for the badge catalogue: GET /api/badges.
 * Badge earning is client-side; this only fetches the (localized) definitions.
 */
export class BadgeApi {
  constructor(private readonly api: BaseApi = baseApi) {}

  getBadges(): Promise<BadgeDef[]> {
    return this.api.get<BadgeDef[]>('/badges');
  }
}

export const badgeApi = new BadgeApi();

/**
 * Offline/first-paint fallback catalogue. Mirrors the seeded definitions (keys + criteria
 * are the contract used by badgeLogic); the API copy is preferred when reachable. Keeping it
 * here means badge earning + the gallery work fully offline (PWA), per ADR-011.
 */
export const FALLBACK_BADGES: BadgeDef[] = [
  {
    id: 1,
    key: 'first-hops',
    iconRef: '🦊',
    criteriaType: 'completeLevel',
    criteriaValue: 1,
    name: { sv: 'Första skutten', en: 'First hops' },
    description: { sv: 'Klara din första nivå.', en: 'Complete your first level.' },
  },
  {
    id: 2,
    key: 'pathfinder',
    iconRef: '🗺️',
    criteriaType: 'completeLevel',
    criteriaValue: 5,
    name: { sv: 'Stigvandrare', en: 'Pathfinder' },
    description: { sv: 'Klara fem nivåer.', en: 'Complete five levels.' },
  },
  {
    id: 3,
    key: 'island-explorer',
    iconRef: '🏝️',
    criteriaType: 'completeSubject',
    criteriaValue: 1,
    name: { sv: 'Öupptäckare', en: 'Island explorer' },
    description: { sv: 'Klara en hel ö.', en: 'Complete a whole island.' },
  },
  {
    id: 4,
    key: 'world-traveller',
    iconRef: '🌍',
    criteriaType: 'completeSubject',
    criteriaValue: 4,
    name: { sv: 'Världsresenär', en: 'World traveller' },
    description: { sv: 'Klara alla fyra öar.', en: 'Complete all four islands.' },
  },
  {
    id: 5,
    key: 'on-a-roll',
    iconRef: '🔥',
    criteriaType: 'streak',
    criteriaValue: 3,
    name: { sv: 'På gång', en: 'On a roll' },
    description: { sv: 'Spela tre dagar i rad.', en: 'Play three days in a row.' },
  },
  {
    id: 6,
    key: 'week-hero',
    iconRef: '⭐',
    criteriaType: 'streak',
    criteriaValue: 7,
    name: { sv: 'Veckohjälte', en: 'Week hero' },
    description: { sv: 'Spela sju dagar i rad.', en: 'Play seven days in a row.' },
  },
  {
    id: 7,
    key: 'coin-collector',
    iconRef: '🪙',
    criteriaType: 'coinTotal',
    criteriaValue: 50,
    name: { sv: 'Myntsamlare', en: 'Coin collector' },
    description: { sv: 'Samla 50 mynt.', en: 'Collect 50 coins.' },
  },
  {
    id: 8,
    key: 'coin-master',
    iconRef: '👑',
    criteriaType: 'coinTotal',
    criteriaValue: 200,
    name: { sv: 'Myntmästare', en: 'Coin master' },
    description: { sv: 'Samla 200 mynt.', en: 'Collect 200 coins.' },
  },
];
