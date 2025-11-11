import apiClient from "../../../app/API/apiClient"
import ApiResponse from "../../../app/API/apiResponse"
import { GetChatMessagesRequest, GetChatMessagesResponse } from "../dto/messagesDto";
import { Chat } from "../types/messagesTypes"


const messagesApi = {
    getMyChats: async () => 
        apiClient.get<ApiResponse<Chat[]>>("user/chats/my-chats", false),
    getChatMessages: async (query: GetChatMessagesRequest) => 
        apiClient.post<GetChatMessagesResponse>(`user/chats/${query.chatId}/messages`, query, false)
}

export default messagesApi;