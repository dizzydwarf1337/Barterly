import apiClient from "../../../app/API/apiClient";
import ApiResponse from "../../../app/API/apiResponse";
import { GetMyOrdersResponseDTO, GetPlacedOrdersResponseDTO } from "../dto/orderDto";


const ordersApi = {
    getMyOrders: async () => 
        apiClient.get<GetMyOrdersResponseDTO>("/users/order/my", false),
    getMyPlacedOrders: async () => 
        apiClient.get<GetPlacedOrdersResponseDTO>("/users/order/my-placed", false),
    markAsShipped: async (orderId:string) => 
        apiClient.put<ApiResponse<void>>(`/users/order/ship/${orderId}`, false),
    markAsDelivered: async (orderId:string) => 
        apiClient.put<ApiResponse<void>>(`/users/order/deliver/${orderId}`, false)
}

export default ordersApi;