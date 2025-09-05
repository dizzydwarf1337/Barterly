import { useParams, useNavigate } from "react-router";
import UserProfile from "../components/UserProfile";
import UserSettingsForm from "../components/UserSettings";
import { useEffect, useState } from "react";
import { UserData, UserSettings } from "../types/userTypes";
import useStore from "../../../../app/stores/store";
import usersApi from "../api/usersApi";
import {
  Box,
  Skeleton,
  Paper,
  Typography,
  IconButton,
  alpha,
  useTheme,
  Breadcrumbs,
  Link,
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";

export const UserPage = observer(() => {
  const { uiStore } = useStore();
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const theme = useTheme();
  const { t } = useTranslation();

  const [userProfile, setUserProfile] = useState<UserData | null>(null);
  const [userSettings, setUserSettings] = useState<UserSettings | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchUser = async () => {
      setIsLoading(true);
      try {
        const response = await usersApi.getUser({ id: id as string });
        if (response.isSuccess) {
          setUserProfile(response.value.userData);
          setUserSettings(response.value.userSettings);
        }
      } catch {
        uiStore.showSnackbar("Error loading user", "error", "center");
      } finally {
        setIsLoading(false);
      }
    };
    fetchUser();
  }, [id]);

  const handleSettingsSubmit = async (data: UserSettings) => {
    try {
      await usersApi.updateUserSettings({...data, id: userProfile!.id as string});
      uiStore.showSnackbar(
        t("adminUsers:settingsUpdated"),
        "success",
        "center"
      );
    } catch {
      uiStore.showSnackbar(t("adminUsers:errorUpdatingSettings"), "error", "center");
    }
  };

  if (isLoading) {
    return (
      <Box sx={{ width: "100%", height: "100%" }}>
        <Skeleton
          variant="rectangular"
          height={60}
          sx={{ mb: 3, borderRadius: 2 }}
        />
        <Box display="flex" gap={3}>
          <Box flex={1}>
            <Skeleton
              variant="rectangular"
              height={600}
              sx={{ borderRadius: 3 }}
            />
          </Box>
          <Box flex={1}>
            <Skeleton
              variant="rectangular"
              height={600}
              sx={{ borderRadius: 3 }}
            />
          </Box>
        </Box>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        width: "90%",
        height: "100%",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        p: 3,
      }}
    >
      <Paper
        elevation={0}
        sx={{
          p: 2,
          mb: 3,
          borderRadius: 3,
          background: `linear-gradient(135deg, ${alpha(
            theme.palette.primary.main,
            0.05
          )} 0%, ${alpha(theme.palette.secondary.main, 0.02)} 100%)`,
          border: `1px solid ${alpha(theme.palette.divider, 0.08)}`,
        }}
      >
        <Box display="flex" alignItems="center" gap={2}>
          <IconButton
            onClick={() => navigate(-1)}
            sx={{
              borderRadius: 2,
              border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
              "&:hover": {
                backgroundColor: alpha(theme.palette.primary.main, 0.08),
                transform: "translateX(-2px)",
              },
              transition: "all 0.2s ease",
            }}
          >
            <ArrowBackIcon />
          </IconButton>

          <Breadcrumbs separator={<NavigateNextIcon fontSize="small" />}>
            <Link
              color="inherit"
              onClick={() => navigate("/admin")}
              sx={{
                textDecoration: "none",
                cursor: "pointer",
                "&:hover": { color: theme.palette.primary.main },
              }}
            >
              Admin
            </Link>
            <Link
              color="inherit"
              onClick={() => navigate("/admin/users")}
              sx={{
                textDecoration: "none",
                cursor: "pointer",
                "&:hover": { color: theme.palette.primary.main },
              }}
            >
              Users
            </Link>
            <Typography color="primary" fontWeight={600}>
              {userProfile
                ? `${userProfile.firstName} ${userProfile.lastName}`
                : "Loading..."}
            </Typography>
          </Breadcrumbs>
        </Box>
      </Paper>

      <Box display="flex" flexDirection={"column"} width="100%">
        <Box>
          <UserProfile userData={userProfile ?? ({} as UserData)} />
        </Box>
        <Box>
          <UserSettingsForm
            userSettings={userSettings ?? ({} as UserSettings)}
            onSubmit={handleSettingsSubmit}
          />
        </Box>
      </Box>
    </Box>
  );
});
