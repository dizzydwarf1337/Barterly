import { useState } from 'react';
import {
  Box,
  TextField,
  Button,
  CircularProgress,
  IconButton,
  InputAdornment,
} from '@mui/material';
import {
  Visibility,
  VisibilityOff,
  Email as EmailIcon,
  Lock as LockIcon,
  Person as PersonIcon,
} from '@mui/icons-material';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import { useTranslation } from 'react-i18next';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router';
import useStore from '../../../app/stores/store';
import { RegisterRequestDTO } from '../dto/authDto';
import authApi from '../api/authApi';

interface RegisterFormData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

interface RegistrationFormProps {
  onSuccess?: (email:string) => void;
  onError?: (error: string) => void;
}

export default observer(function RegistrationForm({ onSuccess, onError }: RegistrationFormProps) {
  const { t } = useTranslation();
  const { uiStore } = useStore();
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  const validationSchema = Yup.object({
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

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors }
  } = useForm<RegisterFormData>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
    },
  });

  const onSubmit = async (data: RegisterFormData) => {
    setLoading(true);
    try {
      const registerData: RegisterRequestDTO = {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        password: data.password,
      };
      
      const response = await authApi.register(registerData);
      
      if (response.isSuccess) {
        uiStore.showSnackbar(t('auth.registerSuccess'), 'success', 'center');
        reset();
        if (onSuccess) {
          onSuccess(registerData.email);
        } else {
          navigate(`/auth/email-confirm/${registerData.email}`);
        }
      } else {
        const errorMessage = response.error || t('auth.registerError');
        uiStore.showSnackbar(errorMessage, 'error', 'center');
        if (onError) {
          onError(errorMessage);
        }
      }
    } catch (err: any) {
      const errorMessage = err.message || t('auth.registerError');
      uiStore.showSnackbar(errorMessage, 'error', 'center');
      if (onError) {
        onError(errorMessage);
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)}>
      <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
        <Controller
          name="firstName"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t('auth.firstName')}
              error={!!errors.firstName}
              helperText={errors.firstName?.message}
              disabled={loading}
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
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t('auth.lastName')}
              error={!!errors.lastName}
              helperText={errors.lastName?.message}
              disabled={loading}
            />
          )}
        />
      </Box>

      <Controller
        name="email"
        control={control}
        render={({ field }) => (
          <TextField
            {...field}
            fullWidth
            label={t('auth.email')}
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
            }}
            sx={{ mb: 2 }}
          />
        )}
      />

      <Controller
        name="password"
        control={control}
        render={({ field }) => (
          <TextField
            {...field}
            fullWidth
            label={t('auth.password')}
            type={showPassword ? 'text' : 'password'}
            error={!!errors.password}
            helperText={errors.password?.message}
            disabled={loading}
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
                    disabled={loading}
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
        control={control}
        render={({ field }) => (
          <TextField
            {...field}
            fullWidth
            label={t('auth.confirmPassword')}
            type={showConfirmPassword ? 'text' : 'password'}
            error={!!errors.confirmPassword}
            helperText={errors.confirmPassword?.message}
            disabled={loading}
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
                    disabled={loading}
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
  );
});