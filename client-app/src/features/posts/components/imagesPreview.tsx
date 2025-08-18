import { Box, IconButton, Tooltip, Badge, alpha } from "@mui/material";
import SearchIcon from '@mui/icons-material/Search';
import PhotoLibraryIcon from '@mui/icons-material/PhotoLibrary';
import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import ImagesPreviewModal from "./imagesPreviewModal";
import postApi from "../api/postApi";

interface Props {
    mainImageUrl: string;
    postId: string;
}

export default function ImagesPreview({ mainImageUrl, postId }: Props) {
    const { t } = useTranslation();
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [imageCount, setImageCount] = useState<number>(1);
    const [imageLoaded, setImageLoaded] = useState<boolean>(false);
    const [imageError, setImageError] = useState<boolean>(false);

    useEffect(() => {
        const fetchImageCount = async () => {
            try {
                const response = await postApi.getPostImages({ postId });
                const totalImages = 1 + (response?.value.secondaryImagesUrl?.length || 0);
                setImageCount(totalImages);
            } catch (error) {
                console.error('Failed to fetch image count:', error);
            }
        };

        fetchImageCount();
    }, [postId]);

    const onCloseModal = () => setIsModalOpen(false);
    
    const handleViewPhotos = (e: React.MouseEvent) => {
        e.stopPropagation();
        e.preventDefault();
        setIsModalOpen(true);
    };

    const handleImageLoad = () => {
        setImageLoaded(true);
        setImageError(false);
    };

    const handleImageError = () => {
        setImageError(true);
        setImageLoaded(false);
    };

    return (
        <Box
            sx={{
                position: 'relative',
                width: 150,
                height: 150,
                borderRadius: '16px',
                overflow: 'hidden',
                cursor: 'pointer',
                background: imageLoaded ? 'transparent' : 'linear-gradient(45deg, #f0f0f0 25%, #e0e0e0 25%, #e0e0e0 50%, #f0f0f0 50%, #f0f0f0 75%, #e0e0e0 75%)',
                backgroundSize: '20px 20px',
                '&:hover': {
                    '& .overlay': {
                        opacity: 1,
                        backdropFilter: 'blur(4px)',
                    },
                    '& .main-image': {
                        transform: 'scale(1.1)',
                    },
                    '& .action-button': {
                        transform: 'scale(1.1)',
                    }
                },
            }}
        >
            {/* Main Image */}
            <img
                className="main-image"
                src={import.meta.env.VITE_API_URL + "/" + mainImageUrl}
                alt={t("postImage")}
                onLoad={handleImageLoad}
                onError={handleImageError}
                style={{
                    width: '100%',
                    height: '100%',
                    objectFit: 'cover',
                    transition: 'transform 0.4s cubic-bezier(0.4, 0, 0.2, 1)',
                    opacity: imageLoaded ? 1 : 0,
                    display: imageError ? 'none' : 'block'
                }}
            />

            {/* Error Placeholder */}
            {imageError && (
                <Box
                    display="flex"
                    alignItems="center"
                    justifyContent="center"
                    width="100%"
                    height="100%"
                    sx={{
                        backgroundColor: 'grey.100',
                        color: 'grey.500'
                    }}
                >
                    <PhotoLibraryIcon sx={{ fontSize: 40 }} />
                </Box>
            )}

            {/* Loading Skeleton */}
            {!imageLoaded && !imageError && (
                <Box
                    sx={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%',
                        background: 'linear-gradient(90deg, transparent, rgba(255,255,255,0.4), transparent)',
                        animation: 'shimmer 1.5s infinite',
                        '@keyframes shimmer': {
                            '0%': { transform: 'translateX(-100%)' },
                            '100%': { transform: 'translateX(100%)' }
                        }
                    }}
                />
            )}

            {/* Image Count Badge */}
            {imageCount > 1 && (
                <Badge
                    badgeContent={`+${imageCount - 1}`}
                    color="primary"
                    sx={{
                        position: 'absolute',
                        top: 8,
                        right: 8,
                        '& .MuiBadge-badge': {
                            backgroundColor: alpha('#000', 0.7),
                            color: 'white',
                            fontWeight: 700,
                            fontSize: '0.7rem',
                            minWidth: 20,
                            height: 20,
                            borderRadius: '10px',
                            backdropFilter: 'blur(10px)',
                        }
                    }}
                >
                    <PhotoLibraryIcon 
                        sx={{ 
                            color: 'white',
                            fontSize: 18,
                            filter: 'drop-shadow(0 2px 4px rgba(0,0,0,0.3))'
                        }} 
                    />
                </Badge>
            )}

            {/* Hover Overlay */}
            <Box
                className="overlay"
                onClick={handleViewPhotos}
                sx={{
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    width: '100%',
                    height: '100%',
                    background: 'linear-gradient(135deg, rgba(0,0,0,0.4) 0%, rgba(0,0,0,0.6) 100%)',
                    opacity: 0,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                    backdropFilter: 'blur(0px)',
                }}
            >
                <Tooltip title={t("viewAllPhotos")} placement="top">
                    <IconButton
                        className="action-button"
                        sx={{
                            backgroundColor: alpha('#fff', 0.2),
                            color: 'white',
                            backdropFilter: 'blur(10px)',
                            border: '1px solid rgba(255,255,255,0.3)',
                            transition: 'all 0.2s ease',
                            '&:hover': {
                                backgroundColor: alpha('#fff', 0.3),
                                borderColor: 'rgba(255,255,255,0.5)',
                            }
                        }}
                    >
                        <SearchIcon sx={{ fontSize: 28 }} />
                    </IconButton>
                </Tooltip>
            </Box>

            {/* Bottom Gradient */}
            <Box
                sx={{
                    position: 'absolute',
                    bottom: 0,
                    left: 0,
                    right: 0,
                    height: '40%',
                    background: 'linear-gradient(transparent, rgba(0,0,0,0.5))',
                    pointerEvents: 'none',
                    opacity: imageLoaded ? 1 : 0,
                    transition: 'opacity 0.3s ease',
                }}
            />

            {/* Modal */}
            {isModalOpen && (
                <ImagesPreviewModal 
                    isOpen={isModalOpen} 
                    onClose={onCloseModal} 
                    postId={postId}
                />
            )}
        </Box>
    );
}