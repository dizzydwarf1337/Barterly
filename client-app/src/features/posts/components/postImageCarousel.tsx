import { useState } from 'react';
import { Box, IconButton, alpha, Skeleton, Tooltip } from '@mui/material';
import { useTranslation } from 'react-i18next';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import PhotoLibraryIcon from '@mui/icons-material/PhotoLibrary';
import FullscreenIcon from '@mui/icons-material/Fullscreen';

interface Props {
    mainImageUrl: string;
    secondaryImageUrls?: string[];
    title: string;
}

export default function PostImageCarousel({ mainImageUrl, secondaryImageUrls = [], title }: Props) {
    const { t } = useTranslation();
    const [currentImage, setCurrentImage] = useState(mainImageUrl);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [imageLoaded, setImageLoaded] = useState<{ [key: string]: boolean }>({});
    const [imageErrors, setImageErrors] = useState<Set<string>>(new Set());

    // Combine all images
    const allImages = [mainImageUrl, ...secondaryImageUrls];
    const hasMultipleImages = allImages.length > 1;

    const handleImageLoad = (imageUrl: string) => {
        setImageLoaded(prev => ({ ...prev, [imageUrl]: true }));
    };

    const handleImageError = (imageUrl: string) => {
        setImageErrors(prev => new Set(prev).add(imageUrl));
    };

    const selectImage = (imageUrl: string, index: number) => {
        setCurrentImage(imageUrl);
        setCurrentIndex(index);
    };

    const nextImage = () => {
        const nextIndex = (currentIndex + 1) % allImages.length;
        selectImage(allImages[nextIndex], nextIndex);
    };

    const prevImage = () => {
        const prevIndex = currentIndex === 0 ? allImages.length - 1 : currentIndex - 1;
        selectImage(allImages[prevIndex], prevIndex);
    };

    const handleFullscreen = () => {
        // This could open a modal or trigger fullscreen view
        console.log('Fullscreen view for:', currentImage);
    };

    return (
        <Box
            display="flex"
            flexDirection={{ xs: 'column', md: 'row' }}
            gap={2}
            sx={{ 
                width: '100%', 
                mb: 3,
                borderRadius: '16px',
                overflow: 'hidden',
                backgroundColor: 'background.paper',
                border: '1px solid',
                borderColor: 'divider',
            }}
        >
            {/* Main Image Display */}
            <Box
                sx={{
                    position: 'relative',
                    flex: 1,
                    minHeight: { xs: 250, sm: 350, md: 400 },
                    borderRadius: { xs: '0', md: '12px' },
                    overflow: 'hidden',
                    backgroundColor: 'grey.100',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                {/* Loading Skeleton */}
                {!imageLoaded[currentImage] && !imageErrors.has(currentImage) && (
                    <Skeleton
                        variant="rectangular"
                        width="100%"
                        height="100%"
                        animation="wave"
                        sx={{
                            position: 'absolute',
                            top: 0,
                            left: 0,
                            borderRadius: '12px',
                        }}
                    />
                )}

                {/* Error State */}
                {imageErrors.has(currentImage) ? (
                    <Box
                        display="flex"
                        alignItems="center"
                        justifyContent="center"
                        flexDirection="column"
                        gap={2}
                        sx={{ color: 'text.secondary' }}
                    >
                        <PhotoLibraryIcon sx={{ fontSize: 60 }} />
                        <Box sx={{ fontSize: '0.875rem' }}>{t("imageLoadError")}</Box>
                    </Box>
                ) : (
                    <img
                        src={import.meta.env.VITE_API_URL.replace("api", "") + currentImage}
                        alt={title}
                        onLoad={() => handleImageLoad(currentImage)}
                        onError={() => handleImageError(currentImage)}
                        style={{
                            width: '100%',
                            height: '100%',
                            objectFit: 'cover',
                            opacity: imageLoaded[currentImage] ? 1 : 0,
                            transition: 'opacity 0.3s ease',
                        }}
                    />
                )}

                {/* Navigation Controls */}
                {hasMultipleImages && imageLoaded[currentImage] && (
                    <>
                        <IconButton
                            onClick={prevImage}
                            sx={{
                                position: 'absolute',
                                left: 12,
                                top: '50%',
                                transform: 'translateY(-50%)',
                                backgroundColor: alpha('#000', 0.5),
                                color: 'white',
                                backdropFilter: 'blur(10px)',
                                border: '1px solid rgba(255,255,255,0.2)',
                                '&:hover': {
                                    backgroundColor: alpha('#000', 0.7),
                                    transform: 'translateY(-50%) scale(1.1)',
                                },
                            }}
                        >
                            <ArrowBackIosIcon sx={{ fontSize: 20 }} />
                        </IconButton>

                        <IconButton
                            onClick={nextImage}
                            sx={{
                                position: 'absolute',
                                right: 12,
                                top: '50%',
                                transform: 'translateY(-50%)',
                                backgroundColor: alpha('#000', 0.5),
                                color: 'white',
                                backdropFilter: 'blur(10px)',
                                border: '1px solid rgba(255,255,255,0.2)',
                                '&:hover': {
                                    backgroundColor: alpha('#000', 0.7),
                                    transform: 'translateY(-50%) scale(1.1)',
                                },
                            }}
                        >
                            <ArrowForwardIosIcon sx={{ fontSize: 20 }} />
                        </IconButton>
                    </>
                )}

                {/* Fullscreen Button */}
                {imageLoaded[currentImage] && (
                    <Tooltip title={t("fullscreen")}>
                        <IconButton
                            onClick={handleFullscreen}
                            sx={{
                                position: 'absolute',
                                top: 12,
                                right: 12,
                                backgroundColor: alpha('#000', 0.5),
                                color: 'white',
                                backdropFilter: 'blur(10px)',
                                border: '1px solid rgba(255,255,255,0.2)',
                                '&:hover': {
                                    backgroundColor: alpha('#000', 0.7),
                                    transform: 'scale(1.1)',
                                },
                            }}
                        >
                            <FullscreenIcon sx={{ fontSize: 20 }} />
                        </IconButton>
                    </Tooltip>
                )}

                {/* Image Counter */}
                {hasMultipleImages && (
                    <Box
                        sx={{
                            position: 'absolute',
                            bottom: 12,
                            left: 12,
                            backgroundColor: alpha('#000', 0.7),
                            color: 'white',
                            px: 2,
                            py: 0.5,
                            borderRadius: '12px',
                            fontSize: '0.75rem',
                            fontWeight: 600,
                            backdropFilter: 'blur(10px)',
                            border: '1px solid rgba(255,255,255,0.2)',
                        }}
                    >
                        {currentIndex + 1} / {allImages.length}
                    </Box>
                )}
            </Box>

            {/* Thumbnail Navigation */}
            {hasMultipleImages && (
                <Box
                    display="flex"
                    flexDirection={{ xs: 'row', md: 'column' }}
                    alignItems="center"
                    gap={1}
                    sx={{
                        maxWidth: { xs: '100%', md: '120px' },
                        maxHeight: { xs: '100px', md: '400px' },
                        overflowX: { xs: 'auto', md: 'hidden' },
                        overflowY: { xs: 'hidden', md: 'auto' },
                        p: 2,
                        '&::-webkit-scrollbar': {
                            width: 6,
                            height: 6,
                        },
                        '&::-webkit-scrollbar-track': {
                            backgroundColor: alpha('#000', 0.1),
                            borderRadius: 3,
                        },
                        '&::-webkit-scrollbar-thumb': {
                            backgroundColor: alpha('#000', 0.3),
                            borderRadius: 3,
                            '&:hover': {
                                backgroundColor: alpha('#000', 0.5),
                            },
                        },
                    }}
                >
                    {allImages.map((imageUrl, index) => (
                        <Box
                            key={index}
                            onClick={() => selectImage(imageUrl, index)}
                            sx={{
                                width: { xs: '80px', md: '100px' },
                                height: { xs: '80px', md: '80px' },
                                minWidth: { xs: '80px', md: '100px' },
                                overflow: 'hidden',
                                borderRadius: '12px',
                                cursor: 'pointer',
                                border: '2px solid',
                                borderColor: currentImage === imageUrl ? 'primary.main' : 'transparent',
                                transition: 'all 0.2s ease',
                                backgroundColor: 'grey.100',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                '&:hover': {
                                    borderColor: 'primary.light',
                                    transform: 'scale(1.05)',
                                },
                            }}
                        >
                            {imageErrors.has(imageUrl) ? (
                                <PhotoLibraryIcon sx={{ color: 'grey.400', fontSize: 24 }} />
                            ) : (
                                <>
                                    {!imageLoaded[imageUrl] && (
                                        <Skeleton
                                            variant="rectangular"
                                            width="100%"
                                            height="100%"
                                            animation="wave"
                                        />
                                    )}
                                    <img
                                        src={import.meta.env.VITE_API_URL.replace("api", "") + imageUrl}
                                        alt={`${title} thumbnail ${index + 1}`}
                                        onLoad={() => handleImageLoad(imageUrl)}
                                        onError={() => handleImageError(imageUrl)}
                                        style={{
                                            width: '100%',
                                            height: '100%',
                                            objectFit: 'cover',
                                            opacity: imageLoaded[imageUrl] ? 1 : 0,
                                            transition: 'opacity 0.3s ease',
                                        }}
                                    />
                                </>
                            )}
                        </Box>
                    ))}
                </Box>
            )}
        </Box>
    );
}