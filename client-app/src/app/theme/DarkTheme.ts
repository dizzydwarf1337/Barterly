import { alpha, createTheme, lighten, darken } from "@mui/material";

// Color palette
const colors = {
    primary: {
        50: '#E8EAF6',
        100: '#C5CAE9', 
        200: '#9FA8DA',
        300: '#7986CB',
        400: '#5C6BC0',
        500: '#3F51B5', // main
        600: '#3949AB',
        700: '#303F9F',
        800: '#283593',
        900: '#1A237E',
        main: '#3F51B5',
        dark: '#303F9F',
        light: '#7986CB',
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
        main: '#00C853',
        dark: '#00A845',
        light: '#5CFF95',
    },
    error: {
        main: '#FF3D00',
        dark: '#DD2C00',
        light: '#FF6E40',
    },
    warning: {
        main: '#FF9800',
        dark: '#F57C00',
        light: '#FFB74D',
    },
    info: {
        main: '#00B0FF',
        dark: '#0081CB',
        light: '#40C4FF',
    },
    background: {
        default: '#0A0E17', // Очень темный синевато-серый
        paper: '#1A1E2E',   // Темный синевато-серый
        surface: '#252A3D', // Средний для карточек
        elevated: '#2F3349', // Для поднятых элементов
    },
    text: {
        primary: '#FFFFFF',
        secondary: '#B8BCC8',
        disabled: '#6B7280',
        hint: '#9CA3AF',
    },
    divider: alpha('#FFFFFF', 0.08),
    action: {
        hover: alpha('#FFFFFF', 0.04),
        selected: alpha('#FFFFFF', 0.08),
        disabled: alpha('#FFFFFF', 0.26),
        disabledBackground: alpha('#FFFFFF', 0.12),
    },
};

const darkTheme = createTheme({
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
        mode: 'dark',
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
        // Custom colors for special cases
        grey: {
            50: '#FAFAFA',
            100: '#F5F5F5',
            200: '#EEEEEE',
            300: '#E0E0E0',
            400: '#BDBDBD',
            500: '#9E9E9E',
            600: '#757575',
            700: '#616161',
            800: '#424242',
            900: '#212121',
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
            background: `linear-gradient(135deg, ${colors.primary.light} 0%, ${colors.secondary.light} 100%)`,
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
            '@media (max-width:600px)': {
                fontSize: '2rem',
            },
        },
        h3: {
            fontWeight: 600,
            fontSize: '2rem',
            lineHeight: 1.4,
            '@media (max-width:600px)': {
                fontSize: '1.5rem',
            },
        },
        h4: {
            fontWeight: 600,
            fontSize: '1.5rem',
            lineHeight: 1.4,
            '@media (max-width:600px)': {
                fontSize: '1.25rem',
            },
        },
        h5: {
            fontWeight: 600,
            fontSize: '1.25rem',
            lineHeight: 1.5,
        },
        h6: {
            fontWeight: 600,
            fontSize: '1.125rem',
            lineHeight: 1.5,
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
        `0px 2px 4px ${alpha('#000000', 0.1)}`,
        `0px 4px 8px ${alpha('#000000', 0.12)}`,
        `0px 8px 16px ${alpha('#000000', 0.14)}`,
        `0px 12px 24px ${alpha('#000000', 0.16)}`,
        `0px 16px 32px ${alpha('#000000', 0.18)}`,
        `0px 20px 40px ${alpha('#000000', 0.20)}`,
        `0px 24px 48px ${alpha('#000000', 0.22)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
        `0px 32px 64px ${alpha('#000000', 0.24)}`,
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
                    background: `linear-gradient(135deg, ${colors.background.default} 0%, ${lighten(colors.background.default, 0.05)} 100%)`,
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
                        boxShadow: `0px 8px 25px ${alpha('#000000', 0.25)}`,
                    },
                    '&:active': {
                        transform: 'translateY(0px)',
                        boxShadow: `0px 4px 12px ${alpha('#000000', 0.2)}`,
                    },
                    '&.Mui-disabled': {
                        backgroundColor: colors.action.disabledBackground,
                        color: colors.action.disabled,
                    },
                },
                contained: {
                    boxShadow: `0px 4px 12px ${alpha('#000000', 0.15)}`,
                    '&:hover': {
                        boxShadow: `0px 8px 25px ${alpha('#000000', 0.25)}`,
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
                        backgroundColor: alpha(colors.primary.main, 0.1),
                        borderColor: colors.primary.light,
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
                    transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                    '&:hover': {
                        transform: 'translateY(-4px)',
                        boxShadow: `0px 12px 32px ${alpha('#000000', 0.2)}`,
                        borderColor: alpha(colors.primary.main, 0.3),
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
                    backgroundColor: alpha(colors.primary.main, 0.2),
                    color: colors.primary.light,
                    border: `1px solid ${alpha(colors.primary.main, 0.3)}`,
                },
                colorSecondary: {
                    backgroundColor: alpha(colors.secondary.main, 0.2),
                    color: colors.secondary.light,
                    border: `1px solid ${alpha(colors.secondary.main, 0.3)}`,
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
                    backgroundColor: colors.background.elevated,
                    color: colors.text.primary,
                    border: `1px solid ${colors.divider}`,
                    borderRadius: 8,
                    backdropFilter: 'blur(10px)',
                },
                arrow: {
                    color: colors.background.elevated,
                },
            },
        },
        MuiDialog: {
            styleOverrides: {
                paper: {
                    backgroundColor: colors.background.paper,
                    borderRadius: 16,
                    border: `1px solid ${colors.divider}`,
                    backdropFilter: 'blur(20px)',
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
                        backgroundColor: alpha(colors.primary.main, 0.1),
                        '&:hover': {
                            backgroundColor: alpha(colors.primary.main, 0.15),
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
    },
});

export default darkTheme;