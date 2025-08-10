import ApiResponse from "../../../app/API/apiResponse";
import { Category } from "../types/categoryTypes";


export interface GetCategoriesResponseDTO extends ApiResponse<Category[]> {}
