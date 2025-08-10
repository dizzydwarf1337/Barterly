
import apiClient from "../../../app/API/apiClient";
import ApiResponse from "../../../app/API/apiResponse";
import type {
  ConfirmEmailRequestDTO,
  LoginRequestDTO,
  LoginResponseDTO,
  LoginWithGoogleRequestDTO,
  LoginWithGoogleResponseDTO,
  RegisterRequestDTO,
  ResendEmailConfirmationRequestDTO,
} from "../dto/authDto";

const authApi = {
  login: (body: LoginRequestDTO) =>
    apiClient.post<LoginResponseDTO>("public/auth/login", body, true),

  loginWithGoogle: (body: LoginWithGoogleRequestDTO) =>
    apiClient.post<LoginWithGoogleResponseDTO>("public/auth/google/login", body, true),

  register: (body: RegisterRequestDTO) =>
    apiClient.post<LoginResponseDTO>("public/auth/register", body, true),

  confirmEmail:(body:ConfirmEmailRequestDTO) =>
    apiClient.post<ApiResponse<void>>("public/auth/confirm-email", body, true),

  resendEmailConfirm:(body:ResendEmailConfirmationRequestDTO) =>
    apiClient.post<ApiResponse<void>>("public/auth/resend-email-confirm", body, true),

  refreshToken: (refreshToken: string) =>
    apiClient.post<{ token: string }>("publlic/auth/refresh", { refreshToken }, true),
};

export default authApi;
