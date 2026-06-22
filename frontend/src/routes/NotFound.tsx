import { useNavigate } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { SkyBackground } from '../components/SkyBackground';
import { ErrorState } from '../components/ErrorState';

/** Catch-all 404 screen. */
export function NotFound() {
  const navigate = useNavigate();
  const t = useT();
  return (
    <div className="min-h-full">
      <SkyBackground />
      <ErrorState message={t('notfound.title')} onRetry={() => void navigate('/')} retryLabel={t('notfound.back')} />
    </div>
  );
}
