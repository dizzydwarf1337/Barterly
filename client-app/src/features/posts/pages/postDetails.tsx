import { useEffect, useState } from 'react';
import {
    Box, 
    Divider, 
    Paper, 
    Typography, 
    Skeleton, 
    Alert,
    Chip,
    Stack,
    Card,
    CardContent,
    IconButton,
} from '@mui/material';
import { observer } from 'mobx-react-lite';
import { useTranslation } from 'react-i18next';
import { useParams, useNavigate } from 'react-router';
import useStore from '../../../app/stores/store';

// Icons
import LocationOnIcon from '@mui/icons-material/LocationOn';
import MoneyIcon from '@mui/icons-material/Money';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import HomeIcon from '@mui/icons-material/Home';
import EventNoteIcon from '@mui/icons-material/EventNote';
import WorkIcon from '@mui/icons-material/Work';
import MeetingRoomIcon from '@mui/icons-material/MeetingRoom';
import ApartmentIcon from '@mui/icons-material/Apartment';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import VisibilityIcon from '@mui/icons-material/Visibility';
import PersonIcon from '@mui/icons-material/Person';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import SquareFootIcon from '@mui/icons-material/SquareFoot';
import BusinessCenterIcon from '@mui/icons-material/BusinessCenter';
import PublicIcon from '@mui/icons-material/Public';

import PostImageCarousel from '../components/postImageCarousel';
import { 
    ContractType, 
    PostCurrency, 
    PostDetails, 
    PostPriceType, 
    PostPromotionType, 
    PostType, 
    RentObjectType, 
    WorkloadType, 
    WorkLocationType 
} from '../types/postTypes';
import postApi from '../api/postApi';

export default observer(function PostDetails() {
    const { postId } = useParams<{ postId: string }>();
    const { uiStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();
    
    const [currentPost, setCurrentPost] = useState<PostDetails | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchPost = async () => {
            if (!postId) {
                setError(t('postNotFound'));
                setLoading(false);
                return;
            }

            try {
                setLoading(true);
                setError(null);
                const response = await postApi.getPost({ postId });
                setCurrentPost(response.value);
            } catch (err) {
                console.error('Failed to fetch post:', err);
                setError(t('failedToLoadPost'));
            } finally {
                setLoading(false);
            }
        };

        fetchPost();
    }, [postId, t]);

    const formatDate = (dateString: string) => {
        try {
            return new Date(dateString).toLocaleDateString(undefined, {
                year: 'numeric',
                month: 'long',
                day: 'numeric'
            });
        } catch {
            return dateString;
        }
    };

    const formatAddress = () => {
        if (!currentPost) return '';
        
        const parts = [
            currentPost.city,
            currentPost.street,
            currentPost.houseNumber,
            currentPost.buildingNumber
        ].filter(Boolean);
        
        return parts.join(', ');
    };

    const renderPriceSection = () => {
        if (!currentPost) return null;

        const currencySymbol = currentPost.currency ? PostCurrency[currentPost.currency] : '';
        const priceTypeTranslation = currentPost.priceType != null && PostPriceType[currentPost.priceType]
            ? t(PostPriceType[currentPost.priceType])
            : '';

        if (currentPost.postType === PostType.Work) {
            let salaryText = '';
            if (currentPost.minSalary && currentPost.maxSalary) {
                salaryText = `${currentPost.minSalary} - ${currentPost.maxSalary}`;
            } else if (currentPost.minSalary) {
                salaryText = `${t('from')} ${currentPost.minSalary}`;
            } else if (currentPost.maxSalary) {
                salaryText = `${t('upTo')} ${currentPost.maxSalary}`;
            } else {
                salaryText = t('negotiable');
            }

            return (
                <Card elevation={2} sx={{ backgroundColor: 'success.light', color: 'success.contrastText' }}>
                    <CardContent>
                        <Box display="flex" alignItems="center" gap={1} mb={1}>
                            <AccountBalanceWalletIcon />
                            <Typography variant="h6" fontWeight="bold">
                                {t('salary')}
                            </Typography>
                        </Box>
                        <Typography variant="h5">
                            {salaryText} {currencySymbol}
                        </Typography>
                        {priceTypeTranslation && (
                            <Typography variant="body2">
                                {t('per')} {priceTypeTranslation}
                            </Typography>
                        )}
                    </CardContent>
                </Card>
            );
        } else if (currentPost.postType === PostType.Rent) {
            return (
                <Card elevation={2} sx={{ backgroundColor: 'primary.light', color: 'primary.contrastText' }}>
                    <CardContent>
                        <Box display="flex" alignItems="center" gap={1} mb={1}>
                            <MoneyIcon />
                            <Typography variant="h6" fontWeight="bold">
                                {t('rent')}
                            </Typography>
                        </Box>
                        <Typography variant="h5">
                            {currentPost.price != null ? `${currentPost.price} ${currencySymbol}` : t('negotiable')}
                        </Typography>
                        {priceTypeTranslation && (
                            <Typography variant="body2">
                                {t('per')} {priceTypeTranslation}
                            </Typography>
                        )}
                    </CardContent>
                </Card>
            );
        } else {
            return (
                <Card elevation={2} sx={{ 
                    backgroundColor: currentPost.price != null && currentPost.price > 0 && currentPost.priceType !== PostPriceType.Free 
                        ? 'warning.light' 
                        : 'success.light',
                    color: currentPost.price != null && currentPost.price > 0 && currentPost.priceType !== PostPriceType.Free 
                        ? 'warning.contrastText' 
                        : 'success.contrastText'
                }}>
                    <CardContent>
                        <Box display="flex" alignItems="center" gap={1} mb={1}>
                            <MoneyIcon />
                            <Typography variant="h6" fontWeight="bold">
                                {t('price')}
                            </Typography>
                        </Box>
                        {currentPost.price != null && currentPost.price > 0 && currentPost.priceType !== PostPriceType.Free ? (
                            <>
                                <Typography variant="h5">
                                    {currentPost.price} {currencySymbol}
                                </Typography>
                                {priceTypeTranslation && (
                                    <Typography variant="body2">
                                        {t('per')} {priceTypeTranslation}
                                    </Typography>
                                )}
                            </>
                        ) : (
                            <Typography variant="h5" fontWeight="bold">
                                {t('Free')}
                            </Typography>
                        )}
                    </CardContent>
                </Card>
            );
        }
    };

    const renderWorkDetails = () => {
        if (!currentPost || currentPost.postType !== PostType.Work) return null;

        return (
            <Card>
                <CardContent>
                    <Typography variant="h6" gutterBottom display="flex" alignItems="center" gap={1}>
                        <BusinessCenterIcon color="primary" />
                        {t('workDetails')}
                    </Typography>
                    <Stack spacing={1}>
                        <Box display="flex" alignItems="center" gap={1}>
                            <WorkIcon fontSize="small" color="action" />
                            <Typography>
                                {currentPost.experienceRequired ? t('ExperienceRequired') : t('ExperienceNotRequired')}
                            </Typography>
                        </Box>
                        {currentPost.contract && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <EventNoteIcon fontSize="small" color="action" />
                                <Typography>{t(ContractType[currentPost.contract])}</Typography>
                            </Box>
                        )}
                        {currentPost.workload && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <EventNoteIcon fontSize="small" color="action" />
                                <Typography>{t(WorkloadType[currentPost.workload])}</Typography>
                            </Box>
                        )}
                        {currentPost.workLocation && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <PublicIcon fontSize="small" color="action" />
                                <Typography>{t(WorkLocationType[currentPost.workLocation])}</Typography>
                            </Box>
                        )}
                    </Stack>
                </CardContent>
            </Card>
        );
    };

    const renderRentDetails = () => {
        if (!currentPost || currentPost.postType !== PostType.Rent) return null;

        return (
            <Card>
                <CardContent>
                    <Typography variant="h6" gutterBottom display="flex" alignItems="center" gap={1}>
                        <HomeIcon color="primary" />
                        {t('propertyDetails')}
                    </Typography>
                    <Stack spacing={1}>
                        {currentPost.rentObjectType && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <HomeIcon fontSize="small" color="action" />
                                <Typography>{t(RentObjectType[currentPost.rentObjectType])}</Typography>
                            </Box>
                        )}
                        {currentPost.area && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <SquareFootIcon fontSize="small" color="action" />
                                <Typography>{currentPost.area} m²</Typography>
                            </Box>
                        )}
                        {currentPost.numberOfRooms && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <MeetingRoomIcon fontSize="small" color="action" />
                                <Typography>{t('rooms')}: {currentPost.numberOfRooms}</Typography>
                            </Box>
                        )}
                        {currentPost.floor && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <ApartmentIcon fontSize="small" color="action" />
                                <Typography>{t('floor')}: {currentPost.floor}</Typography>
                            </Box>
                        )}
                    </Stack>
                </CardContent>
            </Card>
        );
    };

    if (loading) {
        return (
            <Box sx={{ padding: { xs: '15px', md: '30px' }, maxWidth: '1200px', margin: '0 auto' }}>
                <Skeleton variant="rectangular" width="100%" height={300} sx={{ mb: 3, borderRadius: 2 }} />
                <Skeleton variant="text" sx={{ fontSize: '2rem', mb: 2 }} />
                <Skeleton variant="text" width="60%" sx={{ mb: 2 }} />
                <Box display="grid" gridTemplateColumns={{ xs: '1fr', md: '1fr 1fr' }} gap={3}>
                    <Box>
                        <Skeleton variant="rectangular" height={200} sx={{ borderRadius: 2 }} />
                    </Box>
                    <Box>
                        <Skeleton variant="rectangular" height={150} sx={{ borderRadius: 2 }} />
                    </Box>
                </Box>
            </Box>
        );
    }

    if (error || !currentPost) {
        return (
            <Box sx={{ padding: { xs: '15px', md: '30px' }, maxWidth: '1200px', margin: '0 auto' }}>
                <Alert 
                    severity="error" 
                    action={
                        <IconButton color="inherit" size="small" onClick={() => navigate(-1)}>
                            <ArrowBackIcon />
                        </IconButton>
                    }
                >
                    {error || t('postNotFound')}
                </Alert>
            </Box>
        );
    }

    return (
        <Box sx={{
            padding: { xs: '0px', md: '0px' },
            width: '100%',
            minHeight: '100vh',
            display: 'flex',
            flexDirection: 'column',
            backgroundColor: 'background.default',
        }}>
            {/* Back Button */}
            <Box sx={{ position: 'absolute', top: 16, left: 16, zIndex: 10 }}>
                <IconButton
                    onClick={() => navigate(-1)}
                    sx={{
                        backgroundColor: 'rgba(255, 255, 255, 0.9)',
                        '&:hover': { backgroundColor: 'rgba(255, 255, 255, 1)' }
                    }}
                >
                    <ArrowBackIcon />
                </IconButton>
            </Box>

            {/* Image Carousel */}
            {(currentPost.mainImageUrl || currentPost.postImages?.length) && (
                <Box sx={{
                    width: '100%',
                    bgcolor: 'background.paper',
                    py: 3,
                }}>
                    <PostImageCarousel
                        mainImageUrl={currentPost.mainImageUrl!}
                        secondaryImageUrls={currentPost.postImages?.map(img => img.imageUrl!) || []}
                        title={currentPost.title || 'Post Image'}
                    />
                </Box>
            )}

            {/* Main Content */}
            <Paper elevation={3} sx={{
                padding: { xs: '15px', md: '30px' },
                borderRadius: { xs: '0', md: '10px' },
                maxWidth: '1200px',
                margin: { xs: '0', md: '20px auto' },
                flexGrow: 1,
                backgroundColor: 'background.paper',
            }}>
                {/* Header */}
                <Box mb={3}>
                    <Box display="flex" justifyContent="space-between" alignItems="flex-start" mb={2}
                         flexDirection={{ xs: 'column', sm: 'row' }} gap={2}>
                        <Box flex={1}>
                            <Typography variant="h3" component="h1" gutterBottom>
                                {currentPost.title}
                            </Typography>
                            
                            {/* Category */}
                            {currentPost.subCategory && (
                                <Chip 
                                    label={uiStore.lang === "PL" ? currentPost.subCategory.namePL : currentPost.subCategory.nameEN} 
                                    color="primary" 
                                    variant="outlined" 
                                    sx={{ mb: 2 }} 
                                />
                            )}
                        </Box>

                        {/* Promotion Badge */}
                        {currentPost.postPromotionType !== null && currentPost.postPromotionType !== PostPromotionType.None && (
                            <Chip
                                label={t(`promotion.${PostPromotionType[currentPost.postPromotionType]}`)}
                                color="secondary"
                                sx={{ 
                                    fontWeight: 'bold',
                                    fontSize: '0.9rem'
                                }}
                            />
                        )}
                    </Box>

                    {/* Location */}
                    <Box display="flex" alignItems="center" gap={1} mb={2}>
                        <LocationOnIcon color="primary" />
                        <Typography variant="h6" color="text.secondary">
                            {formatAddress()}
                        </Typography>
                    </Box>

                    {/* Stats */}
                    <Stack direction="row" spacing={3} sx={{ color: 'text.secondary' }}>
                        <Box display="flex" alignItems="center" gap={0.5}>
                            <CalendarTodayIcon fontSize="small" />
                            <Typography variant="body2">
                                {formatDate(currentPost.createdAt)}
                            </Typography>
                        </Box>
                        {currentPost.viewsCount && (
                            <Box display="flex" alignItems="center" gap={0.5}>
                                <VisibilityIcon fontSize="small" />
                                <Typography variant="body2">
                                    {currentPost.viewsCount} {t('views')}
                                </Typography>
                            </Box>
                        )}
                        <Box display="flex" alignItems="center" gap={0.5}>
                            <PersonIcon fontSize="small" />
                            <Typography variant="body2">
                                ID: {currentPost.ownerId}
                            </Typography>
                        </Box>
                    </Stack>
                </Box>

                <Divider sx={{ mb: 3 }} />

                {/* Content Grid */}
                <Box display="grid" gridTemplateColumns={{ xs: '1fr', lg: '2fr 1fr' }} gap={4}>
                    {/* Left Column */}
                    <Stack spacing={3}>
                        {/* Description */}
                        <Card>
                            <CardContent>
                                <Typography variant="h6" gutterBottom>
                                    {t('description')}
                                </Typography>
                                <Typography variant="body1" sx={{ whiteSpace: 'pre-wrap', lineHeight: 1.6 }}>
                                    {currentPost.fullDescription || currentPost.shortDescription}
                                </Typography>
                            </CardContent>
                        </Card>

                        {/* Tags */}
                        {currentPost.tags && currentPost.tags.length > 0 && (
                            <Card>
                                <CardContent>
                                    <Typography variant="h6" gutterBottom>
                                        {t('tags')}
                                    </Typography>
                                    <Stack direction="row" spacing={1} flexWrap="wrap">
                                        {currentPost.tags.map((tag, index) => (
                                            <Chip key={index} label={tag} variant="outlined" size="small" />
                                        ))}
                                    </Stack>
                                </CardContent>
                            </Card>
                        )}

                        {/* Type-specific Details */}
                        {renderWorkDetails()}
                        {renderRentDetails()}
                    </Stack>

                    {/* Right Column */}
                    <Stack spacing={3}>
                        {/* Price/Salary */}
                        {renderPriceSection()}

                        {/* Contact Info */}
                        <Card>
                            <CardContent>
                                <Typography variant="h6" gutterBottom>
                                    {t('contactInfo')}
                                </Typography>
                                <Stack spacing={1}>
                                    <Typography variant="body1">
                                        {t('ownerId')}: {currentPost.ownerId}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        {t('posted')}: {formatDate(currentPost.createdAt)}
                                    </Typography>
                                    {currentPost.updatedAt && (
                                        <Typography variant="body2" color="text.secondary">
                                            {t('updated')}: {formatDate(currentPost.updatedAt)}
                                        </Typography>
                                    )}
                                </Stack>
                            </CardContent>
                        </Card>
                    </Stack>
                </Box>
            </Paper>
        </Box>
    );
});