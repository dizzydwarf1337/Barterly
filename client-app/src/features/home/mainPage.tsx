import { Box, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";
import CategoriesDashboard from "../categories/categoriesDashboard";

export default observer( function MainPage() {
    const { t } = useTranslation();



    return (
        <>
            <Box display="flex" flexDirection="column" gap="10px">
               <CategoriesDashboard/>
            </Box>
        </>
    )
})