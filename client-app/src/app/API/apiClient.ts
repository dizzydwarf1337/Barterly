import BaseApi from "./baseApi";

const apiClient = new BaseApi();

export const setToken = (token: string | null) => {
    apiClient.setToken(token);
}

export default apiClient;