import type { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import axios from "axios";

export default class BaseApi {
  private axiosInstance: AxiosInstance;
  private token: string | null = null;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: import.meta.env.VITE_API_URL,
    });

    this.axiosInstance.interceptors.request.use((config) => {
      if (config.headers && !config.headers["NoAuth"]) {
        const token = this.token ?? localStorage.getItem("token");
        if (token) {
          config.headers["Authorization"] = `Bearer ${token}`;
        }
      }
      return config;
    });
  }

  public setToken(token: string | null) {
    this.token = token;
    if (token) {
      localStorage.setItem("token", token);
    } else {
      localStorage.removeItem("token");
    }
  }

  private responseBody = <T>(response: AxiosResponse<T>) => response.data;

  public get = <T>(
    url: string,
    noAuth = false,
    config: AxiosRequestConfig = {}
  ) =>
    this.axiosInstance
      .get<T>(url, {
        ...config,
        headers: {
          ...(config.headers || {}),
          NoAuth: noAuth,
        },
      })
      .then(this.responseBody);

  public post = <T>(
    url: string,
    body: unknown,
    noAuth = false,
    config: AxiosRequestConfig = {}
  ) =>
    this.axiosInstance
      .post<T>(url, body, {
        ...config,
        headers: {
          ...(config.headers || {}),
          NoAuth: noAuth,
        },
      })
      .then(this.responseBody);

  public put = <T>(
    url: string,
    body: unknown,
    noAuth = false,
    config: AxiosRequestConfig = {}
  ) =>
    this.axiosInstance
      .put<T>(url, body, {
        ...config,
        headers: {
          ...(config.headers || {}),
          NoAuth: noAuth,
        },
      })
      .then(this.responseBody);

  public delete = <T>(
    url: string,
    noAuth = false,
    config: AxiosRequestConfig = {}
  ) =>
    this.axiosInstance
      .delete<T>(url, {
        ...config,
        headers: {
          ...(config.headers || {}),
          NoAuth: noAuth,
        },
      })
      .then(this.responseBody);

  public patch = <T>(
    url: string,
    body: unknown,
    noAuth = false,
    config: AxiosRequestConfig = {}
  ) =>
    this.axiosInstance
      .patch<T>(url, body, {
        ...config,
        headers: {
          ...(config.headers || {}),
          NoAuth: noAuth,
        },
      })
      .then(this.responseBody);
}
