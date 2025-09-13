import { PaginationRequest } from "../../../../app/API/pagination"
import { Category } from "../types/categoryTypes"

export interface AddCategoryRequestDto {
    namePL:string,
    nameEN:string,
    description?:string,
    subCategories: {namePL:string,nameEN:string}[]
}

export interface UpdateCategoryRequestDto {
    id:string,
    namePL:string,
    nameEN:string,
    description?:string,
    subCategories: {id?:string,namePL:string,nameEN:string}[]
}

export interface GetAllCategoriesResponseDto {
    categories:Category[],
    totalCount:number,
    totalPages:number,
    currentPage:number,
    pageSize:number
}

export interface GetAllCategoriesRequestDto extends PaginationRequest<Category> {};