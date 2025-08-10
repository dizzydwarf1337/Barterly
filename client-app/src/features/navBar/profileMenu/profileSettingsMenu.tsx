import {alpha, Box, Button, Divider, IconButton, Menu, Typography,} from "@mui/material";
import {observer} from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import ProfileDashboard from "./profileDashboard";
import LoginForm from "./loginForm";
import DarkModeIcon from '@mui/icons-material/DarkMode';
import LightModeIcon from '@mui/icons-material/LightMode';
import i18n from "../../../app/locales/i18n";

export default observer(function ProfileSettingsMenu() {

    const {uiStore, authStore} = useStore();
    const {theme} = uiStore;
    const isOpen = uiStore.getUserSettingIsOpen();

    return (
        <Menu
            open={isOpen}
            anchorEl={uiStore.menuElement}
            onClose={() => {
                uiStore.setUserSettingIsOpen(false);
                uiStore.setMenuElement(undefined)
            }}
            transitionDuration={400}
        >
            <Box sx={{p: "10px", height: "100%"}} display="flex" flexDirection="column" gap="5px"
                 justifyContent="center" alignItems="center">
                {authStore.isLoggedIn ?
                    (
                        <ProfileDashboard/>
                    )
                    :
                    (
                        <LoginForm/>
                    )
                }
            </Box>
            <Divider sx={{m: "0px 10px 5px 10px"}}/>
            <Box display="flex" flexDirection="row" width="100%" justifyContent="center" gap="20px">
                <IconButton onClick={() => {
                    uiStore.changeTheme();
                    console.log("theme changed")
                }} sx={{
                    transition: "all 0.3s ease",
                    '&:hover': {
                        boxShadow: `0px 2px 3px ${alpha("#000", 0.5)}`,
                        transform: 'translateY(-2px)',
                    }
                }}>
                    {uiStore.themeMode === "light" ?
                        <DarkModeIcon sx={{color: theme.palette.primary.contrastText}}/>
                        :
                        <LightModeIcon sx={{color: theme.palette.primary.contrastText}}/>
                    }
                </IconButton>
                <Button onClick={() => uiStore.changeLanguage()} sx={{boxShadow: "none"}}>
                    <Typography variant="button" color="secondary"> {i18n.language === 'en' ? 'PL' : 'EN'} </Typography>
                </Button>
            </Box>
        </Menu>
    )
})