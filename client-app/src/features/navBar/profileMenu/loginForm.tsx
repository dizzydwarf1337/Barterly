import {Box, Button, CircularProgress, Divider, TextField, Typography} from "@mui/material"
import FacebookButton from "./facebookButton"
import GoogleButton from "./googleButton"
import {useState} from "react"
import {useTranslation} from "react-i18next"
import {useNavigate} from "react-router"
import LoginDto from "../../../app/models/loginDto"
import useStore from "../../../app/stores/store"

export default function LoginForm() {
    const {uiStore, userStore} = useStore();
    const {t} = useTranslation();
    const navigate = useNavigate();
    const [login, setLogin] = useState<LoginDto>({
        Email: "",
        Password: "",
    });
    const handleLogin = (e) => {
        e.preventDefault();
        userStore.login(login)
            .then(() => {
                uiStore.showSnackbar(t("loginSuccess"), "success", "right");
                setLogin({Email: "", Password: ""});
            })
            .catch(() => {
                uiStore.showSnackbar(t("loginFailed"), "error", "right");
            });
    }

    const handleLoginDataChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {value, name} = e.target;

        setLogin((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    return (
        <Box width="100%" display="flex" flexDirection="column" gap="5px">
            <form onSubmit={handleLogin}>
                <Box sx={{display: "flex", flexDirection: "column", gap: "8px"}}>
                    <TextField label="Email" onChange={handleLoginDataChange} type="email" name="Email"
                               autoComplete="username" value={login.Email} required/>
                    <TextField label="Password" onChange={handleLoginDataChange} type="password" name="Password"
                               autoComplete="current-password" value={login.Password} required/>
                    <Button variant="outlined" color="success" type="submit">
                        {userStore.userLoading || userStore.googleLoading ?
                            <CircularProgress size="25px"/> : t("login")}
                    </Button>
                </Box>
            </form>
            <Divider sx={{m: "10px"}}>
                <Typography variant="subtitle2">
                    {t("orLogin")}
                </Typography>
            </Divider>
            <Box display="flex" flexDirection="column" gap="5px" width="100%">
                <GoogleButton/>
                <FacebookButton/>
            </Box>
            <Divider/>
            <Box display="flex" justifyContent="center" alignItems="centet" width="100%">
                <Button variant="contained" color="secondary" fullWidth onClick={() => {
                    navigate("/sign-in");
                    uiStore.setIsMobileMenuOpen(false)
                }}>
                    <Typography>{t("signIn")}</Typography>
                </Button>
            </Box>
        </Box>
    )
}