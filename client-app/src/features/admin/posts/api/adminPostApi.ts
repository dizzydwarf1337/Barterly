import apiClient from "../../../../app/API/apiClient";
import ApiResponse from "../../../../app/API/apiResponse";
import {
  GetPostResponseDto,
  GetPostsRequestDto,
  GetPostsResponseDto,
  RejectPostRequestDto,
  UpdatePostSettingsDto,
} from "../dto/postsDto";

const postsApi = {
  getPost: (id: string) =>
    apiClient.get<ApiResponse<GetPostResponseDto>>(`admin/posts/get/${id}`),
  approvePost: (postId: string) =>
    apiClient.post<ApiResponse<void>>("admin/posts/approve-post", { postId }),
  rejectPost: (body: RejectPostRequestDto) =>
    apiClient.post<void>("admin/posts/reject-post", body),
  getPosts: (query: GetPostsRequestDto) =>
    apiClient.post<ApiResponse<GetPostsResponseDto>>("admin/posts", query),
  updatePostSettings: (body: UpdatePostSettingsDto) => {
    apiClient.put<ApiResponse<void>>(
      "admin/posts/update-settings",
      body,
      false
    );
  },
  deletePost: (body: {id:string}) => 
    apiClient.delete<ApiResponse<void>>(
      `admin/posts/delete/${body.id}`,
    )
};

export default postsApi;
