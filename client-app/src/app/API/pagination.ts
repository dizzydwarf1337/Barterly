import ApiResponse from "./apiResponse";

export interface PaginationRequest {
    filterBy?:{
        search?:string;
        subCategoryId?:string;
        userId?:string;
        pageSize:string;
        pageNumber:string;
    }
    sortBy?:{
        soryBy:string;
        isDescending:boolean
    }

}

export interface PaginationResponse<T> extends ApiResponse<PaginationData<T>> {
 
}

export interface PaginationData<T> {
    items: T[];
    totalCount: number;
    totalPages: number;
}