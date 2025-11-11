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
  }

  private getChat(chatId: string) {
    return this.chats.find(c => c.id === chatId);
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
      const chat = this.getChat(msg.chatId!);
      if (chat) {
        chat.messages.push(this.createMessage(msg, MessageType.Common));
      };
    });
  };

  handleProposal = (msg: ProposalMessage) => {
    console.log("Receiving proposal", msg);
    runInAction(() => {
      const chat = this.getChat(msg.chatId!);
      if (chat) chat.messages.push(this.createMessage(msg, MessageType.Proposal));
    });
  };

  handleAccept = (msg: AcceptProposal) => {
    console.log("Accepting proposal:", msg);
    runInAction(() => {
      const chat = this.getChat(msg.chatId!);
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
      const chat = this.getChat(msg.chatId!);
      const message = chat?.messages.find(m => m.id === msg.messageId);
      if (message) message.isAccepted = false;
    });
  };

  handleRead = (msg: ReadMessage) => {
    console.log("Marking message as read:", msg);
    runInAction(() => {
    const chat = this.getChat(msg.chatId!);
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
      id: '',
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
      postId: msg.postId
    };
  }

  // Methods to send messages via SignalR
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

  markAsRead = async (mark: ReadMessage) => {
    try {
      await this.chatHub.readMessage(mark);
    } catch (error) {
      console.error("Failed to mark as read:", error);
      throw error;
    }
  };
}