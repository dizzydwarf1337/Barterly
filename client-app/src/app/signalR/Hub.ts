import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { AcceptProposal, CommonMessage, ProposalMessage, ReadMessage, RejectProposal } from "./HubTypes";

export class ChatHub {
  private connection: HubConnection | null = null;

  private handlers = {
    ReceiveMessage: (_: CommonMessage) => {},
    ReceiveProposal: (_: ProposalMessage) => {},
    ProposeAccepted: (_: AcceptProposal) => {},
    ProposeRejected: (_: RejectProposal) => {},
    ReadMessage: (_: ReadMessage) => {},
    ProposePaid: (_: { messageId: string; chatId: string }) => {}, 
  };

  setHandler<T extends keyof typeof this.handlers>(
    event: T,
    handler: typeof this.handlers[T]
  ) {
    this.handlers[event] = handler;
    if (this.connection) {
      this.connection.off(event);
      this.connection.on(event, handler);
    }
  }

  async connect(token: string) {
    if (this.connection) return;

    this.connection = new HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_API_URL}/chatHub`, {
        accessTokenFactory: () => token ?? localStorage.getItem("token"),
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    for (const [event, handler] of Object.entries(this.handlers)) {
      this.connection.on(event, handler);
    }

    try {
      await this.connection.start();
      console.log("Connected to chat hub");
    } catch (err) {
      console.error("Failed to connect:", err);
    }
  }

  private async invoke<T>(method: string, payload: T) {
    if (this.connection?.state === HubConnectionState.Connected) {
      await this.connection.invoke(method, payload);
    } else {
      console.warn(`Can't invoke ${method} — not connected`);
    }
  }

  sendMessage(msg: CommonMessage) {
    console.log("Sending message:", msg);
    return this.invoke("SendMessage", msg);
  }

  sendProposal(msg: ProposalMessage) {
    console.log("Sending proposal:", msg);
    return this.invoke("SendPropose", msg);
  }

  acceptProposal(msg: AcceptProposal) {
    console.log("Accepting proposal:", msg);
    return this.invoke("AcceptPropose", msg);
  }

  rejectProposal(msg: RejectProposal) {
    console.log("Rejecting proposal:", msg);
    return this.invoke("RejectPropose", msg);
  }

  payProposal(msg: { messageId: string; chatId: string }) {
    console.log("Paying proposal:", msg);
    return this.invoke("PayPropose", msg);
  }

  readMessage(msg: ReadMessage) {
    return this.invoke("ReadMessage", msg);
  }

  async disconnect() {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      console.log("Disconnected from chat hub");
    }
  }
}