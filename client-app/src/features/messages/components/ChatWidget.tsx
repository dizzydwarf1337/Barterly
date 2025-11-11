import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { Fab, Paper, IconButton, Typography, Box } from "@mui/material";
import {
  Message as MessageIcon,
  Close as CloseIcon,
} from "@mui/icons-material";
import ChatDashboard from "./ChatDashboard";
import MessagesDashboard from "./MessagesDashboard";
import useStore from "../../../app/stores/store";

const ChatWidget = observer(() => {
  const { t } = useTranslation();
  const { authStore, messageStore, uiStore } = useStore();

  const handleChatSelect = (chatId: string) => {
    messageStore.setSelectedChatId(chatId);
  };

  const handleBackToChats = () => {
    messageStore.setSelectedChatId(null);
    messageStore.clearSelectedUser();
  };

  if (!authStore.user) return null;

  return (
    <>
      {!uiStore.isMessagesWidgetOpen && (
        <Fab
          color="primary"
          aria-label={t("chat.openChat")}
          onClick={() =>
            uiStore.setIsMessagesWidgetOpen(!uiStore.isMessagesWidgetOpen)
          }
          sx={{
            position: "fixed",
            bottom: 24,
            right: 24,
            zIndex: 1000,
          }}
        >
          <MessageIcon />
        </Fab>
      )}

      {uiStore.isMessagesWidgetOpen && (
        <Paper
          elevation={8}
          sx={{
            position: "fixed",
            bottom: 24,
            right: 24,
            width: 400,
            height: 600,
            display: "flex",
            flexDirection: "column",
            zIndex: 1000,
            overflow: "hidden",
          }}
        >
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              p: 2,
              bgcolor: "primary.main",
              color: "white",
            }}
          >
            <Typography variant="h6">
              {messageStore.selectedChatId
                ? t("chat.conversation")
                : t("chat.messages")}
            </Typography>
            <IconButton
              size="small"
              onClick={() =>
                uiStore.setIsMessagesWidgetOpen(!uiStore.isMessagesWidgetOpen)
              }
              sx={{ color: "white" }}
            >
              <CloseIcon />
            </IconButton>
          </Box>

          <Box sx={{ flex: 1, overflow: "hidden" }}>
            {messageStore.selectedChatId ||
            messageStore.selectedUserIdForChat ? (
              <MessagesDashboard
                chatId={messageStore.selectedChatId}
                receiverId={messageStore.selectedUserIdForChat}
                onBack={handleBackToChats}
              />
            ) : (
              <ChatDashboard onChatSelect={handleChatSelect} />
            )}
          </Box>
        </Paper>
      )}
    </>
  );
});

export default ChatWidget;