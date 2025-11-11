import apiClient from "../../../../app/API/apiClient";
import { CreateUserRequestDTO, GetUserResponseDTO, GetUsersRequestDTO, GetUsersResponseDTO } from "../dto/usersDto";
import { UserSettings } from "../types/userTypes";

const usersApi = {
  createUser: (body: CreateUserRequestDTO ) =>
    apiClient.post<void>("admin/users/create-user", body, false),
  getUsers: (body: GetUsersRequestDTO) => 
    apiClient.post<GetUsersResponseDTO>("admin/users",body,false),
  deleteUser: (body: {id:string}) => 
    apiClient.delete<void>(`admin/users/${body.id}`, false),
  getUser: (body:{id:string}) =>
    apiClient.get<GetUserResponseDTO>(`admin/users/${body.id}`, false),
  updateUserSettings: (body : UserSettings) =>
    apiClient.post<void>(`admin/users/settings`, body, false)
};

export default usersApi;
