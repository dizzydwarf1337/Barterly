
import apiClient from "../../../app/API/apiClient";
import { GetPostOwnerRequestDTO, GetPostOwnerResponseDTO, GetUserDataRequestDTO, GetUserDataResponseDTO, UpdateProfileResponseDTO } from "../dto/userDto";


const userApi = {
  getPostOwner: (body: GetPostOwnerRequestDTO) =>
    apiClient.get<GetPostOwnerResponseDTO>(`public/user/post-owner/${body.id}`, true),
  getUserData: (body: GetUserDataRequestDTO) => 
    apiClient.get<GetUserDataResponseDTO>(`user/authorized/${body.userId}`, false),
  updateProfile: (formData: FormData) =>
    apiClient.post<UpdateProfileResponseDTO>(`user/authorized/update-profile`, formData, false),
}


export default userApi;
