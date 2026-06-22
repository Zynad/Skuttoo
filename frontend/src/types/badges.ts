import type { Loc } from './content';

/** How a badge is earned. Mirrors the backend BadgeCriteriaType (camelCase JSON). */
export type BadgeCriteriaType = 'completeLevel' | 'completeSubject' | 'streak' | 'coinTotal';

/** A badge definition as served by GET /api/badges. Earning is tracked client-side. */
export interface BadgeDef {
  id: number;
  key: string;
  name: Loc;
  description: Loc;
  iconRef: string;
  criteriaType: BadgeCriteriaType;
  criteriaValue: number;
}
