import type { ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { useProgress } from '../hooks/useProgress';
import { SkyBackground } from './SkyBackground';
import { LoadingState } from './LoadingState';

export interface RequireAgeProps {
  children: ReactNode;
}

/**
 * First-run gate: until the child's exact age is set, redirect to onboarding. Honors the progress
 * `loading` flag so an already-onboarded child isn't bounced to onboarding for a frame while the
 * stored profile resolves from IndexedDB.
 */
export function RequireAge({ children }: RequireAgeProps) {
  const { profile, loading } = useProgress();

  if (loading) {
    return (
      <div className="min-h-full">
        <SkyBackground />
        <LoadingState />
      </div>
    );
  }

  if (profile.age === null) {
    return <Navigate to="/onboarding" replace />;
  }

  return <>{children}</>;
}
