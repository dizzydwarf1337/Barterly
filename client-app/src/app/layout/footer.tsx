import {
  Box,
  Typography,
  Container,
  IconButton,
  Link,
  Divider,
  useTheme,
  alpha,
} from "@mui/material";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import {
  Facebook as FacebookIcon,
  Twitter as TwitterIcon,
  Instagram as InstagramIcon,
  LinkedIn as LinkedInIcon,
  GitHub as GitHubIcon,
  Email as EmailIcon,
  Phone as PhoneIcon,
  LocationOn as LocationIcon,
} from "@mui/icons-material";

export default observer(function Footer() {
  const { t } = useTranslation();
  const theme = useTheme();

  // Динамические цвета в зависимости от темы
  const isDark = theme.palette.mode === "dark";

  const colors = {
    background: isDark
      ? alpha(theme.palette.background.paper, 0.9)
      : alpha(theme.palette.grey[900], 0.95),
    mainText: isDark ? "#fff" : "#fff",
    secondaryText: isDark ? theme.palette.grey[300] : theme.palette.grey[200],
    tertiaryText: isDark ? theme.palette.grey[400] : theme.palette.grey[300],
    quaternaryText: isDark ? theme.palette.grey[500] : theme.palette.grey[400],
    divider: isDark ? alpha("#fff", 0.1) : alpha("#fff", 0.2),
    iconHover: isDark
      ? alpha(theme.palette.primary.main, 0.1)
      : alpha(theme.palette.primary.light, 0.2),
  };

  const socialLinks = [
    { icon: <FacebookIcon />, href: "#", label: "Facebook" },
    { icon: <TwitterIcon />, href: "#", label: "Twitter" },
    { icon: <InstagramIcon />, href: "#", label: "Instagram" },
    { icon: <LinkedInIcon />, href: "#", label: "LinkedIn" },
    { icon: <GitHubIcon />, href: "#", label: "GitHub" },
  ];

  const contactInfo = [
    {
      icon: <EmailIcon />,
      text: "barterlymailservice@gmail.com",
      href: "mailto:barterlymailservice@gmail.com",
    },
    { icon: <PhoneIcon />, text: "+48 123 456 789", href: "tel:+48123456789" },
    { icon: <LocationIcon />, text: "Warsaw, Poland", href: "#" },
  ];

  const footerLinks = [
    { text: t("about"), href: "/about" },
    { text: t("privacy"), href: "/privacy" },
    { text: t("terms"), href: "/terms" },
    { text: t("help"), href: "/help" },
    { text: t("contact"), href: "/contact" },
  ];

  return (
    <Box
      component="footer"
      sx={{
        bgcolor: colors.background,
        color: colors.mainText,
        py: { xs: 3, md: 4 },
        borderTop: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
        backdropFilter: "blur(10px)",
        position: "relative",
        overflow: "hidden",
        "&::before": {
          content: '""',
          position: "absolute",
          top: 0,
          left: 0,
          right: 0,
          height: "2px",
          background: `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
        },
      }}
    >
      <Container maxWidth="xl">
        <Box
          display="grid"
          gridTemplateColumns={{
            xs: "1fr",
            sm: "1fr 1fr",
            md: "2fr 1fr 1fr 1fr",
          }}
          gap={{ xs: 3, md: 4 }}
          mb={3}
        >
          {/* Brand Section */}
          <Box>
            <Typography
              variant="h5"
              component="h2"
              gutterBottom
              sx={{
                fontWeight: "bold",
                background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                backgroundClip: "text",
                WebkitBackgroundClip: "text",
                WebkitTextFillColor: "transparent",
              }}
            >
              Barterly
            </Typography>
            <Typography
              variant="body2"
              sx={{
                color: colors.secondaryText,
                mb: 2,
                maxWidth: { xs: "100%", md: "300px" },
              }}
            >
              {t("footerDescription")}
            </Typography>

            {/* Social Media */}
            <Box display="flex" gap={1} flexWrap="wrap">
              {socialLinks.map((social, index) => (
                <IconButton
                  key={index}
                  component="a"
                  href={social.href}
                  target="_blank"
                  rel="noopener noreferrer"
                  aria-label={social.label}
                  sx={{
                    color: colors.tertiaryText,
                    transition: "all 0.3s ease",
                    "&:hover": {
                      color: theme.palette.primary.main,
                      transform: "translateY(-2px)",
                      bgcolor: colors.iconHover,
                    },
                  }}
                >
                  {social.icon}
                </IconButton>
              ))}
            </Box>
          </Box>

          {/* Quick Links */}
          <Box>
            <Typography
              variant="h6"
              gutterBottom
              fontWeight="bold"
              sx={{ color: colors.mainText }}
            >
              {t("quickLinks")}
            </Typography>
            <Box display="flex" flexDirection="column" gap={1}>
              {footerLinks.map((link, index) => (
                <Link
                  key={index}
                  href={link.href}
                  underline="none"
                  sx={{
                    color: colors.secondaryText,
                    fontSize: "0.875rem",
                    transition: "color 0.3s ease",
                    "&:hover": {
                      color: theme.palette.primary.main,
                    },
                  }}
                >
                  {link.text}
                </Link>
              ))}
            </Box>
          </Box>

          {/* Contact Info */}
          <Box>
            <Typography
              variant="h6"
              gutterBottom
              fontWeight="bold"
              sx={{ color: colors.mainText }}
            >
              {t("contact")}
            </Typography>
            <Box display="flex" flexDirection="column" gap={1.5}>
              {contactInfo.map((contact, index) => (
                <Box
                  key={index}
                  display="flex"
                  alignItems="center"
                  gap={1}
                  component={contact.href !== "#" ? "a" : "div"}
                  href={contact.href !== "#" ? contact.href : undefined}
                  sx={{
                    color: colors.secondaryText,
                    textDecoration: "none",
                    fontSize: "0.875rem",
                    transition: "color 0.3s ease",
                    "&:hover":
                      contact.href !== "#"
                        ? {
                            color: theme.palette.primary.main,
                          }
                        : {},
                  }}
                >
                  {contact.icon}
                  <Typography variant="body2" sx={{ color: "inherit" }}>
                    {contact.text}
                  </Typography>
                </Box>
              ))}
            </Box>
          </Box>

          {/* Additional Info */}
          <Box>
            <Typography
              variant="h6"
              gutterBottom
              fontWeight="bold"
              sx={{ color: colors.mainText }}
            >
              {t("moreInfo")}
            </Typography>
            <Typography
              variant="body2"
              sx={{ color: colors.secondaryText, mb: 1 }}
            >
              {t("availableLanguages")}:
            </Typography>
            <Box display="flex" gap={1} mb={2}>
              <Typography
                variant="caption"
                sx={{
                  px: 1,
                  py: 0.5,
                  bgcolor: alpha(
                    theme.palette.primary.main,
                    isDark ? 0.2 : 0.3
                  ),
                  borderRadius: 1,
                  color: isDark
                    ? theme.palette.primary.main
                    : theme.palette.primary.light,
                  fontWeight: "bold",
                }}
              >
                PL
              </Typography>
              <Typography
                variant="caption"
                sx={{
                  px: 1,
                  py: 0.5,
                  bgcolor: alpha(
                    theme.palette.secondary.main,
                    isDark ? 0.2 : 0.3
                  ),
                  borderRadius: 1,
                  color: isDark
                    ? theme.palette.secondary.main
                    : theme.palette.secondary.light,
                  fontWeight: "bold",
                }}
              >
                EN
              </Typography>
            </Box>
          </Box>
        </Box>

        <Divider sx={{ bgcolor: colors.divider, my: 2 }} />

        {/* Copyright */}
        <Box
          display="flex"
          justifyContent="space-between"
          alignItems="center"
          flexWrap="wrap"
          gap={2}
        >
          <Typography variant="body2" sx={{ color: colors.tertiaryText }}>
            © {new Date().getFullYear()} Barterly. {t("allRightsReserved")}.
          </Typography>
          <Typography
            variant="body2"
            sx={{
              color: colors.quaternaryText,
              fontSize: "0.75rem",
            }}
          >
            {t("madeWithLove")} 💙
          </Typography>
        </Box>
      </Container>
    </Box>
  );
});
