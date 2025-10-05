import apiClient from "../../../../app/API/apiClient";
import ApiResponse from "../../../../app/API/apiResponse";
import {
  AddCategoryRequestDto,
  GetAllCategoriesRequestDto,
  GetAllCategoriesResponseDto,
  UpdateCategoryRequestDto,
} from "../dto/categoriesDto";

const categoriesApi = {
  getCategories: (request?: GetAllCategoriesRequestDto) =>
    apiClient.post<ApiResponse<GetAllCategoriesResponseDto>>(
      "admin/categories",
      request || {
        filterBy: {
          pageNumber: 1,
          pageSize: 10,
        },
        sortBy: {
          sortBy: "namePL",
          isDescending: false,
        },
      },
      false
    ),
  addCategory: (body: AddCategoryRequestDto) =>
    apiClient.post<void>("admin/categories/create-category", body, false),
  deleteCategory: (body: { id: string }) =>
    apiClient.delete<void>(
      `admin/categories/delete-category/${body.id}`,
      false
    ),
  updateCategory: (body: UpdateCategoryRequestDto) =>
    apiClient.put<void>(`admin/categories/update-category`, body, false),
};

export default categoriesApi;
