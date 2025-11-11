import { 
    Drawer, 
    Box, 
    Typography, 
    IconButton, 
    List, 
    ListItem,
    CircularProgress,
} from "@mui/material";
import { Close, ArrowBack } from "@mui/icons-material";
import { observer } from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import { useEffect, useState } from "react";
import notificationsApi from "../api/notificationsApi";
import { useTranslation } from "react-i18next";
import { Notification } from "../types/notificationsTypes";

const NotificationsDrawer = observer(() => {
    const { uiStore, authStore } = useStore();
    const { t } = useTranslation();
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [selectedNotification, setSelectedNotification] = useState<Notification | null>(null);

    useEffect(() => {
        if (uiStore.notificationsOpen) {
            fetchNotifications();
            setSelectedNotification(null);
        }
    }, [uiStore.notificationsOpen]);

    const fetchNotifications = async () => {
        try {
            setIsLoading(true);
            const response = await notificationsApi.getMyNotifications();
            if (response.isSuccess) {
                setNotifications(response.value.notifications);
            }
        } catch (error) {
            console.error("Error loading notifications:", error);
            uiStore.showSnackbar(t("notifications:loadError"), "error");
        } finally {
            setIsLoading(false);
        }
    };

    const handleNotificationClick = async (notification: Notification) => {
        setSelectedNotification(notification);
        if (!notification.isRead) {
            try {
                const response = await notificationsApi.readNotification(notification.id);
                if (response.isSuccess) {
                    setNotifications(prev => 
                        prev.map(n => n.id === notification.id ? { ...n, isRead: "true" } : n)
                    );
                    authStore.reduceNotificationsCount();
                }
            } catch (error) {
                console.error("Error marking notification as read:", error);
            }
        }
    };

    const handleClose = () => {
        setSelectedNotification(null);
    };

    const unreadCount = notifications.filter(n => n.isRead === "false").length;

    return (
        <>
            <Drawer
                anchor="right"
                open={uiStore.notificationsOpen && !selectedNotification}
                onClose={() => uiStore.setNotificationsOpen()}
                sx={{
                    '& .MuiDrawer-paper': {
                        width: { xs: '100%', sm: 400 },
                        maxWidth: '100%'
                    }
                }}
            >
                <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column', py:10, px:1, gap:2 }}>
                    <Box sx={{ 
                        p: 2, 
                        display: 'flex', 
                        alignItems: 'center', 
                        justifyContent: 'space-between',
                        borderBottom: 1,
                        borderColor: 'divider'
                    }}>
                        <Box>
                            <Typography variant="h6" fontWeight={700}>
                                {t("notifications:title")}
                            </Typography>
                            {unreadCount > 0 && (
                                <Typography variant="caption" color="text.secondary">
                                    {t("notifications:unread", { count: unreadCount })}
                                </Typography>
                            )}
                        </Box>
                        <IconButton onClick={() => uiStore.setNotificationsOpen()}>
                            <Close />
                        </IconButton>
                    </Box>

                    <Box sx={{ flex: 1, overflow: 'auto' }}>
                        {isLoading ? (
                            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                                <CircularProgress />
                            </Box>
                        ) : notifications.length === 0 ? (
                            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%', p: 3 }}>
                                <Typography color="text.secondary" textAlign="center">
                                    {t("notifications:empty")}
                                </Typography>
                            </Box>
                        ) : (
                            <List sx={{ p: 0 }}>
                                {notifications.map((notification) => (
                                    <ListItem
                                        key={notification.id}
                                        onClick={() => handleNotificationClick(notification)}
                                        sx={{
                                            bgcolor: !!!notification.isRead ? 'action.hover' : 'transparent',
                                            flexDirection: 'column',
                                            alignItems: 'flex-start',
                                            gap: 1,
                                            py: 2,
                                            cursor: 'pointer',
                                            borderBottom: 1,
                                            borderColor: 'divider',
                                            '&:hover': {
                                                bgcolor: 'action.selected'
                                            }
                                        }}
                                    >
                                        <Typography 
                                            fontWeight={!!!notification.isRead ? 700 : 400} 
                                            variant="body1"
                                        >
                                            {notification.title}
                                        </Typography>
                                        <Typography 
                                            variant="body2" 
                                            color="text.secondary" 
                                            sx={{ 
                                                overflow: 'hidden',
                                                textOverflow: 'ellipsis',
                                                display: '-webkit-box',
                                                WebkitLineClamp: 2,
                                                WebkitBoxOrient: 'vertical',
                                            }}
                                        >
                                            {notification.message}
                                        </Typography>
                                        <Typography variant="caption" color="text.secondary">
                                            {new Date(notification.createdAt).toLocaleString()}
                                        </Typography>
                                    </ListItem>
                                ))}
                            </List>
                        )}
                    </Box>
                </Box>
            </Drawer>

            <Drawer
                anchor="right"
                open={!!selectedNotification}
                onClose={handleClose}
                sx={{
                    '& .MuiDrawer-paper': {
                        width: { xs: '100%', sm: 500 },
                        maxWidth: '100%'
                    }
                }}
            >
                {selectedNotification && (
                    <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column', py:10 }}>
                        <Box sx={{ 
                            p: 2, 
                            display: 'flex', 
                            alignItems: 'center', 
                            gap: 2,
                            borderBottom: 1,
                            borderColor: 'divider'
                        }}>
                            <IconButton onClick={handleClose}>
                                <ArrowBack />
                            </IconButton>
                            <Typography variant="h6" fontWeight={700}>
                                {t("notifications:details")}
                            </Typography>
                        </Box>

                        <Box sx={{ flex: 1, overflow: 'auto', p: 3 }}>
                            <Typography variant="h5" fontWeight={700} sx={{ mb: 2 }}>
                                {selectedNotification.title}
                            </Typography>
                            <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mb: 3 }}>
                                {new Date(selectedNotification.createdAt).toLocaleString()}
                            </Typography>
                            <Typography variant="body1" sx={{ whiteSpace: 'pre-wrap' }}>
                                {selectedNotification.message}
                            </Typography>
                        </Box>
                    </Box>
                )}
            </Drawer>
        </>
    );
});

export default NotificationsDrawer;