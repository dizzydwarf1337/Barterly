import { useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Box,
  TextField,
  Button,
  IconButton,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  InputAdornment,
  Alert,
  alpha,
  useTheme,
  Fade,
  Paper,
  Chip,
  FormHelperText,
} from "@mui/material";
import { useForm, Controller } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import useStore from "../../../../app/stores/store";
import usersApi from "../api/usersApi";
import { CreateUserRequestDTO } from "../dto/usersDto";
import { UserRoles } from "../types/userTypes";

// Icons
import CloseIcon from "@mui/icons-material/Close";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import EmailIcon from "@mui/icons-material/Email";
import LockIcon from "@mui/icons-material/Lock";
import PersonIcon from "@mui/icons-material/Person";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import BadgeIcon from "@mui/icons-material/Badge";
import SaveIcon from "@mui/icons-material/Save";

interface AddUserDialogProps {
  open: boolean;
  onClose: () => void;
  onSuccess?: () => void;
}

const validationSchema = Yup.object().shape({
  firstName: Yup.string()
    .required("First name is required")
    .min(2, "First name must be at least 2 characters")
    .max(50, "First name must be less than 50 characters"),
  lastName: Yup.string()
    .required("Last name is required")
    .min(2, "Last name must be at least 2 characters")
    .max(50, "Last name must be less than 50 characters"),
  email: Yup.string()
    .required("Email is required")
    .email("Invalid email address"),
  password: Yup.string()
    .required("Password is required")
    .min(8, "Password must be at least 8 characters")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]/,
      "Password must contain uppercase, lowercase, number and special character"
    ),
  role: Yup.number()
    .required("Role is required")
    .oneOf([UserRoles.User, UserRoles.Moderator, UserRoles.Admin]),
});

export const AddUserDialog = observer(
  ({ open, onClose, onSuccess }: AddUserDialogProps) => {
    const { uiStore } = useStore();
    const { t } = useTranslation();
    const theme = useTheme();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const {
      control,
      handleSubmit,
      reset,
      formState: { errors, isDirty },
    } = useForm<CreateUserRequestDTO>({
      resolver: yupResolver(validationSchema),
      defaultValues: {
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        role: UserRoles.User,
      },
    });

    const handleClose = () => {
      if (!loading) {
        reset();
        setError(null);
        onClose();
      }
    };

    const onSubmit = async (data: CreateUserRequestDTO) => {
      setLoading(true);
      setError(null);

      try {
        await usersApi.createUser(data);
        uiStore.showSnackbar(
          t("adminUsers:userCreatedSuccess") || "User created successfully",
          "success",
          "center"
        );
        reset();
        onClose();
        if (onSuccess) {
          onSuccess();
        }
      } catch (err: any) {
        const errorMessage =
          err?.response?.data?.error || err?.message || "Failed to create user";
        setError(errorMessage);
        uiStore.showSnackbar(errorMessage, "error", "center");
      } finally {
        setLoading(false);
      }
    };

    const roleOptions = [
      {
        value: UserRoles.User,
        label: "User",
        color: theme.palette.info.main,
        icon: <PersonIcon fontSize="small" />,
      },
      {
        value: UserRoles.Moderator,
        label: "Moderator",
        color: theme.palette.warning.main,
        icon: <BadgeIcon fontSize="small" />,
      },
      {
        value: UserRoles.Admin,
        label: "Administrator",
        color: theme.palette.error.main,
        icon: <AdminPanelSettingsIcon fontSize="small" />,
      },
    ];

    return (
      <Dialog
        open={open}
        onClose={handleClose}
        maxWidth="sm"
        fullWidth
        TransitionComponent={Fade}
        PaperProps={{
          sx: {
            borderRadius: 3,
            overflow: "visible",
          },
        }}
      >
        {/* Header */}
        <DialogTitle
          sx={{
            p: 3,
            pb: 2,
            background: `linear-gradient(135deg, ${alpha(
              theme.palette.primary.main,
              0.1
            )} 0%, ${alpha(theme.palette.secondary.main, 0.05)} 100%)`,
            borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
          }}
        >
          <Box
            display="flex"
            alignItems="center"
            justifyContent="space-between"
          >
            <Box display="flex" alignItems="center" gap={2}>
              <Box
                sx={{
                  p: 1.5,
                  borderRadius: 2,
                  background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
                  boxShadow: `0 8px 16px ${alpha(
                    theme.palette.primary.main,
                    0.3
                  )}`,
                  display: "flex",
                  alignItems: "center",
                }}
              >
                <PersonAddIcon sx={{ color: "white", fontSize: 24 }} />
              </Box>
              <Box>
                <Typography
                  variant="h5"
                  fontWeight="bold"
                  sx={{
                    background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                    backgroundClip: "text",
                    WebkitBackgroundClip: "text",
                    WebkitTextFillColor: "transparent",
                  }}
                >
                  {t("adminUsers:addNewUser") || "Add New User"}
                </Typography>
                <Typography variant="caption" color="text.secondary">
                  {t("adminUsers:fillUserDetails") ||
                    "Fill in the user details below"}
                </Typography>
              </Box>
            </Box>
            <IconButton
              onClick={handleClose}
              disabled={loading}
              sx={{
                "&:hover": {
                  backgroundColor: alpha(theme.palette.error.main, 0.1),
                },
              }}
            >
              <CloseIcon />
            </IconButton>
          </Box>
        </DialogTitle>

        {/* Content */}
        <form onSubmit={handleSubmit(onSubmit)}>
          <DialogContent sx={{ p: 3 }}>
            {error && (
              <Alert
                severity="error"
                sx={{ mb: 2 }}
                onClose={() => setError(null)}
              >
                {error}
              </Alert>
            )}

            <Box display="flex" flexDirection="column" gap={2.5}>
              {/* Name Fields */}
              <Box display="flex" gap={2}>
                <Controller
                  name="firstName"
                  control={control}
                  render={({ field }) => (
                    <TextField
                      {...field}
                      fullWidth
                      label={t("firstName") || "First Name"}
                      error={!!errors.firstName}
                      helperText={errors.firstName?.message}
                      disabled={loading}
                      InputProps={{
                        startAdornment: (
                          <InputAdornment position="start">
                            <PersonIcon color="action" />
                          </InputAdornment>
                        ),
                        sx: {
                          borderRadius: 2,
                        },
                      }}
                    />
                  )}
                />

                <Controller
                  name="lastName"
                  control={control}
                  render={({ field }) => (
                    <TextField
                      {...field}
                      fullWidth
                      label={t("lastName") || "Last Name"}
                      error={!!errors.lastName}
                      helperText={errors.lastName?.message}
                      disabled={loading}
                      InputProps={{
                        sx: {
                          borderRadius: 2,
                        },
                      }}
                    />
                  )}
                />
              </Box>

              {/* Email Field */}
              <Controller
                name="email"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t("Email") || "Email Address"}
                    type="email"
                    error={!!errors.email}
                    helperText={errors.email?.message}
                    disabled={loading}
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <EmailIcon color="action" />
                        </InputAdornment>
                      ),
                      sx: {
                        borderRadius: 2,
                      },
                    }}
                  />
                )}
              />

              {/* Password Field */}
              <Controller
                name="password"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t("password") || "Password"}
                    type="password"
                    error={!!errors.password}
                    helperText={errors.password?.message}
                    disabled={loading}
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <LockIcon color="action" />
                        </InputAdornment>
                      ),
                      sx: {
                        borderRadius: 2,
                      },
                    }}
                  />
                )}
              />

              {/* Role Field */}
              <Controller
                name="role"
                control={control}
                render={({ field }) => (
                  <FormControl fullWidth error={!!errors.role}>
                    <InputLabel>
                      {t("adminUsers:userRole") || "User Role"}
                    </InputLabel>
                    <Select
                      {...field}
                      label={t("adminUsers:userRole") || "User Role"}
                      disabled={loading}
                      sx={{
                        borderRadius: 2,
                        "& .MuiSelect-select": {
                          display: "flex",
                          alignItems: "center",
                          gap: 1,
                        },
                      }}
                    >
                      {roleOptions.map((role) => (
                        <MenuItem key={role.value} value={role.value}>
                          <Box display="flex" alignItems="center" gap={1}>
                            <Box sx={{ color: role.color, display: "flex" }}>
                              {role.icon}
                            </Box>
                            <Typography>{role.label}</Typography>
                            <Chip
                              size="small"
                              label={
                                role.value === UserRoles.Admin
                                  ? t("adminUsers:fullAccess")
                                  : role.value === UserRoles.Moderator
                                  ? t("adminUsers:limitedAccess")
                                  : t("adminUsers:basicAccess")
                              }
                              sx={{
                                ml: "auto",
                                height: 20,
                                fontSize: "0.7rem",
                                backgroundColor: alpha(role.color, 0.1),
                                color: role.color,
                              }}
                            />
                          </Box>
                        </MenuItem>
                      ))}
                    </Select>
                    {errors.role && (
                      <FormHelperText>{errors.role.message}</FormHelperText>
                    )}
                  </FormControl>
                )}
              />

              {/* Info Box */}
              <Paper
                elevation={0}
                sx={{
                  p: 2,
                  borderRadius: 2,
                  backgroundColor: alpha(theme.palette.info.main, 0.05),
                  border: `1px solid ${alpha(theme.palette.info.main, 0.2)}`,
                }}
              >
                <Typography
                  variant="caption"
                  color="info.main"
                  fontWeight={500}
                >
                  {t("adminUsers:userCreationInfo") ||
                    "Note: The user will receive an email with login credentials"}
                </Typography>
              </Paper>
            </Box>
          </DialogContent>

          {/* Actions */}
          <DialogActions
            sx={{
              p: 3,
              pt: 0,
              gap: 1,
            }}
          >
            <Button
              onClick={handleClose}
              disabled={loading}
              sx={{
                borderRadius: 2,
                textTransform: "none",
                px: 3,
              }}
            >
              {t("cancel") || "Cancel"}
            </Button>
            <Button
              type="submit"
              variant="contained"
              disabled={loading || !isDirty}
              startIcon={loading ? null : <SaveIcon />}
              sx={{
                borderRadius: 2,
                textTransform: "none",
                px: 3,
                background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
                boxShadow: `0 4px 12px ${alpha(
                  theme.palette.primary.main,
                  0.3
                )}`,
                "&:hover": {
                  boxShadow: `0 6px 20px ${alpha(
                    theme.palette.primary.main,
                    0.4
                  )}`,
                },
              }}
            >
              {loading
                ? t("adminUsers:creating") || "Creating..."
                : t("adminUsers:createUser") || "Create User"}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    );
  }
);
