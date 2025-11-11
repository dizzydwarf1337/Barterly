export interface Chat {
    id: string;
    createdAt: string;
    user1: ChatUser;
    user2: ChatUser;
    messages: Message[];
}

export interface Message {
    id: string;
    chatId: string;
    content: string;
    isRead: boolean;
    type: MessageType;
    senderId: string;
    receiverId: string;
    readBy: string | null;
    sentAt: string;
    readAt: string | null;
    acceptedAt: string | null;
    price: number | null;
    isAccepted: boolean | null;
    isPaid: boolean | null; 
    postId: string | null;
}

export enum MessageType {
    Common,
    Proposal
}

export interface ChatUser {
    userId: string;
    firstName: string;
    lastName: string;
    imagePath?: string | null;
}