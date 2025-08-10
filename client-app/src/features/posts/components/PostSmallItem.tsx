import { Box, Typography, Chip, alpha } from "@mui/material";
import MoneyIcon from '@mui/icons-material/Money';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import ScheduleIcon from '@mui/icons-material/Schedule';
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import { PostCurrency, PostPreview, PostPriceType, PostType, PostPromotionType } from "../types/postTypes";

interface Props {
    post: PostPreview;
}

export default function PostSmallItem({ post }: Props) {
    const { t } = useTranslation();
    const navigate = useNavigate();

    const renderPriceOrSalary = () => {
        const currencySymbol = post.currency ? PostCurrency[post.currency] : '';
        const IconComponent = post.postType === PostType.Work ? AccountBalanceWalletIcon : MoneyIcon;
        
        if (post.postType === PostType.Work) {
            let salaryText = '';
            if (post.minSalary && post.maxSalary) {
                salaryText = `${post.minSalary} - ${post.maxSalary}`;
            } else if (post.minSalary) {
                salaryText = `${t("from")} ${post.minSalary}`;
            } else if (post.maxSalary) {
                salaryText = `${t("upTo")} ${post.maxSalary}`;
            } else {
                salaryText = t("negotiable");
            }
            return (
                <Box display="flex" alignItems="center" gap={0.5}>
                    <IconComponent 
                        fontSize="small" 
                        color="success"
                        sx={{ fontSize: 16 }}
                    />
                    <Typography 
                        variant="body2" 
                        color="success.main"
                        sx={{ fontWeight: 600, fontSize: '0.75rem' }}
                    >
                        {salaryText} {currencySymbol}
                    </Typography>
                </Box>
            );
        } else if (post.postType === PostType.Rent) {
            const priceText = post.price != null ? `${post.price} ${currencySymbol}` : t('negotiable');
            return (
                <Box display="flex" alignItems="center" gap={0.5}>
                    <IconComponent 
                        fontSize="small" 
                        color="success"
                        sx={{ fontSize: 16 }}
                    />
                    <Typography 
                        variant="body2" 
                        color="success.main"
                        sx={{ fontWeight: 600, fontSize: '0.75rem' }}
                    >
                        {priceText}
                    </Typography>
                </Box>
            );
        } else {
            return (
                <Box display="flex" alignItems="center" gap={0.5}>
                    {post.price != null && post.price > 0 && post.priceType !== PostPriceType.Free ? (
                        <>
                            <IconComponent 
                                fontSize="small" 
                                color="success"
                                sx={{ fontSize: 16 }}
                            />
                            <Typography 
                                variant="body2" 
                                color="success.main"
                                sx={{ fontWeight: 600, fontSize: '0.75rem' }}
                            >
                                {post.price} {currencySymbol}
                            </Typography>
                        </>
                    ) : (
                        <Typography 
                            variant="body2" 
                            color="success.main"
                            sx={{ fontWeight: 600, fontSize: '0.75rem' }}
                        >
                            {t("Free")}
                        </Typography>
                    )}
                </Box>
            );
        }
    };

    const isPromoted = post.postPromotionType !== null && 
                     post.postPromotionType !== PostPromotionType.None;

    return (
        <Box
            onClick={() => navigate(`posts/${post.id}`)}
            sx={{
                display: 'flex',
                flexDirection: 'row',
                width: '100%',
                minHeight: 140,
                backgroundColor: 'background.paper',
                borderRadius: '16px',
                border: '1px solid',
                borderColor: isPromoted ? 'primary.main' : 'divider',
                cursor: 'pointer',
                position: 'relative',
                overflow: 'hidden',
                transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: (theme) => `0 8px 32px ${alpha(theme.palette.primary.main, 0.15)}`,
                    borderColor: 'primary.main',
                    '& .post-content': {
                        transform: 'translateX(2px)',
                    }
                },
                '&:active': {
                    transform: 'translateY(-2px)',
                    transition: 'all 0.1s ease'
                }
            }}
        >
            {/* Promotion Badge */}
            {isPromoted && (
                <Box
                    sx={{
                        position: 'absolute',
                        top: 8,
                        right: 8,
                        zIndex: 2,
                    }}
                >
                    <Chip
                        label={t(`promotion.${PostPromotionType[post.postPromotionType!]}`)}
                        size="small"
                        color="primary"
                        sx={{
                            fontSize: '0.65rem',
                            height: 20,
                            fontWeight: 700,
                            textTransform: 'uppercase',
                            letterSpacing: 0.5,
                            borderRadius: '10px'
                        }}
                    />
                </Box>
            )}

            {/* Content */}
            <Box
                className="post-content"
                display="flex"
                flexDirection="column"
                justifyContent="space-between"
                flex={1}
                p={2}
                sx={{
                    transition: 'transform 0.2s ease',
                    pr: isPromoted ? 5 : 2
                }}
            >
                {/* Header */}
                <Box>
                    <Typography
                        variant="subtitle2"
                        sx={{
                            fontWeight: 700,
                            fontSize: '1rem',
                            lineHeight: 1.2,
                            mb: 0.5,
                            display: '-webkit-box',
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: 'vertical',
                            overflow: 'hidden',
                            color: 'text.primary'
                        }}
                    >
                        {post.title}
                    </Typography>

                    {/* Location */}
                    <Box display="flex" alignItems="center" gap={0.5} mb={1}>
                        <LocationOnIcon 
                            sx={{ 
                                fontSize: 14, 
                                color: 'text.secondary',
                                opacity: 0.7 
                            }} 
                        />
                        <Typography
                            variant="caption"
                            color="text.secondary"
                            sx={{ fontSize: '0.7rem' }}
                        >
                            {post.city}
                        </Typography>
                    </Box>
                </Box>

                {/* Description */}
                <Box flex={1} display="flex" alignItems="center" my={1}>
                    <Typography
                        variant="caption"
                        color="text.secondary"
                        sx={{
                            display: '-webkit-box',
                            WebkitLineClamp: 2,
                            WebkitBoxOrient: 'vertical',
                            overflow: 'hidden',
                            fontSize: '0.75rem',
                            lineHeight: 1.3
                        }}
                    >
                        {post.shortDescription}
                    </Typography>
                </Box>

                {/* Footer */}
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    {renderPriceOrSalary()}
                    
                    <Box display="flex" alignItems="center" gap={0.5}>
                        <ScheduleIcon 
                            sx={{ 
                                fontSize: 12, 
                                color: 'text.secondary',
                                opacity: 0.6 
                            }} 
                        />
                    </Box>
                </Box>
            </Box>

            {/* Gradient Overlay for promoted posts */}
            {isPromoted && (
                <Box
                    sx={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        right: 0,
                        height: '2px',
                        background: (theme) => 
                            `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                    }}
                />
            )}
        </Box>
    );
}