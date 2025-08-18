import apiClient from "../../../app/API/apiClient";
import {
  AddFavPostRequestDTO,
  CreatePostRequestDTO,
  GetFavPostsRequestDTO,
  GetFavPostsResponseDTO,
  GetPostImagesRequestDTO,
  GetPostImagesResponseDTO,
  GetPostRequestDTO,
  GetPostResponseDTO,
  GetPostsRequestDTO,
  GetPostsResponseDTO,
} from "../dto/postDto";

const postApi = {
  getPosts: async (body: GetPostsRequestDTO) =>
    apiClient.post<GetPostsResponseDTO>("public/posts/search", body, true),
  getPost: async ({ postId }: GetPostRequestDTO) =>
    apiClient.get<GetPostResponseDTO>(`public/posts/get/${postId}`),
  getFeed: async (body: GetPostsRequestDTO) =>
    apiClient.post<GetPostsResponseDTO>("public/posts/feed", body, true),
  getPostImages: async (body: GetPostImagesRequestDTO) =>
    apiClient.get<GetPostImagesResponseDTO>(
      `public/posts/images/${body.postId}`,
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
    console.log(Object.fromEntries(formData.entries()));
    return apiClient.post<void>("user/posts/create", formData, false);
},
};

export default postApi;
