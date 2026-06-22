/**
 * Resolves a backend `imageRef` to a public asset URL. Refs are stored as
 * relative paths (e.g. "assets/img/apples-3.svg"); we serve them from /public.
 */
export function imageUrl(imageRef: string | null | undefined): string | null {
  if (!imageRef) {
    return null;
  }
  return imageRef.startsWith('/') || imageRef.startsWith('http') ? imageRef : `/${imageRef}`;
}
