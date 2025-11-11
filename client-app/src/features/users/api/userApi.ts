
import apiClient from "../../../app/API/apiClient";
import { GetPostOwnerRequestDTO, GetPostOwnerResponseDTO } from "../dto/userDto";


const userApi = {
  getPostOwner: (body: GetPostOwnerRequestDTO) =>
    apiClient.get<GetPostOwnerResponseDTO>(`public/user/post-owner/${body.id}`, true),
}


export default userApi;
