import {Box, Button, CircularProgress, FormControl, FormHelperText, Input, InputLabel, Typography} from "@mui/material";
import {ChangeEvent, useState} from "react";
import RegisterDto from "../../app/models/registerDto";
import {useTranslation} from "react-i18next";
import useStore from "../../app/stores/store";
import {useNavigate} from "react-router";
import {observer} from "mobx-react-lite";

export default observer(function RegistrationForm() {

    const {t} = useTranslation();
    const navigate = useNavigate();
    const {userStore, uiStore} = useStore();
    const [passwordConfirm, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [showPassword, SetShowPassword] = useState<boolean>(false);
    const [registerData, setRegisterData] = useState<RegisterDto>({
        email: "",
        password: "",
        firstName: "",
        lastName: "",
    });
    const handleDataChange = (e: ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setRegisterData(prev => ({...prev, [name]: value}));
    };
    const handlePasswordConfirmChange = (e: ChangeEvent<HTMLInputElement>) => {
        setConfirmPassword(e.target.value);
    };
    const hanldeSubmit = async (event) => {
        event.preventDefault();
        try {
            if (registerData.password !== passwordConfirm) {
                setError(t("passwordConfirmFailed"))
                throw e

            }
            await userStore.register(registerData);
            uiStore.showSnackbar(t("signInSuccess"), "success", "right");
            navigate(`/email-confirm/${registerData.email}`);
        } catch (e) {
            uiStore.showSnackbar(t("signInFailed"), "error", "right");
        }
    }

    return (
        <Box display="flex" flexDirection="column" gap="10px" sx={{
            backgroundColor: "background.paper",
            p: "50px",
            borderRadius: "20px",
            boxShadow: "5px 5px 1px 0px #22333B"
        }}>
            <form onSubmit={hanldeSubmit}>
                <Box display="flex" flexDirection="column" gap="20px">
                    <Box display="flex" flexDirection="row" width="100%" gap={uiStore.isMobile ? "10px" : "60px"}>

                        <Box display="flex" flexDirection="column" gap="20px">
                            <FormControl>
                                <InputLabel htmlFor="email-input" color="info">
                                    <Typography>Email</Typography>
                                </InputLabel>
                                <Input name="email" id="email-input" color="info" type="email" autoComplete="email"
                                       value={registerData.email} onChange={handleDataChange} required/>
                            </FormControl>
                            <Box display="flex" flexDirection="column" gap="10px">
                                <FormControl>
                                    <InputLabel htmlFor="password-input" color="info">
                                        <Typography>{t("password")}</Typography>
                                    </InputLabel>
                                    <Input name="password" id="password-input" color="info"
                                           type={showPassword ? "text" : "password"} autoComplete="new-password"
                                           value={registerData.password} onChange={handleDataChange} required/>
                                </FormControl>
                                <Box alignItems="left">
                                    <input type="checkbox" value={!showPassword}
                                           onChange={() => SetShowPassword(!showPassword)}/>
                                </Box>
                            </Box>
                            <FormControl>
                                <InputLabel htmlFor="passwordConfirm-input" color="info">
                                    <Typography>{t("passwordConfirm")}</Typography>
                                </InputLabel>
                                <Input name="passwordConfirm" id="passwordConfirm-input" color="info" type="password"
                                       autoComplete="new-password" value={passwordConfirm}
                                       onChange={handlePasswordConfirmChange} required/>
                                <FormHelperText>
                                    <Typography variant="caption" color="error.main">{error}</Typography>
                                </FormHelperText>
                            </FormControl>


                            <FormControl>
                                <InputLabel htmlFor="firstName-input" color="info">
                                    <Typography>{t("firstName")}</Typography>
                                </InputLabel>
                                <Input name="firstName" id="firstName-input" color="info" value={registerData.firstName}
                                       onChange={handleDataChange} required/>
                            </FormControl>

                            <FormControl>
                                <InputLabel htmlFor="lastName-input" color="info">
                                    <Typography>{t("lastName")}</Typography>
                                </InputLabel>
                                <Input name="lastName" id="lastName-input" color="info" value={registerData.lastName}
                                       onChange={handleDataChange} required/>
                            </FormControl>


                        </Box>
                    </Box>
                    <Box display="flex" justifyContent="center" alignItems="center">
                        <Button type="submit" variant="contained" color="success" size="medium"
                                sx={{minWidth: "100px"}}>
                            {userStore.getLoading()
                                ? <CircularProgress color="primary"/>
                                : <Typography>{t("signIn")}</Typography>
                            }
                        </Button>
                    </Box>
                </Box>
            </form>
        </Box>
    )
})