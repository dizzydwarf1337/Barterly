

import apiClient from "../../../../app/API/apiClient";
import { AddCategoryRequestDto, UpdateCategoryRequestDto } from "../dto/categoriesDto";
import { Category } from "../types/categoryTypes";


const categoriesApi = {
    getCategories: () =>
        apiClient.get<Category[]>("admin/categories", false),
    addCategory: (body: AddCategoryRequestDto) =>
        apiClient.post<void>("admin/categories/create-category", body, false),
    deleteCategory: (body: {id:string}) =>
        apiClient.delete<void>(`admin/categories/${body.id}`, false),
    updateCategory: (body: UpdateCategoryRequestDto) =>
        apiClient.put<void>(`admin/categories`, body, false),
};

export default categoriesApi;
