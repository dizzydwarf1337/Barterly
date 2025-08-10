import {
  AppBar,
  Box,
  Button,
  IconButton,
  Toolbar,
  Typography,
  InputBase,
  Badge,
  Avatar,
  Chip,
  Tooltip,
  useScrollTrigger,
  Slide,
  alpha,
  Divider,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";
import { useState, ReactElement } from "react";

// Icons
import ReorderIcon from "@mui/icons-material/Reorder";
import SettingsIcon from "@mui/icons-material/Settings";
import SearchIcon from "@mui/icons-material/Search";
import AddIcon from "@mui/icons-material/Add";
import NotificationsIcon from "@mui/icons-material/Notifications";
import PersonIcon from "@mui/icons-material/Person";
import LoginIcon from "@mui/icons-material/Login";
import LogoutIcon from "@mui/icons-material/Logout";
import HomeIcon from "@mui/icons-material/Home";
import LightModeIcon from "@mui/icons-material/LightMode";
import DarkModeIcon from "@mui/icons-material/DarkMode";

import ProfileSettingsMenu from "../../features/navBar/profileMenu/profileSettingsMenu";
import useStore from "../stores/store";
import MobileNavDialog from "./MobileNavDialog";
import { Link, useLocation, useNavigate } from "react-router";
import darkTheme from "../theme/DarkTheme";
import lightTheme from "../theme/LightTheme";

// Styled Components
const StyledAppBar = styled(AppBar)(({ theme }) => ({
  backgroundColor: alpha(theme.palette.background.paper, 0.95),
  backdropFilter: "blur(10px)",
  borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
  transition: theme.transitions.create(["background-color", "box-shadow"], {
    easing: theme.transitions.easing.easeInOut,
    duration: theme.transitions.duration.standard,
  }),
}));

const SearchContainer = styled("div")(({ theme }) => ({
  position: "relative",
  borderRadius: theme.shape.borderRadius * 2,
  backgroundColor: alpha(theme.palette.common.black, 0.05),
  "&:hover": {
    backgroundColor: alpha(theme.palette.common.black, 0.1),
  },
  "&:focus-within": {
    backgroundColor: alpha(theme.palette.primary.main, 0.1),
    boxShadow: `0 0 0 2px ${alpha(theme.palette.primary.main, 0.2)}`,
  },
  marginLeft: 0,
  width: "100%",
  maxWidth: "400px",
  transition: theme.transitions.create(["background-color", "box-shadow"], {
    easing: theme.transitions.easing.easeInOut,
    duration: theme.transitions.duration.short,
  }),
  [theme.breakpoints.up("sm")]: {
    marginLeft: theme.spacing(1),
    width: "auto",
  },
}));

const SearchIconWrapper = styled("div")(({ theme }) => ({
  padding: theme.spacing(0, 2),
  height: "100%",
  position: "absolute",
  pointerEvents: "none",
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
  color: theme.palette.text.secondary,
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
  color: "inherit",
  width: "100%",
  "& .MuiInputBase-input": {
    padding: theme.spacing(1, 1, 1, 0),
    paddingLeft: `calc(1em + ${theme.spacing(4)})`,
    transition: theme.transitions.create("width"),
    [theme.breakpoints.up("sm")]: {
      width: "20ch",
      "&:focus": {
        width: "30ch",
      },
    },
  },
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

// Hide on scroll component
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
  const location = useLocation();

  const [searchQuery, setSearchQuery] = useState("");
  const [notificationCount] = useState(3); // Mock notification count

  const theme = uiStore.getTheme();

  const handleOpenUserSettings = (element: HTMLElement) => {
    uiStore.setUserSettingIsOpen(true);
    uiStore.setMenuElement(element);
  };

  const handleSearch = (event: React.FormEvent) => {
    event.preventDefault();
    if (searchQuery.trim()) {
      navigate(`/search?q=${encodeURIComponent(searchQuery.trim())}`);
    }
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
      await authStore.logout();
      uiStore.showSnackbar(t("logoutSuccess"), "success", "center");
      navigate("/");
    } catch (error) {
      uiStore.showSnackbar(t("logoutFailed"), "error", "center");
    }
  };

  const handleLanguageChange = () => {
    const newLanguage = i18n.language === 'en' ? 'pl' : 'en';
    i18n.changeLanguage(newLanguage);
    localStorage.setItem('brt_lng', newLanguage);
  };

  const handleThemeChange = () => {
    uiStore.setTheme(uiStore.themeMode == "dark" ? lightTheme : darkTheme);
  };

  const isActiveRoute = (path: string) => location.pathname === path;

  const renderDesktopContent = () => (
    <Box display="flex" flexDirection="row" gap={2} alignItems="center">
      {/* Navigation Links */}
      <Box display="flex" gap={1}>
        <Tooltip title={t("home")}>
          <Link to="/" style={{ textDecoration: 'none' }}>
            <NavButton
              variant={isActiveRoute("/") ? "contained" : "text"}
              size="small"
              startIcon={<HomeIcon />}
            >
              {t("home")}
            </NavButton>
          </Link>
        </Tooltip>
      </Box>

      <Divider orientation="vertical" flexItem />

      {/* Theme Toggle */}
      <Tooltip title={uiStore.themeMode == "dark"? t("switchToLight") : t("switchToDark")}>
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
          {uiStore.themeMode == "dark" ? <LightModeIcon /> : <DarkModeIcon />}
        </IconButton>
      </Tooltip>

      {/* Language Toggle */}
      <Tooltip title={t("changeLanguage")}>
        <NavButton
          variant="text"
          size="small"
          onClick={handleLanguageChange}
          sx={{ minWidth: 'auto', px: 1 }}
        >
          {i18n.language.toUpperCase()}
        </NavButton>
      </Tooltip>

      <Divider orientation="vertical" flexItem />

      {/* Add Post Button */}
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
          {/* Notifications */}
          <Tooltip title={t("notifications")}>
            <IconButton color="inherit">
              <Badge badgeContent={notificationCount} color="error">
                <NotificationsIcon />
              </Badge>
            </IconButton>
          </Tooltip>

          {/* User Info */}
          <UserInfo>
            <Avatar
              sx={{ width: 32, height: 32 }}
              src={authStore.user?.profilePicturePath ?? ""}
              alt={authStore.user?.firstName}
            >
              {authStore.user?.firstName?.charAt(0) || <PersonIcon />}
            </Avatar>
            <Box display="flex" flexDirection="column" alignItems="flex-start">
              <Typography variant="body2" fontWeight="600" noWrap>
                {t("hello")}, {authStore.user?.firstName || t("user")}!
              </Typography>
              <Chip
                label={authStore.user?.role || "User"}
                size="small"
                color="primary"
                variant="outlined"
                sx={{ height: 16, fontSize: "0.6rem" }}
              />
            </Box>
          </UserInfo>

          {/* Settings */}
          <Tooltip title={t("settings")}>
            <IconButton
              onClick={(event) => handleOpenUserSettings(event.currentTarget)}
              sx={{
                transition: "transform 0.3s ease",
                "&:hover": {
                  transform: "rotate(90deg)",
                },
              }}
            >
              <SettingsIcon />
            </IconButton>
          </Tooltip>

          {/* Logout */}
          <Tooltip title={t("logout")}>
            <IconButton onClick={handleLogout} color="error">
              <LogoutIcon />
            </IconButton>
          </Tooltip>
        </>
      ) : (
        <Tooltip title={t("login")}>
          <Link to="/login" style={{ textDecoration: 'none' }}>
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
    </Box>
  );

  const renderMobileContent = () => (
    <Box display="flex" alignItems="center" gap={1}>
      {/* Language Toggle - Mobile */}
      <Tooltip title={t("changeLanguage")}>
        <IconButton
          onClick={handleLanguageChange}
          sx={{
            color: theme.palette.text.primary,
            fontSize: '0.8rem',
            fontWeight: 'bold',
            minWidth: 32,
            minHeight: 32,
          }}
        >
          {i18n.language.toUpperCase()}
        </IconButton>
      </Tooltip>

      {/* Mobile Search */}
      <Tooltip title={t("search")}>
        <IconButton
          onClick={() => navigate("/search")}
          sx={{ color: theme.palette.text.primary }}
        >
          <SearchIcon />
        </IconButton>
      </Tooltip>

      {/* Add Post Button - Mobile */}
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

      {/* User Avatar or Login - Mobile */}
      {authStore.isLoggedIn ? (
        <Tooltip title={`${t("hello")}, ${authStore.user?.firstName}`}>
          <IconButton
            onClick={(event) => handleOpenUserSettings(event.currentTarget)}
            sx={{ p: 0.5 }}
          >
            <Badge
              badgeContent={notificationCount}
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

      {/* Mobile Menu Button */}
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
            zIndex: (theme) => theme.zIndex.appBar,
          }}
        >
          <Toolbar
            sx={{
              justifyContent: "space-between",
              minHeight: { xs: 64, sm: 70 },
              px: { xs: 2, sm: 3 },
            }}
          >
            {/* Logo Section */}
            <Box display="flex" alignItems="center" gap={2}>
              <Link to="/" style={{ textDecoration: 'none', color: 'inherit' }}>
                <LogoContainer>
                  <Box
                    sx={{
                      width: 40,
                      height: 40,
                      borderRadius: "50%",
                      background:
                        "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      color: "white",
                      fontWeight: "bold",
                      fontSize: "1.2rem",
                    }}
                  >
                    B
                  </Box>
                  <Typography
                    variant="h5"
                    component="div"
                    sx={{
                      fontWeight: 700,
                      background:
                        "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
                      backgroundClip: "text",
                      WebkitBackgroundClip: "text",
                      WebkitTextFillColor: "transparent",
                      display: { xs: "none", sm: "block" },
                    }}
                  >
                    Barterly
                  </Typography>
                </LogoContainer>
              </Link>

              {/* Search Bar - Desktop only */}
              {!uiStore.isMobile && (
                <SearchContainer>
                  <form onSubmit={handleSearch}>
                    <SearchIconWrapper>
                      <SearchIcon />
                    </SearchIconWrapper>
                    <StyledInputBase
                      placeholder={`${t("searchPlaceholder")}...`}
                      inputProps={{ "aria-label": "search" }}
                      value={searchQuery}
                      onChange={(e) => setSearchQuery(e.target.value)}
                    />
                  </form>
                </SearchContainer>
              )}
            </Box>

            {/* Navigation Content */}
            {uiStore.isMobile ? renderMobileContent() : renderDesktopContent()}
          </Toolbar>
        </StyledAppBar>
      </HideOnScroll>

      {/* Dialogs and Menus */}
      <ProfileSettingsMenu />
      {uiStore.isMobile && <MobileNavDialog />}
    </>
  );
});