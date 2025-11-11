import React, { useState, useEffect } from "react";
import {
  Box,
  Paper,
  Typography,
  TextField,
  Button,
  Divider,
  Alert,
  CircularProgress,
  IconButton,
  InputAdornment,
  Tabs,
  Tab,
  Container,
} from "@mui/material";
import {
  Visibility,
  VisibilityOff,
  Google as GoogleIcon,
  Email as EmailIcon,
  Lock as LockIcon,
} from "@mui/icons-material";
import { useForm, Controller } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { useTranslation } from "react-i18next";
import { useGoogleLogin } from "@react-oauth/google";
import { observer } from "mobx-react-lite";
import { LoginRequestDTO, LoginWithGoogleRequestDTO } from "../dto/authDto";
import authApi from "../api/authApi";
import { useNavigate } from "react-router";
import useStore from "../../../app/stores/store";
import RegistrationForm from "../components/registrationForm";

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

interface LoginFormData {
  email: string;
  password: string;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;
  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`auth-tabpanel-${index}`}
      aria-labelledby={`auth-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ pt: 3 }}>{children}</Box>}
    </div>
  );
}

const LoginPage: React.FC = observer(() => {
  const { t } = useTranslation();
  const { authStore, uiStore } = useStore();
  const [tabValue, setTabValue] = useState(0);
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (authStore.isLoggedIn) {
      navigate("/");
    }
  }, [authStore.isLoggedIn, navigate]);

  const loginSchema = Yup.object({
    email: Yup.string()
      .email(t("validation.invalidEmail"))
      .required(t("validation.required")),
    password: Yup.string()
      .min(6, t("validation.passwordMinLength"))
      .required(t("validation.required")),
  });

  const {
    control: loginControl,
    handleSubmit: handleLoginSubmit,
    reset: resetLogin,
    formState: { errors: loginErrors },
  } = useForm<LoginFormData>({
    resolver: yupResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onLoginSubmit = async (data: LoginFormData) => {
    setLoading(true);
    setError(null);
    try {
      const loginData: LoginRequestDTO = {
        email: data.email,
        password: data.password,
      };

      const response = await authApi.login(loginData);

      if (response.isSuccess && response.value) {
        await authStore.login(response.value.token);
        uiStore.showSnackbar(t("auth.loginSuccess"), "success", "center");
        navigate("/");
      } else {
        setError(response.error || t("auth.loginError"));
      }
    } catch {
      setError(t("auth.loginError"));
    } finally {
      setLoading(false);
    }
  };

  const handleRegistrationSuccess = (email: string) => {
    navigate(`resend-email-confirmation/${email}`);
    setSuccess(t("auth.registerSuccess"));
  };

  const handleRegistrationError = (errorMessage: string) => {
    setError(errorMessage);
  };

  // Google login hook
  const googleLogin = useGoogleLogin({
    onSuccess: async (codeResponse) => {
      setLoading(true);
      setError(null);
      try {
        const googleLoginData: LoginWithGoogleRequestDTO = {
          token: codeResponse.code,
        };

        const response = await authApi.loginWithGoogle(googleLoginData);

        if (response.isSuccess) {
          authStore.loginWithGoogle(response.value.token);
          setSuccess(t("auth.googleLoginSuccess"));
          uiStore.showSnackbar(
            t("auth.googleLoginSuccess"),
            "success",
            "center"
          );
          navigate("/");
        } else {
          setError(response.error || t("auth.googleLoginError"));
        }
      } catch (err: any) {
        setError(err.message || t("auth.googleLoginError"));
      } finally {
        setLoading(false);
      }
    },
    onError: (error) => {
      console.log("Google Login Failed:", error);
      setError(t("auth.googleLoginError"));
    },
    flow: "auth-code",
  });

  const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
    setError(null);
    setSuccess(null);
    resetLogin();
  };

  // Если пользователь уже залогинен, не показываем форму
  if (authStore.isLoggedIn) {
    return null;
  }

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          minHeight: "100vh",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <Paper
          elevation={3}
          sx={{
            p: 4,
            width: "100%",
            maxWidth: 480,
            borderRadius: 3,
          }}
        >
          <Box sx={{ textAlign: "center", mb: 3 }}>
            <Typography variant="h4" component="h1" gutterBottom>
              {t("auth.welcome")}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {t("auth.subtitle")}
            </Typography>
          </Box>

          {/* Уведомления */}
          {error && (
            <Alert
              severity="error"
              sx={{ mb: 2 }}
              onClose={() => setError(null)}
            >
              {error}
            </Alert>
          )}
          {success && (
            <Alert
              severity="success"
              sx={{ mb: 2 }}
              onClose={() => setSuccess(null)}
            >
              {success}
            </Alert>
          )}

          {/* Вкладки */}
          <Tabs
            value={tabValue}
            onChange={handleTabChange}
            centered
            sx={{ mb: 2 }}
          >
            <Tab label={t("auth.login")} />
            <Tab label={t("auth.register")} />
          </Tabs>

          {/* Панель логина */}
          <TabPanel value={tabValue} index={0}>
            <Box component="form" onSubmit={handleLoginSubmit(onLoginSubmit)}>
              <Controller
                name="email"
                control={loginControl}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t("auth.email")}
                    type="email"
                    error={!!loginErrors.email}
                    helperText={loginErrors.email?.message}
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <EmailIcon color="action" />
                        </InputAdornment>
                      ),
                    }}
                    sx={{ mb: 2 }}
                  />
                )}
              />

              <Controller
                name="password"
                control={loginControl}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t("auth.password")}
                    type={showPassword ? "text" : "password"}
                    error={!!loginErrors.password}
                    helperText={loginErrors.password?.message}
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <LockIcon color="action" />
                        </InputAdornment>
                      ),
                      endAdornment: (
                        <InputAdornment position="end">
                          <IconButton
                            onClick={() => setShowPassword(!showPassword)}
                            edge="end"
                          >
                            {showPassword ? <VisibilityOff /> : <Visibility />}
                          </IconButton>
                        </InputAdornment>
                      ),
                    }}
                    sx={{ mb: 3 }}
                  />
                )}
              />

              <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                disabled={loading}
                sx={{ mb: 2 }}
              >
                {loading ? <CircularProgress size={24} /> : t("auth.login")}
              </Button>
            </Box>
          </TabPanel>

          {/* Панель регистрации */}
          <TabPanel value={tabValue} index={1}>
            <RegistrationForm
              onSuccess={(email) => handleRegistrationSuccess(email)}
              onError={handleRegistrationError}
            />
          </TabPanel>

          {/* Разделитель */}
          <Divider sx={{ my: 3 }}>
            <Typography variant="body2" color="text.secondary">
              {t("auth.or")}
            </Typography>
          </Divider>

          {/* Логин через Google */}
          <Button
            fullWidth
            variant="outlined"
            size="large"
            startIcon={<GoogleIcon />}
            onClick={() => googleLogin()}
            disabled={loading}
            sx={{
              borderColor: "#4285f4",
              color: "#4285f4",
              "&:hover": {
                borderColor: "#3367d6",
                backgroundColor: "rgba(66, 133, 244, 0.04)",
              },
            }}
          >
            {t("auth.continueWithGoogle")}
          </Button>
        </Paper>
      </Box>
    </Container>
  );
});

export default LoginPage;
