import ApiResponse from "../../../app/API/apiResponse";
import { PaginationRequest, PaginationResponse } from "../../../app/API/pagination";
import { PostDetails, PostPreview } from "../types/postTypes";


export interface GetPostsResponseDTO extends PaginationResponse<PostPreview>{}

export interface GetPostsRequestDTO extends PaginationRequest{}

export interface GetPostRequestDTO {
    postId: string;
}

export interface GetPostResponseDTO extends ApiResponse<PostDetails>{}
