import {Box, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import useStore from "../app/stores/store";
import {useTranslation} from "react-i18next";
import FeedDashboard from "../features/posts/pages/FeedDashboard";
import CategoriesDashboard from "../features/categories/pages/categoriesDashboard";

export default observer(function HomePage() {
    const {uiStore} = useStore();
    const {t} = useTranslation();

    return (
        <Box display="flex" flexDirection="column" gap="20px">
            <Box display="flex" flexDirection="column" gap="10px">
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
                    <CategoriesDashboard/>
                </Box>
            </Box>

            <Box display="grid" gridTemplateColumns={uiStore.isMobile ? undefined : "3fr 1fr"} gap="20px">
                <Box display="flex" flexDirection="column" gap="10px">
                    <FeedDashboard/>
                </Box>
            </Box>
        </Box>
    )
})