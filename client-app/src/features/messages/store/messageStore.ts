import { makeAutoObservable, runInAction } from "mobx";
import { ChatHub } from "../../../app/signalR/Hub";
import { Chat, Message, MessageType } from "../types/messagesTypes";
import { AcceptProposal, CommonMessage, ProposalMessage, ReadMessage, RejectProposal } from "../../../app/signalR/HubTypes";

export default class MessageStore {
  chats: Chat[] = [];
  chatHub: ChatHub;
  selectedUserIdForChat: string | null = null;
  selectedChatId: string | null = null;

  constructor(chatHub: ChatHub) {
    makeAutoObservable(this, {}, { deep: true, autoBind: true });
    this.chatHub = chatHub;

    chatHub.setHandler("ReceiveMessage", this.handleMessage);
    chatHub.setHandler("ReceiveProposal", this.handleProposal);
    chatHub.setHandler("ProposeAccepted", this.handleAccept);
    chatHub.setHandler("ProposeRejected", this.handleReject);
    chatHub.setHandler("ReadMessage", this.handleRead);
    chatHub.setHandler("ProposePaid", this.handlePaid);
  }

  private getChat(chatId: string) {
    return this.chats.find(c => c.id === chatId);
  }
  
  private sortMessages(messages: Message[]): Message[] {
    return messages.sort((a, b) => new Date(a.sentAt).getTime() - new Date(b.sentAt).getTime());
  }

  setSelectedUserForChat = (userId: string, currentUserId: string) => {
    this.selectedUserIdForChat = userId;

    const existingChat = this.chats.find(
      chat =>
        (chat.user1.userId === userId && chat.user2.userId === currentUserId) ||
        (chat.user2.userId === userId && chat.user1.userId === currentUserId)
    );

    this.selectedChatId = existingChat ? existingChat.id : null;
  };

  setSelectedChatId = (chatId: string | null) => {
    this.selectedChatId = chatId;
    if (chatId) {
      this.selectedUserIdForChat = null;
    }
  };

  clearSelectedUser = () => {
    this.selectedUserIdForChat = null;
  };

  handleMessage = (msg: CommonMessage) => {
    console.log("Receiving message", msg);
    runInAction(() => {
      if (!msg.chatId) {
        console.error("Received message without chatId:", msg);
        return;
      }
      
      const chat = this.getChat(msg.chatId);
      if (chat) {
        const existingMessage = chat.messages.find(m => m.id === msg.messageId);
        if (!existingMessage) {
          chat.messages.push(this.createMessage(msg, MessageType.Common));
          chat.messages = this.sortMessages(chat.messages);
        }
      } else {
        console.warn("Chat not found for message:", msg.chatId);
      }
    });
  };

  handleProposal = (msg: ProposalMessage) => {
    console.log("Receiving proposal", msg);
    runInAction(() => {
      if (!msg.chatId) {
        console.error("Received proposal without chatId:", msg);
        return;
      }
      
      const chat = this.getChat(msg.chatId);
      if (chat) {
        const existingMessage = chat.messages.find(m => m.id === msg.messageId);
        if (!existingMessage) {
          chat.messages.push(this.createMessage(msg, MessageType.Proposal));
          chat.messages = this.sortMessages(chat.messages);
        }
      } else {
        console.warn("Chat not found for proposal:", msg.chatId);
      }
    });
  };

  handleAccept = (msg: AcceptProposal) => {
    console.log("Accepting proposal:", msg);
    runInAction(() => {
      if (!msg.chatId) return;
      
      const chat = this.getChat(msg.chatId);
      const message = chat?.messages.find(m => m.id === msg.messageId);
      if (message) {
        message.isAccepted = true;
        message.acceptedAt = new Date().toISOString();
      }
    });
  };

  handleReject = (msg: RejectProposal) => {
    console.log("Rejecting proposal:", msg);
    runInAction(() => {
      if (!msg.chatId) return;
      
      const chat = this.getChat(msg.chatId);
      const message = chat?.messages.find(m => m.id === msg.messageId);
      if (message) message.isAccepted = false;
    });
  };
  
  handlePaid = (msg: { messageId: string; chatId: string }) => {
    console.log("Payment confirmed:", msg);
    runInAction(() => {
      if (!msg.chatId) return;
      
      const chat = this.getChat(msg.chatId);
      const message = chat?.messages.find(m => m.id === msg.messageId);
      if (message) {
        message.isPaid = true;
      }
    });
  };

  handleRead = (msg: ReadMessage) => {
    console.log("Marking message as read:", msg);
    runInAction(() => {
      if (!msg.chatId) return;
      
      const chat = this.getChat(msg.chatId);
      const message = chat?.messages.find(m => m.id === msg.messageId);
      if (message) {
        message.isRead = true;
        message.readBy = msg.readBy;
        message.readAt = new Date().toISOString();
      }
    });
  };

  createMessage(msg: CommonMessage | ProposalMessage, type: MessageType): Message {
    return {
      id: msg.messageId || '',
      chatId: msg.chatId!,
      content: msg.content,
      isRead: false,
      senderId: msg.senderId,
      receiverId: msg.receiverId,
      type: type,
      sentAt: new Date().toISOString(),
      readAt: null,
      acceptedAt: null,
      readBy: null,
      price: "price" in msg ? msg.price : null,
      isAccepted: null,
      isPaid: false,  
      postId: msg.postId
    };
  }

  sendMessage = async (chatId: string | undefined, content: string, receiverId: string, senderId: string) => {
    try {
      await this.chatHub.sendMessage({
        chatId: chatId ?? null,
        content,
        receiverId,
        senderId,
        postId: null
      });
    } catch (error) {
      console.error("Failed to send message:", error);
      throw error;
    }
  };

  sendProposal = async (chatId: string | undefined, content: string, receiverId: string, senderId: string, price: number, postId: string) => {
    try {
      await this.chatHub.sendProposal({
        chatId: chatId || null,
        content,
        receiverId,
        senderId,
        price,
        postId
      });
    } catch (error) {
      console.error("Failed to send proposal:", error);
      throw error;
    }
  };

  acceptProposal = async (accept: AcceptProposal) => {
    try {
      await this.chatHub.acceptProposal(accept);
    } catch (error) {
      console.error("Failed to accept proposal:", error);
      throw error;
    }
  };

  rejectProposal = async (reject: RejectProposal) => {
    try {
      await this.chatHub.rejectProposal(reject);
    } catch (error) {
      console.error("Failed to reject proposal:", error);
      throw error;
    }
  };

  payProposal = async (messageId: string, chatId: string) => {
    try {
      await this.chatHub.payProposal({ messageId, chatId });
    } catch (error) {
      console.error("Failed to pay proposal:", error);
      throw error;
    }
  };

  markAsRead = async (mark: ReadMessage) => {
    try {
      await this.chatHub.readMessage(mark);
    } catch (error) {
      console.error("Failed to mark as read:", error);
      throw error;
    }
  };
}