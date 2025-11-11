import { useEffect, useState } from "react";
import {
  Box,
  Container,
  Paper,
  Typography,
  CircularProgress,
  Button,
  Alert,
  Fade,
  LinearProgress,
} from "@mui/material";
import {
  CheckCircle as CheckCircleIcon,
  Error as ErrorIcon,
  Email as EmailIcon,
  ArrowBack as ArrowBackIcon,
} from "@mui/icons-material";
import { useTranslation } from "react-i18next";
import useStore from "../../../app/stores/store";
import authApi from "../api/authApi";
import { useNavigate, useSearchParams } from "react-router";

type ConfirmationStatus = 'loading' | 'success' | 'error' | 'invalid';

export const EmailConfirmLink = () => {
  const { uiStore } = useStore();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { t } = useTranslation();
  
  const [status, setStatus] = useState<ConfirmationStatus>('loading');
  const [countdown, setCountdown] = useState(5);

  const email = searchParams.get("email")?.replace(/ /g, "+");
  const token = searchParams.get("token")?.replace(/ /g, "+");

  useEffect(() => {
    if (!email || !token) {
      setStatus('invalid');
      uiStore.showSnackbar(t("invalidConfirmationLink"), "error", "center");
      return;
    }

    const confirmEmail = async () => {
      try {
        await authApi.confirmEmail({ userMail: email, token });
        setStatus('success');
        uiStore.showSnackbar(t("confirmationMailSuccess"), "success", "center");
        
        const timer = setInterval(() => {
          setCountdown((prev) => {
            if (prev <= 1) {
              clearInterval(timer);
              navigate("/login");
              return 0;
            }
            return prev - 1;
          });
        }, 1000);

        return () => clearInterval(timer);
      } catch (error: any) {
        setStatus('error');
        uiStore.showSnackbar(error.message || t("confirmationMailFailed"), "error", "center");
      }
    };

    confirmEmail();
  }, [email, token, navigate, uiStore, t]);

  const handleGoToLogin = () => {
    navigate("/login");
  };

  const handleGoHome = () => {
    navigate("/");
  };

  const renderContent = () => {
    switch (status) {
      case 'loading':
        return (
          <Fade in timeout={500}>
            <Box textAlign="center">
              <EmailIcon
                sx={{
                  fontSize: 80,
                  color: 'primary.main',
                  mb: 3,
                  animation: 'pulse 2s infinite',
                  '@keyframes pulse': {
                    '0%': { opacity: 1 },
                    '50%': { opacity: 0.5 },
                    '100%': { opacity: 1 },
                  },
                }}
              />
              <Typography variant="h4" component="h1" gutterBottom fontWeight="bold">
                {t("emailConfirm.verifying")}
              </Typography>
              <Typography variant="body1" color="text.secondary" mb={4}>
                {t("emailConfirm.verifyingDescription")}
              </Typography>
              <CircularProgress size={50} thickness={4} />
              <Box mt={3}>
                <LinearProgress 
                  sx={{ 
                    borderRadius: 1,
                    height: 6,
                    backgroundColor: 'grey.200',
                  }} 
                />
              </Box>
            </Box>
          </Fade>
        );

      case 'success':
        return (
          <Fade in timeout={500}>
            <Box textAlign="center">
              <CheckCircleIcon
                sx={{
                  fontSize: 100,
                  color: 'success.main',
                  mb: 3,
                  animation: 'bounce 1s ease-in-out',
                  '@keyframes bounce': {
                    '0%, 20%, 50%, 80%, 100%': { transform: 'translateY(0)' },
                    '40%': { transform: 'translateY(-10px)' },
                    '60%': { transform: 'translateY(-5px)' },
                  },
                }}
              />
              <Typography variant="h4" component="h1" gutterBottom fontWeight="bold" color="success.main">
                {t("emailConfirm.success")}
              </Typography>
              <Typography variant="body1" color="text.secondary" mb={3}>
                {t("emailConfirm.successDescription")}
              </Typography>
              
              <Alert severity="success" sx={{ mb: 3, textAlign: 'left' }}>
                <Typography variant="body2">
                  {t("emailConfirm.accountActivated")}
                </Typography>
              </Alert>

              <Typography variant="body2" color="text.secondary" mb={3}>
                {t("emailConfirm.redirecting", { seconds: countdown })}
              </Typography>

              <Box display="flex" gap={2} justifyContent="center" flexWrap="wrap">
                <Button
                  variant="contained"
                  size="large"
                  onClick={handleGoToLogin}
                  sx={{ minWidth: 150 }}
                >
                  {t("emailConfirm.goToLogin")}
                </Button>
                <Button
                  variant="outlined"
                  size="large"
                  startIcon={<ArrowBackIcon />}
                  onClick={handleGoHome}
                  sx={{ minWidth: 150 }}
                >
                  {t("emailConfirm.goHome")}
                </Button>
              </Box>
            </Box>
          </Fade>
        );

      case 'error':
        return (
          <Fade in timeout={500}>
            <Box textAlign="center">
              <ErrorIcon
                sx={{
                  fontSize: 100,
                  color: 'error.main',
                  mb: 3,
                  animation: 'shake 0.5s ease-in-out',
                  '@keyframes shake': {
                    '0%, 100%': { transform: 'translateX(0)' },
                    '25%': { transform: 'translateX(-5px)' },
                    '75%': { transform: 'translateX(5px)' },
                  },
                }}
              />
              <Typography variant="h4" component="h1" gutterBottom fontWeight="bold" color="error.main">
                {t("emailConfirm.failed")}
              </Typography>
              <Typography variant="body1" color="text.secondary" mb={3}>
                {t("emailConfirm.failedDescription")}
              </Typography>

              <Alert severity="error" sx={{ mb: 3, textAlign: 'left' }}>
                <Typography variant="body2">
                  {t("emailConfirm.possibleReasons")}
                </Typography>
              </Alert>

              <Box display="flex" gap={2} justifyContent="center" flexWrap="wrap">
                <Button
                  variant="contained"
                  size="large"
                  onClick={handleGoToLogin}
                  sx={{ minWidth: 150 }}
                >
                  {t("emailConfirm.tryLogin")}
                </Button>
                <Button
                  variant="outlined"
                  size="large"
                  startIcon={<ArrowBackIcon />}
                  onClick={handleGoHome}
                  sx={{ minWidth: 150 }}
                >
                  {t("emailConfirm.goHome")}
                </Button>
              </Box>
            </Box>
          </Fade>
        );

      case 'invalid':
        return (
          <Fade in timeout={500}>
            <Box textAlign="center">
              <ErrorIcon
                sx={{
                  fontSize: 100,
                  color: 'warning.main',
                  mb: 3,
                }}
              />
              <Typography variant="h4" component="h1" gutterBottom fontWeight="bold" color="warning.main">
                {t("emailConfirm.invalidLink")}
              </Typography>
              <Typography variant="body1" color="text.secondary" mb={3}>
                {t("emailConfirm.invalidLinkDescription")}
              </Typography>

              <Alert severity="warning" sx={{ mb: 3, textAlign: 'left' }}>
                <Typography variant="body2">
                  {t("emailConfirm.invalidLinkHelp")}
                </Typography>
              </Alert>

              <Box display="flex" gap={2} justifyContent="center" flexWrap="wrap">
                <Button
                  variant="contained"
                  size="large"
                  onClick={handleGoToLogin}
                  sx={{ minWidth: 150 }}
                >
                  {t("emailConfirm.goToLogin")}
                </Button>
                <Button
                  variant="outlined"
                  size="large"
                  startIcon={<ArrowBackIcon />}
                  onClick={handleGoHome}
                  sx={{ minWidth: 150 }}
                >
                  {t("emailConfirm.goHome")}
                </Button>
              </Box>
            </Box>
          </Fade>
        );

      default:
        return null;
    }
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
            p: { xs: 3, sm: 5 },
            width: '100%',
            borderRadius: 3,
            textAlign: 'center',
            position: 'relative',
            overflow: 'hidden',
            '&::before': {
              content: '""',
              position: 'absolute',
              top: 0,
              left: 0,
              right: 0,
              height: 4,
              background: (theme) => 
                `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
            },
          }}
        >
          {renderContent()}
        </Paper>
      </Box>
    </Container>
  );
};