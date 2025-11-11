import { observer } from "mobx-react-lite";
import { 
    Typography, 
    Card, 
    CardContent, 
    Box,
    List,
    ListItem,
    ListItemButton,
    ListItemText,
    IconButton,
    useTheme
} from "@mui/material";
import { useNavigate } from "react-router";
import { useState } from "react";

// Icons
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';

import useStore from "../../../app/stores/store";
import { Category } from "../types/categoryTypes";
import { useTranslation } from "react-i18next";

interface Props {
    category: Category;
}

export default observer(function CategoryListItem({ category }: Props) {
    const { uiStore } = useStore();
    const theme = useTheme();
    const navigate = useNavigate();
    const [expanded, setExpanded] = useState(false);
    const {t} = useTranslation();
    const categoryName = uiStore.lang === "en" ? category.nameEN : category.namePL;
    const hasSubcategories = category.subCategories && category.subCategories.length > 0;

    const handleCategoryClick = () => {
        navigate(`/search?categoryId=${category.id}`);
    };

    const handleSubcategoryClick = (subcategoryId: string) => {
        navigate(`/search?subcategoryId=${subcategoryId}`);
    };

    const toggleExpanded = (e: React.MouseEvent) => {
        e.stopPropagation();
        setExpanded(!expanded);
    };

    return (
        <Box sx={{ position: 'relative' }}>
            <Card
                sx={{
                    cursor: "pointer",
                    transition: "all 0.2s ease",
                    border: `1px solid ${theme.palette.divider}`,
                    '&:hover': {
                        boxShadow: theme.shadows[2],
                        borderColor: theme.palette.primary.main,
                    },
                    zIndex: expanded ? 10 : 1,
                }}
            >
                <CardContent
                    onClick={handleCategoryClick}
                    sx={{
                        p: 2,
                        '&:last-child': { pb: 2 },
                    }}
                >
                    <Box
                        display="flex"
                        justifyContent="space-between"
                        alignItems="center"
                    >
                        <Typography
                            variant="subtitle1"
                            fontWeight="500"
                            color="text.primary"
                            sx={{
                                flex: 1,
                                overflow: "hidden",
                                textOverflow: "ellipsis",
                                whiteSpace: "nowrap",
                            }}
                        >
                            {categoryName}
                        </Typography>

                        {hasSubcategories && (
                            <IconButton
                                size="small"
                                onClick={toggleExpanded}
                                sx={{
                                    transition: "transform 0.2s ease",
                                    transform: expanded ? "rotate(180deg)" : "rotate(0deg)",
                                }}
                            >
                                <ExpandMoreIcon />
                            </IconButton>
                        )}
                    </Box>

                    {/* Subcategories count indicator */}
                    {hasSubcategories && (
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ mt: 0.5, display: 'block' }}
                        >
                            {category.subCategories.length} {category.subCategories.length === 1 ? t('subCategory') : t('subCategories')}
                        </Typography>
                    )}
                </CardContent>
            </Card>

            {/* Subcategories overlay */}
            {hasSubcategories && expanded && (
                <Card
                    sx={{
                        position: 'absolute',
                        top: '100%',
                        left: 0,
                        right: 0,
                        zIndex: 20,
                        mt: 0.5,
                        boxShadow: theme.shadows[8],
                        border: `1px solid ${theme.palette.primary.main}`,
                    }}
                >
                    <List dense sx={{ py: 0 }}>
                        {category.subCategories.map((subcategory) => (
                            <ListItem key={subcategory.id} disablePadding>
                                <ListItemButton
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        handleSubcategoryClick(subcategory.id);
                                    }}
                                    sx={{
                                        pl: 3,
                                        py: 1,
                                        '&:hover': {
                                            backgroundColor: theme.palette.action.hover,
                                        },
                                    }}
                                >
                                    <ChevronRightIcon 
                                        sx={{ 
                                            fontSize: 16, 
                                            color: theme.palette.text.secondary,
                                            mr: 1
                                        }} 
                                    />
                                    <ListItemText
                                        primary={
                                            <Typography
                                                variant="body2"
                                                color="text.secondary"
                                                sx={{
                                                    overflow: "hidden",
                                                    textOverflow: "ellipsis",
                                                    whiteSpace: "nowrap",
                                                }}
                                            >
                                                {uiStore.lang === "en" ? subcategory.nameEN : subcategory.namePL}
                                            </Typography>
                                        }
                                    />
                                </ListItemButton>
                            </ListItem>
                        ))}
                    </List>
                </Card>
            )}
        </Box>
    );
});