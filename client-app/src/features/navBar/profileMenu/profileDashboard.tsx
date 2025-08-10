import useStore from "../../../app/stores/store";
import {Box, Button, Typography} from "@mui/material";
import SettingsIcon from '@mui/icons-material/Settings';
import LogoutIcon from '@mui/icons-material/Logout';
import {useTranslation} from "react-i18next";
import {observer} from "mobx-react-lite";

export default observer(function ProfileDashboard() {

    const {uiStore, authStore} = useStore();
    const {t} = useTranslation();
    const user = authStore.getUser();
    const handleLogout = () => {
        uiStore.setUserSettingIsOpen(false);
        authStore.logout()
            .then(() => {
                uiStore.showSnackbar(t("logoutSuccess"), "success", "right")
            })
            .catch(() => {
                uiStore.showSnackbar(t("logoutFailed"), "error", "right")
            });
    }
    return (
        <>
            <Box sx={{
                display: "flex",
                flexDirection: "column",
                m: "2px",
                p: "2px",
                justtifyContent: "center",
                alignItems: "center",
                gap: "20px",
                width: "200px"
            }}>
                <Box component="img" src={user!.profilePicturePath || "/user/blankUser.svg"}
                     sx={{width: "100px", borderRadius: "50%", boxShadow: `1px 1px 2px 2px background.main`}}/>
                <Box sx={{
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "center",
                    gap: "10px"
                }} width="100%">
                    <Button variant="outlined" color="warning" fullWidth>
                        <Box display="flex" gap="10px">
                            <SettingsIcon/>

                            <Typography variant="button">
                                {t("settings")}
                            </Typography>
                        </Box>
                    </Button>
                    <Button variant="contained" color="error" onClick={handleLogout} fullWidth>
                        <Box display="flex" gap="10px">
                            <LogoutIcon/>
                            <Typography variant="button">
                                {t("logout")}
                            </Typography>
                        </Box>
                    </Button>

                </Box>

            </Box>
        </>
    )
})