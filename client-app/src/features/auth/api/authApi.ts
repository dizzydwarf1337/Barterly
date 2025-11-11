import apiClient from "../../../app/API/apiClient";
import ApiResponse from "../../../app/API/apiResponse";
import type {
  ConfirmEmailRequestDTO,
  GetMeResponse,
  LoginRequestDTO,
  LoginResponseDTO,
  LoginWithGoogleRequestDTO,
  RegisterRequestDTO,
  ResendEmailConfirmationRequestDTO,
} from "../dto/authDto";

const authApi = {
  login: (body: LoginRequestDTO) =>
    apiClient.post<LoginResponseDTO>("public/auth/login", body, true),
  getMe: () => apiClient.get<GetMeResponse>("public/auth/get-me", false),

  loginWithGoogle: (body: LoginWithGoogleRequestDTO) =>
    apiClient.post<LoginResponseDTO>("public/auth/login-google", body, true),

  register: (body: RegisterRequestDTO) =>
    apiClient.post<LoginResponseDTO>("public/auth/register", body, true),

  confirmEmail: (body: ConfirmEmailRequestDTO) =>
    apiClient.post<ApiResponse<void>>("public/auth/confirm-email", body, true),

  resendEmailConfirm: (body: ResendEmailConfirmationRequestDTO) =>
    apiClient.post<ApiResponse<void>>(
      "public/auth/resend-email-confirm",
      body,
      true
    ),
  logout: () => apiClient.post<ApiResponse<void>>("public/auth/logout", false),
  refreshToken: (refreshToken: string) =>
    apiClient.post<{ token: string }>(
      "public/auth/refresh",
      { refreshToken },
      true
    ),
};

export default authApi;
