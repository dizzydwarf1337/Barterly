import { useEffect, useState } from 'react';
import {
    Box, 
    Typography, 
    Skeleton, 
    Alert,
    Chip,
    Button,
    Avatar,
    Card,
    CardContent,
    alpha,
    Tooltip,
    IconButton,
    Fade
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
import WorkIcon from '@mui/icons-material/Work';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import VisibilityIcon from '@mui/icons-material/Visibility';
import PersonIcon from '@mui/icons-material/Person';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import SquareFootIcon from '@mui/icons-material/SquareFoot';
import BusinessCenterIcon from '@mui/icons-material/Business';
import MessageIcon from '@mui/icons-material/Message';
import ShareIcon from '@mui/icons-material/Share';
import FavoriteIcon from '@mui/icons-material/Favorite';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import ApartmentIcon from '@mui/icons-material/Apartment';
import MeetingRoomIcon from '@mui/icons-material/MeetingRoom';

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
import { PostOwner } from '../../users/types/userTypes';
import userApi from '../../users/api/userApi';

export default observer(function PostDetails() {
    const { postId } = useParams<{ postId: string }>();
    const { uiStore } = useStore();
    const { t } = useTranslation();
    const navigate = useNavigate();
    
    const [owner, setOwner] = useState<PostOwner | null>(null);
    const [currentPost, setCurrentPost] = useState<PostDetails | null>(null);
    const [loading, setLoading] = useState(true);
    const [ownerLoading, setOwnerLoading] = useState(false);
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

    useEffect(() => {
        const fetchOwner = async () => {
            if (!currentPost) return;

            try {
                setOwnerLoading(true);
                const response = await userApi.getPostOwner({ id: currentPost.ownerId });
                setOwner(response.value);
            } catch (err) {
                console.error('Failed to fetch post owner:', err);
            } finally {
                setOwnerLoading(false);
            }
        };

        fetchOwner();
    }, [currentPost]);

    const formatDate = (dateString: string) => {
        try {
            return new Date(dateString).toLocaleDateString(uiStore.lang === 'pl' ? 'pl-PL' : 'en-US', {
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
            currentPost.street,
            currentPost.houseNumber,
            currentPost.buildingNumber,
            currentPost.city
        ].filter(Boolean);
        
        return parts.join(', ');
    };

    const renderPriceSection = () => {
        if (!currentPost) return null;

        const currencySymbol = currentPost.currency ? PostCurrency[currentPost.currency] : '';
        const priceTypeTranslation = currentPost.priceType != null && PostPriceType[currentPost.priceType]
            ? t(PostPriceType[currentPost.priceType])
            : '';

        let priceDisplay = '';
        let IconComponent = MoneyIcon;
        let color: 'success' | 'primary' = 'success';
        let title = t('price');

        if (currentPost.postType === PostType.Work) {
            IconComponent = AccountBalanceWalletIcon;
            title = t('salary');
            if (currentPost.minSalary && currentPost.maxSalary) {
                priceDisplay = `${currentPost.minSalary} - ${currentPost.maxSalary} ${currencySymbol}`;
            } else if (currentPost.minSalary) {
                priceDisplay = `${t('from')} ${currentPost.minSalary} ${currencySymbol}`;
            } else if (currentPost.maxSalary) {
                priceDisplay = `${t('upTo')} ${currentPost.maxSalary} ${currencySymbol}`;
            } else {
                priceDisplay = t('negotiable');
            }
        } else if (currentPost.price != null && currentPost.price > 0) {
            priceDisplay = `${currentPost.price} ${currencySymbol}`;
        } else {
            priceDisplay = t('free');
            color = 'primary';
        }

        return (
            <Card 
                elevation={3}
                sx={{ 
                    background: (theme) => `linear-gradient(135deg, ${alpha(theme.palette[color].main, 0.1)} 0%, ${alpha(theme.palette[color].main, 0.05)} 100%)`,
                    border: `2px solid ${alpha(color === 'success' ? '#4CAF50' : '#2196F3', 0.2)}`,
                    borderRadius: '16px'
                }}
            >
                <CardContent sx={{ textAlign: 'center', py: 3 }}>
                    <Typography 
                        variant="h6" 
                        sx={{ 
                            color: `${color}.main`,
                            fontWeight: 700,
                            mb: 1
                        }}
                    >
                        {title}
                    </Typography>
                    <IconComponent 
                        color={color} 
                        sx={{ fontSize: 48, mb: 2 }}
                    />
                    <Typography 
                        variant="h4" 
                        sx={{ 
                            fontWeight: 800,
                            color: `${color}.main`,
                            mb: 1
                        }}
                    >
                        {priceDisplay}
                    </Typography>
                    {priceTypeTranslation && (
                        <Typography 
                            variant="body2" 
                            color="text.secondary"
                            sx={{ fontWeight: 600 }}
                        >
                            {priceTypeTranslation}
                        </Typography>
                    )}
                </CardContent>
            </Card>
        );
    };

    const renderOwnerCard = () => {
        if (ownerLoading) {
            return (
                <Card elevation={3} sx={{ borderRadius: '16px' }}>
                    <CardContent>
                        <Box display="flex" alignItems="center" gap={2} mb={2}>
                            <Skeleton variant="circular" width={60} height={60} />
                            <Box flex={1}>
                                <Skeleton variant="text" width="70%" height={24} />
                                <Skeleton variant="text" width="50%" height={20} />
                            </Box>
                        </Box>
                        <Skeleton variant="rectangular" height={48} sx={{ borderRadius: '12px' }} />
                    </CardContent>
                </Card>
            );
        }

        if (!owner) {
            return (
                <Card elevation={3} sx={{ borderRadius: '16px' }}>
                    <CardContent>
                        <Box display="flex" alignItems="center" gap={2} mb={2}>
                            <Avatar sx={{ width: 60, height: 60, bgcolor: 'primary.main' }}>
                                <PersonIcon fontSize="large" />
                            </Avatar>
                            <Box flex={1}>
                                <Typography variant="h6">{t('anonymous')}</Typography>
                                <Typography variant="body2" color="text.secondary">
                                    {t('userNotAvailable')}
                                </Typography>
                            </Box>
                        </Box>
                    </CardContent>
                </Card>
            );
        }

        return (
            <Card 
                elevation={3} 
                sx={{ 
                    borderRadius: '16px',
                    background: (theme) => `linear-gradient(135deg, ${alpha(theme.palette.primary.main, 0.02)} 0%, ${alpha(theme.palette.secondary.main, 0.02)} 100%)`,
                }}
            >
                <CardContent>
                    <Typography variant="h6" gutterBottom sx={{ color: 'primary.main', fontWeight: 700 }}>
                        {t('postOwner')}
                    </Typography>
                    
                    <Box display="flex" alignItems="center" gap={2} mb={3}>
                        <Avatar 
                            src={owner.profilePicturePath ? `${import.meta.env.VITE_API_URL.replace("api", "")}${owner.profilePicturePath}` : undefined}
                            sx={{ 
                                width: 60, 
                                height: 60, 
                                bgcolor: 'primary.main',
                                fontSize: '1.5rem',
                                fontWeight: 700
                            }}
                        >
                            {!owner.profilePicturePath && `${owner.firstName.charAt(0)}${owner.lastName.charAt(0)}`}
                        </Avatar>
                        <Box flex={1}>
                            <Typography variant="h6" sx={{ fontWeight: 700 }}>
                                {owner.firstName} {owner.lastName}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                {t('memberSince')} {formatDate(currentPost?.createdAt || '')}
                            </Typography>
                        </Box>
                    </Box>

                    <Button
                        variant="contained"
                        fullWidth
                        startIcon={<MessageIcon />}
                        onClick={() => {
                            // TODO: Implement messaging functionality
                            console.log('Contact owner:', owner.id);
                        }}
                        sx={{
                            borderRadius: '12px',
                            py: 1.5,
                            fontWeight: 700,
                            textTransform: 'none',
                            background: (theme) => `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                            '&:hover': {
                                background: (theme) => `linear-gradient(135deg, ${theme.palette.primary.dark} 0%, ${theme.palette.secondary.dark} 100%)`,
                                transform: 'translateY(-2px)',
                                boxShadow: (theme) => `0 8px 25px ${alpha(theme.palette.primary.main, 0.4)}`,
                            },
                            transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)'
                        }}
                    >
                        {t('contactOwner')}
                    </Button>
                </CardContent>
            </Card>
        );
    };

    const renderPropertyDetails = () => {
        if (!currentPost || currentPost.postType !== PostType.Rent) return null;

        return (
            <Card elevation={2} sx={{ borderRadius: '16px', mb: 3 }}>
                <CardContent>
                    <Typography variant="h6" gutterBottom sx={{ color: 'primary.main', fontWeight: 700 }}>
                        {t('propertyDetails')}
                    </Typography>
                    
                    <Box display="grid" gridTemplateColumns="repeat(auto-fit, minmax(200px, 1fr))" gap={2}>
                        {currentPost.rentObjectType != null && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <HomeIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('propertyType')}:</strong> {t(RentObjectType[currentPost.rentObjectType])}
                                </Typography>
                            </Box>
                        )}
                        
                        {currentPost.numberOfRooms && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <MeetingRoomIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('rooms')}:</strong> {currentPost.numberOfRooms}
                                </Typography>
                            </Box>
                        )}
                        
                        {currentPost.area && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <SquareFootIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('area')}:</strong> {currentPost.area} m²
                                </Typography>
                            </Box>
                        )}
                        
                        {currentPost.floor && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <ApartmentIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('floor')}:</strong> {currentPost.floor}
                                </Typography>
                            </Box>
                        )}
                    </Box>
                </CardContent>
            </Card>
        );
    };

    const renderWorkDetails = () => {
        if (!currentPost || currentPost.postType !== PostType.Work) return null;

        return (
            <Card elevation={2} sx={{ borderRadius: '16px', mb: 3 }}>
                <CardContent>
                    <Typography variant="h6" gutterBottom sx={{ color: 'primary.main', fontWeight: 700 }}>
                        {t('workDetails')}
                    </Typography>
                    
                    <Box display="grid" gridTemplateColumns="repeat(auto-fit, minmax(200px, 1fr))" gap={2}>
                        {currentPost.workload != null && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <BusinessCenterIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('workload')}:</strong> {t(WorkloadType[currentPost.workload])}
                                </Typography>
                            </Box>
                        )}
                        
                        {currentPost.workLocation != null && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <LocationOnIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('workLocation')}:</strong> {t(WorkLocationType[currentPost.workLocation])}
                                </Typography>
                            </Box>
                        )}
                        
                        {currentPost.contract != null && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <WorkIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('contractType')}:</strong> {t(ContractType[currentPost.contract])}
                                </Typography>
                            </Box>
                        )}
                        
                        {currentPost.experienceRequired != null && (
                            <Box display="flex" alignItems="center" gap={1}>
                                <BusinessCenterIcon color="primary" />
                                <Typography variant="body2">
                                    <strong>{t('experience')}:</strong> {currentPost.experienceRequired ? t('ExperienceRequired') : t('ExperienceNotRequired')}
                                </Typography>
                            </Box>
                        )}
                    </Box>
                </CardContent>
            </Card>
        );
    };

    if (loading) {
        return (
            <Box p={3}>
                <Skeleton variant="text" width={200} height={40} sx={{ mb: 2 }} />
                <Skeleton variant="rectangular" height={300} sx={{ mb: 3, borderRadius: '16px' }} />
                <Box display="grid" gridTemplateColumns={{ xs: '1fr', md: '2fr 1fr' }} gap={3}>
                    <Box>
                        <Skeleton variant="rectangular" height={200} sx={{ mb: 2, borderRadius: '16px' }} />
                        <Skeleton variant="rectangular" height={150} sx={{ borderRadius: '16px' }} />
                    </Box>
                    <Skeleton variant="rectangular" height={200} sx={{ borderRadius: '16px' }} />
                </Box>
            </Box>
        );
    }

    if (error || !currentPost) {
        return (
            <Box p={3}>
                <Alert severity="error" sx={{ borderRadius: '12px' }}>
                    {error || t('postNotFound')}
                </Alert>
            </Box>
        );
    }

    const isPromoted = currentPost.postPromotionType && PostPromotionType[currentPost.postPromotionType] !== PostPromotionType.None;

    return (
        <Fade in timeout={600}>
            <Box sx={{ maxWidth: 1200, mx: 'auto', p: { xs: 2, md: 3 } }}>
                {/* Back Button */}
                <Box mb={3}>
                    <Button
                        startIcon={<ArrowBackIcon />}
                        onClick={() => navigate(-1)}
                        sx={{ 
                            color: 'text.secondary',
                            textTransform: 'none',
                            '&:hover': { backgroundColor: alpha('#000', 0.04) }
                        }}
                    >
                        {t('goBack')}
                    </Button>
                </Box>

                {/* Images */}
                {currentPost.postImages && currentPost.postImages.length > 0 && (
                    <Box mb={4}>
                        <PostImageCarousel
                            mainImageUrl={currentPost.postImages[0].imageUrl!}
                            secondaryImageUrls={currentPost.postImages.slice(1).map(img => img.imageUrl!)}
                            title={currentPost.title}
                        />
                    </Box>
                )}

                {/* Main Content Grid */}
                <Box display="grid" gridTemplateColumns={{ xs: '1fr', md: '2fr 1fr' }} gap={4}>
                    {/* Left Column - Main Content */}
                    <Box>
                        {/* Header Card */}
                        <Card elevation={3} sx={{ borderRadius: '20px', mb: 3, overflow: 'hidden' }}>
                            {isPromoted && (
                                <Box
                                    sx={{
                                        height: '4px',
                                        background: (theme) => 
                                            `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                                    }}
                                />
                            )}
                            
                            <CardContent sx={{ p: 4 }}>
                                {/* Category and Promotion Chips */}
                                <Box display="flex" flexWrap="wrap" gap={1} mb={2}>
                                    {currentPost.subCategory && (
                                        <Chip
                                            label={uiStore.lang === "pl" ? currentPost.subCategory.titlePL : currentPost.subCategory.titleEN}
                                            color="primary"
                                            variant="outlined"
                                            size="small"
                                        />
                                    )}
                                    
                                    {isPromoted && (
                                        <Chip
                                            icon={<TrendingUpIcon />}
                                            label={currentPost.postPromotionType === PostPromotionType.Highlight 
                                                ? t('promotion.Highlight') 
                                                : t('promotion.Top')
                                            }
                                            color="secondary"
                                            size="small"
                                            sx={{ fontWeight: 700 }}
                                        />
                                    )}
                                </Box>

                                {/* Title */}
                                <Typography 
                                    variant="h3" 
                                    component="h1" 
                                    gutterBottom
                                    sx={{ 
                                        fontWeight: 800,
                                        lineHeight: 1.2,
                                        background: (theme) => 
                                            `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.text.primary} 100%)`,
                                        backgroundClip: 'text',
                                        WebkitBackgroundClip: 'text',
                                        WebkitTextFillColor: 'transparent',
                                    }}
                                >
                                    {currentPost.title}
                                </Typography>

                                {/* Location and Meta */}
                                <Box display="flex" flexWrap="wrap" alignItems="center" gap={3} mb={3}>
                                    {formatAddress() && (
                                        <Box display="flex" alignItems="center" gap={1}>
                                            <LocationOnIcon color="primary" />
                                            <Typography variant="body1" color="text.secondary">
                                                {formatAddress()}
                                            </Typography>
                                        </Box>
                                    )}
                                    
                                    <Box display="flex" alignItems="center" gap={1}>
                                        <CalendarTodayIcon color="primary" />
                                        <Typography variant="body2" color="text.secondary">
                                            {formatDate(currentPost.createdAt)}
                                        </Typography>
                                    </Box>
                                    
                                    <Box display="flex" alignItems="center" gap={1}>
                                        <VisibilityIcon color="primary" />
                                        <Typography variant="body2" color="text.secondary">
                                            {currentPost.viewsCount || 0} {t('views')}
                                        </Typography>
                                    </Box>
                                </Box>

                                {/* Action Buttons */}
                                <Box display="flex" gap={2}>
                                    <Tooltip title={t('share')}>
                                        <IconButton 
                                            sx={{ 
                                                bgcolor: alpha('#000', 0.04),
                                                '&:hover': { 
                                                    bgcolor: alpha('#000', 0.08),
                                                    transform: 'scale(1.1)'
                                                }
                                            }}
                                        >
                                            <ShareIcon />
                                        </IconButton>
                                    </Tooltip>
                                    
                                    <Tooltip title={t('addToFavorites')}>
                                        <IconButton 
                                            sx={{ 
                                                bgcolor: alpha('#000', 0.04),
                                                '&:hover': { 
                                                    bgcolor: alpha('#000', 0.08),
                                                    transform: 'scale(1.1)'
                                                }
                                            }}
                                        >
                                            <FavoriteIcon />
                                        </IconButton>
                                    </Tooltip>
                                </Box>
                            </CardContent>
                        </Card>

                        {/* Property/Work Details */}
                        {renderPropertyDetails()}
                        {renderWorkDetails()}

                        {/* Description Card */}
                        <Card elevation={2} sx={{ borderRadius: '16px' }}>
                            <CardContent>
                                <Typography variant="h6" gutterBottom sx={{ color: 'primary.main', fontWeight: 700 }}>
                                    {t('description')}
                                </Typography>
                                <Typography 
                                    variant="body1" 
                                    sx={{ 
                                        lineHeight: 1.7,
                                        whiteSpace: 'pre-wrap'
                                    }}
                                >
                                    {currentPost.fullDescription}
                                </Typography>
                                
                                {/* Tags */}
                                {currentPost.tags && currentPost.tags.length > 0 && (
                                    <Box mt={3}>
                                        <Typography variant="subtitle2" gutterBottom sx={{ fontWeight: 700 }}>
                                            {t('tags')}:
                                        </Typography>
                                        <Box display="flex" flexWrap="wrap" gap={1}>
                                            {currentPost.tags.map((tag, index) => (
                                                <Chip
                                                    key={index}
                                                    label={`#${tag}`}
                                                    size="small"
                                                    color="primary"
                                                    variant="filled"
                                                    sx={{ 
                                                        borderRadius: '16px',
                                                        fontWeight: 600,
                                                        fontSize: '0.75rem'
                                                    }}
                                                />
                                            ))}
                                        </Box>
                                    </Box>
                                )}
                            </CardContent>
                        </Card>
                    </Box>

                    {/* Right Column - Sidebar */}
                    <Box>
                        {/* Owner Card */}
                        <Box mb={3}>
                            {renderOwnerCard()}
                        </Box>

                        {/* Price Card */}
                        {renderPriceSection()}
                    </Box>
                </Box>
            </Box>
        </Fade>
    );
});