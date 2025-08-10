import { 
    Box, 
    Typography, 
    useTheme, 
    Skeleton,
    Alert,
    AlertTitle,
    Fade,
    Grid,
    useMediaQuery
} from "@mui/material";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

// Icons
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import CategoryIcon from '@mui/icons-material/Category';

import categoryApi from "../api/categoriesApi";
import { Category } from "../types/categoryTypes";
import CategoryListItem from "./categoryListItem";

export default observer(function CategoryList() {
    const theme = useTheme();
    const { t } = useTranslation();
    
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const isTablet = useMediaQuery(theme.breakpoints.down('md'));
    
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                setLoading(true);
                setError(null);
                const response = await categoryApi.getCategories();
                setCategories(response.value || []);
            } catch (error) {
                console.error("Failed to fetch categories:", error);
                setError(t('failedToLoadCategories'));
            } finally {
                setLoading(false);
            }
        };

        fetchCategories();
    }, [t]);

    // Loading skeleton
    if (loading) {
        const skeletonCount = isMobile ? 6 : isTablet ? 8 : 12;
        return (
            <Box>
                <Grid container spacing={2}>
                    {Array.from({ length: skeletonCount }).map((_, index) => (
                        <Grid 
                            item 
                            xs={6} 
                            sm={4} 
                            md={3} 
                            lg={2.4} 
                            xl={2} 
                            key={index}
                        >
                            <Skeleton
                                variant="rectangular"
                                height={isMobile ? 80 : 100}
                                sx={{
                                    borderRadius: 2,
                                    bgcolor: theme.palette.action.hover,
                                }}
                            />
                        </Grid>
                    ))}
                </Grid>
            </Box>
        );
    }

    // Error state
    if (error) {
        return (
            <Fade in timeout={500}>
                <Alert 
                    severity="error" 
                    icon={<ErrorOutlineIcon fontSize="inherit" />}
                    sx={{
                        borderRadius: 2,
                        '& .MuiAlert-message': {
                            width: '100%',
                        },
                    }}
                >
                    <AlertTitle>{t('error')}</AlertTitle>
                    {error}
                </Alert>
            </Fade>
        );
    }

    // Empty state
    if (!categories || categories.length === 0) {
        return (
            <Fade in timeout={500}>
                <Box
                    display="flex"
                    flexDirection="column"
                    alignItems="center"
                    justifyContent="center"
                    minHeight="200px"
                    gap={2}
                    sx={{
                        py: 4,
                        px: 2,
                        textAlign: 'center',
                    }}
                >
                    <CategoryIcon 
                        sx={{ 
                            fontSize: 64, 
                            color: theme.palette.text.disabled,
                            mb: 1,
                        }} 
                    />
                    <Typography 
                        variant="h6" 
                        color="text.secondary"
                        gutterBottom
                    >
                        {t('noCategoriesFound')}
                    </Typography>
                    <Typography 
                        variant="body2" 
                        color="text.disabled"
                        sx={{ maxWidth: 400 }}
                    >
                        {t('noCategoriesDescription')}
                    </Typography>
                </Box>
            </Fade>
        );
    }

    // Main content
    return (
        <Fade in timeout={800}>
            <Box>
                {/* Categories count */}
                <Box mb={3}>
                    <Typography 
                        variant="body2" 
                        color="text.secondary"
                        sx={{ 
                            fontWeight: 500,
                            display: 'flex',
                            alignItems: 'center',
                            gap: 1,
                        }}
                    >
                        <CategoryIcon fontSize="small" />
                        {t('categoriesCount', { count: categories.length })}
                    </Typography>
                </Box>

                {/* Categories grid */}
                <Grid container spacing={2}>
                    {categories.map((category, index) => (
                        <Grid 
                            item 
                            xs={6} 
                            sm={4} 
                            md={3} 
                            lg={2.4} 
                            xl={2} 
                            key={category.id}
                        >
                            <Fade 
                                in 
                                timeout={300} 
                                style={{ 
                                    transitionDelay: `${index * 50}ms` 
                                }}
                            >
                                <Box>
                                    <CategoryListItem category={category} />
                                </Box>
                            </Fade>
                        </Grid>
                    ))}
                </Grid>

                {/* Subcategories info */}
                {categories.some(cat => cat.subCategories?.length > 0) && (
                    <Box mt={3}>
                        <Typography 
                            variant="caption" 
                            color="text.disabled"
                            sx={{ 
                                fontStyle: 'italic',
                                display: 'block',
                                textAlign: 'center',
                            }}
                        >
                            {t('subcategoriesAvailable')}
                        </Typography>
                    </Box>
                )}
            </Box>
        </Fade>
    );
});