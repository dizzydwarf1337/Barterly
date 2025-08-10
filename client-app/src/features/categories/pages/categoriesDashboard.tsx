import {Box, Typography} from "@mui/material"
import {observer} from "mobx-react-lite"

import CategoryList from "../components/categoryList";
import {useTranslation} from "react-i18next";
import useStore from "../../../app/stores/store";

export default observer(function CategoriesDashboard() {
    const {t} = useTranslation();
    const {uiStore} = useStore();
    return (
        <>
            <Box
                display="flex"
                width="100%"
                alignItems={uiStore.isMobile ? "center" : "flex-start"}
                justifyContent={uiStore.isMobile ? "center" : "flex-start"}
            >
                <Typography variant="h3" sx={{pl: uiStore.isMobile ? "20px" : "0px"}}>
                    {t('categories')}
                </Typography>
            </Box>
            <Box p="20px" sx={{backgroundColor: "background.paper", borderRadius: "12px"}}>
                <CategoryList/>
            </Box>
        </>
    )
})