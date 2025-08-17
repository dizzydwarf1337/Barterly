import {
  Dialog,
  DialogContent,
  Box,
  Typography,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Divider,
  Avatar,
  TextField,
  InputAdornment,
  Slide,
  useTheme,
  alpha,
  Switch,
  Chip,
  Badge,
} from "@mui/material";
import { TransitionProps } from "@mui/material/transitions";
import { forwardRef, useState, ReactElement } from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";

// Icons
import CloseIcon from "@mui/icons-material/Close";
import SearchIcon from "@mui/icons-material/Search";
import HomeIcon from "@mui/icons-material/Home";
import CategoryIcon from "@mui/icons-material/Category";
import AddIcon from "@mui/icons-material/Add";
import PersonIcon from "@mui/icons-material/Person";
import LoginIcon from "@mui/icons-material/Login";
import LogoutIcon from "@mui/icons-material/Logout";
import SettingsIcon from "@mui/icons-material/Settings";
import NotificationsIcon from "@mui/icons-material/Notifications";
import DarkModeIcon from "@mui/icons-material/DarkMode";
import LightModeIcon from "@mui/icons-material/LightMode";
import LanguageIcon from "@mui/icons-material/Language";
import FavoriteIcon from "@mui/icons-material/Favorite";
import HelpIcon from "@mui/icons-material/Help";
import InfoIcon from "@mui/icons-material/Info";
import useStore from "../stores/store";
import authApi from "../../features/auth/api/authApi";

const Transition = forwardRef(function Transition(
  props: TransitionProps & {
    children: ReactElement;
  },
  ref: React.Ref<unknown>
) {
  return <Slide direction="left" ref={ref} {...props} />;
});

interface MenuItem {
  icon: ReactElement;
  text: string;
  path?: string;
  onClick?: () => void;
  badge?: number;
  chip?: string;
  divider?: boolean;
  requireAuth?: boolean;
  hideWhenAuth?: boolean;
}

export default observer(function MobileNavDialog() {
  const { uiStore, authStore } = useStore();
  const { t, i18n } = useTranslation();
  const navigate = useNavigate();
  const theme = useTheme();

  const [searchQuery, setSearchQuery] = useState("");

  const handleClose = () => {
    uiStore.setIsMobileMenuOpen(false);
  };

  const handleNavigate = (path: string) => {
    navigate(path);
    handleClose();
  };

  const handleSearch = (event: React.FormEvent) => {
    event.preventDefault();
    if (searchQuery.trim()) {
      navigate(`/search?q=${encodeURIComponent(searchQuery.trim())}`);
      handleClose();
    }
  };

  const handleAddPost = () => {
    if (authStore.isLoggedIn) {
      navigate("/posts/create");
    } else {
      uiStore.showSnackbar(t("pleaseLoginToAddPost"), "warning", "center");
      navigate("/login");
    }
    handleClose();
  };

  const handleLogin = () => {
    navigate("/login");
    handleClose();
  };

  const handleLogout = async () => {
    try {
      await authApi.logout();
      await authStore.logout();
      uiStore.showSnackbar(t("logoutSuccess"), "success", "center");
      navigate("/");
    } catch (error) {
      uiStore.showSnackbar(t("logoutFailed"), "error", "center");
    }
    handleClose();
  };

  // ✅ ИСПРАВЛЕНО: Теперь используем правильный метод из uiStore
  const handleThemeChange = () => {
    uiStore.changeTheme();
  };

  // ✅ ИСПРАВЛЕНО: Добавлен обработчик смены языка как в основном NavBar
  const handleLanguageChange = () => {
    const newLanguage = i18n.language === "en" ? "pl" : "en";
    i18n.changeLanguage(newLanguage);
    localStorage.setItem("brt_lng", newLanguage);
    uiStore.setLanguage(newLanguage);
  };

  // Menu items configuration
  const menuItems: MenuItem[] = [
    {
      icon: <HomeIcon />,
      text: t("Home"),
      path: "/",
    },
    {
      icon: <CategoryIcon />,
      text: t("categories"),
      path: "/categories",
    },
    {
      icon: <AddIcon />,
      text: t("addPost"),
      onClick: handleAddPost,
      chip: authStore.isLoggedIn ? undefined : t("loginRequired"),
    },
    { divider: true } as MenuItem,
    ...(authStore.isLoggedIn
      ? [
          {
            icon: (
              <Badge
                badgeContent={authStore.user?.notificationCount}
                color="error"
              >
                <NotificationsIcon />
              </Badge>
            ),
            text: t("notifications"),
            badge: authStore.user?.notificationCount,
            path: "/notifications",
          },
          {
            icon: <FavoriteIcon />,
            text: t("favorites"),
            path: "/favorites",
          },
          {
            icon: <SettingsIcon />,
            text: t("settings"),
            path: "/settings",
          },
          { divider: true } as MenuItem,
          {
            icon: <LogoutIcon />,
            text: t("logout"),
            onClick: handleLogout,
          },
        ]
      : [
          {
            icon: <LoginIcon />,
            text: t("login"),
            onClick: handleLogin,
          },
        ]),
    { divider: true } as MenuItem,
    // Help section
    {
      icon: <HelpIcon />,
      text: t("help"),
      path: "/help",
    },
    {
      icon: <InfoIcon />,
      text: t("about"),
      path: "/about",
    },
  ];

  const filteredMenuItems = menuItems.filter((item) => {
    if (item.requireAuth && !authStore.isLoggedIn) return false;
    if (item.hideWhenAuth && authStore.isLoggedIn) return false;
    return true;
  });

  return (
    <Dialog
      open={uiStore.isMobileMenuOpen}
      onClose={handleClose}
      TransitionComponent={Transition}
      keepMounted
      PaperProps={{
        sx: {
          width: "100%",
          maxWidth: 400,
          height: "100%",
          m: 0,
          position: "fixed",
          right: 0,
          top: 0,
          borderRadius: "16px 0 0 16px",
          background: `linear-gradient(145deg, ${alpha(
            theme.palette.background.paper,
            0.95
          )}, ${alpha(theme.palette.background.default, 0.9)})`,
          backdropFilter: "blur(20px)",
          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        },
      }}
      BackdropProps={{
        sx: {
          backgroundColor: alpha(theme.palette.common.black, 0.5),
          backdropFilter: "blur(4px)",
        },
      }}
    >
      <DialogContent
        sx={{
          p: 0,
          height: "100%",
          display: "flex",
          flexDirection: "column",
        }}
      >
        {/* Header */}
        <Box
          sx={{
            p: 3,
            borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            background: `linear-gradient(135deg, ${theme.palette.primary.main}15, ${theme.palette.secondary.main}10)`,
          }}
        >
          <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center"
            mb={2}
          >
            <Typography
              variant="h5"
              fontWeight="bold"
              sx={{
                background: `linear-gradient(45deg, ${theme.palette.primary.main}, ${theme.palette.secondary.main})`,
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              Barterly
            </Typography>
            <IconButton
              onClick={handleClose}
              sx={{
                transition: "all 0.2s ease",
                "&:hover": {
                  transform: "rotate(90deg)",
                  backgroundColor: alpha(theme.palette.error.main, 0.1),
                },
              }}
            >
              <CloseIcon />
            </IconButton>
          </Box>

          {authStore.isLoggedIn && (
            <Box display="flex" alignItems="center" gap={2} mb={2}>
              <Avatar
                src={authStore.user?.profilePicturePath ?? undefined}
                sx={{
                  width: 48,
                  height: 48,
                  border: `2px solid ${theme.palette.primary.main}`,
                }}
              >
                {authStore.user?.firstName?.charAt(0) || <PersonIcon />}
              </Avatar>
            </Box>
          )}

          {/* Search */}
          <Box
            component="form"
            onSubmit={handleSearch}
            sx={{
              position: "relative",
            }}
          >
            <TextField
              fullWidth
              size="small"
              placeholder={t("searchPlaceholder")}
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon color="action" />
                  </InputAdornment>
                ),
                sx: {
                  borderRadius: 3,
                  backgroundColor: alpha(theme.palette.background.paper, 0.8),
                  "&:hover": {
                    backgroundColor: alpha(theme.palette.background.paper, 0.9),
                  },
                },
              }}
            />
          </Box>
        </Box>

        {/* Menu Items */}
        <Box sx={{ flex: 1, overflowY: "auto" }}>
          <List sx={{ p: 1 }}>
            {filteredMenuItems.map((item, index) => {
              if (item.divider) {
                return (
                  <Divider
                    key={`divider-${index}`}
                    sx={{
                      my: 1,
                      mx: 2,
                      borderColor: alpha(theme.palette.divider, 0.1),
                    }}
                  />
                );
              }

              return (
                <ListItem key={index} disablePadding sx={{ mb: 0.5 }}>
                  <ListItemButton
                    onClick={
                      item.onClick ||
                      (() => item.path && handleNavigate(item.path))
                    }
                    sx={{
                      borderRadius: 2,
                      mx: 1,
                      transition: "all 0.2s ease",
                      "&:hover": {
                        backgroundColor: alpha(theme.palette.primary.main, 0.1),
                        transform: "translateX(4px)",
                      },
                    }}
                  >
                    <ListItemIcon
                      sx={{
                        color: theme.palette.primary.main,
                        minWidth: 40,
                      }}
                    >
                      {item.icon}
                    </ListItemIcon>
                    <ListItemText
                      primary={item.text}
                      primaryTypographyProps={{
                        fontWeight: 500,
                        fontSize: "0.95rem",
                      }}
                    />
                    {item.chip && (
                      <Chip
                        label={item.chip}
                        size="small"
                        color="secondary"
                        variant="outlined"
                        sx={{ fontSize: "0.7rem", height: 24 }}
                      />
                    )}
                  </ListItemButton>
                </ListItem>
              );
            })}
          </List>
        </Box>

        {/* Settings Footer */}
        <Box
          sx={{
            p: 2,
            borderTop: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            background: alpha(theme.palette.background.paper, 0.5),
          }}
        >
          <Typography
            variant="subtitle2"
            color="text.secondary"
            mb={2}
            sx={{ fontWeight: 600 }}
          >
            {t("settings")}
          </Typography>

          {/* Theme Toggle */}
          <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center"
            mb={2}
          >
            <Box display="flex" alignItems="center" gap={1}>
              {uiStore.themeMode === "dark" ? (
                <DarkModeIcon color="primary" />
              ) : (
                <LightModeIcon color="primary" />
              )}
              <Typography variant="body2" fontWeight={500}>
                {uiStore.themeMode === "dark"
                  ? t("switchToLight")
                  : t("switchToDark")}
              </Typography>
            </Box>
            <Switch
              checked={uiStore.themeMode === "dark"}
              onChange={handleThemeChange}
              color="primary"
              size="small"
            />
          </Box>

          {/* Language Toggle */}
          <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center"
          >
            <Box display="flex" alignItems="center" gap={1}>
              <LanguageIcon color="primary" />
              <Typography variant="body2" fontWeight={500}>
                {t("changeLanguage")}
              </Typography>
            </Box>
            <Chip
              label={i18n.language.toUpperCase()}
              onClick={handleLanguageChange}
              color="primary"
              variant="outlined"
              size="small"
              sx={{
                fontWeight: "bold",
                cursor: "pointer",
                transition: "all 0.2s ease",
                "&:hover": {
                  backgroundColor: alpha(theme.palette.primary.main, 0.1),
                  transform: "scale(1.05)",
                },
              }}
            />
          </Box>
        </Box>
      </DialogContent>
    </Dialog>
  );
});
