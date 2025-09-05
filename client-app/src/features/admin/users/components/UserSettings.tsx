import React from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  Switch,
  FormControlLabel,
  Button,
  alpha,
  useTheme,
} from "@mui/material";
import { useForm, Controller } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";

// Icons
import SettingsIcon from "@mui/icons-material/Settings";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import DeleteIcon from "@mui/icons-material/Delete";
import BlockIcon from "@mui/icons-material/Block";
import PostAddIcon from "@mui/icons-material/PostAdd";
import CommentIcon from "@mui/icons-material/Comment";
import ChatIcon from "@mui/icons-material/Chat";
import SaveIcon from "@mui/icons-material/Save";
import { UserSettings } from "../types/userTypes";

interface UserSettingsFormProps {
  userSettings: UserSettings;
  onSubmit: (data: UserSettings) => void;
  loading?: boolean;
}

const validationSchema = Yup.object().shape({
  id: Yup.string().required(),
  isHidden: Yup.boolean().required(),
  isDeleted: Yup.boolean().required(),
  isBanned: Yup.boolean().required(),
  isPostRestricted: Yup.boolean().required(),
  isOpinionRestricted: Yup.boolean().required(),
  isChatRestricted: Yup.boolean().required(),
});

const UserSettingsForm: React.FC<UserSettingsFormProps> = observer(
  ({ userSettings, onSubmit, loading = false }) => {
    const { t } = useTranslation();
    const theme = useTheme();

    const {
      control,
      handleSubmit,
      formState: {  },
    } = useForm<UserSettings>({
      resolver: yupResolver(validationSchema),
      defaultValues: userSettings,
    });

    const switchSettings = [
      {
        name: "isHidden" as keyof UserSettings,
        label: t("adminUsers:hiddenAccount"),
        icon: <VisibilityOffIcon />,
        color: theme.palette.warning.main,
      },
      {
        name: "isDeleted" as keyof UserSettings,
        label: t("adminUsers:deletedAccount"),
        icon: <DeleteIcon />,
        color: theme.palette.error.main,
      },
      {
        name: "isBanned" as keyof UserSettings,
        label: t("adminUsers:bannedAccount"),
        icon: <BlockIcon />,
        color: theme.palette.error.main,
      },
      {
        name: "isPostRestricted" as keyof UserSettings,
        label: t("adminUsers:postRestrictions"),
        icon: <PostAddIcon />,
        color: theme.palette.warning.main,
      },
      {
        name: "isOpinionRestricted" as keyof UserSettings,
        label: t("adminUsers:opinionRestrictions"),
        icon: <CommentIcon />,
        color: theme.palette.warning.main,
      },
      {
        name: "isChatRestricted" as keyof UserSettings,
        label: t("adminUsers:chatRestrictions"),
        icon: <ChatIcon />,
        color: theme.palette.warning.main,
      },
    ];

    return (
      <Box sx={{ margin: "0 auto", p: 3 }}>
        <Card
          elevation={0}
          sx={{
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            borderRadius: 3,
            background: `linear-gradient(135deg, ${alpha(
              theme.palette.primary.main,
              0.02
            )} 0%, ${alpha(theme.palette.secondary.main, 0.02)} 100%)`,
          }}
        >
          <CardContent sx={{ p: 4 }}>
            <Typography
              variant="h5"
              fontWeight="bold"
              color="primary"
              mb={1}
              display="flex"
              alignItems="center"
              gap={1}
            >
              <SettingsIcon />
              {t("adminUsers:userSettings")}
            </Typography>

            <Box component="form" onSubmit={handleSubmit(onSubmit)}>
              <Box display="grid" gridTemplateColumns="repeat(3, 1fr)" gap={2}>
                {switchSettings.map((setting, _) => (
                  <Box key={setting.name}>
                    <Controller
                      name={setting.name}
                      control={control}
                      render={({ field }) => (
                        <Box
                          sx={{
                            p: 2,
                            borderRadius: 2,
                            border: `1px solid ${alpha(
                              theme.palette.divider,
                              0.1
                            )}`,
                            backgroundColor: alpha(
                              theme.palette.background.paper,
                              0.5
                            ),
                            transition: "all 0.2s ease",
                            "&:hover": {
                              backgroundColor: alpha(
                                theme.palette.background.paper,
                                0.8
                              ),
                              borderColor: alpha(setting.color, 0.3),
                            },
                          }}
                        >
                          <Box
                            display="flex"
                            alignItems="center"
                            justifyContent="space-between"
                          >
                            <Box
                              display="flex"
                              alignItems="center"
                              gap={1.5}
                              flex={1}
                            >
                              <Box
                                sx={{
                                  color: setting.color,
                                  display: "flex",
                                  alignItems: "center",
                                }}
                              >
                                {setting.icon}
                              </Box>
                              <Box>
                                <Typography
                                  variant="subtitle1"
                                  fontWeight="medium"
                                >
                                  {setting.label}
                                </Typography>
                              </Box>
                            </Box>

                            <FormControlLabel
                              control={
                                <Switch
                                  checked={field.value as boolean}
                                  onChange={(e) =>
                                    field.onChange(e.target.checked)
                                  }
                                  sx={{
                                    "& .MuiSwitch-switchBase.Mui-checked": {
                                      color: setting.color,
                                      "& + .MuiSwitch-track": {
                                        backgroundColor: alpha(
                                          setting.color,
                                          0.5
                                        ),
                                      },
                                    },
                                  }}
                                />
                              }
                              label=""
                              sx={{ m: 0 }}
                            />
                          </Box>
                        </Box>
                      )}
                    />
                  </Box>
                ))}
              </Box>

              <Box mt={4} display="flex" justifyContent="flex-end">
                <Button
                  type="submit"
                  variant="contained"
                  size="large"
                  startIcon={<SaveIcon />}
                  sx={{
                    px: 4,
                    py: 1.5,
                    borderRadius: 2,
                    background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
                    boxShadow: `0 8px 24px ${alpha(
                      theme.palette.primary.main,
                      0.3
                    )}`,
                    "&:hover": {
                      background: `linear-gradient(135deg, ${theme.palette.primary.dark} 0%, ${theme.palette.primary.main} 100%)`,
                      transform: "translateY(-2px)",
                      boxShadow: `0 12px 32px ${alpha(
                        theme.palette.primary.main,
                        0.4
                      )}`,
                    },
                    "&:disabled": {
                      background: alpha(theme.palette.action.disabled, 0.1),
                      color: theme.palette.action.disabled,
                      boxShadow: "none",
                    },
                  }}
                >
                  {loading ? t("adminUsers:saving") : t("adminUsers:saveSettings")}
                </Button>
              </Box>
            </Box>
          </CardContent>
        </Card>
      </Box>
    );
  }
);

export default UserSettingsForm;
