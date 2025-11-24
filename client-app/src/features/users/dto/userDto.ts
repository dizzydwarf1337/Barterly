import ApiResponse from "../../../app/API/apiResponse";
import { PostOwner, UserData } from "../types/userTypes";

export interface GetPostOwnerRequestDTO {
  id: string;
}
export interface GetPostOwnerResponseDTO extends ApiResponse<PostOwner> {}

export interface GetUserDataRequestDTO {
  userId: string;
}
export interface GetUserDataResponseDTO extends ApiResponse<UserData> { }

export interface UpdateProfileRequestDTO extends UserData {
  file: File
}

export interface UpdateProfileResponseDTO extends ApiResponse<UserData> { }