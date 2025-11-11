import { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { 
  Box, 
  Paper, 
  Typography, 
  Button, 
  Chip, 
  Card,
  CardContent,
  CircularProgress,
  alpha
} from "@mui/material";
import {
  Done as DoneIcon,
  DoneAll as DoneAllIcon,
  Check as CheckIcon,
  Close as CloseIcon,
  Payment as PaymentIcon,
  HourglassEmpty as PendingIcon,
} from "@mui/icons-material";
import {
  Message as MessageType,
  MessageType as MsgType,
} from "../types/messagesTypes";
import {
  AcceptProposal,
  ReadMessage,
  RejectProposal,
} from "../../../app/signalR/HubTypes";
import useStore from "../../../app/stores/store";
import PostSmallItem from "../../posts/components/PostSmallItem";
import userApi from "../../users/api/userApi";
import { PostPreview } from "../../posts/types/postTypes";

interface MessageProps {
  message: MessageType;
}

const Message = observer(({ message }: MessageProps) => {
  const { authStore, messageStore, uiStore } = useStore();
  const { t } = useTranslation();
  const isOwnMessage = message.senderId === authStore.user?.id;
  const [post, setPost] = useState<PostPreview | null>(null);
  const [loadingPost, setLoadingPost] = useState(false);
  const [paying, setPaying] = useState(false);

  useEffect(() => {
    if (!isOwnMessage && !message.isRead && authStore.user) {
      const readMessage: ReadMessage = {
        messageId: message.id,
        readBy: authStore.user.id,
        senderId: message.senderId,
        chatId: message.chatId,
      };
      messageStore.markAsRead(readMessage);
    }
  }, []);

  useEffect(() => {
    const loadPost = async () => {
      if (message.type === MsgType.Proposal && message.postId) {
        setLoadingPost(true);
        try {
          const ownerId = isOwnMessage ? message.receiverId : message.senderId;
          const ownerResponse = await userApi.getPostOwner({ id: ownerId });
          
          if (ownerResponse.isSuccess && ownerResponse.value?.posts) {
            const foundPost = ownerResponse.value.posts.find(
              (p: PostPreview) => p.id === message.postId
            );
            if (foundPost) {
              setPost(foundPost);
            }
          }
        } catch (error) {
          console.error("Failed to load post:", error);
        } finally {
          setLoadingPost(false);
        }
      }
    };

    loadPost();
  }, [message.postId, message.type]);

  const handleAcceptProposal = async () => {
    if (!authStore.user) return;

    try {
      const accept: AcceptProposal = {
        messageId: message.id,
        senderId: message.senderId,
        receiverId: message.receiverId,
        chatId: message.chatId,
      };
      await messageStore.acceptProposal(accept);
      uiStore.showSnackbar(t("chat.proposalAcceptedSuccess"), "success");
    } catch (error) {
      console.error("Failed to accept proposal:", error);
      uiStore.showSnackbar(t("chat.failedToAcceptProposal"), "error");
    }
  };

  const handleRejectProposal = async () => {
    if (!authStore.user) return;

    try {
      const reject: RejectProposal = {
        messageId: message.id,
        senderId: message.senderId,
        receiverId: message.receiverId,
        chatId: message.chatId,
      };
      await messageStore.rejectProposal(reject);
      uiStore.showSnackbar(t("chat.proposalRejectedSuccess"), "success");
    } catch (error) {
      console.error("Failed to reject proposal:", error);
      uiStore.showSnackbar(t("chat.failedToRejectProposal"), "error");
    }
  };

  const handlePayProposal = async () => {
    if (!authStore.user) return;

    setPaying(true);
    try {
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      await messageStore.payProposal(message.id, message.chatId);
      uiStore.showSnackbar(t("chat.paymentSuccess"), "success");
    } catch (error) {
      console.error("Failed to pay proposal:", error);
      uiStore.showSnackbar(t("chat.failedToPayProposal"), "error");
    } finally {
      setPaying(false);
    }
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
  };

  const renderReadStatus = () => {
    if (!isOwnMessage) return null;

    if (message.isRead) {
      return <DoneAllIcon sx={{ fontSize: 16, color: "primary.main" }} />;
    }
    return <DoneIcon sx={{ fontSize: 16, color: "text.secondary" }} />;
  };

  const renderProposalActions = () => {
    if (message.type !== MsgType.Proposal || isOwnMessage) return null;

    if (message.isPaid) {
      return (
        <Chip
          icon={<CheckIcon />}
          label={t("chat.paid")}
          color="success"
          size="small"
          sx={{ mt: 1 }}
        />
      );
    }

    if (message.isAccepted === true) {
      return (
        <Chip
          icon={<PendingIcon />}
          label={t("chat.pendingPayment")}
          color="warning"
          size="small"
          sx={{ mt: 1 }}
        />
      );
    }

    if (message.isAccepted === false) {
      return (
        <Chip
          icon={<CloseIcon />}
          label={t("chat.proposalRejected")}
          color="error"
          size="small"
          sx={{ mt: 1 }}
        />
      );
    }

    return (
      <Box sx={{ display: "flex", gap: 1, mt: 1 }}>
        <Button
          size="small"
          variant="contained"
          color="success"
          onClick={handleAcceptProposal}
          startIcon={<CheckIcon />}
        >
          {t("chat.accept")}
        </Button>
        <Button
          size="small"
          variant="outlined"
          color="error"
          onClick={handleRejectProposal}
          startIcon={<CloseIcon />}
        >
          {t("chat.reject")}
        </Button>
      </Box>
    );
  };

  const renderProposalStatus = () => {
    if (message.type !== MsgType.Proposal || !isOwnMessage) return null;

    if (message.isPaid) {
      return (
        <Chip
          icon={<CheckIcon />}
          label={t("chat.paid")}
          color="success"
          size="small"
          sx={{ mt: 1 }}
        />
      );
    }

    if (message.isAccepted === true) {
      return (
        <Button
          size="small"
          variant="contained"
          color="primary"
          onClick={handlePayProposal}
          disabled={paying}
          startIcon={paying ? <CircularProgress size={16} /> : <PaymentIcon />}
          sx={{ mt: 1 }}
        >
          {paying ? t("chat.processing") : t("chat.pay")}
        </Button>
      );
    }

    if (message.isAccepted === false) {
      return (
        <Chip
          icon={<CloseIcon />}
          label={t("chat.proposalRejected")}
          color="error"
          size="small"
          sx={{ mt: 1 }}
        />
      );
    }

    return (
      <Chip label={t("chat.proposalPending")} size="small" sx={{ mt: 1 }} />
    );
  };

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: isOwnMessage ? "flex-end" : "flex-start",
        mb: 1,
      }}
    >
      <Paper
        elevation={1}
        sx={{
          p: 1.5,
          maxWidth: "70%",
          bgcolor: isOwnMessage ? "primary.light" : "background.paper",
          color: isOwnMessage ? "primary.contrastText" : "text.primary",
        }}
      >
        {message.type === MsgType.Proposal && (
          <Box
            sx={{
              mb: 1,
              pb: 1,
              borderBottom: 1,
              borderColor: isOwnMessage ? "primary.contrastText" : "divider",
            }}
          >
            <Typography variant="caption" sx={{ fontWeight: 600 }}>
              {t("chat.proposal")}
            </Typography>
            <Typography variant="h6" sx={{ mt: 0.5 }}>
              ${message.price}
            </Typography>
            
            {loadingPost && (
              <Box sx={{ display: "flex", justifyContent: "center", mt: 1 }}>
                <CircularProgress size={20} />
              </Box>
            )}
            
            {post && (
              <Card 
                sx={{ 
                  mt: 1, 
                  bgcolor: alpha(isOwnMessage ? "#fff" : "background.default", 0.7),
                  cursor: "default"
                }}
              >
                <CardContent sx={{ p: 1, "&:last-child": { pb: 1 } }}>
                  <PostSmallItem post={post} />
                </CardContent>
              </Card>
            )}
          </Box>
        )}

        <Typography variant="body2" sx={{ wordBreak: "break-word" }}>
          {message.content}
        </Typography>

        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            mt: 0.5,
            gap: 1,
          }}
        >
          <Typography
            variant="caption"
            sx={{
              color: isOwnMessage ? "primary.contrastText" : "text.secondary",
              opacity: 0.7,
            }}
          >
            {formatTime(message.sentAt)}
          </Typography>
          {renderReadStatus()}
        </Box>

        {renderProposalActions()}
        {renderProposalStatus()}
      </Paper>
    </Box>
  );
});

export default Message;