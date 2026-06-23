/**
 * Returns a new array with the items in a random order (Fisher–Yates). Used to vary where answers
 * and match items appear so the correct one isn't always in the same slot. Call it once per exercise
 * (e.g. in a `useMemo` keyed on the exercise id) so the order is stable while the child works.
 */
export function shuffle<T>(items: readonly T[]): T[] {
  const result = [...items];
  for (let i = result.length - 1; i > 0; i -= 1) {
    const j = Math.floor(Math.random() * (i + 1));
    const tmp = result[i];
    result[i] = result[j];
    result[j] = tmp;
  }
  return result;
}
