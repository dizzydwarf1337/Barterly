import { useEffect } from 'react';
import { Box, Typography, Paper, Divider, CircularProgress } from '@mui/material';
import { observer } from 'mobx-react-lite';
import { useTranslation } from 'react-i18next';
import { useParams } from 'react-router'; // Correct import path for useParams

// Import your enums and models
import { PostType } from '../../../app/enums/postType';
import { PostCurrency } from '../../../app/enums/postCurrency';
import { PostPriceType } from '../../../app/enums/postPriceType';
import { ContractType } from '../../../app/enums/contractType';
import { WorkloadType } from '../../../app/enums/workloadType'; // Corrected enum name based on your PostStore
import { WorkLocationType } from '../../../app/enums/WorkLocationType';
import { RentObjectType } from '../../../app/enums/rentObjectType';
import { PostPromotionType } from '../../../app/enums/postPromotionType';
import useStore from '../../../app/stores/store'; // Your main store hook

// Icons
import LocationOnIcon from '@mui/icons-material/LocationOn';
import MoneyIcon from '@mui/icons-material/Money';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import HomeIcon from '@mui/icons-material/Home';
import EventNoteIcon from '@mui/icons-material/EventNote';
import WorkIcon from '@mui/icons-material/Work';
import MeetingRoomIcon from '@mui/icons-material/MeetingRoom'; // Icon for rooms
import ApartmentIcon from '@mui/icons-material/Apartment'; // Icon for floor/building

// New: Import the custom image carousel
import PostImageCarousel from './PostImageCarousel'; // Make sure this path is correct

export default observer(function PostDetails() {
    const { postId } = useParams<{ postId: string }>();
    const { postStore, uiStore } = useStore();
    const { currentPost, loadingCurrentPost, loadPost, clearCurrentPost } = postStore;
    const { t } = useTranslation();

    useEffect(() => {
        if (postId) {
            loadPost(postId);
        }
        return () => {
            clearCurrentPost();
        };
    }, [postId, loadPost, clearCurrentPost]);

    // Render loading state
    if (loadingCurrentPost) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" height="calc(100vh - 64px)">
                <CircularProgress size={60} />
            </Box>
        );
    }

    // Render error/not found state
    if (!currentPost) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" height="calc(100vh - 64px)">
                <Typography variant="h6" color="error">
                    {t('postNotFoundOrError')}
                </Typography>
            </Box>
        );
    }

    // Helper function to render price/salary
    const renderPriceAndType = () => {
        const currencySymbol = currentPost.currency ? PostCurrency[currentPost.currency] : '';
        // Check if priceType is valid before translating
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
                <Box display="flex" alignItems="center" gap={1}>
                    <AccountBalanceWalletIcon color="success" />
                    <Typography variant="h6">
                        {salaryText} {currencySymbol} {priceTypeTranslation ? `/ ${priceTypeTranslation}` : ''}
                    </Typography>
                </Box>
            );
        } else if (currentPost.postType === PostType.Rent) {
            return (
                <Box display="flex" alignItems="center" gap={1}>
                    <MoneyIcon color="success" />
                    <Typography variant="h6">
                        {currentPost.price != null ? `${currentPost.price} ${currencySymbol}` : t('negotiable')}
                        {priceTypeTranslation ? ` / ${priceTypeTranslation}` : ''}
                    </Typography>
                </Box>
            );
        } else { // For Common PostType
            return (
                <Box display="flex" alignItems="center" gap={1}>
                    {currentPost.price != null && currentPost.price > 0 && currentPost.priceType !== PostPriceType.Free ? (
                        <>
                            <MoneyIcon color="success" />
                            <Typography variant="h6">
                                {currentPost.price} {currencySymbol} {priceTypeTranslation ? `/ ${priceTypeTranslation}` : ''}
                            </Typography>
                        </>
                    ) : (
                        <Typography variant="h6" color="success">{t('Free')}</Typography>
                    )}
                </Box>
            );
        }
    };

    return (
        <Box sx={{
            padding: uiStore.isMobile ? '0px' : '0px', // No padding on outer box for full width
            width: '100%', // Take full width
            minHeight: '100vh', // Ensure it takes full viewport height
            display: 'flex',
            flexDirection: 'column',
            backgroundColor: 'background.default', // Use default background for the page
        }}>
            {/* Image Carousel Section */}
            {currentPost.mainImageUrl && (
                <Box sx={{
                    width: '100%',
                    bgcolor: 'background.paper', // A subtle background for the image area
                    py: 3, // Vertical padding
                }}>
                    <PostImageCarousel
                        mainImageUrl={currentPost.mainImageUrl}
                        // secondaryImageUrls={currentPost.secondaryImageUrls} // Pass secondary images when available
                        title={currentPost.title || 'Post Image'}
                    />
                </Box>
            )}

            {/* Main Content Area (Paper component) */}
            <Paper elevation={3} sx={{
                padding: { xs: '15px', md: '30px' }, // Responsive padding
                borderRadius: { xs: '0', md: '10px' }, // No border radius on mobile
                maxWidth: '1200px', // Max width for content
                margin: { xs: '0', md: '20px auto' }, // Center on desktop
                flexGrow: 1, // Allow content to grow
                backgroundColor: 'background.paper',
            }}>
                {/* Title and Promotion Badge */}
                <Box display="flex" justifyContent="space-between" alignItems="center" mb={2} flexDirection={{ xs: 'column', sm: 'row' }} gap={1}>
                    <Typography variant="h4" component="h1" sx={{ textAlign: { xs: 'center', sm: 'left' } }}>
                        {currentPost.title}
                    </Typography>
                    {currentPost.postPromotionType !== null && currentPost.postPromotionType !== PostPromotionType.None && (
                        <Box sx={{
                            backgroundColor: "secondary.main",
                            color: "secondary.contrastText",
                            padding: '4px 10px',
                            borderRadius: '5px',
                            minWidth: 'fit-content' // Prevent shrinking
                        }}>
                            <Typography variant="subtitle1" fontWeight="bold" textAlign="center">
                                {t(`promotion.${PostPromotionType[currentPost.postPromotionType]}`)} {/* Use PostPromotionType enum directly */}
                            </Typography>
                        </Box>
                    )}
                </Box>

                <Divider sx={{ mb: 3 }} />

                {/* Main Details Grid/Layout */}
                <Box display="grid" gridTemplateColumns={{ xs: '1fr', md: '1fr 1fr' }} gap={3}>
                    {/* Left Column: Location, Description, Specifics */}
                    <Box>
                        {/* Location */}
                        <Box display="flex" alignItems="center" gap={1} mb={1}>
                            <LocationOnIcon color="primary" />
                            <Typography variant="h6">
                                {currentPost.city}
                                {currentPost.street && `, ${currentPost.street}`}
                                {currentPost.houseNumber && ` ${currentPost.houseNumber}`}
                                {currentPost.workLocation && ` - ${t(WorkLocationType[currentPost.workLocation])}`}
                            </Typography>
                        </Box>

                        <Typography variant="body1" sx={{ whiteSpace: 'pre-wrap', mb: 2 }}>
                            {currentPost.shortDescription}
                        </Typography>

                        {/* Work Specific Details */}
                        {currentPost.postType === PostType.Work && (
                            <Box sx={{ mt: 2 }}>
                                <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                    <WorkIcon color="action" fontSize="small" />
                                    {currentPost.experienceRequired ? t('ExperienceRequired') : t('ExperienceNotRequired')}
                                </Typography>
                                {currentPost.contract && (
                                    <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                        <EventNoteIcon color="action" fontSize="small" />
                                        {t(ContractType[currentPost.contract])}
                                    </Typography>
                                )}
                                {currentPost.workload && (
                                    <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                        <EventNoteIcon color="action" fontSize="small" />
                                        {t(WorkloadType[currentPost.workload])}
                                    </Typography>
                                )}
                            </Box>
                        )}

                        {/* Rent Specific Details */}
                        {currentPost.postType === PostType.Rent && (
                            <Box sx={{ mt: 2 }}>
                                {currentPost.rentObjectType && (
                                    <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                        <HomeIcon color="action" fontSize="small" />
                                        {t(RentObjectType[currentPost.rentObjectType])}
                                    </Typography>
                                )}
                                {currentPost.area && (
                                    <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                        <Box component="span" sx={{ display: 'flex', alignItems: 'center', mr: 0.5 }}>
                                            {/* Adjusted SVG icon for area */}
                                            <svg xmlns="http://www.w3.org/2000/svg" height="20px" viewBox="0 0 24 24" width="20px" fill="currentColor">
                                                <path d="M0 0h24v24H0V0z" fill="none" /><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 15h2v-6h-2v6zm0-8h2V7h-2v2z" />
                                            </svg>
                                        </Box>
                                        {currentPost.area} m<sup>2</sup>
                                    </Typography>
                                )}
                                {currentPost.numberOfRooms && (
                                    <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                        <MeetingRoomIcon color="action" fontSize="small" />
                                        {t('rooms')}: {currentPost.numberOfRooms}
                                    </Typography>
                                )}
                                {currentPost.floor && (
                                    <Typography variant="subtitle1" display="flex" alignItems="center" gap={1}>
                                        <ApartmentIcon color="action" fontSize="small" />
                                        {t('floor')}: {currentPost.floor}
                                    </Typography>
                                )}
                            </Box>
                        )}
                    </Box>

                    {/* Right Column: Price/Salary, Contact Info */}
                    <Box display="flex" flexDirection="column" alignItems={uiStore.isMobile ? 'flex-start' : 'flex-end'} justifyContent="space-between">
                        {renderPriceAndType()}
                        <Box sx={{ mt: 3, textAlign: uiStore.isMobile ? 'left' : 'right' }}>
                            <Typography variant="h6">{t('contactInfo')}</Typography>
                            {/* In a real app, fetch owner details from a separate endpoint or include in PostPreview */}
                            <Typography variant="body1">Owner ID: {currentPost.ownerId}</Typography>
                            <Typography variant="body2">Posted: {new Date().toLocaleDateString()}</Typography>
                        </Box>
                    </Box>
                </Box>
            </Paper>
        </Box>
    );
});