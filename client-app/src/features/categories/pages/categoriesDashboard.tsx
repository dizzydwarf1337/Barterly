import { 
    Box, 
    Typography, 
    Container
} from "@mui/material"
import { observer } from "mobx-react-lite"
import { useTranslation } from "react-i18next";

import CategoryList from "../components/categoryList";
import useStore from "../../../app/stores/store";

export default observer(function CategoriesDashboard() {
    const { t } = useTranslation();
    const { uiStore } = useStore();

    return (
        <Container maxWidth="xl" sx={{ py: { xs: 2, md: 3 } }}>
            <Box mb={3}>
                <Typography 
                    variant={uiStore.isMobile ? "h4" : "h3"} 
                    component="h1"
                    fontWeight="bold"
                    color="text.primary"
                    textAlign={uiStore.isMobile ? "center" : "left"}
                >
                    {t('categories')}
                </Typography>
            </Box>

            <CategoryList />
        </Container>
    )
})