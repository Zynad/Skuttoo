/**
 * Decorative atmosphere layer: a soft sky gradient with a drifting sun and a
 * sea band. Purely visual; sits behind page content.
 */
export function SkyBackground() {
  return (
    <div aria-hidden="true" className="pointer-events-none fixed inset-0 -z-10 overflow-hidden">
      <div
        className="absolute inset-0"
        style={{ background: 'linear-gradient(180deg, var(--bg-sky-top) 0%, var(--bg-sky-bottom) 60%)' }}
      />
      {/* sun */}
      <div
        className="absolute left-[12%] top-[8%] h-28 w-28 rounded-full blur-[2px] animate-drift"
        style={{ background: 'radial-gradient(circle, var(--color-accent) 0%, transparent 70%)' }}
      />
      {/* clouds */}
      <div
        className="absolute right-[10%] top-[14%] h-12 w-32 rounded-full opacity-70 blur-md animate-drift"
        style={{ background: 'var(--color-surface-raised)', animationDelay: '1.5s' }}
      />
      {/* sea */}
      <div
        className="absolute inset-x-0 bottom-0 h-[28%]"
        style={{ background: 'linear-gradient(180deg, transparent, var(--bg-sea))' }}
      />
    </div>
  );
}
