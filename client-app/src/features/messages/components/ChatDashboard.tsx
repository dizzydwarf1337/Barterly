import { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import {
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  ListItemAvatar,
  Avatar,
  Typography,
  CircularProgress,
  Badge,
  Divider,
} from "@mui/material";
import { Chat, ChatUser } from "../types/messagesTypes";
import useStore from "../../../app/stores/store";
import messagesApi from "../api/messagesApi";

interface ChatDashboardProps {
  onChatSelect: (chatId: string) => void;
}

const ChatDashboard = observer(({ onChatSelect }: ChatDashboardProps) => {
  const { t } = useTranslation();
  const [loading, setLoading] = useState(false);
  const { authStore, messageStore } = useStore();
  useEffect(() => {
    loadChats();
  }, []);

  const loadChats = async () => {
    setLoading(true);
    try {
      const response = await messagesApi.getMyChats();
      if(response.isSuccess && response.value) {
        messageStore.chats = response.value;
      }
    } catch (error) {
      console.error("Failed to load chats:", error);
    } finally {
      setLoading(false);
    }
  };

  const getOtherUserId = (chat: Chat): ChatUser => {
    return chat.user1.userId === authStore.user?.id ? chat.user2 : chat.user1;
  };

  const getLastMessage = (chat: Chat) => {
    return chat.messages[chat.messages.length - 1];
  };

  const getUnreadCount = (chat: Chat): number => {
    return chat.messages.filter(
      (m) => !m.isRead && m.receiverId === authStore.user?.id
    ).length;
  };

  if (loading) {
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

  if (messageStore.chats.length === 0) {
    return (
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "100%",
          p: 3,
        }}
      >
        <Typography color="text.secondary">{t("chat.noChats")}</Typography>
      </Box>
    );
  }

  return (
    <List sx={{ width: "100%", bgcolor: "background.paper", p: 0 }}>
      {messageStore.chats.map((chat, index) => {
        const lastMessage = getLastMessage(chat);
        const unreadCount = getUnreadCount(chat);
        const otherUser = getOtherUserId(chat);

        return (
          <>
            <ListItem disablePadding>
              <ListItemButton onClick={() => onChatSelect(chat.id)}>
                <ListItemAvatar>
                  <Badge
                    badgeContent={unreadCount}
                    color="primary"
                    overlap="circular"
                  >
                    <Avatar src={otherUser.imagePath || undefined}>
                      {(!otherUser.imagePath && otherUser.firstName?.charAt(0).toUpperCase())}
                    </Avatar>
                  </Badge>
                </ListItemAvatar>
                <ListItemText
                  primary={
                    <Typography
                      variant="subtitle2"
                      fontWeight={unreadCount > 0 ? 600 : 400}
                    >
                      {otherUser.firstName} {otherUser.lastName}
                    </Typography>
                  }
                  secondary={
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                        whiteSpace: "nowrap",
                      }}
                    >
                      {lastMessage?.content || t("chat.noMessages")}
                    </Typography>
                  }
                />
              </ListItemButton>
            </ListItem>
            {index < messageStore.chats.length - 1 && <Divider />}
          </>
        );
      })}
    </List>
  );
});

export default ChatDashboard;
