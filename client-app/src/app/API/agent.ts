import axios, {AxiosResponse, InternalAxiosRequestConfig} from "axios";
import RegisterDto from "../models/registerDto";
import ApiResponse from "../models/apiResponse";
import LoginDto from "../models/loginDto";
import GoogleLoginDto from "../models/googleLoginDto";
import {store} from "../stores/store";


export default class agentBase {

axios.defaults.baseURL = import.meta.env.VITE_API_URL;

axios.interceptors.request.use((config: InternalAxiosRequestConfig) => {

    if (config.headers && !config.headers['NoAuth']) {
        const token = store.userStore.user!.token;

        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        } else {
            console.error("Token not found or is invalid.");
        }
        console.log(config);
    }
    return config;
});

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: <T>(url: string, noAuth = false, config = {}) =>
        axios.get<T>(url, {
            ...config,
            headers: {
                ...(config as any).headers,
                NoAuth: noAuth
            }
        }).then(responseBody),
    post: <T>(url: string, body: {}, noAuth = false) =>
        axios.post(url, body, {headers: {NoAuth: noAuth}}).then(responseBody),
    put: <T>(url: string, body: {}, noAuth = false) =>
        axios.put(url, body, {headers: {NoAuth: noAuth}}).then(responseBody),
    delete: <T>(url: string, noAuth = false) =>
        axios.delete(url, {headers: {NoAuth: noAuth}}).then(responseBody),
    patch: <T>(url: string, body: {}, noAuth = false) =>
        axios.patch(url, {headers: {NoAuth: noAuth}}).then(responseBody)
}

}

