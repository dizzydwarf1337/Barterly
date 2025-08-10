import apiClient from "../../../app/API/apiClient";
import { GetCategoriesResponseDTO } from "../dto/categoryDto";



const categoryApi = {
  getCategories: async () =>
    apiClient.get<GetCategoriesResponseDTO>("public/categories/", true),
};

export default categoryApi;
