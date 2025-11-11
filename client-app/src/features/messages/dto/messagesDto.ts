import { PaginationResponse } from "../../../app/API/pagination";
import { Message } from "../types/messagesTypes";


export interface GetChatMessagesRequest {
    chatId: string;
    page: number;
    pageSize: number;
};

export interface GetChatMessagesResponse extends PaginationResponse<Message> {};