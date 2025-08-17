import {
  AppBar,
  Box,
  Button,
  IconButton,
  Toolbar,
  Typography,
  Badge,
  Avatar,
  Tooltip,
  useScrollTrigger,
  Slide,
  alpha,
  Divider,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";
import { ReactElement } from "react";

// Icons
import ReorderIcon from "@mui/icons-material/Reorder";
import AddIcon from "@mui/icons-material/Add";
import NotificationsIcon from "@mui/icons-material/Notifications";
import PersonIcon from "@mui/icons-material/Person";
import LoginIcon from "@mui/icons-material/Login";
import LogoutIcon from "@mui/icons-material/Logout";
import LightModeIcon from "@mui/icons-material/LightMode";
import DarkModeIcon from "@mui/icons-material/DarkMode";
import LanguageIcon from "@mui/icons-material/Language";
import { Favorite } from "@mui/icons-material";
import useStore from "../stores/store";
import MobileNavDialog from "./MobileNavDialog";
import { Link, useNavigate } from "react-router";
import authApi from "../../features/auth/api/authApi";

const StyledAppBar = styled(AppBar)(({ theme }) => ({
  backgroundColor: alpha(theme.palette.background.paper, 0.95),
  backdropFilter: "blur(10px)",
  borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
  transition: theme.transitions.create(["background-color", "box-shadow"], {
    easing: theme.transitions.easing.easeInOut,
    duration: theme.transitions.duration.standard,
  }),
}));

const LogoContainer = styled(Box)(({ theme }) => ({
  textDecoration: "none",
  color: "inherit",
  display: "flex",
  alignItems: "center",
  gap: theme.spacing(1),
  transition: theme.transitions.create("transform", {
    easing: theme.transitions.easing.easeInOut,
    duration: theme.transitions.duration.short,
  }),
  "&:hover": {
    transform: "scale(1.05)",
  },
  cursor: "pointer",
}));

const NavButton = styled(Button)(({ theme }) => ({
  borderRadius: theme.shape.borderRadius * 2,
  textTransform: "none",
  fontWeight: 600,
  padding: theme.spacing(1, 2),
  transition: theme.transitions.create(
    ["background-color", "transform", "box-shadow"],
    {
      easing: theme.transitions.easing.easeInOut,
      duration: theme.transitions.duration.short,
    }
  ),
  "&:hover": {
    transform: "translateY(-2px)",
    boxShadow: theme.shadows[4],
  },
}));

const UserInfo = styled(Box)(({ theme }) => ({
  display: "flex",
  alignItems: "center",
  gap: theme.spacing(1),
  padding: theme.spacing(0.5, 1),
  borderRadius: theme.shape.borderRadius * 2,
  backgroundColor: alpha(theme.palette.primary.main, 0.1),
  transition: theme.transitions.create(["background-color"], {
    easing: theme.transitions.easing.easeInOut,
    duration: theme.transitions.duration.short,
  }),
  "&:hover": {
    backgroundColor: alpha(theme.palette.primary.main, 0.15),
  },
}));

interface HideOnScrollProps {
  children: ReactElement;
}

function HideOnScroll({ children }: HideOnScrollProps) {
  const trigger = useScrollTrigger({
    threshold: 100,
  });

  return (
    <Slide appear={false} direction="down" in={!trigger}>
      {children}
    </Slide>
  );
}

export default observer(function NavBar() {
  const { t, i18n } = useTranslation();
  const { uiStore, authStore } = useStore();
  const navigate = useNavigate();

  const theme = uiStore.getTheme();

  const handleOpenUserSettings = (element: HTMLElement) => {
    uiStore.setUserSettingIsOpen(true);
    uiStore.setMenuElement(element);
  };

  const handleAddPost = () => {
    if (authStore.isLoggedIn) {
      navigate("/posts/create");
    } else {
      uiStore.showSnackbar(t("pleaseLoginToAddPost"), "warning", "center");
      navigate("/login");
    }
  };

  const handleLogin = () => {
    navigate("/login");
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
  };

  const handleLanguageChange = () => {
    const newLanguage = i18n.language === "en" ? "pl" : "en";
    i18n.changeLanguage(newLanguage);
    localStorage.setItem("brt_lng", newLanguage);
    uiStore.setLanguage(newLanguage);
  };

  const handleThemeChange = () => {
    uiStore.changeTheme();
  };

  const renderDesktopContent = () => (
    <Box display="flex" flexDirection="row" gap={2} alignItems="center">
      <Tooltip
        title={authStore.isLoggedIn ? t("addPost") : t("loginToAddPost")}
      >
        <NavButton
          variant="contained"
          color="secondary"
          onClick={handleAddPost}
          startIcon={<AddIcon />}
          sx={{ fontWeight: "bold" }}
        >
          {t("addPost")}
        </NavButton>
      </Tooltip>

      {authStore.isLoggedIn ? (
        <>
          <Tooltip title={t("notifications")}>
            <IconButton color="inherit">
              <Badge
                badgeContent={authStore.user?.notificationCount}
                color="error"
              >
                <NotificationsIcon />
              </Badge>
            </IconButton>
          </Tooltip>

          <Tooltip title={t("favorites")}>
            <IconButton color="inherit">
              <Badge
                badgeContent={authStore.user?.favPostIds?.length}
                color="error"
              >
                <Favorite />
              </Badge>
            </IconButton>
          </Tooltip>

          <UserInfo>
            <Avatar
              src={authStore.user?.profilePicturePath ?? undefined}
              sx={{
                width: 32,
                height: 32,
                border: `2px solid ${theme.palette.primary.main}`,
              }}
            >
              {authStore.user?.firstName?.charAt(0) || <PersonIcon />}
            </Avatar>
            <Box display="flex" flexDirection="column" alignItems="flex-start">
              <Typography variant="body2" fontWeight="600" noWrap>
                {t("hello")}, {authStore.user?.firstName || t("user")}!
              </Typography>
            </Box>
          </UserInfo>

          <Tooltip title={t("logout")}>
            <IconButton onClick={handleLogout} color="error">
              <LogoutIcon />
            </IconButton>
          </Tooltip>
        </>
      ) : (
        <Tooltip title={t("login")}>
          <Link to="/login" style={{ textDecoration: "none" }}>
            <NavButton
              variant="outlined"
              color="primary"
              startIcon={<LoginIcon />}
            >
              {t("login")}
            </NavButton>
          </Link>
        </Tooltip>
      )}

      <Divider orientation="vertical" flexItem />

      <Tooltip
        title={
          uiStore.themeMode === "dark" ? t("switchToLight") : t("switchToDark")
        }
      >
        <IconButton
          onClick={handleThemeChange}
          sx={{
            color: theme.palette.text.primary,
            transition: "transform 0.3s ease",
            "&:hover": {
              transform: "rotate(180deg)",
            },
          }}
        >
          {uiStore.themeMode === "dark" ? <LightModeIcon /> : <DarkModeIcon />}
        </IconButton>
      </Tooltip>
      <Tooltip title={t("changeLanguage")}>
        <IconButton
          onClick={handleLanguageChange}
          sx={{
            color: theme.palette.text.primary,
            transition: "transform 0.2s ease",
            "&:hover": {
              transform: "scale(1.1)",
            },
          }}
        >
          <LanguageIcon />
          <Typography
            variant="caption"
            sx={{
              ml: 0.5,
              fontWeight: "bold",
              fontSize: "0.7rem",
            }}
          >
            {i18n.language.toUpperCase()}
          </Typography>
        </IconButton>
      </Tooltip>
    </Box>
  );

  const renderMobileContent = () => (
    <Box display="flex" alignItems="center" gap={1}>
      <Tooltip title={t("addPost")}>
        <IconButton
          onClick={handleAddPost}
          sx={{
            bgcolor: theme.palette.secondary.main,
            color: theme.palette.secondary.contrastText,
            "&:hover": {
              bgcolor: theme.palette.secondary.dark,
              transform: "scale(1.1)",
            },
            transition: "all 0.2s ease",
          }}
        >
          <AddIcon />
        </IconButton>
      </Tooltip>

      {authStore.isLoggedIn ? (
        <Tooltip title={`${t("hello")}, ${authStore.user?.firstName}`}>
          <IconButton
            onClick={(event) => handleOpenUserSettings(event.currentTarget)}
            sx={{ p: 0.5 }}
          >
            <Badge
              badgeContent={authStore.user?.notificationCount}
              color="error"
              max={9}
              overlap="circular"
              anchorOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
            >
              <Avatar
                sx={{
                  width: 32,
                  height: 32,
                  border: `2px solid ${theme.palette.primary.main}`,
                }}
                src={authStore.user?.profilePicturePath ?? ""}
                alt={authStore.user?.firstName}
              >
                {authStore.user?.firstName?.charAt(0) || <PersonIcon />}
              </Avatar>
            </Badge>
          </IconButton>
        </Tooltip>
      ) : (
        <Tooltip title={t("login")}>
          <IconButton
            onClick={handleLogin}
            sx={{
              color: theme.palette.primary.main,
              border: `1px solid ${theme.palette.primary.main}`,
              "&:hover": {
                bgcolor: alpha(theme.palette.primary.main, 0.1),
              },
            }}
          >
            <LoginIcon />
          </IconButton>
        </Tooltip>
      )}

      <Tooltip title={t("menu")}>
        <IconButton
          onClick={() => uiStore.setIsMobileMenuOpen(true)}
          sx={{
            transition: "transform 0.3s ease",
            "&:hover": {
              transform: "scale(1.1)",
            },
          }}
        >
          <ReorderIcon />
        </IconButton>
      </Tooltip>
    </Box>
  );

  return (
    <>
      <HideOnScroll>
        <StyledAppBar
          position="sticky"
          elevation={0}
          sx={{
            height: { xs: 64, sm: 70 },
            zIndex: (theme) => theme.zIndex.drawer + 1,
          }}
        >
          <Toolbar
            sx={{
              height: "100%",
              px: { xs: 2, sm: 3 },
              justifyContent: "space-between",
            }}
          >
            <LogoContainer onClick={() => navigate("/")}>
              <Typography
                variant="h5"
                component="div"
                sx={{
                  fontWeight: "bold",
                  background: `linear-gradient(45deg, ${theme.palette.primary.main} 30%, ${theme.palette.secondary.main} 90%)`,
                  backgroundClip: "text",
                  WebkitBackgroundClip: "text",
                  WebkitTextFillColor: "transparent",
                }}
              >
                Barterly
              </Typography>
            </LogoContainer>

            <Box sx={{ display: { xs: "none", md: "flex" } }}>
              {renderDesktopContent()}
            </Box>

            <Box sx={{ display: { xs: "flex", md: "none" } }}>
              {renderMobileContent()}
            </Box>
          </Toolbar>
        </StyledAppBar>
      </HideOnScroll>

      <MobileNavDialog />
    </>
  );
});
