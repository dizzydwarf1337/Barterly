import { 
    Box, 
    Typography, 
    Skeleton,
    Alert,
    Grid,
    useMediaQuery,
    useTheme
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
    }, []);

    // Loading skeleton
    if (loading) {
        const skeletonCount = isMobile ? 4 : isTablet ? 6 : 8;
        return (
            <Grid container spacing={2}>
                {Array.from({ length: skeletonCount }).map((_, index) => (
                    <Grid 
                        item 
                        xs={12}
                        sm={6} 
                        md={4} 
                        lg={3}
                        key={index}
                    >
                        <Skeleton
                            variant="rectangular"
                            height={100}
                            sx={{
                                borderRadius: 2,
                                bgcolor: theme.palette.action.hover,
                            }}
                        />
                    </Grid>
                ))}
            </Grid>
        );
    }

    // Error state
    if (error) {
        return (
            <Alert 
                severity="error" 
                icon={<ErrorOutlineIcon fontSize="inherit" />}
                sx={{ borderRadius: 2 }}
            >
                {error}
            </Alert>
        );
    }

    // Empty state
    if (!categories || categories.length === 0) {
        return (
            <Box
                display="flex"
                flexDirection="column"
                alignItems="center"
                justifyContent="center"
                minHeight="200px"
                gap={2}
                sx={{ py: 4, textAlign: 'center' }}
            >
                <CategoryIcon 
                    sx={{ 
                        fontSize: 64, 
                        color: theme.palette.text.disabled 
                    }} 
                />
                <Typography variant="h6" color="text.secondary">
                    {t('noCategoriesFound')}
                </Typography>
            </Box>
        );
    }

    // Main content - compact grid
    return (
        <Grid container spacing={2}>
            {categories.map((category) => (
                <Grid 
                    item 
                    xs={12}
                    sm={6} 
                    md={4} 
                    lg={3}
                    key={category.id}
                >
                    <CategoryListItem category={category} />
                </Grid>
            ))}
        </Grid>
    );
});