import ApiResponse from "../../../../app/API/apiResponse";
import {
  PaginationRequest,
  PaginationResponse,
} from "../../../../app/API/pagination";
import {
  UserData,
  UserPreview,
  UserRoles,
  UserSettings,
} from "../types/userTypes";

export type CreateUserRequestDTO = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: UserRoles;
};

export interface GetUsersResponseDTO extends PaginationResponse<UserPreview> {}

export interface GetUsersRequestDTO extends PaginationRequest<UserFilters> {}
interface UserFilters {
  isBanned?: boolean | null;
  isDeleted?: boolean | null;
  isEmailConfirmed?: boolean | null;
}
export interface GetUserResponseDTO
  extends ApiResponse<{ userData: UserData; userSettings: UserSettings }> {
  userData: UserData;
  userSettings: UserSettings;
}
