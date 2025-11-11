import apiClient from "../../../app/API/apiClient";
import ApiResponse from "../../../app/API/apiResponse";
import { GetMyNotificationsResponse } from "../dto/notificationsDto";

const notificationsApi = {
    getMyNotifications: () => 
        apiClient.get<ApiResponse<GetMyNotificationsResponse>>("notifications/my-notifications", false),
    readNotification: (id:string) =>
        apiClient.post<ApiResponse<void>>(`notifications/read/${id}`, null, false)
}

export default notificationsApi;