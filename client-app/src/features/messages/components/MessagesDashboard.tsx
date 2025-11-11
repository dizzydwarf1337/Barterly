import React, { useEffect, useState, useRef } from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import {
  Box,
  IconButton,
  TextField,
  CircularProgress,
  Menu,
  MenuItem,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Avatar,
  Typography,
  FormControl,
  InputLabel,
  Select,
} from "@mui/material";
import {
  ArrowBack as ArrowBackIcon,
  Send as SendIcon,
  MoreVert as MoreVertIcon,
} from "@mui/icons-material";
import useStore from "../../../app/stores/store";
import MessageComponent from "./Message";
import userApi from "../../users/api/userApi";
import { PostOwner } from "../../users/types/userTypes";
import messagesApi from "../api/messagesApi";
import PostSmallItem from "../../posts/components/PostSmallItem";

interface MessagesDashboardProps {
  chatId: string | null;
  receiverId: string | null;
  onBack: () => void;
}
const MessagesDashboard = observer(
  ({ chatId, receiverId, onBack }: MessagesDashboardProps) => {
    console.log(chatId);

    const { messageStore, authStore, uiStore } = useStore();
    const { t } = useTranslation();
    const [messageText, setMessageText] = useState("");
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [proposalDialogOpen, setProposalDialogOpen] = useState(false);
    const [proposalPrice, setProposalPrice] = useState("");
    const [proposalPostId, setProposalPostId] = useState("");
    const [proposalContent, setProposalContent] = useState("");
    const [otherUser, setOtherUser] = useState<PostOwner | null>(null);
    const [userLoading, setUserLoading] = useState(false);
    const messagesEndRef = useRef<HTMLDivElement>(null);
    const messagesContainerRef = useRef<HTMLDivElement>(null);
    const [prevScrollHeight, setPrevScrollHeight] = useState(0);

    const chat = chatId
      ? messageStore.chats.find((c) => c.id === chatId)
      : null;
    const isNewChat = !chatId && receiverId;

    useEffect(() => {
      if (chatId) {
        loadMessages(1);
      }
    }, [chatId]);

    useEffect(() => {
      if (isNewChat && receiverId) {
        loadOtherUser(receiverId);
      } else if (chat) {
        const otherUserId = getOtherUserId();
        if (otherUserId) {
          loadOtherUser(otherUserId);
        }
      }
    }, [receiverId, chat]);

    useEffect(() => {
      if (page === 1) {
        setTimeout(() => scrollToBottom(false), 100);
      } else {
        const container = messagesContainerRef.current;
        if (container) {
          container.scrollTop = container.scrollHeight - prevScrollHeight;
        }
      }
    }, [chat?.messages.length]);

    const loadOtherUser = async (userId: string) => {
      try {
        setUserLoading(true);
        const response = await userApi.getPostOwner({ id: userId });
        setOtherUser(response.value);
      } catch (error) {
        console.error("Failed to fetch user:", error);
      } finally {
        setUserLoading(false);
      }
    };

    const loadMessages = async (
      pageNum: number,
      shouldReplace: boolean = false
    ) => {
      if (!chatId || !chat) return;

      setLoading(true);
      try {
        const res = await messagesApi.getChatMessages({
          chatId: chatId,
          page: pageNum,
          pageSize: 20,
        });

        if (res.isSuccess) {
          if (shouldReplace || pageNum === 1) {
            chat.messages = res.value.items;
          } else {
            const existingIds = new Set(chat.messages.map((m) => m.id));
            const newMessages = res.value.items.filter(
              (m) => !existingIds.has(m.id)
            );
            chat.messages = [...newMessages, ...chat.messages];
          }
          setHasMore(res.value.totalPages > pageNum);
        }
      } catch (error) {
        console.error("Failed to load messages:", error);
        uiStore.showSnackbar("Failed to load messages", "error");
      } finally {
        setLoading(false);
      }
    };

    const currentPageRef = useRef(page);
    useEffect(() => {
      currentPageRef.current = page;
    }, [page]);

    const handleScroll = () => {
      const container = messagesContainerRef.current;
      if (!container || loading || !hasMore || isNewChat) return;

      if (container.scrollTop === 0) {
        setPrevScrollHeight(container.scrollHeight);
        const nextPage = currentPageRef.current + 1;
        setPage(nextPage);
        loadMessages(nextPage, false);
      }
    };

    const scrollToBottom = (smooth: boolean = true) => {
      messagesEndRef.current?.scrollIntoView({
        behavior: smooth ? "smooth" : "auto",
      });
    };

    const getOtherUserId = (): string => {
      if (!chat || !authStore.user) return "";
      return chat.user1.userId === authStore.user.id
        ? chat.user2.userId
        : chat.user1.userId;
    };

    const handleSendMessage = async () => {
      if (!messageText || !messageText.trim()) return;
      if (!authStore.user) return;

      const receiver = isNewChat ? receiverId! : getOtherUserId();

      try {
        const newChatId = chatId ?? crypto.randomUUID();
        if (isNewChat && !chatId) {
          messageStore.chats.push({
            id: newChatId,
            user1: {
              ...authStore.user,
              userId: authStore.user.id,
              imagePath: authStore.user.profilePicturePath,
            },
            user2: {
              userId: receiver,
              ...otherUser,
              imagePath: otherUser?.profilePicturePath,
              firstName: otherUser?.firstName ?? "",
              lastName: otherUser?.lastName ?? "",
            },
            messages: [],
            createdAt: new Date().toISOString(),
          });
          messageStore.setSelectedChatId(newChatId);
        }
        await messageStore.sendMessage(
          newChatId,
          messageText,
          receiver,
          authStore.user.id
        );
        setMessageText("");
        scrollToBottom();
      } catch (error) {
        console.error("Failed to send message:", error);
        uiStore.showSnackbar(t("chat.failedToSendMessage"), "error");
      }
    };

    const handleSendProposal = async () => {
      if (
        !proposalContent.trim() ||
        !proposalPrice ||
        !proposalPostId ||
        !authStore.user
      )
        return;
      const receiver = isNewChat ? receiverId! : getOtherUserId();
      const price = Number(proposalPrice);
      const newChatId = chatId ?? crypto.randomUUID();
      if (isNewChat && !chatId) {
        messageStore.chats.push({
          id: newChatId,
          user1: {
            ...authStore.user,
            userId: authStore.user.id,
            imagePath: authStore.user.profilePicturePath,
          },
          user2: {
            userId: receiver,
            ...otherUser,
            imagePath: otherUser?.profilePicturePath,
            firstName: otherUser?.firstName ?? "",
            lastName: otherUser?.lastName ?? "",
          },
          messages: [],
          createdAt: new Date().toISOString(),
        });
        messageStore.setSelectedChatId(newChatId);
      }
      if (isNaN(price) || price <= 0) {
        uiStore.showSnackbar(t("chat.invalidPrice"), "error");
        return;
      }

      try {
        await messageStore.sendProposal(
          newChatId,
          proposalContent,
          receiver,
          authStore.user.id,
          price,
          proposalPostId
        );
        setProposalContent("");
        setProposalPrice("");
        setProposalDialogOpen(false);
        handleMenuClose();
        scrollToBottom();
      } catch (error) {
        console.error("Failed to send proposal:", error);
        uiStore.showSnackbar(t("chat.failedToSendProposal"), "error");
      }
    };

    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
      setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
      setAnchorEl(null);
    };

    const handleProposalDialogOpen = () => {
      setProposalDialogOpen(true);
      handleMenuClose();
    };

    const handleProposalDialogClose = () => {
      setProposalDialogOpen(false);
      setProposalContent("");
      setProposalPrice("");
    };

    const handleKeyPress = (e: React.KeyboardEvent) => {
      if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();
        handleSendMessage();
      }
    };

    if (loading && page === 1 && !isNewChat) {
      return (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
          }}
        >
          <CircularProgress />
        </Box>
      );
    }

    return (
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          height: "100%",
        }}
      >
        {/* Header */}
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            p: 1,
            borderBottom: 1,
            borderColor: "divider",
            gap: 2,
          }}
        >
          <IconButton onClick={onBack} size="small">
            <ArrowBackIcon />
          </IconButton>

          {userLoading ? (
            <CircularProgress size={24} />
          ) : (
            otherUser && (
              <>
                <Avatar
                  src={otherUser.profilePicturePath ?? undefined}
                  sx={{ width: 40, height: 40 }}
                >
                  {!otherUser.profilePicturePath &&
                    `${otherUser.firstName.charAt(
                      0
                    )}${otherUser.lastName.charAt(0)}`}
                </Avatar>
                <Box flex={1}>
                  <Typography variant="subtitle2" fontWeight={600}>
                    {otherUser.firstName} {otherUser.lastName}
                  </Typography>
                </Box>
              </>
            )
          )}

          <IconButton onClick={handleMenuOpen} size="small">
            <MoreVertIcon />
          </IconButton>
          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={handleMenuClose}
          >
            <MenuItem onClick={handleProposalDialogOpen}>
              {t("chat.sendProposal")}
            </MenuItem>
          </Menu>
        </Box>

        {/* Messages */}
        <Box
          ref={messagesContainerRef}
          onScroll={handleScroll}
          sx={{
            flex: 1,
            overflowY: "auto",
            p: 2,
            display: "flex",
            flexDirection: "column",
            gap: 1,
          }}
        >
          {loading && page > 1 && (
            <Box sx={{ display: "flex", justifyContent: "center", py: 2 }}>
              <CircularProgress size={20} />
            </Box>
          )}

          {isNewChat && (
            <Box
              sx={{
                flex: 1,
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                textAlign: "center",
                p: 3,
              }}
            >
              <Typography variant="body2" color="text.secondary">
                {t("chat.startConversation")}
              </Typography>
            </Box>
          )}

          {chat?.messages.map((message) => (
            <MessageComponent key={message.id} message={message} />
          ))}
          <div ref={messagesEndRef} />
        </Box>

        {/* Input */}
        <Box
          sx={{
            p: 2,
            borderTop: 1,
            borderColor: "divider",
            display: "flex",
            gap: 1,
          }}
        >
          <TextField
            fullWidth
            size="small"
            placeholder={t("chat.typeMessage")}
            value={messageText}
            onChange={(e) => setMessageText(e.target.value)}
            onKeyPress={handleKeyPress}
            multiline
            maxRows={3}
          />
          <IconButton
            color="primary"
            onClick={handleSendMessage}
            disabled={!messageText.trim()}
          >
            <SendIcon />
          </IconButton>
        </Box>

        {/* Proposal Dialog */}
        <Dialog
          open={proposalDialogOpen}
          onClose={handleProposalDialogClose}
          maxWidth="sm"
          fullWidth
        >
          <DialogTitle>{t("chat.sendProposal")}</DialogTitle>
          <DialogContent>
            <FormControl fullWidth margin="normal">
              <InputLabel>{t("chat.selectPost")}</InputLabel>
              <Select
                value={proposalPostId}
                label={t("chat.selectPost")}
                onChange={(e) => setProposalPostId(e.target.value)}
                renderValue={(selected) => {
                  const post = otherUser?.posts?.find((p) => p.id === selected);
                  return post ? post.title : "";
                }}
              >
                {otherUser?.posts?.map((post) => (
                  <MenuItem key={post.id} value={post.id}>
                    <PostSmallItem post={post} />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <TextField
              fullWidth
              label={t("chat.price")}
              type="number"
              value={proposalPrice}
              onChange={(e) => setProposalPrice(e.target.value)}
              margin="normal"
              inputProps={{ min: 0, step: 0.01 }}
            />
            <TextField
              fullWidth
              label={t("chat.message")}
              multiline
              rows={3}
              value={proposalContent}
              onChange={(e) => setProposalContent(e.target.value)}
              margin="normal"
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleProposalDialogClose}>
              {t("common.cancel")}
            </Button>
            <Button
              onClick={handleSendProposal}
              variant="contained"
              disabled={
                !proposalContent.trim() || !proposalPrice || !proposalPostId
              }
            >
              {t("common.send")}
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
    );
  }
);

export default MessagesDashboard;
