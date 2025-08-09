import {alpha, createTheme, darken, lighten} from "@mui/material";

const lightTheme = createTheme({
    breakpoints: {
        values: {
            xs: 0,
            sm: 480,
            md: 768,
            lg: 1024,
            xl: 1440,
        },
    },
    palette: {
        primary: {
            main: "#768CF7", // 768CF7 5CA4A9
            contrastText: "#000000",
        },
        secondary: {
            main: "#FF87A2", // FF87A2 FF8A5C
        },
        error: {
            main: "#DC0909", // DC0909 D32F2F
        },
        warning: {
            main: "#FCEA4A", // FCEA4A FBC02D
        },
        success: {
            main: "#71C675",
        },
        background: {
            default: "#F9FAFB", // F9FAFB E1F1F2
            paper: "#E0E1E6", // E0E1E6 B8D8D8
        },
        text: {
            primary: '#333333',
            secondary: '#555555',
        },
    },
    components: {
        MuiButton: {
            styleOverrides: {

                root: {
                    textTransform: "none",
                    borderRadius: "12px",
                    size: "small",
                    color: "#000000",
                    boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
                    transition: "all 0.3s ease",
                    '&:hover': {
                        boxShadow: `0px 2px 3px ${alpha("#000", 0.5)}`,
                        transform: 'translateY(-2px)',
                    },
                },
                outlined: {
                    borderWidth: "2px",
                    '&:hover': {
                        color: darken("#0B2027", 0.9),
                    },
                },

                contained: {
                    '&:hover': {
                        boxShadow: `0px 4px 8px ${alpha("#000", 0.5)}`,
                    },
                },

                containedSecondary: {
                    '&:hover': {
                        backgroundColor: darken("#FF87A2", 0.1), // 
                    },
                },
                containedSuccess: {
                    '&:hover': {
                        backgroundColor: darken("#71C675", 0.1),
                    },
                },
                containedError: {
                    '&:hover': {
                        backgroundColor: lighten("#DC0909", 0.05),
                    },
                },
                outlinedPrimary: {
                    '&:hover': {
                        backgroundColor: alpha("#768CF7", 0.2),
                        borderColor: darken("#768CF7", 0.2),
                    },
                },
                outlinedSecondary: {
                    ':hover': {
                        backgroundColor: alpha("#FF8A5C", 0.2),
                        borderColor: darken("#FF8A5C", 0.2),
                    },
                },
                outlinedError: {
                    '&:hover': {
                        backgroundColor: alpha("#D32F2F", 0.2),
                        borderColor: darken("#D32F2F", 0.2),
                    },
                },
                outlinedWarning: {
                    '&:hover': {
                        backgroundColor: alpha("#FCEA4A", 0.2),
                        borderColor: darken("#FCEA4A", 0.2),
                    },
                },
                outlinedSuccess: {
                    '&:hover': {
                        backgroundColor: alpha("#388E3C", 0.2),
                        borderColor: darken("#388E3C", 0.2),
                    },
                },
            },
        },

        MuiTypography: {
            styleOverrides: {
                root: {
                    fontFamily: "Tahoma, Verdana, sans-serif",
                    color: "#242424",
                },
                h1: ({theme}) => ({
                    [theme.breakpoints.down("sm")]: {
                        fontSize: "3rem",
                    },
                    [theme.breakpoints.up("sm")]: {
                        fontSize: "6rem",
                    },
                }),
                h2: ({theme}) => ({
                    [theme.breakpoints.down("sm")]: {
                        fontSize: "1.3rem",
                    },
                    [theme.breakpoints.up("sm")]: {
                        fontSize: "4rem",
                    },
                }),
            },
        },
    },
});

export default lightTheme;
