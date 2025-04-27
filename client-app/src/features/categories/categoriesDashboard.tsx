import { Box, Typography } from "@mui/material"
import { observer } from "mobx-react-lite"

import CategoryList from "./categoryList";
import { useTranslation } from "react-i18next";

export default observer(function CategoriesDashboard() {
    const { t } = useTranslation();

    return (
        <>
            <Box>
                <Typography variant="h2" pl="20px">
                    {t('categories')}
                </Typography>
            </Box>
            <Box p="20px" sx={{ backgroundColor: "background.paper", borderRadius: "20px" }}>
                <CategoryList />
            </Box>
        </>
    )
})