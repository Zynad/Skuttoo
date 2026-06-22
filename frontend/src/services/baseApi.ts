import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';

/**
 * Thin Axios wrapper for the Skuttoo JSON API. All typed services build on this.
 * The base URL is "/api" by default (served by the .NET app; proxied in dev to
 * http://localhost:5080). Only a public VITE_* var may override it.
 */
export class BaseApi {
  private readonly client: AxiosInstance;

  constructor(baseURL: string = import.meta.env.VITE_API_BASE_URL ?? '/api', client?: AxiosInstance) {
    this.client =
      client ??
      axios.create({
        baseURL,
        headers: { 'Content-Type': 'application/json' },
        timeout: 15000,
      });
  }

  async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.get<T>(url, config);
    return response.data;
  }

  async post<TResponse, TBody = unknown>(url: string, body?: TBody, config?: AxiosRequestConfig): Promise<TResponse> {
    const response = await this.client.post<TResponse>(url, body, config);
    return response.data;
  }
}

/** Shared default instance. */
export const baseApi = new BaseApi();
