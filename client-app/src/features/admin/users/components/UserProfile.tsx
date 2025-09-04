import React from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  Avatar,
  Divider,
  alpha,
  useTheme,
  Paper,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";

// Icons
import PersonIcon from "@mui/icons-material/Person";
import EmailIcon from "@mui/icons-material/Email";
import CalendarTodayIcon from "@mui/icons-material/CalendarToday";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import AccessTimeIcon from "@mui/icons-material/AccessTime";
import { UserData } from "../types/userTypes";

interface UserProfileProps {
  userData: UserData;
}

const UserProfile: React.FC<UserProfileProps> = observer(({ userData }) => {
  const { t } = useTranslation();
  const theme = useTheme();

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString(undefined, {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  const formatDateTime = (dateString: string) => {
    return new Date(dateString).toLocaleString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getFullAddress = () => {
    const addressParts = [
      userData.street,
      userData.houseNumber,
      userData.city,
      userData.postalCode,
      userData.country,
    ].filter(Boolean);

    return addressParts.length > 0 ? addressParts.join(", ") : null;
  };

  return (
    <Box sx={{ margin: "0 auto", p: 3 }}>
      {/* Main Profile Card */}
      <Card
        elevation={0}
        sx={{
          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
          borderRadius: 3,
          mb: 3,
          overflow: "visible",
          background: `linear-gradient(135deg, ${alpha(
            theme.palette.primary.main,
            0.02
          )} 0%, ${alpha(theme.palette.secondary.main, 0.02)} 100%)`,
        }}
      >
        <CardContent sx={{ p: 4 }}>
          {/* Header Section */}
          <Box display="flex" alignItems="center" gap={3} mb={3}>
            <Avatar
              src={userData.profilePicturePath || undefined}
              sx={{
                width: 80,
                height: 80,
                border: `3px solid ${theme.palette.primary.main}`,
                background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.light} 100%)`,
                fontSize: "2rem",
                fontWeight: "bold",
              }}
            >
              {userData.firstName?.[0]}
              {userData.lastName?.[0]}
            </Avatar>

            <Box flex={1}>
              <Typography
                variant="h4"
                fontWeight="bold"
                sx={{
                  background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                  backgroundClip: "text",
                  textFillColor: "transparent",
                  WebkitBackgroundClip: "text",
                  WebkitTextFillColor: "transparent",
                  mb: 0.5,
                }}
              >
                {userData.firstName} {userData.lastName}
              </Typography>

              <Box display="flex" alignItems="center" gap={1} mb={1}>
                <EmailIcon color="primary" fontSize="small" />
                <Typography variant="body1" color="text.secondary">
                  {userData.email}
                </Typography>
              </Box>
            </Box>
          </Box>

          <Divider sx={{ my: 3 }} />

          {/* Profile Information - Без Grid, используем Flexbox */}
          <Box
            display="flex"
            flexDirection={{ xs: "column", md: "row" }}
            gap={3}
          >
            {/* Personal Info Section */}
            <Box flex={1}>
              <Paper
                elevation={0}
                sx={{
                  p: 3,
                  borderRadius: 2,
                  backgroundColor: alpha(theme.palette.background.paper, 0.7),
                  border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                }}
              >
                <Typography
                  variant="h6"
                  fontWeight="bold"
                  color="primary"
                  mb={2}
                  display="flex"
                  alignItems="center"
                  gap={1}
                >
                  <PersonIcon />
                  {t("personalInfo")}
                </Typography>

                {userData.bio && (
                  <Box mb={2}>
                    <Typography
                      variant="subtitle2"
                      color="text.secondary"
                      mb={1}
                    >
                      {t("bio")}
                    </Typography>
                    <Typography variant="body2">{userData.bio}</Typography>
                  </Box>
                )}

                {getFullAddress() && (
                  <Box mb={2}>
                    <Typography
                      variant="subtitle2"
                      color="text.secondary"
                      mb={1}
                      display="flex"
                      alignItems="center"
                      gap={0.5}
                    >
                      <LocationOnIcon fontSize="small" />
                      {t("address")}
                    </Typography>
                    <Typography variant="body2">{getFullAddress()}</Typography>
                  </Box>
                )}

                <Box>
                  <Typography
                    variant="subtitle2"
                    color="text.secondary"
                    mb={1}
                    display="flex"
                    alignItems="center"
                    gap={0.5}
                  >
                    <CalendarTodayIcon fontSize="small" />
                    {t("memberSince")}
                  </Typography>
                  <Typography variant="body2">
                    {formatDate(userData.createdAt)}
                  </Typography>
                </Box>
              </Paper>
            </Box>

            {/* Activity Info Section */}
            <Box flex={1}>
              <Paper
                elevation={0}
                sx={{
                  p: 3,
                  borderRadius: 2,
                  backgroundColor: alpha(theme.palette.background.paper, 0.7),
                  border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                }}
              >
                <Typography
                  variant="h6"
                  fontWeight="bold"
                  color="primary"
                  mb={2}
                  display="flex"
                  alignItems="center"
                  gap={1}
                >
                  <AccessTimeIcon />
                  {t("activityInfo")}
                </Typography>

                <Box>
                  <Typography variant="subtitle2" color="text.secondary" mb={1}>
                    {t("lastSeen")}
                  </Typography>
                  <Typography variant="body2">
                    {formatDateTime(userData.lastSeen)}
                  </Typography>
                </Box>
              </Paper>
            </Box>
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
});

export default UserProfile;
