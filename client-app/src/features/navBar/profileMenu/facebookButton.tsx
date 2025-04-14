
import {  useGoogleLogin } from "@react-oauth/google";
import useStore from "../../../app/stores/store";
import { Box, Button, CircularProgress, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";


export default function FacebookButton() {
    const { uiStore, userStore } = useStore();
    const { t } = useTranslation();
    const handleLogin = (code: string) => {
        userStore.loginWithGoogle(code)
            .then(() => {
                uiStore.showSnackbar(t("loginSuccess"), "success", "right")
            })
            .catch(() => {
                uiStore.showSnackbar(t("loginFailed"), "error", "right")
            });
    };

    const login = useGoogleLogin({
        onSuccess: (codeResponse) => handleLogin(codeResponse.code),
        onError: (error) => console.log('Login Failed:', error),
        onNonOAuthError: (error) => console.log(error),
        flow: "auth-code",

    });
    return (
        <Box width="auto" display="flex" justifyContent="center" alignItems="center">
            <Button variant="outlined" color="primary" onClick={login} fullWidth>
                {userStore.googleLoading ?
                    <CircularProgress color="primary" />
                    :
                    (
                        <Box display="flex" flexDirection="row" justifyContent="center" alignItems="center" gap="10px" >
                            <img width="30px" src={uiStore.themeMode == "light" ? "/facebookIcons/fbLogoLight.png" : "/facebookIcons/fbLogoDark.png"} alt="icon" />
                            <Typography variant="button" color="secondary">Login with Facebook!</Typography>
                        </Box>
                    )
                }

            </Button>
        </Box>
    );

}
