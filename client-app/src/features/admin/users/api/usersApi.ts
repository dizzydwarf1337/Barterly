import apiClient from "../../../../app/API/apiClient";
import { CreateUserRequestDTO, GetUserResponseDTO, GetUsersRequestDTO, GetUsersResponseDTO } from "../dto/usersDto";

const usersApi = {
  createUser: (body: CreateUserRequestDTO ) =>
    apiClient.post<void>("admin/users/create-user", body, false),
  getUsers: (body: GetUsersRequestDTO) => 
    apiClient.post<GetUsersResponseDTO>("admin/users",body,false),
  deleteUser: (body: {id:string}) => 
    apiClient.delete<void>(`admin/users/${body.id}`, false),
  getUser: (body:{id:string}) =>
    apiClient.get<GetUserResponseDTO>(`admin/users/${body.id}`, false),
};

export default usersApi;
