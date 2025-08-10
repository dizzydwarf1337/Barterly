import {
  Box,
  Button,
  Typography,
  Container,
  Paper,
  Alert,
  CircularProgress,
  LinearProgress,
  Divider,
  Card,
  CardContent,
  Fade,
  Zoom,
} from "@mui/material";
import {
  Email as EmailIcon,
  CheckCircle as CheckCircleIcon,
  Timer as TimerIcon,
  ArrowBack as ArrowBackIcon,
  Send as SendIcon,
} from "@mui/icons-material";
import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import useStore from "../../../app/stores/store";
import authApi from "../api/authApi";
import { useNavigate, useParams } from "react-router";

export const EmailConfirm = () => {
  const { t } = useTranslation();
  const { email } = useParams();
  const { uiStore } = useStore();
  const navigate = useNavigate();

  const [resendTime, setResendTime] = useState(0);
  const [isResending, setIsResending] = useState(false);
  const [emailSentCount, setEmailSentCount] = useState(0);

  useEffect(() => {
    if (resendTime > 0) {
      const timer = setTimeout(() => setResendTime(resendTime - 1), 1000);
      return () => clearTimeout(timer);
    }
  }, [resendTime]);

  const handleResendEmail = async () => {
    if (resendTime > 0 || isResending) return;
    
    setIsResending(true);
    setResendTime(60);
    
    try {
      if (email) {
        await authApi.resendEmailConfirm({ email });
        setEmailSentCount(prev => prev + 1);
        uiStore.showSnackbar(t("confirmationMailSent"), "success", "right");
      }
    } catch (error) {
      uiStore.showSnackbar(t("confirmationMailError"), "error", "right");
      setResendTime(0); // Reset timer on error
    } finally {
      setIsResending(false);
    }
  };

  const handleGoBack = () => {
    navigate("/login");
  };

  const handleGoHome = () => {
    navigate("/");
  };

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins}:${secs.toString().padStart(2, '0')}`;
  };

  if (!email) {
    return (
      <Container maxWidth="sm">
        <Box
          sx={{
            minHeight: '100vh',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
          }}
        >
          <Paper elevation={3} sx={{ p: 4, textAlign: 'center', borderRadius: 3 }}>
            <Typography variant="h5" color="error" gutterBottom>
              {t("emailConfirm.noEmailProvided")}
            </Typography>
            <Button
              variant="outlined"
              onClick={handleGoBack}
              startIcon={<ArrowBackIcon />}
              sx={{ mt: 2 }}
            >
              {t("emailConfirm.goToLogin")}
            </Button>
          </Paper>
        </Box>
      </Container>
    );
  }

  return (
    <Container maxWidth="md">
      <Box
        sx={{
          minHeight: '100vh',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          py: 3,
        }}
      >
        <Fade in timeout={800}>
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
            {/* Header Section */}
            <Box mb={4}>
              <Zoom in timeout={1000}>
                <Box>
                  <CheckCircleIcon
                    sx={{
                      fontSize: 80,
                      color: 'success.main',
                      mb: 2,
                      animation: 'pulse 2s infinite',
                      '@keyframes pulse': {
                        '0%': { opacity: 1, transform: 'scale(1)' },
                        '50%': { opacity: 0.7, transform: 'scale(1.05)' },
                        '100%': { opacity: 1, transform: 'scale(1)' },
                      },
                    }}
                  />
                </Box>
              </Zoom>
              
              <Typography 
                variant="h3" 
                component="h1" 
                gutterBottom 
                fontWeight="bold"
                sx={{
                  background: (theme) => 
                    `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                  backgroundClip: 'text',
                  WebkitBackgroundClip: 'text',
                  WebkitTextFillColor: 'transparent',
                  mb: 1,
                }}
              >
                {t("emailConfirm.thankYou")}
              </Typography>
              
              <Typography variant="h6" color="text.secondary" fontWeight="500">
                {t("emailConfirm.registrationComplete")}
              </Typography>
            </Box>

            {/* Email Info Card */}
            <Card 
              sx={{ 
                mb: 4, 
                backgroundColor: (theme) => theme.palette.mode === 'dark' 
                  ? 'rgba(255,255,255,0.05)' 
                  : 'rgba(0,0,0,0.02)',
                border: '1px solid',
                borderColor: 'divider',
              }}
            >
              <CardContent>
                <Box display="flex" alignItems="center" justifyContent="center" gap={2} mb={2}>
                  <EmailIcon color="primary" sx={{ fontSize: 24 }} />
                  <Typography variant="h6" fontWeight="600">
                    {email}
                  </Typography>
                </Box>
                
                <Typography variant="body1" color="text.secondary" sx={{ lineHeight: 1.6 }}>
                  {t("emailConfirm.checkInbox")}
                </Typography>
              </CardContent>
            </Card>

            {/* Instructions */}
            <Alert 
              severity="info" 
              sx={{ 
                mb: 4, 
                textAlign: 'left',
                '& .MuiAlert-message': {
                  width: '100%'
                }
              }}
            >
              <Typography variant="body2" sx={{ lineHeight: 1.6 }}>
                {t("emailConfirm.instructions")}
              </Typography>
            </Alert>

            {/* Stats */}
            {emailSentCount > 0 && (
              <Box mb={3}>
                <Typography variant="body2" color="text.secondary">
                  {t("emailConfirm.emailsSent", { count: emailSentCount + 1 })}
                </Typography>
              </Box>
            )}

            {/* Resend Section */}
            <Box mb={4}>
              <Typography variant="h6" gutterBottom fontWeight="600">
                {t("emailConfirm.didntReceive")}
              </Typography>
              
              {resendTime > 0 && (
                <Box mb={2}>
                  <Box display="flex" alignItems="center" justifyContent="center" gap={1} mb={1}>
                    <TimerIcon fontSize="small" color="action" />
                    <Typography variant="body2" color="text.secondary">
                      {t("emailConfirm.resendAvailable")} {formatTime(resendTime)}
                    </Typography>
                  </Box>
                  <LinearProgress 
                    variant="determinate" 
                    value={((60 - resendTime) / 60) * 100}
                    sx={{ 
                      borderRadius: 1,
                      height: 6,
                      backgroundColor: 'grey.200',
                    }}
                  />
                </Box>
              )}

              <Button
                onClick={handleResendEmail}
                variant={resendTime > 0 ? "outlined" : "contained"}
                color="primary"
                size="large"
                disabled={resendTime > 0 || isResending}
                startIcon={
                  isResending ? 
                    <CircularProgress size={20} /> : 
                    resendTime > 0 ? <TimerIcon /> : <SendIcon />
                }
                sx={{
                  minWidth: 200,
                  height: 48,
                  fontWeight: 600,
                  ...(resendTime === 0 && !isResending && {
                    animation: 'glow 2s ease-in-out infinite alternate',
                    '@keyframes glow': {
                      from: { boxShadow: '0 0 5px rgba(25, 118, 210, 0.4)' },
                      to: { boxShadow: '0 0 20px rgba(25, 118, 210, 0.6)' },
                    },
                  }),
                }}
              >
                {isResending 
                  ? t("emailConfirm.sending")
                  : resendTime > 0 
                    ? `${t("emailConfirm.resendIn")} ${formatTime(resendTime)}`
                    : t("emailConfirm.resendEmail")
                }
              </Button>
            </Box>

            <Divider sx={{ my: 3 }} />

            {/* Action Buttons */}
            <Box display="flex" gap={2} justifyContent="center" flexWrap="wrap">
              <Button
                variant="outlined"
                size="large"
                startIcon={<ArrowBackIcon />}
                onClick={handleGoBack}
                sx={{ minWidth: 140 }}
              >
                {t("emailConfirm.backToLogin")}
              </Button>
              <Button
                variant="text"
                size="large"
                onClick={handleGoHome}
                sx={{ minWidth: 140 }}
              >
                {t("emailConfirm.goHome")}
              </Button>
            </Box>

            {/* Tips */}
            <Box mt={4}>
              <Typography variant="body2" color="text.secondary" sx={{ fontStyle: 'italic' }}>
                💡 {t("emailConfirm.checkSpamTip")}
              </Typography>
            </Box>
          </Paper>
        </Fade>
      </Box>
    </Container>
  );
};