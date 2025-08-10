import { 
    Box, 
    Typography, 
    Container, 
    Breadcrumbs, 
    Link, 
    alpha,
    Paper,
    useTheme,
    Fade,
} from "@mui/material"
import { observer } from "mobx-react-lite"
import { useTranslation } from "react-i18next";
import { Link as RouterLink } from "react-router";

// Icons
import HomeIcon from '@mui/icons-material/Home';
import CategoryIcon from '@mui/icons-material/Category';

import CategoryList from "../components/categoryList";
import useStore from "../../../app/stores/store";

export default observer(function CategoriesDashboard() {
    const { t } = useTranslation();
    const { uiStore } = useStore();
    const theme = useTheme();

    return (
        <Container maxWidth="xl" sx={{ py: { xs: 2, md: 4 } }}>
            <Fade in timeout={600}>
                <Box>
                    {/* Breadcrumbs */}
                    <Box mb={3}>
                        <Breadcrumbs
                            aria-label="breadcrumb"
                            sx={{
                                '& .MuiBreadcrumbs-separator': {
                                    color: theme.palette.text.secondary,
                                },
                            }}
                        >
                            <Link
                                component={RouterLink}
                                to="/"
                                color="inherit"
                                sx={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    textDecoration: 'none',
                                    transition: 'color 0.2s ease',
                                    '&:hover': {
                                        color: theme.palette.primary.main,
                                    },
                                }}
                            >
                                <HomeIcon sx={{ mr: 0.5 }} fontSize="inherit" />
                                {t('Home')}
                            </Link>
                            <Typography
                                color="text.primary"
                                sx={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    fontWeight: 500,
                                }}
                            >
                                <CategoryIcon sx={{ mr: 0.5 }} fontSize="inherit" />
                                {t('categories')}
                            </Typography>
                        </Breadcrumbs>
                    </Box>

                    {/* Header Section */}
                    <Box mb={4}>
                        <Box
                            display="flex"
                            flexDirection="column"
                            alignItems={uiStore.isMobile ? "center" : "flex-start"}
                            textAlign={uiStore.isMobile ? "center" : "left"}
                            gap={2}
                        >
                            <Typography 
                                variant={uiStore.isMobile ? "h4" : "h3"} 
                                component="h1"
                                fontWeight="bold"
                                sx={{
                                    background: `linear-gradient(45deg, ${theme.palette.primary.main}, ${theme.palette.secondary.main})`,
                                    backgroundClip: 'text',
                                    WebkitBackgroundClip: 'text',
                                    WebkitTextFillColor: 'transparent',
                                    mb: 1,
                                }}
                            >
                                {t('categories')}
                            </Typography>
                            
                            <Typography 
                                variant="h6" 
                                color="text.secondary"
                                sx={{ 
                                    maxWidth: '600px',
                                    lineHeight: 1.6,
                                }}
                            >
                                {t('categoriesSubtitle')}
                            </Typography>
                        </Box>
                    </Box>

                    {/* Categories Section */}
                    <Paper
                        elevation={0}
                        sx={{
                            p: { xs: 2, md: 3 },
                            borderRadius: 3,
                            background: `linear-gradient(145deg, ${alpha(
                                theme.palette.background.paper,
                                0.8
                            )}, ${alpha(theme.palette.background.default, 0.4)})`,
                            backdropFilter: 'blur(10px)',
                            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                            transition: 'all 0.3s ease',
                            '&:hover': {
                                boxShadow: theme.shadows[4],
                                transform: 'translateY(-2px)',
                            },
                        }}
                    >
                        <Box mb={2}>
                            <Typography 
                                variant="h5" 
                                fontWeight="600"
                                color="text.primary"
                                sx={{
                                    mb: 1,
                                    display: 'flex',
                                    alignItems: 'center',
                                    gap: 1,
                                }}
                            >
                                <CategoryIcon color="primary" />
                                {t('browseCategories')}
                            </Typography>
                            <Typography 
                                variant="body1" 
                                color="text.secondary"
                            >
                                {t('selectCategoryDescription')}
                            </Typography>
                        </Box>
                        
                        <CategoryList />
                    </Paper>

                    {/* Additional Info Section */}
                    <Box mt={4}>
                        <Paper
                            elevation={0}
                            sx={{
                                p: 3,
                                borderRadius: 2,
                                background: alpha(theme.palette.info.main, 0.05),
                                border: `1px solid ${alpha(theme.palette.info.main, 0.2)}`,
                            }}
                        >
                            <Typography 
                                variant="body2" 
                                color="text.secondary"
                                textAlign="center"
                                sx={{ fontStyle: 'italic' }}
                            >
                                {t('categoriesHelpText')}
                            </Typography>
                        </Paper>
                    </Box>
                </Box>
            </Fade>
        </Container>
    )
})