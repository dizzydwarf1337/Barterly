import apiClient from "../../../app/API/apiClient";
import {
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
};

export default postApi;
