
export interface CommonMessage {
  messageId?: string; 
  senderId: string;
  receiverId: string;
  content: string;
  chatId?: string | null;
  postId: string | null;
  sentAt?: string;
}

export interface ProposalMessage {
  messageId?: string;  
  senderId: string;
  receiverId: string;
  content: string;
  price: number;
  chatId?: string | null;
  postId: string | null;
  sentAt?: string;  
}

export interface AcceptProposal {
  messageId: string;
  senderId: string;
  receiverId: string;
  chatId?: string | null;
}

export interface RejectProposal {
  messageId: string;
  senderId: string;
  receiverId: string;
  chatId?: string | null;
}

export interface ReadMessage {
  messageId: string;
  readBy: string;
  senderId: string;
  chatId?: string | null;
}