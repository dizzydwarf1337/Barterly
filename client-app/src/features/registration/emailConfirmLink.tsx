import { useEffect } from "react";
import useStore from "../../app/stores/store";
import { useNavigate } from "react-router";
import { useTranslation } from "react-i18next";
import { CircularProgress } from "@mui/material";
import { useSearchParams } from "react-router";


export default function EmailConfirmLink() {
    const { userStore, uiStore } = useStore();
    const [searchParams] = useSearchParams();
    const email = searchParams.get("email");
    const token = searchParams.get("token");
    const { userLoading } = userStore;
    const navigate = useNavigate();
    const { t } = useTranslation();
    if (!email || !token) {
        navigate("/");
    }
    useEffect(() => {
        userStore.confirmEmail({ email: email!, token: token! })
            .then(() => uiStore.showSnackbar(t("confirmationMailSuccess"), "success", "center"))
            .catch(() => uiStore.showSnackbar(t("confirmationMailFailed"), "error", "center"))
    },[email,token])


    return (
        <>
            {
                userLoading ?? <CircularProgress />
            }
        </>
    )
}