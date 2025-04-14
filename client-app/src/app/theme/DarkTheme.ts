import { alpha, createTheme, lighten } from "@mui/material";


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
            main: "#0B2027", //#1F4342
            contrastText: "#F1FAF9",
        },
        secondary: {
            main: "#B5523B",

        },
        error: {
            main: "#C62828",
        },
        warning: {
            main: "#F9A825",
        },
        success: {
            main: "#2C6B2F",
        },
        info: {
            main:"#F1FAF9",
        },
        background: {
            default: "#242424",
            paper:"#343232",
        },


    },
    components: {
        MuiButton: {
            styleOverrides: {
                root: {
                    borderRadius: "15px",
                    size: "small",
                    color: "#CBE1E0",
                    transition: "all 0.3s ease",
                },
                outlined: {
                    borderWidth: "2px",
                    '&:hover': {
                        color: lighten("#0B2027", 0.7),
                    }
                },
                contained: {
                    '&:hover': {
                        color: lighten("#0B2027", 0.9)
                    }
                },
                outlinedPrimary: {
                    "&:hover": {
                        backgroundColor: alpha("#0B2027", 0.3),


                    },
                },

                outlinedSecondary: {
                    ":hover": {
                        backgroundColor: alpha("#B5523B", 0.3),

                    },
                },
                outlinedError: {
                    ":hover": {
                        backgroundColor: alpha("#C62828", 0.3),

                    },
                },
                outlinedWarning: {
                    ":hover": {
                        backgroundColor: alpha("#F9A825", 0.3),

                    },
                },
                outlinedSuccess: {
                    ":hover": {
                        backgroundColor: alpha("#2C6B2F", 0.3),

                    },
                },
                containedPrimary: {
                    '&:hover': {
                        backgroundColor: lighten("#0B2027", 0.1),
                    }
                },
                containedSecondary: {
                    '&:hover': {
                        backgroundColor: lighten("#B5523B", 0.1),
                    }
                },
                containedError: {
                    '&:hover': {
                        backgroundColor: lighten("#C62828", 0.1),
                    }
                },
                containedWarning: {
                    '&:hover': {
                        backgroundColor: lighten("#F9A825", 0.1),
                    }
                },
                containedSuccess: {
                    '&:hover': {
                        backgroundColor: lighten("#2C6B2F", 0.1),
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