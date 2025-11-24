import apiClient from "../../../app/API/apiClient";
import { GetMyOrdersResponseDTO, GetPlacedOrdersResponseDTO } from "../dto/orderDto";


const ordersApi = {
    getMyOrders: async () => 
        apiClient.get<GetMyOrdersResponseDTO>("/users/order/my", false),
    getMyPlacedOrders: async () => 
        apiClient.get<GetPlacedOrdersResponseDTO>("/users/order/my-placed", false)
}

export default ordersApi;