import { 
    Box, 
    Typography, 
    Container,
    Fade,
} from "@mui/material"
import { observer } from "mobx-react-lite"
import { useTranslation } from "react-i18next";
import CategoryIcon from '@mui/icons-material/Category';

import CategoryList from "../components/categoryList";
export default observer(function CategoriesDashboard() {
    const { t } = useTranslation();

    return (
        <Container maxWidth="xl" sx={{ py: { xs: 2, md: 3 } }}>
            <Fade in timeout={400}>
                <Box 
                    display="flex" 
                    justifyContent="space-between" 
                    alignItems="center" 
                    mb={4}
                    flexWrap="wrap"
                    gap={2}
                >
                    <Box display="flex" alignItems="center" gap={2}>
                        <CategoryIcon 
                            color="primary" 
                            sx={{ 
                                fontSize: { xs: 28, sm: 32 },
                                filter: 'drop-shadow(0 2px 4px rgba(0,0,0,0.1))'
                            }} 
                        />
                        <Box>
                            <Typography 
                                variant="h3" 
                                sx={{
                                    background: (theme) => 
                                        `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                                    backgroundClip: 'text',
                                    WebkitBackgroundClip: 'text',
                                    WebkitTextFillColor: 'transparent',
                                    fontWeight: 700,
                                    fontSize: { xs: '1.75rem', sm: '2.125rem' }
                                }}
                            >
                                {t("categories")}
                            </Typography>
                        </Box>
                    </Box>
                    
                </Box>
            </Fade>

            <CategoryList />
        </Container>
    )
})