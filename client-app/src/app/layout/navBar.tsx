import { AppBar, Box, Button, IconButton, Toolbar, Typography } from '@mui/material'
import { useTranslation } from 'react-i18next'
import ReorderIcon from '@mui/icons-material/Reorder';
import MobileNavDialog from '../../features/mobile/navBar/MobileNavDialog';
import useStore from '../stores/store';
import { observer } from 'mobx-react-lite';
import ProfileSettingsMenu from '../../features/navBar/profileMenu/profileSettingsMenu';
import { Link } from 'react-router';
import SettingsIcon from '@mui/icons-material/Settings';

export default observer(function NavBar() {
    const { t } = useTranslation();
    const { uiStore, userStore } = useStore();
    const theme = uiStore.getTheme();
    const handleOpenUserSettings = (el: HTMLElement) => {
        uiStore.setUserSettingIsOpen(true); uiStore.setMenuElement(el);
    }
    return (
        <>
        <AppBar sx={{ height:"50px", display: "flex", width:"100vw", top:0,left:0, justifyContent:"center"}}>
            <Toolbar sx={{ justifyContent: "space-between" }}>
                    <Box display="flex">
                     <Link to="/" style={{ textDecoration:"none" }}>
                            <Typography variant="h6" > Barterly </Typography>
                    </Link>
                    {/* Search bar */ }
                </Box>
                

                    {uiStore.isMobile ? (
                        <Box>
                            <IconButton onClick={()=>uiStore.setIsMobileMenuOpen(true)}>
                            <ReorderIcon/>
                        </IconButton>
                    </Box>
                ) : (
                        <Box display="flex" flexDirection="row" gap="10px" alignItems="center">
                           <Button variant="contained" color = "secondary" >
                              { t('addPost') }
                           </Button>
                                <Typography variant="body1">
                                    {userStore.isLoggedIn
                                        ? `${t("hello")}, ${userStore.user?.firstName}` 
                                        : ""
                                    }
                                </Typography>
                           <IconButton onClick={(event) => { handleOpenUserSettings(event.target) }}>
                               <SettingsIcon sx={{ color: theme.palette.primary.contrastText, transition:"0.3s linear","&:hover": {transform:"rotate(90deg)"} }} />
                           </IconButton>
                        </Box>
                    )}

            </Toolbar>
            </AppBar>
            <ProfileSettingsMenu />
            {uiStore.isMobile && <MobileNavDialog />}
            
        </>
    )
})