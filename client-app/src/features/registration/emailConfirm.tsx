import {Box, Button, Typography} from "@mui/material";
import {useTranslation} from "react-i18next";
import useStore from "../../app/stores/store";
import {useParams} from "react-router";
import {useEffect, useState} from "react";

export default function EmailConfirm() {
    const {t} = useTranslation();
    const {email} = useParams();
    const {userStore, uiStore} = useStore();

    const [resendTime, setResendTime] = useState(0);

    useEffect(() => {
        if (resendTime > 0) {
            const timer = setTimeout(() => setResendTime(resendTime - 1), 1000);
            return () => clearTimeout(timer);
        }
    }, [resendTime]);

    const handleResendEmail = async () => {
        if (resendTime > 0) return;
        setResendTime(60);
        try {
            await userStore.resendEmailConfirmation(email);
            uiStore.showSnackbar(t("confirmationMailSent"), "success", "right");
        } catch {
            uiStore.showSnackbar(t("confirmationMailError"), "error", "right");
        }
    };

    return (
        <Box display="flex" flexDirection="column" justifyContent="center" alignItems="center" gap="20px" mt="150px">
            <Typography variant="h2">{t("thankForSignIn")}</Typography>
            <Typography variant="h5" sx={{textAlign: "center"}}>
                {t("emailConfirmation")}
            </Typography>
            <Button
                onClick={handleResendEmail}
                component="a"
                variant="outlined"
                color="warning"
                disabled={resendTime > 0}
            >
                {resendTime > 0 ? `${t("sendAgain")}:${resendTime}s` : t("resendEmailConfirmation")}
            </Button>
        </Box>
    );
}
