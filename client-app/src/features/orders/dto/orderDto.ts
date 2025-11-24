import ApiResponse from "../../../app/API/apiResponse";
import { Order } from "../types/orderTypes";

export interface GetMyOrdersResponseDTO extends ApiResponse<Order[]> { }

export interface GetPlacedOrdersResponseDTO extends GetMyOrdersResponseDTO { }