import {useGoogleLogin} from "@react-oauth/google";
import useStore from "../../../app/stores/store";
import {Box, Button, Typography} from "@mui/material";
import {useTranslation} from "react-i18next";
import authApi from "../../auth/api/authApi";


export default function GoogleButton() {
    const {uiStore} = useStore();
    const {t} = useTranslation();
    const handleLogin = (token: string) => {
        authApi.loginWithGoogle({token})
            .then(() => {
                uiStore.showSnackbar(t("loginSuccess"), "success", "right")
            })
            .catch(() => {
                uiStore.showSnackbar(t("loginFailed"), "error", "right")
            });
    };

    const login =
        useGoogleLogin({
            onSuccess: (codeResponse) => handleLogin(codeResponse.code),
            onError: (error) => console.log('Login Failed:', error),
            onNonOAuthError: (error) => console.log(error),
            flow: "auth-code",

        });
    return (
        <Box width="auto" display="flex" justifyContent="center" alignItems="center">
            <Button variant="outlined" color="primary" onClick={login} fullWidth>
                <Box display="flex" flexDirection="row" justifyContent="center" alignItems="center" gap="10px">
                    <img width="30px"
                            src={uiStore.themeMode == "light" ? "/googleIcons/web_light_rd_na.svg" : "/googleIcons/web_dark_rd_na.svg"}
                            alt="icon"/>
                    <Typography variant="button" color="secondary">Login with Google!</Typography>
                </Box>
            </Button>
        </Box>
    );

}
