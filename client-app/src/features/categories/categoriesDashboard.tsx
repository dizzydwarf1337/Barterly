import { Box, Typography } from "@mui/material"
import { observer } from "mobx-react-lite"
import CategoryList from "./categoryList";
import { t } from "i18next";


export default observer(function CategoriesDashboard() {


    return (
        <>
            <Box>
                <Typography variant="h2" pl="20px">
                    {t('categories')}
                </Typography>
            </Box>
            <Box p="20px" sx={{ backgroundColor: "background.paper", borderRadius: "20px" }}>
                <CategoryList/>
            </Box>
        </>
    )
})