import { observer } from "mobx-react-lite";
import { 
    Typography, 
    Card, 
    CardContent, 
    Box,
    alpha,
    useTheme,
    Tooltip,
    Badge
} from "@mui/material";
import { useNavigate } from "react-router";
import { useState } from "react";

// Icons
import CategoryIcon from '@mui/icons-material/Category';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import SubdirectoryArrowRightIcon from '@mui/icons-material/SubdirectoryArrowRight';

import useStore from "../../../app/stores/store";
import { Category } from "../types/categoryTypes";

interface Props {
    category: Category;
}

export default observer(function CategoryListItem({ category }: Props) {
    const { uiStore } = useStore();
    const theme = useTheme();
    const navigate = useNavigate();
    const [isHovered, setIsHovered] = useState(false);

    const handleClick = () => {
        console.log("Navigate to category:", category.id);
        navigate(`/categories/${category.id}`);
    };

    const categoryName = uiStore.lang === "en" ? category.nameEN : category.namePL;
    const hasSubcategories = category.subCategories && category.subCategories.length > 0;
    const subcategoriesCount = category.subCategories?.length || 0;

    return (
        <Tooltip
            title={category.description || categoryName}
            placement="top"
            arrow
            enterDelay={500}
        >
            <Card
                onClick={handleClick}
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
                sx={{
                    minHeight: uiStore.isMobile ? "100px" : "120px",
                    cursor: "pointer",
                    transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                    position: "relative",
                    overflow: "visible",
                    background: `linear-gradient(135deg, ${alpha(
                        theme.palette.background.paper,
                        0.9
                    )}, ${alpha(theme.palette.background.default, 0.5)})`,
                    backdropFilter: "blur(10px)",
                    border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                    borderRadius: 3,
                    '&:hover': {
                        transform: 'translateY(-8px) scale(1.02)',
                        boxShadow: `0 12px 24px ${alpha(theme.palette.primary.main, 0.15)}`,
                        backgroundColor: alpha(theme.palette.primary.main, 0.05),
                        borderColor: alpha(theme.palette.primary.main, 0.3),
                    },
                    '&:active': {
                        transform: 'translateY(-4px) scale(1.01)',
                    },
                }}
            >
                {/* Subcategories badge */}
                {hasSubcategories && (
                    <Badge
                        badgeContent={subcategoriesCount}
                        color="secondary"
                        sx={{
                            position: "absolute",
                            top: 8,
                            right: 8,
                            zIndex: 1,
                            '& .MuiBadge-badge': {
                                fontSize: '0.7rem',
                                height: 18,
                                minWidth: 18,
                                borderRadius: '50%',
                            },
                        }}
                    />
                )}

                <CardContent
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        justifyContent: "center",
                        textAlign: "center",
                        height: "100%",
                        p: { xs: 1.5, sm: 2 },
                        position: "relative",
                        '&:last-child': {
                            pb: { xs: 1.5, sm: 2 },
                        },
                    }}
                >
                    {/* Category icon */}
                    <Box
                        sx={{
                            mb: 1,
                            p: 1,
                            borderRadius: "50%",
                            background: `linear-gradient(45deg, ${theme.palette.primary.main}, ${theme.palette.secondary.main})`,
                            transition: "all 0.3s ease",
                            transform: isHovered ? "scale(1.1)" : "scale(1)",
                        }}
                    >
                        <CategoryIcon 
                            sx={{ 
                                color: theme.palette.primary.contrastText,
                                fontSize: uiStore.isMobile ? 20 : 24,
                            }} 
                        />
                    </Box>

                    {/* Category name */}
                    <Typography
                        variant={uiStore.isMobile ? "body2" : "subtitle1"}
                        fontWeight="600"
                        color="text.primary"
                        sx={{
                            overflow: "hidden",
                            textOverflow: "ellipsis",
                            display: "-webkit-box",
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: "vertical",
                            lineHeight: 1.3,
                            mb: hasSubcategories ? 1 : 0,
                            transition: "color 0.3s ease",
                        }}
                    >
                        {categoryName}
                    </Typography>

                    {/* Subcategories indicator */}
                    {hasSubcategories && (
                        <Box
                            display="flex"
                            alignItems="center"
                            gap={0.5}
                            sx={{
                                opacity: isHovered ? 1 : 0.7,
                                transition: "opacity 0.3s ease",
                            }}
                        >
                            <SubdirectoryArrowRightIcon 
                                sx={{ 
                                    fontSize: 12, 
                                    color: theme.palette.text.secondary 
                                }} 
                            />
                            <Typography
                                variant="caption"
                                color="text.secondary"
                                sx={{ fontSize: '0.7rem' }}
                            >
                                {subcategoriesCount} {subcategoriesCount === 1 ? 'subcat' : 'subcats'}
                            </Typography>
                        </Box>
                    )}

                    {/* Hover arrow indicator */}
                    <Box
                        sx={{
                            position: "absolute",
                            top: 8,
                            left: 8,
                            opacity: isHovered ? 1 : 0,
                            transform: isHovered ? "translateX(0)" : "translateX(-10px)",
                            transition: "all 0.3s ease",
                        }}
                    >
                        <ArrowForwardIosIcon
                            sx={{
                                fontSize: 14,
                                color: theme.palette.primary.main,
                            }}
                        />
                    </Box>
                </CardContent>

                {/* Bottom accent line */}
                <Box
                    sx={{
                        position: "absolute",
                        bottom: 0,
                        left: 0,
                        right: 0,
                        height: 3,
                        background: `linear-gradient(90deg, ${theme.palette.primary.main}, ${theme.palette.secondary.main})`,
                        transform: isHovered ? "scaleX(1)" : "scaleX(0)",
                        transformOrigin: "center",
                        transition: "transform 0.3s ease",
                        borderRadius: "0 0 12px 12px",
                    }}
                />
            </Card>
        </Tooltip>
    );
});