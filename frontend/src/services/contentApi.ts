import { BaseApi, baseApi } from './baseApi';
import type {
  AttemptRequest,
  AttemptResult,
  Exercise,
  LevelDetail,
  Subject,
  SubjectDetail,
  SubjectKey,
} from '../types/content';

/**
 * Typed client for the content + attempt endpoints.
 *   GET  /api/subjects
 *   GET  /api/subjects/{key}
 *   GET  /api/levels/{id}
 *   GET  /api/exercises/{id}
 *   POST /api/exercises/{id}/attempt
 */
export class ContentApi {
  constructor(private readonly api: BaseApi = baseApi) {}

  getSubjects(): Promise<Subject[]> {
    return this.api.get<Subject[]>('/subjects');
  }

  getSubject(key: SubjectKey): Promise<SubjectDetail> {
    return this.api.get<SubjectDetail>(`/subjects/${key}`);
  }

  getLevel(id: number): Promise<LevelDetail> {
    return this.api.get<LevelDetail>(`/levels/${id}`);
  }

  getExercise(id: number): Promise<Exercise> {
    return this.api.get<Exercise>(`/exercises/${id}`);
  }

  submitAttempt(exerciseId: number, request: AttemptRequest): Promise<AttemptResult> {
    return this.api.post<AttemptResult, AttemptRequest>(`/exercises/${exerciseId}/attempt`, request);
  }
}

export const contentApi = new ContentApi();
