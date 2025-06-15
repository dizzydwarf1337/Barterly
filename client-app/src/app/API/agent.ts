import axios, { AxiosResponse, InternalAxiosRequestConfig } from "axios";
import RegisterDto from "../models/registerDto";
import ApiResponse from "../models/apiResponse";
import LoginDto from "../models/loginDto";
import GoogleLoginDto from "../models/googleLoginDto";
import { store } from "../stores/store";
import Category from "../models/category";
import ConfirmEmail from "../models/confirmEmail";
import SubCategory from "../models/subCategory";
import { PostPreview } from "../models/postPreview";
import User from "../models/user";
import PostImages from "../models/postImages";
import Email from "../models/Email";
import ResetPassword from "../models/ResetPassword";




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
    post:<T>(url: string, body: {}, noAuth = false) =>
        axios.post(url, body, { headers: { NoAuth: noAuth } }).then(responseBody),
    put: <T>(url: string, body: {}, noAuth = false) =>
        axios.put(url, body, { headers: { NoAuth: noAuth } }).then(responseBody),
    delete: <T>(url:string, noAuth = false) =>
        axios.delete(url, { headers: { NoAuth: noAuth } }).then(responseBody),
    patch: <T>(url: string, body: {}, noAuth = false) =>
        axios.patch(url, { headers: { NoAuth: noAuth } }).then(responseBody)
}

const Auth = {
    Register: (registerDto: RegisterDto, noAuth = true) => requests.post<ApiResponse<void>>("auth/register", registerDto, noAuth),
    Login: (loginDto: LoginDto, noAuth = true) => requests.post<ApiResponse<User>>("auth/login", loginDto, noAuth),
    LoginGoogle: (googleLoginDto: GoogleLoginDto, noAuth=true) => requests.post<ApiResponse<User>>("auth/login-google", googleLoginDto,noAuth),
    Logout: (email:Email,noAuth = false) => requests.post<ApiResponse<boolean>>("auth/logout", email,noAuth),
    ResendEmailConfirm: (email: Email, noAuth = true) => requests.post<ApiResponse<void>>("auth/resendEmailConfirm", { email }, noAuth),
}

const UserAPI = {
    ConfirmEmail: (confirmEmail: ConfirmEmail, noAuth = true) => requests.post<ApiResponse<void>>("user/confirm", confirmEmail, noAuth),
    ResetPassword:(resetPassword:ResetPassword,noAuth=true) => requests.post<ApiResponse<void>>("user/resetPassword",resetPassword,noAuth),
}
const Categories = {
    GetCategories: (noAuth = true) => requests.get<ApiResponse<Category[]>>("category", noAuth),
    AddCategory: (category: Category, noAuth = false) => requests.post<ApiResponse<void>>("category/createCategory", category, noAuth),
    DeleteCategory: (id: string, noAuth = false) => requests.delete<ApiResponse<void>>(`category/delete/${id}`, noAuth),
    EditCategory: (category: Category, noAuth = false) => requests.put<ApiResponse<void>>(`category/update`, category, noAuth),
    AddSubCategory: (subCategory: SubCategory, noAuth = false) => requests.patch<ApiResponse<void>>(`category/addSubCategory`, subCategory, noAuth),
    DeleteSubCategory:(id:string,noAuth=false) => requests.delete<ApiResponse<void>>(`category/deleteSubCategory/${id}`,noAuth),
} 
const Recommendation = {
    GetPopularPosts: (noAuth = true, count: number, city?: string) =>
        requests.get<ApiResponse<PostPreview[]>>('recommendation/popular', noAuth, {
            params: { count, city }
    }),
    GetFeed: (noAuth = true, page: number) => requests.get<ApiResponse<PostPreview[]>>('recommendation/feed', noAuth, {
        params: { page }
    }),
}
const Posts = {
    GetPostImages: (postId: string, noAuth = true) => requests.get<ApiResponse<PostImages>>(`post/images/${postId}`, noAuth),
    GetPostById:(postId:string,noAuth = true) => requests.get<ApiResponse<PostPreview>>(`post/${postId}`,noAuth),
}

const agent = {
    Auth,
    UserAPI,
    Categories,
    Recommendation,
    Posts
}
export default agent;

