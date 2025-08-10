import { Box, Button, CircularProgress, Divider, TextField, Typography } from "@mui/material";
import FacebookButton from "./facebookButton";
import GoogleButton from "./googleButton";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import { useForm, Controller } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import useStore from "../../../app/stores/store";
import authApi from "../../auth/api/authApi";
import { LoginRequestDTO } from "../../auth/dto/authDto";

export default function LoginForm() {
    const { uiStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();

    const validationSchema = yup.object().shape({
        email: yup
            .string()
            .required(t("emailRequired") || "Email is required")
            .email(t("emailInvalid") || "Please enter a valid email address")
            .trim(),
        
        password: yup
            .string()
            .required(t("passwordRequired") || "Password is required")
            .min(1, t("passwordRequired") || "Password is required"), 
    });

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting }
    } = useForm<LoginRequestDTO>({
        resolver: yupResolver(validationSchema),
        defaultValues: {
            email: "",
            password: "",
        }
    });

    const onSubmit = async (data: LoginRequestDTO) => {
        try {
            await authApi.login(data);
            uiStore.showSnackbar(t("loginSuccess"), "success", "right");
            reset(); 
        } catch (error) {
            console.error("Login failed:", error);
            uiStore.showSnackbar(t("loginFailed"), "error", "right");
        }
    };

    return (
        <Box width="100%" display="flex" flexDirection="column" gap="5px">
            <form onSubmit={handleSubmit(onSubmit)}>
                <Box sx={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                    
    
                    <Controller
                        name="email"
                        control={control}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label="Email"
                                type="email"
                                autoComplete="username"
                                error={!!errors.email}
                                helperText={errors.email?.message}
                                fullWidth
                                variant="outlined"
                            />
                        )}
                    />

                 
                    <Controller
                        name="password"
                        control={control}
                        render={({ field }) => (
                            <TextField
                                {...field}
                                label={t("password") || "Password"}
                                type="password"
                                autoComplete="current-password"
                                error={!!errors.password}
                                helperText={errors.password?.message}
                                fullWidth
                                variant="outlined"
                            />
                        )}
                    />

             
                    <Button 
                        variant="outlined" 
                        color="success" 
                        type="submit"
                        disabled={isSubmitting}
                        fullWidth
                        sx={{ mt: 1 }}
                    >
                        {isSubmitting ? (
                            <CircularProgress size="25px" />
                        ) : (
                            t("login")
                        )}
                    </Button>
                </Box>
            </form>


            <Divider sx={{ m: "10px" }}>
                <Typography variant="subtitle2">
                    {t("orLogin")}
                </Typography>
            </Divider>

  
            <Box display="flex" flexDirection="column" gap="5px" width="100%">
                <GoogleButton />
                <FacebookButton />
            </Box>


            <Divider sx={{ mt: "10px" }} />

      
            <Box display="flex" justifyContent="center" alignItems="center" width="100%">
                <Button
                    variant="contained"
                    color="secondary"
                    fullWidth
                    onClick={() => {
                        navigate("/sign-in");
                        uiStore.setIsMobileMenuOpen(false);
                    }}
                    sx={{ mt: 1 }}
                >
                    <Typography>{t("signIn")}</Typography>
                </Button>
            </Box>
        </Box>
    );
}