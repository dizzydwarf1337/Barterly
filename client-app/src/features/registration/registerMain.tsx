import { observer } from "mobx-react-lite"
import useStore from "../../app/stores/store";
import { useNavigate } from "react-router";
import { Box, Typography } from "@mui/material";
import RegistrationForm from "./registrationForm";
import { useTranslation } from "react-i18next";
import { useEffect } from "react";


export default observer(function RegisterMain() {

    const { userStore, uiStore } = useStore();
    const navigate = useNavigate();
    const { t } = useTranslation();
    useEffect(() => {
        if(userStore.user)
            navigate("/");
    })
    

    return (
        <>
            <Box display="flex" flexDirection={uiStore.isMobile ? "column" : "row"} justifyContent="center" alignItems="center" gap="40px"  >
                <Box display="flex" justifyContent="center" alignItems="center" flexDirection="column" gap="20px">
                    <Typography variant="h4">{t("signIn")}</Typography>
                    <RegistrationForm />
                </Box>
                <Box>

                </Box>
            </Box>
        </>
    )
})