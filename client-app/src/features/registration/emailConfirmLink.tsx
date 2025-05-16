import { useEffect } from "react";
import useStore from "../../app/stores/store";
import { useNavigate } from "react-router";
import { useTranslation } from "react-i18next";
import { CircularProgress } from "@mui/material";
import { useSearchParams } from "react-router";


export default function EmailConfirmLink() {
    const { userStore, uiStore } = useStore();
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const { t } = useTranslation();
    const { userLoading } = userStore;

    const email = searchParams.get("email");
    const token = searchParams.get("token")?.replace(/ /g, "+");

    useEffect(() => {
        if (!email || !token) {
            navigate("/");
            return;
        }

        userStore.confirmEmail({ email, token })
            .then(() => uiStore.showSnackbar(t("confirmationMailSuccess"), "success", "center"))
            .catch(() => uiStore.showSnackbar(t("confirmationMailFailed"), "error", "center"));
    }, [email, token, navigate, userStore, uiStore, t]);

    return (
        <>
            {
                userLoading ?? <CircularProgress />
            }
        </>
    )
}