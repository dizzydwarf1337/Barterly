import { alpha, createTheme, darken, lighten } from "@mui/material";


const darkTheme = createTheme({
    breakpoints: {
        values: {
            xs: 0,
            sm: 480,
            md: 768,
            lg: 1024,
            xl: 1440,
        }
    },
    palette: {
        primary: {
            main: "#2F44AB", // 2F44AB 0B2027
            contrastText: "#F1FAF9",
        },
        secondary: {
            main: "#B13659", // B13659 B5523B

        },
        error: {
            main: "#E53935", // E53935 C62828
        },
        warning: {
            main: "#FFA000", // FFA000 F9A825
        },
        success: {
            main: "#388E3C", // 388E3C 2C6B2F
        },
        info: {
            main: "#1976D2", // 1976D2 F1FAF9
        },
        background: {
            default: "#242424",
            paper:"#343232",
        },
        text: {
            primary: '#F1FAF9', // F1FAF9
            secondary: '#B0BEC5',
        },

    },
    components: {
        MuiButton: {
            styleOverrides: {

                root: {
                    borderRadius: "12px",
                    size: "small",
                    color: "##F1FAF9",
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
                        color: lighten("#2F44AB", 0.9),
                    }
                },
                contained: {
                    '&:hover': {
                        transform: "translateY(-2px)",  
                        color: lighten("#2F44AB", 0.9)
                    }
                },
                outlinedPrimary: {
                    "&:hover": {
                        backgroundColor: alpha("#2F44AB", 0.3),


                    },
                },

                outlinedSecondary: {
                    ":hover": {
                        backgroundColor: alpha("#B13659", 0.3),

                    },
                },
                outlinedError: {
                    ":hover": {
                        backgroundColor: alpha("#E53935", 0.3),

                    },
                },
                outlinedWarning: {
                    ":hover": {
                        backgroundColor: alpha("#FFA000", 0.3),

                    },
                },
                outlinedSuccess: {
                    ":hover": {
                        backgroundColor: alpha("#388E3C", 0.3),

                    },
                },
                containedPrimary: {
                    '&:hover': {
                        backgroundColor: lighten("#2F44AB", 0.1),
                    }
                },
                containedSecondary: {
                    '&:hover': {
                        backgroundColor: lighten("#B13659", 0.1),
                    }
                },
                containedError: {
                    '&:hover': {
                        backgroundColor: lighten("#E53935", 0.1),
                    }
                },
                containedWarning: {
                    '&:hover': {
                        backgroundColor: lighten("#FFA000", 0.1),
                    }
                },
                containedSuccess: {
                    '&:hover': {
                        backgroundColor: lighten("#388E3C", 0.1),
                    }
                },


            }
        },

        MuiTypography: {
            styleOverrides: {
                root: {
                    fontFamily: "Tahoma, Verdana, sans-serif",
                    color: "#F1FAF9"
                },
                h1: ({ theme }) => ({
                      [theme.breakpoints.down("sm")]: {
                        fontSize: "2rem",
                    },
                    [theme.breakpoints.up("sm")]: {
                        fontSize: "6rem",
                    },
                }),
                h2: ({ theme }) => ({
                    [theme.breakpoints.down("sm")]: {
                        fontSize: "1.3rem",
                    },
                    [theme.breakpoints.up("sm")]: {
                        fontSize: "4rem",
                    },
                }),

            },
        }

    }
})
export default darkTheme;