import { alpha, createTheme, darken } from "@mui/material";



const lightTheme = createTheme({
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
            main: "#5CA4A9",
            contrastText: "#000000",
            
        },
        secondary: {
            main: "#FF8A5C",
        },
        error: {
            main: "#D32F2F", 
        },
        warning: {
            main: "#FBC02D", 
        },
        success: {
            main:"#71C675",
        },
        background: {
            default: "#E1F1F2",
            paper:"#B8D8D8",
        },
    },
    components: {
        MuiButton: {
            styleOverrides: {
                root: {
                    borderRadius: "15px",
                    size: "small",
                    color: "#000000",
                    transition: "all 0.3s ease",
                },

                outlined: {
                    borderWidth: "2px",
                    '&:hover': {
                        color: darken("#333333",0.7),
                    }
                },

                contained: {
                   
                    '&:hover': {
                        color: darken("#333333", 0.9),
                    }
                },

                outlinedPrimary: {

                    "&:hover": {
                        backgroundColor: alpha("#5CA4A9", 0.2),  
                    },
                },
                outlinedSecondary: {
         
                    ":hover": {
                        backgroundColor: alpha("#FF8A5C", 0.2),
                      
                    },
                },
                outlinedError: {
                    "&:hover": {
                        backgroundColor: alpha("#D32F2F", 0.2),
                    },
                },
                outlinedWarning: {
                    "&:hover": {
                        backgroundColor: alpha("#FBC02D", 0.2),
                    },
                },
                outlinedSuccess: {
                    "&:hover": {
                        backgroundColor: alpha("#388E3C", 0.2),
                    },
                }

            }
        },
        MuiTypography: {
            styleOverrides: {
                root: {
                    fontFamily: "Tahoma, Verdana, sans-serif",
                    color:"#242424",
                },
                h1: ({ theme }) => ({
                    [theme.breakpoints.down("sm")]: {
                        fontSize: "3rem",
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
export default lightTheme;