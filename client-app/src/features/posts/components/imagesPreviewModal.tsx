import { Box, Modal, IconButton, Typography, alpha, Fade, Backdrop } from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import CloseIcon from "@mui/icons-material/Close";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import DownloadIcon from "@mui/icons-material/Download";
import PhotoLibraryIcon from "@mui/icons-material/PhotoLibrary";
import { Carousel } from "react-responsive-carousel";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import postApi from "../api/postApi";

interface Props {
    postId: string;
    isOpen: boolean;
    onClose: () => void;
}

export default function ImagesPreviewModal({ postId, isOpen, onClose }: Props) {
    const { t } = useTranslation();
    const [images, setImages] = useState<string[]>([]);
    const [loading, setLoading] = useState(true);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [imageErrors, setImageErrors] = useState<Set<number>>(new Set());

    useEffect(() => {
        if (isOpen) {
            const fetchImages = async () => {
                try {
                    setLoading(true);
                    const response = await postApi.getPostImages({ postId });
                    const allImages = [
                        response?.value.mainImageUrl,
                        ...(response?.value.secondaryImagesUrl || [])
                    ].filter(Boolean);
                    setImages(allImages);
                } catch (error) {
                    console.error('Failed to fetch images:', error);
                    setImages([]);
                } finally {
                    setLoading(false);
                }
            };

            fetchImages();
        }
    }, [postId, isOpen]);

    const handleClose = (_event?: {}, _reason?: "backdropClick" | "escapeKeyDown") => {
        setImages([]);
        setImageErrors(new Set());
        setCurrentIndex(0);
        onClose();
    };

    const handleImageError = (index: number) => {
        setImageErrors(prev => new Set(prev).add(index));
    };

    const handleDownload = (imageUrl: string) => {
        const link = document.createElement('a');
        link.href = import.meta.env.VITE_API_URL.replace("api", "") + imageUrl;
        link.download = `image-${Date.now()}.jpg`;
        link.target = '_blank';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };

    const customArrowPrev = (onClickHandler: () => void, hasPrev: boolean) => (
        <IconButton
            onClick={onClickHandler}
            disabled={!hasPrev}
            sx={{
                position: 'absolute',
                left: 20,
                top: '50%',
                transform: 'translateY(-50%)',
                zIndex: 3,
                backgroundColor: alpha('#000', 0.5),
                color: 'white',
                backdropFilter: 'blur(10px)',
                border: '1px solid rgba(255,255,255,0.2)',
                '&:hover': {
                    backgroundColor: alpha('#000', 0.7),
                },
                '&:disabled': {
                    opacity: 0.3,
                }
            }}
        >
            <ArrowBackIosIcon />
        </IconButton>
    );

    const customArrowNext = (onClickHandler: () => void, hasNext: boolean) => (
        <IconButton
            onClick={onClickHandler}
            disabled={!hasNext}
            sx={{
                position: 'absolute',
                right: 20,
                top: '50%',
                transform: 'translateY(-50%)',
                zIndex: 3,
                backgroundColor: alpha('#000', 0.5),
                color: 'white',
                backdropFilter: 'blur(10px)',
                border: '1px solid rgba(255,255,255,0.2)',
                '&:hover': {
                    backgroundColor: alpha('#000', 0.7),
                },
                '&:disabled': {
                    opacity: 0.3,
                }
            }}
        >
            <ArrowForwardIosIcon />
        </IconButton>
    );

    if (!isOpen) return null;

    return (
        <Modal
            open={isOpen}
            onClose={handleClose}
            closeAfterTransition
            BackdropComponent={Backdrop}
            BackdropProps={{
                timeout: 500,
                sx: {
                    backgroundColor: 'rgba(0, 0, 0, 0.9)',
                    backdropFilter: 'blur(10px)',
                }
            }}
        >
            <Fade in={isOpen}>
                <Box
                    onClick={(e) => e.stopPropagation()}
                    sx={{
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                        width: { xs: '95vw', sm: '90vw', md: '80vw' },
                        maxWidth: '1200px',
                        height: { xs: '85vh', sm: '80vh' },
                        bgcolor: 'background.paper',
                        boxShadow: (theme) => `0 25px 50px ${alpha(theme.palette.primary.main, 0.3)}`,
                        borderRadius: '20px',
                        border: '1px solid',
                        borderColor: 'divider',
                        outline: 'none',
                        overflow: 'hidden',
                        display: 'flex',
                        flexDirection: 'column',
                    }}
                >
                    {/* Header */}
                    <Box
                        display="flex"
                        justifyContent="space-between"
                        alignItems="center"
                        p={2}
                        sx={{
                            borderBottom: '1px solid',
                            borderColor: 'divider',
                            backgroundColor: alpha('#000', 0.02),
                        }}
                    >
                        <Box display="flex" alignItems="center" gap={2}>
                            <PhotoLibraryIcon color="primary" />
                            <Box>
                                <Typography variant="h6" sx={{ fontWeight: 700 }}>
                                    {t("imageGallery")}
                                </Typography>
                                <Typography variant="caption" color="text.secondary">
                                    {images.length > 0 && !loading && 
                                        `${currentIndex + 1} ${t("of")} ${images.length}`
                                    }
                                </Typography>
                            </Box>
                        </Box>

                        <Box display="flex" alignItems="center" gap={1}>
                            {images.length > 0 && !loading && (
                                <IconButton
                                    onClick={() => handleDownload(images[currentIndex])}
                                    sx={{
                                        backgroundColor: alpha('#000', 0.04),
                                        '&:hover': {
                                            backgroundColor: alpha('#000', 0.08),
                                        }
                                    }}
                                >
                                    <DownloadIcon />
                                </IconButton>
                            )}
                            <IconButton
                                onClick={handleClose}
                                sx={{
                                    backgroundColor: alpha('#000', 0.04),
                                    '&:hover': {
                                        backgroundColor: alpha('#000', 0.08),
                                    }
                                }}
                            >
                                <CloseIcon />
                            </IconButton>
                        </Box>
                    </Box>

                    {/* Content */}
                    <Box flex={1} position="relative" overflow="hidden">
                        {loading ? (
                            <Box
                                display="flex"
                                alignItems="center"
                                justifyContent="center"
                                height="100%"
                                flexDirection="column"
                                gap={2}
                            >
                                <Box
                                    sx={{
                                        width: 60,
                                        height: 60,
                                        border: '3px solid',
                                        borderColor: 'primary.main',
                                        borderTopColor: 'transparent',
                                        borderRadius: '50%',
                                        animation: 'spin 1s linear infinite',
                                        '@keyframes spin': {
                                            '0%': { transform: 'rotate(0deg)' },
                                            '100%': { transform: 'rotate(360deg)' }
                                        }
                                    }}
                                />
                                <Typography color="text.secondary">
                                    {t("loadingImages")}...
                                </Typography>
                            </Box>
                        ) : images.length === 0 ? (
                            <Box
                                display="flex"
                                alignItems="center"
                                justifyContent="center"
                                height="100%"
                                flexDirection="column"
                                gap={2}
                            >
                                <PhotoLibraryIcon sx={{ fontSize: 80, color: 'text.secondary' }} />
                                <Typography color="text.secondary">
                                    {t("noImagesAvailable")}
                                </Typography>
                            </Box>
                        ) : (
                            <Box
                                sx={{
                                    height: '100%',
                                    '& .carousel-root': {
                                        height: '100%',
                                    },
                                    '& .carousel .slide': {
                                        backgroundColor: 'transparent',
                                        height: '100%',
                                        display: 'flex',
                                        alignItems: 'center',
                                        justifyContent: 'center',
                                    }
                                }}
                            >
                                <Carousel
                                    showThumbs={false}
                                    dynamicHeight={false}
                                    emulateTouch
                                    swipeable
                                    useKeyboardArrows
                                    onChange={(index) => setCurrentIndex(index)}
                                    renderArrowPrev={customArrowPrev}
                                    renderArrowNext={customArrowNext}
                                    showStatus={false}
                                    showIndicators={images.length > 1}
                                    infiniteLoop={images.length > 1}
                                >
                                    {images.map((imgUrl, index) => (
                                        <Box
                                            key={index}
                                            sx={{
                                                height: '100%',
                                                display: 'flex',
                                                alignItems: 'center',
                                                justifyContent: 'center',
                                                p: 2,
                                            }}
                                        >
                                            {imageErrors.has(index) ? (
                                                <Box
                                                    display="flex"
                                                    alignItems="center"
                                                    justifyContent="center"
                                                    flexDirection="column"
                                                    gap={2}
                                                    sx={{
                                                        backgroundColor: 'grey.100',
                                                        borderRadius: '12px',
                                                        p: 4,
                                                        minHeight: 200,
                                                        minWidth: 300,
                                                    }}
                                                >
                                                    <PhotoLibraryIcon sx={{ fontSize: 60, color: 'grey.400' }} />
                                                    <Typography color="text.secondary">
                                                        {t("imageLoadError")}
                                                    </Typography>
                                                </Box>
                                            ) : (
                                                <img
                                                    src={import.meta.env.VITE_API_URL.replace("api", "") + imgUrl}
                                                    alt={`${t("image")} ${index + 1}`}
                                                    onError={() => handleImageError(index)}
                                                    style={{
                                                        maxWidth: '100%',
                                                        maxHeight: '100%',
                                                        objectFit: 'contain',
                                                        borderRadius: '12px',
                                                        boxShadow: '0 10px 30px rgba(0,0,0,0.2)',
                                                    }}
                                                />
                                            )}
                                        </Box>
                                    ))}
                                </Carousel>
                            </Box>
                        )}
                    </Box>
                </Box>
            </Fade>
        </Modal>
    );
}