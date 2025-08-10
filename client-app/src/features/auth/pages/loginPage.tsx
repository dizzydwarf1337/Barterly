import React, { useState } from 'react';
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
} from '@mui/material';
import {
  Visibility,
  VisibilityOff,
  Google as GoogleIcon,
  Email as EmailIcon,
  Lock as LockIcon,
  Person as PersonIcon,
} from '@mui/icons-material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import { useTranslation } from 'react-i18next';
import { LoginRequestDTO, LoginWithGoogleRequestDTO, RegisterRequestDTO } from '../dto/authDto';
import authApi from '../api/authApi';
import { useNavigate } from 'react-router';
import { values } from 'mobx';


interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

interface LoginFormData {
  email: string;
  password: string;
}

interface RegisterFormData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
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

const LoginPage: React.FC = () => {
  const { t } = useTranslation();
  const [tabValue, setTabValue] = useState(0);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const navigate = useNavigate();

  // Схемы валидации
  const loginSchema = Yup.object({
    email: Yup.string()
      .email(t('validation.invalidEmail'))
      .required(t('validation.required')),
    password: Yup.string()
      .min(6, t('validation.passwordMinLength'))
      .required(t('validation.required')),
  });

  const registerSchema = Yup.object({
    firstName: Yup.string()
      .min(2, t('validation.nameMinLength'))
      .required(t('validation.required')),
    lastName: Yup.string()
      .min(2, t('validation.nameMinLength'))
      .required(t('validation.required')),
    email: Yup.string()
      .email(t('validation.invalidEmail'))
      .required(t('validation.required')),
    password: Yup.string()
      .min(6, t('validation.passwordMinLength'))
      .required(t('validation.required')),
    confirmPassword: Yup.string()
      .oneOf([Yup.ref('password')], t('validation.passwordsDoNotMatch'))
      .required(t('validation.required')),
  });

  // Форма логина
  const {
    control: loginControl,
    handleSubmit: handleLoginSubmit,
    reset: resetLogin,
    formState: { errors: loginErrors }
  } = useForm<LoginFormData>({
    resolver: yupResolver(loginSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  // Форма регистрации
  const {
    control: registerControl,
    handleSubmit: handleRegisterSubmit,
    reset: resetRegister,
    formState: { errors: registerErrors }
  } = useForm<RegisterFormData>({
    resolver: yupResolver(registerSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
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
      
      if (response.isSuccess) {
        setSuccess(t('auth.loginSuccess'));
        localStorage.setItem('token', response.value.token);
        
      } else {
        setError(response.error || t('auth.loginError'));
      }
    } catch (err: any) {
      setError(err.message || t('auth.loginError'));
    } finally {
      setLoading(false);
    }
  };

  // Обработчик регистрации
  const onRegisterSubmit = async (data: RegisterFormData) => {
    setLoading(true);
    setError(null);
    try {
      const registerData: RegisterRequestDTO = {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        password: data.password,
      };
      
      const response = await authApi.register(registerData);
      
      if (response.isSuccess) {
        setSuccess(t('auth.registerSuccess'));
        navigate(`/auth/email-confirm/${response.value.email}`);
      } else {
        setError(response.error || t('auth.registerError'));
      }
    } catch (err: any) {
      setError(err.message || t('auth.registerError'));
    } finally {
      setLoading(false);
    }
  };

  // Логин через Google
  const handleGoogleLogin = async () => {
    setLoading(true);
    setError(null);
    try {
      // Здесь должна быть интеграция с Google OAuth
      // Для примера используем mock токен
      const googleToken = await getGoogleToken(); // Ваша функция получения токена от Google
      
      const googleLoginData: LoginWithGoogleRequestDTO = {
        token: googleToken,
      };
      
      const response = await authApi.loginWithGoogle(googleLoginData);
      
      if (response.isSuccess) {
        setSuccess(t('auth.googleLoginSuccess'));
        localStorage.setItem('token', response.value.token);
      } else {
        setError(response.error || t('auth.googleLoginError'));
      }
    } catch (err: any) {
      setError(err.message || t('auth.googleLoginError'));
    } finally {
      setLoading(false);
    }
  };

  const getGoogleToken = async (): Promise<string> => {
    return new Promise((resolve) => {
      setTimeout(() => resolve('mock-google-token'), 1000);
    });
  };

  const handleTabChange = (_: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
    setError(null);
    setSuccess(null);
    resetLogin();
    resetRegister();
  };

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          minHeight: '100vh',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          py: 3,
        }}
      >
        <Paper
          elevation={3}
          sx={{
            p: 4,
            width: '100%',
            maxWidth: 480,
            borderRadius: 3,
          }}
        >
          {/* Заголовок */}
          <Box sx={{ textAlign: 'center', mb: 3 }}>
            <Typography variant="h4" component="h1" gutterBottom>
              {t('auth.welcome')}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {t('auth.subtitle')}
            </Typography>
          </Box>

          {/* Уведомления */}
          {error && (
            <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError(null)}>
              {error}
            </Alert>
          )}
          {success && (
            <Alert severity="success" sx={{ mb: 2 }} onClose={() => setSuccess(null)}>
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
            <Tab label={t('auth.login')} />
            <Tab label={t('auth.register')} />
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
                    label={t('auth.email')}
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
                    label={t('auth.password')}
                    type={showPassword ? 'text' : 'password'}
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
                {loading ? <CircularProgress size={24} /> : t('auth.login')}
              </Button>
            </Box>
          </TabPanel>

          {/* Панель регистрации */}
          <TabPanel value={tabValue} index={1}>
            <Box component="form" onSubmit={handleRegisterSubmit(onRegisterSubmit)}>
              <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
                <Controller
                  name="firstName"
                  control={registerControl}
                  render={({ field }) => (
                    <TextField
                      {...field}
                      label={t('auth.firstName')}
                      error={!!registerErrors.firstName}
                      helperText={registerErrors.firstName?.message}
                      InputProps={{
                        startAdornment: (
                          <InputAdornment position="start">
                            <PersonIcon color="action" />
                          </InputAdornment>
                        ),
                      }}
                    />
                  )}
                />

                <Controller
                  name="lastName"
                  control={registerControl}
                  render={({ field }) => (
                    <TextField
                      {...field}
                      label={t('auth.lastName')}
                      error={!!registerErrors.lastName}
                      helperText={registerErrors.lastName?.message}
                    />
                  )}
                />
              </Box>

              <Controller
                name="email"
                control={registerControl}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t('auth.email')}
                    type="email"
                    error={!!registerErrors.email}
                    helperText={registerErrors.email?.message}
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
                control={registerControl}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t('auth.password')}
                    type={showPassword ? 'text' : 'password'}
                    error={!!registerErrors.password}
                    helperText={registerErrors.password?.message}
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
                    sx={{ mb: 2 }}
                  />
                )}
              />

              <Controller
                name="confirmPassword"
                control={registerControl}
                render={({ field }) => (
                  <TextField
                    {...field}
                    fullWidth
                    label={t('auth.confirmPassword')}
                    type={showConfirmPassword ? 'text' : 'password'}
                    error={!!registerErrors.confirmPassword}
                    helperText={registerErrors.confirmPassword?.message}
                    InputProps={{
                      startAdornment: (
                        <InputAdornment position="start">
                          <LockIcon color="action" />
                        </InputAdornment>
                      ),
                      endAdornment: (
                        <InputAdornment position="end">
                          <IconButton
                            onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                            edge="end"
                          >
                            {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
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
                {loading ? <CircularProgress size={24} /> : t('auth.register')}
              </Button>
            </Box>
          </TabPanel>

          {/* Разделитель */}
          <Divider sx={{ my: 3 }}>
            <Typography variant="body2" color="text.secondary">
              {t('auth.or')}
            </Typography>
          </Divider>

          {/* Логин через Google */}
          <Button
            fullWidth
            variant="outlined"
            size="large"
            startIcon={<GoogleIcon />}
            onClick={handleGoogleLogin}
            disabled={loading}
            sx={{
              borderColor: '#4285f4',
              color: '#4285f4',
              '&:hover': {
                borderColor: '#3367d6',
                backgroundColor: 'rgba(66, 133, 244, 0.04)',
              },
            }}
          >
            {t('auth.continueWithGoogle')}
          </Button>
        </Paper>
      </Box>
    </Container>
  );
};

export default LoginPage;