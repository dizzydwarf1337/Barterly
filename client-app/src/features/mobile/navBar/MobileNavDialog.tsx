import { Accordion, AccordionDetails, AccordionSummary, Box, Button, Dialog, IconButton, Typography } from "@mui/material";
import CloseIcon from '@mui/icons-material/Close';
import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import i18n from "../../../app/locales/i18n";
import darkTheme from "../../../app/theme/DarkTheme";
import lightTheme from "../../../app/theme/LightTheme";
import DarkModeIcon from '@mui/icons-material/DarkMode';
import LightModeIcon from '@mui/icons-material/LightMode';
import useStore from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoginForm from "../../navBar/profileMenu/loginForm";
import ProfileDashboard from "../../navBar/profileMenu/profileDashboard";
export default observer(function MobileNavDialog() {
    const { t } = useTranslation();
    const { userStore, uiStore } = useStore();
    const { isMobileMenuOpen, setIsMobileMenuOpen } = uiStore;
    useEffect(() => {
        const handleBackButton = (event: PopStateEvent) => {
            if (uiStore.isMobileMenuOpen) {
                event.preventDefault();
                uiStore.setIsMobileMenuOpen(false);
            }
        };

        if (uiStore.isMobileMenuOpen) {
            window.history.pushState(null, "", window.location.href);
        }

        window.addEventListener("popstate", handleBackButton);

        return () => {
            window.removeEventListener("popstate", handleBackButton);
        };
    }, [isMobileMenuOpen]);   
    return (
        <>

            <Dialog open={isMobileMenuOpen} fullScreen={true} onClose={()=>setIsMobileMenuOpen(false)}>

                <Box display="flex" width="100%" height="100%" sx={{ backgroundColor: "primary.main" }}>
                    <Box display="flex" flexDirection="column" width="100%" p="10px" m="10px" >
                        <Box display="flex" flexDirection="row" justifyContent="flex-end" alignItems="center" height="50px" sx={{ backgroundColor: "primary." }}  width="100%">
                            <IconButton onClick={()=>setIsMobileMenuOpen(false)}>
                                <CloseIcon color="error" />
                            </IconButton>
                        </Box>
                        <Box display="flex" flexDirection="column" justifyContent="" alignItems="center" width="100%" gap="30px" >
                            <Box width="100%">
                                <Box width="100%" display="flex" justifyContent="center">
                                    {userStore.isLoggedIn
                                        ?
                                        (
                                            <Box>
                                                <ProfileDashboard/>
                                            </Box>
                                        ) : 
                                        (
                                            <Accordion>
                                                <AccordionSummary>
                                                    <Typography variant="button" sx={{ width: "100%", textAlign: "center" }}>
                                                        {t("login")}
                                                    </Typography>
                                                </AccordionSummary>
                                                <AccordionDetails onClick={() => { }}>
                                                    <LoginForm />
                                                </AccordionDetails>
                                            </Accordion>
                                        )
                                    } 
                                </Box>
                            </Box>
                            <Box sx={{ display: "flex", width: "100%", alignItems: "flex-end", justifyContent: "center", gap: "100px", height: "100%", }}>
                                <IconButton onClick={uiStore.changeTheme} >
                                    {uiStore.themeMode == "light" ?
                                        <DarkModeIcon sx={{ color: darkTheme.palette.background.default }} />
                                        :
                                        <LightModeIcon sx={{ color: lightTheme.palette.background.default }} />
                                    }
                                </IconButton>
                                <Button onClick={uiStore.changeLanguage} >
                                    <Typography variant="button" color="secondary" > {i18n.language === 'en' ? 'PL' : 'EN'} </Typography>
                                </Button>
                            </Box>
                        </Box>
                    </Box>
                </Box>
            </Dialog>
        </>
    )
})