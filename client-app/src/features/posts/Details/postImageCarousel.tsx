import { useState } from 'react';
import { Box, Typography } from '@mui/material';

interface Props {
    mainImageUrl: string;
    secondaryImageUrls?: string[]; // Assuming you'll fetch secondary images later
    title: string; // For alt text
}

export default function PostImageCarousel({ mainImageUrl, secondaryImageUrls, title }: Props) {
    const [currentImage, setCurrentImage] = useState(mainImageUrl);

    // Combine main image with secondary images for the carousel
    const allImages = [mainImageUrl, ...(secondaryImageUrls || [])];

    return (
        <Box
            display="flex"
            flexDirection={{ xs: 'column', md: 'row' }} // Column on mobile, row on larger screens
            gap={2}
            sx={{ width: '100%', mb: 3 }} // Full width
        >
            {/* Vertical Thumbnails (only if more than one image) */}
            {allImages.length > 1 && (
                <Box
                    display="flex"
                    flexDirection={{ xs: 'row', md: 'column' }} // Row on mobile, column on larger screens
                    alignItems="center" // Center horizontally for rows, vertically for columns
                    gap={1}
                    sx={{
                        maxWidth: { xs: '100%', md: '100px' }, // Limit width of thumbnails column
                        overflowX: { xs: 'auto', md: 'hidden' }, // Scroll horizontally on mobile
                        overflowY: { xs: 'hidden', md: 'auto' }, // Scroll vertically on desktop
                        py: { xs: 1, md: 0 },
                        px: { xs: 0, md: 1 },
                        '&::-webkit-scrollbar': { display: 'none' }, // Hide scrollbar
                        msOverflowStyle: 'none', // IE and Edge
                        scrollbarWidth: 'none', // Firefox
                    }}
                >
                    {allImages.map((imageUrl, index) => (
                        <Box
                            key={index}
                            onClick={() => setCurrentImage(imageUrl)}
                            sx={{
                                width: { xs: '80px', md: '80px' }, // Thumbnail width
                                height: { xs: '80px', md: '80px' }, // Thumbnail height
                                overflow: 'hidden',
                                borderRadius: '8px',
                                cursor: 'pointer',
                                border: (theme) =>
                                    currentImage === imageUrl
                                        ? `2px solid ${theme.palette.primary.main}`
                                        : '2px solid transparent',
                                transition: 'border 0.2s ease-in-out',
                                flexShrink: 0, // Prevent shrinking on mobile horizontal scroll
                            }}
                        >
                            <img
                                src={imageUrl}
                                alt={`${title} - Thumbnail ${index + 1}`}
                                style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                            />
                        </Box>
                    ))}
                </Box>
            )}

            {/* Main Image Display */}
            <Box
                flexGrow={1} // Takes remaining space
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    minHeight: { xs: '250px', md: '450px' }, // Responsive height
                    maxHeight: { xs: '400px', md: '550px' },
                    overflow: 'hidden',
                    borderRadius: '10px',
                    backgroundColor: 'grey.200', // Placeholder background
                }}
            >
                <img
                    src={currentImage}
                    alt={title || 'Post Image'}
                    style={{
                        width: '100%',
                        height: '100%',
                        objectFit: 'contain', // Changed to contain to show full image
                        borderRadius: '10px',
                    }}
                />
            </Box>
        </Box>
    );
}