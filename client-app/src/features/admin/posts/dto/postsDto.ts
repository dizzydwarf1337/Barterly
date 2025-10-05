import { PaginationRequest } from "../../../../app/API/pagination";
import { PostDetails, PostPreview } from "../../../posts/types/postTypes";
import {
  OwnerData,
  PostOpinion,
  PostSettings,
  PostStatusType,
} from "../types/postTypes";

export interface RejectPostRequestDto {
  postId: string;
  reason: string;
}

export interface GetPostsRequestDto
  extends PaginationRequest<{
    search?: string;
    categoryId?: string | null;
    subCategoryId?: string | null;
    userId?: string | null;
    isActive?: boolean | null;
    isDeleted?: boolean | null;
  }> {}

export interface GetPostsResponseDto {
  posts: PostPreview[];
  totalCount: number;
  totalPages: number;
}

export interface GetPostResponseDto {
  post: PostDetails;
  settings: PostSettings;
  opinions: PostOpinion[];
  owner: OwnerData;
}

export interface UpdatePostSettingsDto {
  id: string;
  isHidden: string;
  isDeleted: string;
  postStatusType: PostStatusType;
  rejectionMessage?: string | null;
}
