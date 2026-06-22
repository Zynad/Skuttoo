import { useNavigate, useParams } from 'react-router-dom';
import { useT } from '../i18n/useT';
import { useAsync } from '../hooks/useAsync';
import { contentApi } from '../services/contentApi';
import { TopBar } from '../components/TopBar';
import { SkyBackground } from '../components/SkyBackground';
import { LoadingState } from '../components/LoadingState';
import { ErrorState } from '../components/ErrorState';
import { ExerciseRunner } from '../features/exercise/ExerciseRunner';

/** Route wrapper: loads the exercise by id, then hands it to the runner. */
export function Exercise() {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const t = useT();
  const exerciseId = Number(id);

  const {
    data: exercise,
    loading,
    error,
    reload,
  } = useAsync(() => {
    if (!Number.isFinite(exerciseId)) {
      return Promise.reject(new Error('invalid exercise id'));
    }
    return contentApi.getExercise(exerciseId);
  }, [exerciseId]);

  return (
    <div className="min-h-full">
      <SkyBackground />
      <TopBar showBack showLanguage={false} />
      {loading && <LoadingState />}
      {Boolean(error) && !loading && (
        <ErrorState message={t('exercise.error')} onRetry={reload} retryLabel={t('exercise.retry')} />
      )}
      {exercise && !loading && <ExerciseRunner key={exercise.id} exercise={exercise} />}
      {!exercise && !loading && !error && (
        <ErrorState message={t('exercise.error')} onRetry={() => void navigate('/')} retryLabel={t('nav.home')} />
      )}
    </div>
  );
}
