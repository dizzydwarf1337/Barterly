import apiClient from "../../../app/API/apiClient";
import ApiResponse from "../../../app/API/apiResponse";
import {
  AddFavPostRequestDTO,
  BuyPromotionRequestDTO,
  CreatePostRequestDTO,
  GetFavPostsRequestDTO,
  GetFavPostsResponseDTO,
  GetMyPostsResponseDTO,
  GetPostImagesRequestDTO,
  GetPostImagesResponseDTO,
  GetPostRequestDTO,
  GetPostResponseDTO,
  GetPostsRequestDTO,
  GetPostsResponseDTO,
} from "../dto/postDto";
import { PostPreview } from "../types/postTypes";

const userPostApi = {
  getPosts: async (body: GetPostsRequestDTO) =>
    apiClient.post<GetPostsResponseDTO>("user/posts/search", body, false),
  getPost: async ({ postId }: GetPostRequestDTO) =>
    apiClient.get<GetPostResponseDTO>(`user/posts/get/${postId}`),
  getFeed: async (body: GetPostsRequestDTO) =>
    apiClient.post<GetPostsResponseDTO>("user/posts/feed", body, false),
  getPostImages: async (body: GetPostImagesRequestDTO) =>
    apiClient.get<GetPostImagesResponseDTO>(
      `user/posts/images/${body.postId}`,
      true
    ),
  addFavPost: async ({ id }: AddFavPostRequestDTO) =>
    apiClient.put<void>(`user/posts/fav-post/${id}`, false),
  getFavPosts: async (body: GetFavPostsRequestDTO) =>
    apiClient.post<GetFavPostsResponseDTO>("user/posts/fav-posts", body, false),
  createPost: async (body: CreatePostRequestDTO) => {
    const formData = new FormData();
    Object.keys(body).forEach(key => {
        const value = ((body as unknown) as Record<string, unknown>)[key];
        if (value !== null && value !== undefined) {
            if (value instanceof File) {
                formData.append(key, value);
            } else if (Array.isArray(value)) {
                value.forEach((item, index) => {
                    if (item instanceof File) {
                        formData.append(key, item);
                    } else {
                        formData.append(`${key}[${index}]`, item);
                    }
                });
            } else {
                formData.append(key, value as any);
            }
        }
    });
    return apiClient.post<void>("user/posts/create", formData, false);
},
  getMyPosts: async () =>
    apiClient.get<GetMyPostsResponseDTO>("user/posts/my-posts", false),
  deletePost: async (id:string) =>
    apiClient.delete<ApiResponse<void>>(`user/posts/delete/${id}`, false),
  changePostVisibility: async (id:string) => 
    apiClient.put<ApiResponse<void>>(`user/posts/hide/${id}`, false),
  buyPromotion: async (body: BuyPromotionRequestDTO) =>
    apiClient.put<ApiResponse<void>>("user/posts/buy-promotion", body, false),
  getPostPreview: async (id:string) => 
    apiClient.get<ApiResponse<PostPreview>>(`user/posts/preview/${id}`, false),
  getUserPosts: async (id:string) => 
    apiClient.get<ApiResponse<PostPreview[]>>(`user/posts/user/${id}`, false)
};

export default userPostApi;
