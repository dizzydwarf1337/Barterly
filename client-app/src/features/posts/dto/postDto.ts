import ApiResponse from "../../../app/API/apiResponse";
import { PaginationRequest, PaginationResponse } from "../../../app/API/pagination";
import { PostDetails, PostImagesModal, PostPreview } from "../types/postTypes";


export interface GetPostsResponseDTO extends PaginationResponse<PostPreview>{}

export interface GetPostsRequestDTO extends PaginationRequest{}

export interface GetPostRequestDTO {
    postId: string;
}

export interface GetPostResponseDTO extends ApiResponse<PostDetails>{}

export interface GetPostImagesRequestDTO {
    postId:string;
}

export interface GetPostImagesResponseDTO extends ApiResponse<PostImagesModal> {}