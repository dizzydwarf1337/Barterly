import ApiResponse from "./apiResponse";

export interface PaginationRequest<TFilters = {}> {
  filterBy?: {
    search?: string;
    pageSize: number;
    pageNumber: number;
  } & TFilters;
  sortBy?: {
    sortBy: string | null;
    isDescending: boolean;
  };
}

export interface PaginationResponse<T> extends ApiResponse<PaginationData<T>> {}

export interface PaginationData<T> {
  items: T[];
  totalCount: number;
  totalPages: number;
}
