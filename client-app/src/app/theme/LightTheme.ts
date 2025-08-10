import { alpha, createTheme, lighten, darken } from "@mui/material";

// Цветовая палитра
const colors = {
    primary: {
        50: '#E8F2FF',
        100: '#BBDEFB',
        200: '#90CAF9',
        300: '#64B5F6',
        400: '#42A5F5',
        500: '#2196F3', // main
        600: '#1E88E5',
        700: '#1976D2',
        800: '#1565C0',
        900: '#0D47A1',
        main: '#2196F3',
        dark: '#1976D2',
        light: '#64B5F6',
    },
    secondary: {
        50: '#FCE4EC',
        100: '#F8BBD9',
        200: '#F48FB1',
        300: '#F06292',
        400: '#EC407A',
        500: '#E91E63', // main
        600: '#D81B60',
        700: '#C2185B',
        800: '#AD1457',
        900: '#880E4F',
        main: '#E91E63',
        dark: '#C2185B',
        light: '#F06292',
    },
    success: {
        main: '#4CAF50',
        dark: '#388E3C',
        light: '#81C784',
    },
    error: {
        main: '#F44336',
        dark: '#D32F2F',
        light: '#EF5350',
    },
    warning: {
        main: '#FF9800',
        dark: '#F57C00',
        light: '#FFB74D',
    },
    info: {
        main: '#2196F3',
        dark: '#1976D2',
        light: '#64B5F6',
    },
    background: {
        default: '#FAFBFC', // Очень светлый серо-голубой
        paper: '#FFFFFF',   // Чистый белый для карточек
        surface: '#F8F9FA', // Светло-серый для поверхностей
        elevated: '#F5F6F7', // Для поднятых элементов
    },
    text: {
        primary: '#1A202C',
        secondary: '#4A5568',
        disabled: '#A0AEC0',
        hint: '#718096',
    },
    divider: alpha('#000000', 0.08),
    action: {
        hover: alpha('#000000', 0.04),
        selected: alpha('#000000', 0.08),
        disabled: alpha('#000000', 0.26),
        disabledBackground: alpha('#000000', 0.12),
    },
};

const lightTheme = createTheme({
    breakpoints: {
        values: {
            xs: 0,
            sm: 600,   // Стандартные MUI breakpoints
            md: 900,
            lg: 1200,
            xl: 1536,
        }
    },
    palette: {
        mode: 'light',
        primary: {
            main: colors.primary.main,
            dark: colors.primary.dark,
            light: colors.primary.light,
            contrastText: '#FFFFFF',
        },
        secondary: {
            main: colors.secondary.main,
            dark: colors.secondary.dark,
            light: colors.secondary.light,
            contrastText: '#FFFFFF',
        },
        error: {
            main: colors.error.main,
            dark: colors.error.dark,
            light: colors.error.light,
            contrastText: '#FFFFFF',
        },
        warning: {
            main: colors.warning.main,
            dark: colors.warning.dark,
            light: colors.warning.light,
            contrastText: '#000000',
        },
        success: {
            main: colors.success.main,
            dark: colors.success.dark,
            light: colors.success.light,
            contrastText: '#FFFFFF',
        },
        info: {
            main: colors.info.main,
            dark: colors.info.dark,
            light: colors.info.light,
            contrastText: '#FFFFFF',
        },
        background: {
            default: colors.background.default,
            paper: colors.background.paper,
        },
        text: {
            primary: colors.text.primary,
            secondary: colors.text.secondary,
            disabled: colors.text.disabled,
        },
        divider: colors.divider,
        action: colors.action,
        // Кастомные цвета для особых случаев
        grey: {
            50: '#F9FAFB',
            100: '#F3F4F6',
            200: '#E5E7EB',
            300: '#D1D5DB',
            400: '#9CA3AF',
            500: '#6B7280',
            600: '#4B5563',
            700: '#374151',
            800: '#1F2937',
            900: '#111827',
            A100: '#F5F5F5',
            A200: '#EEEEEE',
            A400: '#BDBDBD',
            A700: '#616161',
        },
    },
    typography: {
        fontFamily: [
            'Inter',
            '-apple-system',
            'BlinkMacSystemFont',
            '"Segoe UI"',
            'Roboto',
            '"Helvetica Neue"',
            'Arial',
            'sans-serif',
        ].join(','),
        h1: {
            fontWeight: 700,
            fontSize: '3.5rem',
            lineHeight: 1.2,
            letterSpacing: '-0.02em',
            background: `linear-gradient(135deg, ${colors.primary.main} 0%, ${colors.secondary.main} 100%)`,
            backgroundClip: 'text',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            '@media (max-width:600px)': {
                fontSize: '2.5rem',
            },
        },
        h2: {
            fontWeight: 600,
            fontSize: '2.5rem',
            lineHeight: 1.3,
            letterSpacing: '-0.01em',
            color: colors.text.primary,
            '@media (max-width:600px)': {
                fontSize: '2rem',
            },
        },
        h3: {
            fontWeight: 600,
            fontSize: '2rem',
            lineHeight: 1.4,
            color: colors.text.primary,
            '@media (max-width:600px)': {
                fontSize: '1.5rem',
            },
        },
        h4: {
            fontWeight: 600,
            fontSize: '1.5rem',
            lineHeight: 1.4,
            color: colors.text.primary,
            '@media (max-width:600px)': {
                fontSize: '1.25rem',
            },
        },
        h5: {
            fontWeight: 600,
            fontSize: '1.25rem',
            lineHeight: 1.5,
            color: colors.text.primary,
        },
        h6: {
            fontWeight: 600,
            fontSize: '1.125rem',
            lineHeight: 1.5,
            color: colors.text.primary,
        },
        body1: {
            fontSize: '1rem',
            lineHeight: 1.6,
            color: colors.text.primary,
        },
        body2: {
            fontSize: '0.875rem',
            lineHeight: 1.5,
            color: colors.text.secondary,
        },
        button: {
            fontWeight: 600,
            fontSize: '0.875rem',
            textTransform: 'none',
            letterSpacing: '0.02em',
        },
        caption: {
            fontSize: '0.75rem',
            lineHeight: 1.4,
            color: colors.text.secondary,
        },
        overline: {
            fontSize: '0.75rem',
            fontWeight: 600,
            textTransform: 'uppercase',
            letterSpacing: '0.08em',
            color: colors.text.secondary,
        },
    },
    shape: {
        borderRadius: 12,
    },
    shadows: [
        'none',
        `0px 2px 4px ${alpha('#000000', 0.05)}`,
        `0px 4px 8px ${alpha('#000000', 0.08)}`,
        `0px 8px 16px ${alpha('#000000', 0.1)}`,
        `0px 12px 24px ${alpha('#000000', 0.12)}`,
        `0px 16px 32px ${alpha('#000000', 0.14)}`,
        `0px 20px 40px ${alpha('#000000', 0.15)}`,
        `0px 24px 48px ${alpha('#000000', 0.16)}`,
        `0px 32px 64px ${alpha('#000000', 0.18)}`,
        `0px 36px 72px ${alpha('#000000', 0.2)}`,
        `0px 40px 80px ${alpha('#000000', 0.22)}`,
        `0px 44px 88px ${alpha('#000000', 0.24)}`,
        `0px 48px 96px ${alpha('#000000', 0.26)}`,
        `0px 52px 104px ${alpha('#000000', 0.28)}`,
        `0px 56px 112px ${alpha('#000000', 0.3)}`,
        `0px 60px 120px ${alpha('#000000', 0.32)}`,
        `0px 64px 128px ${alpha('#000000', 0.34)}`,
        `0px 68px 136px ${alpha('#000000', 0.36)}`,
        `0px 72px 144px ${alpha('#000000', 0.38)}`,
        `0px 76px 152px ${alpha('#000000', 0.4)}`,
        `0px 80px 160px ${alpha('#000000', 0.42)}`,
        `0px 84px 168px ${alpha('#000000', 0.44)}`,
        `0px 88px 176px ${alpha('#000000', 0.46)}`,
        `0px 92px 184px ${alpha('#000000', 0.48)}`,
        `0px 96px 192px ${alpha('#000000', 0.5)}`,
    ],
    components: {
        MuiCssBaseline: {
            styleOverrides: {
                '*': {
                    boxSizing: 'border-box',
                },
                html: {
                    scrollBehavior: 'smooth',
                },
                body: {
                    background: `linear-gradient(135deg, ${colors.background.default} 0%, ${lighten(colors.background.default, 0.02)} 100%)`,
                    minHeight: '100vh',
                },
            },
        },
        MuiButton: {
            styleOverrides: {
                root: {
                    borderRadius: 12,
                    fontWeight: 600,
                    fontSize: '0.875rem',
                    textTransform: 'none',
                    minHeight: 40,
                    paddingX: 24,
                    boxShadow: 'none',
                    transition: 'all 0.2s cubic-bezier(0.4, 0, 0.2, 1)',
                    '&:hover': {
                        transform: 'translateY(-2px)',
                        boxShadow: `0px 8px 25px ${alpha('#000000', 0.15)}`,
                    },
                    '&:active': {
                        transform: 'translateY(0px)',
                        boxShadow: `0px 4px 12px ${alpha('#000000', 0.12)}`,
                    },
                    '&.Mui-disabled': {
                        backgroundColor: colors.action.disabledBackground,
                        color: colors.action.disabled,
                    },
                },
                contained: {
                    boxShadow: `0px 4px 12px ${alpha('#000000', 0.1)}`,
                    '&:hover': {
                        boxShadow: `0px 8px 25px ${alpha('#000000', 0.15)}`,
                    },
                },
                containedPrimary: {
                    background: `linear-gradient(135deg, ${colors.primary.main} 0%, ${lighten(colors.primary.main, 0.1)} 100%)`,
                    '&:hover': {
                        background: `linear-gradient(135deg, ${darken(colors.primary.main, 0.1)} 0%, ${colors.primary.main} 100%)`,
                    },
                },
                containedSecondary: {
                    background: `linear-gradient(135deg, ${colors.secondary.main} 0%, ${lighten(colors.secondary.main, 0.1)} 100%)`,
                    '&:hover': {
                        background: `linear-gradient(135deg, ${darken(colors.secondary.main, 0.1)} 0%, ${colors.secondary.main} 100%)`,
                    },
                },
                containedSuccess: {
                    background: `linear-gradient(135deg, ${colors.success.main} 0%, ${lighten(colors.success.main, 0.1)} 100%)`,
                    '&:hover': {
                        background: `linear-gradient(135deg, ${darken(colors.success.main, 0.1)} 0%, ${colors.success.main} 100%)`,
                    },
                },
                containedError: {
                    background: `linear-gradient(135deg, ${colors.error.main} 0%, ${lighten(colors.error.main, 0.1)} 100%)`,
                    '&:hover': {
                        background: `linear-gradient(135deg, ${darken(colors.error.main, 0.1)} 0%, ${colors.error.main} 100%)`,
                    },
                },
                outlined: {
                    borderWidth: '2px',
                    backgroundColor: alpha(colors.background.paper, 0.5),
                    backdropFilter: 'blur(10px)',
                    '&:hover': {
                        borderWidth: '2px',
                        backgroundColor: alpha(colors.background.paper, 0.8),
                    },
                },
                outlinedPrimary: {
                    '&:hover': {
                        backgroundColor: alpha(colors.primary.main, 0.08),
                        borderColor: colors.primary.main,
                    },
                },
                outlinedSecondary: {
                    '&:hover': {
                        backgroundColor: alpha(colors.secondary.main, 0.08),
                        borderColor: colors.secondary.main,
                    },
                },
                outlinedError: {
                    '&:hover': {
                        backgroundColor: alpha(colors.error.main, 0.08),
                        borderColor: colors.error.main,
                    },
                },
                outlinedWarning: {
                    '&:hover': {
                        backgroundColor: alpha(colors.warning.main, 0.08),
                        borderColor: colors.warning.main,
                    },
                },
                outlinedSuccess: {
                    '&:hover': {
                        backgroundColor: alpha(colors.success.main, 0.08),
                        borderColor: colors.success.main,
                    },
                },
                text: {
                    '&:hover': {
                        backgroundColor: colors.action.hover,
                    },
                },
            },
        },
        MuiCard: {
            styleOverrides: {
                root: {
                    backgroundColor: colors.background.paper,
                    backgroundImage: 'none',
                    borderRadius: 16,
                    border: `1px solid ${colors.divider}`,
                    boxShadow: `0px 4px 12px ${alpha('#000000', 0.05)}`,
                    transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                    '&:hover': {
                        transform: 'translateY(-4px)',
                        boxShadow: `0px 12px 32px ${alpha('#000000', 0.12)}`,
                        borderColor: alpha(colors.primary.main, 0.2),
                    },
                },
            },
        },
        MuiPaper: {
            styleOverrides: {
                root: {
                    backgroundColor: colors.background.paper,
                    backgroundImage: 'none',
                },
                elevation1: {
                    backgroundColor: colors.background.surface,
                },
                elevation2: {
                    backgroundColor: colors.background.elevated,
                },
            },
        },
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundColor: alpha(colors.background.paper, 0.9),
                    backdropFilter: 'blur(20px)',
                    borderBottom: `1px solid ${colors.divider}`,
                    boxShadow: 'none',
                    color: colors.text.primary,
                },
            },
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    '& .MuiOutlinedInput-root': {
                        backgroundColor: alpha(colors.background.surface, 0.5),
                        borderRadius: 12,
                        transition: 'all 0.2s ease',
                        '&:hover': {
                            backgroundColor: alpha(colors.background.surface, 0.8),
                            '& .MuiOutlinedInput-notchedOutline': {
                                borderColor: alpha(colors.primary.main, 0.5),
                            },
                        },
                        '&.Mui-focused': {
                            backgroundColor: colors.background.surface,
                            '& .MuiOutlinedInput-notchedOutline': {
                                borderColor: colors.primary.main,
                                borderWidth: 2,
                            },
                        },
                    },
                    '& .MuiInputLabel-root': {
                        color: colors.text.secondary,
                        '&.Mui-focused': {
                            color: colors.primary.main,
                        },
                    },
                },
            },
        },
        MuiChip: {
            styleOverrides: {
                root: {
                    backgroundColor: alpha(colors.background.elevated, 0.8),
                    color: colors.text.primary,
                    border: `1px solid ${colors.divider}`,
                    backdropFilter: 'blur(10px)',
                    '&:hover': {
                        backgroundColor: alpha(colors.background.elevated, 1),
                    },
                },
                colorPrimary: {
                    backgroundColor: alpha(colors.primary.main, 0.1),
                    color: colors.primary.main,
                    border: `1px solid ${alpha(colors.primary.main, 0.2)}`,
                },
                colorSecondary: {
                    backgroundColor: alpha(colors.secondary.main, 0.1),
                    color: colors.secondary.main,
                    border: `1px solid ${alpha(colors.secondary.main, 0.2)}`,
                },
            },
        },
        MuiIconButton: {
            styleOverrides: {
                root: {
                    borderRadius: 10,
                    transition: 'all 0.2s ease',
                    '&:hover': {
                        backgroundColor: colors.action.hover,
                        transform: 'scale(1.05)',
                    },
                },
            },
        },
        MuiTooltip: {
            styleOverrides: {
                tooltip: {
                    backgroundColor: colors.text.primary,
                    color: colors.background.paper,
                    border: `1px solid ${colors.divider}`,
                    borderRadius: 8,
                    fontSize: '0.75rem',
                },
                arrow: {
                    color: colors.text.primary,
                },
            },
        },
        MuiDialog: {
            styleOverrides: {
                paper: {
                    backgroundColor: colors.background.paper,
                    borderRadius: 16,
                    border: `1px solid ${colors.divider}`,
                    boxShadow: `0px 24px 48px ${alpha('#000000', 0.15)}`,
                },
            },
        },
        MuiDivider: {
            styleOverrides: {
                root: {
                    borderColor: colors.divider,
                    opacity: 1,
                },
            },
        },
        MuiList: {
            styleOverrides: {
                root: {
                    padding: 8,
                },
            },
        },
        MuiListItem: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    marginBottom: 4,
                },
            },
        },
        MuiListItemButton: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    transition: 'all 0.2s ease',
                    '&:hover': {
                        backgroundColor: colors.action.hover,
                        transform: 'translateX(4px)',
                    },
                    '&.Mui-selected': {
                        backgroundColor: alpha(colors.primary.main, 0.08),
                        '&:hover': {
                            backgroundColor: alpha(colors.primary.main, 0.12),
                        },
                    },
                },
            },
        },
        MuiSwitch: {
            styleOverrides: {
                root: {
                    '& .MuiSwitch-switchBase.Mui-checked': {
                        color: colors.primary.main,
                        '& + .MuiSwitch-track': {
                            backgroundColor: alpha(colors.primary.main, 0.5),
                        },
                    },
                },
                track: {
                    backgroundColor: alpha(colors.text.secondary, 0.3),
                },
            },
        },
        MuiLinearProgress: {
            styleOverrides: {
                root: {
                    borderRadius: 4,
                    backgroundColor: alpha(colors.primary.main, 0.1),
                },
                bar: {
                    borderRadius: 4,
                    background: `linear-gradient(90deg, ${colors.primary.main} 0%, ${colors.primary.light} 100%)`,
                },
            },
        },
        MuiTypography: {
            styleOverrides: {
                root: {
                    color: colors.text.primary,
                },
            },
        },
    },
});

export default lightTheme;