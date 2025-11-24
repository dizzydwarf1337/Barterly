import { PaginationResponse } from "../../../app/API/pagination";
import { Message } from "../types/messagesTypes";


export interface GetChatMessagesRequest {
    chatId: string;
    lastSentAt?: Date;
    page: number;
    pageSize: number;
};

export interface GetChatMessagesResponse extends PaginationResponse<Message> {};