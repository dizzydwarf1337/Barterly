import ApiResponse from "../../../app/API/apiResponse";
import {
  PaginationRequest,
  PaginationResponse,
} from "../../../app/API/pagination";
import { PostDetails, PostFormData, PostPreview, PostType, RentObjectType, WorkloadType, WorkLocationType } from "../types/postTypes";

export interface GetPostsResponseDTO extends PaginationResponse<PostPreview> {}

export interface SearchFilters {
    search?: string;
    categoryId?: string;
    subCategoryId?: string;
    city?: string;
    minPrice?: number;
    maxPrice?: number;
    postType?: PostType;
    rentObjectType?: RentObjectType;
    numberOfRooms?: number;
    area?: number;
    floor?: number;
    workload?: WorkloadType;
    workLocation?: WorkLocationType;
    minSalary?: number;
    maxSalary?: number;
    experienceRequired?: boolean;
}

export interface GetPostsRequestDTO extends PaginationRequest<SearchFilters> {}

export interface GetPostRequestDTO {
  postId: string;
}

export interface GetPostResponseDTO extends ApiResponse<PostDetails> {}

export interface GetPostImagesRequestDTO {
  postId: string;
}

export interface GetPostImagesResponseDTO extends ApiResponse<{id:string,imageUrl:string}[]> { }


export interface AddFavPostRequestDTO {
  id: string;
}

export interface GetFavPostsRequestDTO extends PaginationRequest {}

export interface GetFavPostsResponseDTO
  extends PaginationResponse<PostPreview> {}

export interface CreatePostRequestDTO extends PostFormData {}
