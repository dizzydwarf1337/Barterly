import ApiResponse from "../../../app/API/apiResponse";
import { PostOwner } from "../types/userTypes";


export interface GetPostOwnerRequestDTO {
    id:string;
} 
export interface GetPostOwnerResponseDTO extends ApiResponse<PostOwner> {}