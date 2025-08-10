import apiClient from "../../../app/API/apiClient";
import { GetPostRequestDTO, GetPostResponseDTO, GetPostsRequestDTO, GetPostsResponseDTO } from "../dto/postDto";



const postApi = {
  getPosts: async (body:GetPostsRequestDTO) =>
    apiClient.post<GetPostsResponseDTO>("public/posts/search", body,true),
  getPost: async ({postId}:GetPostRequestDTO) =>
    apiClient.get<GetPostResponseDTO>(`public/posts/${postId}`),
  getFeed: async (body:GetPostsRequestDTO) =>
    apiClient.post<GetPostsResponseDTO>("public/posts/feed", body, true)
};

export default postApi;
