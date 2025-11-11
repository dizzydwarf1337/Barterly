import { useState } from "react";
import { Box, IconButton, alpha, Skeleton, Tooltip, Fade } from "@mui/material";
import { useTranslation } from "react-i18next";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import PhotoLibraryIcon from "@mui/icons-material/PhotoLibrary";
import FullscreenIcon from "@mui/icons-material/Fullscreen";
import ImageIcon from "@mui/icons-material/Image";

interface Props {
  mainImageUrl?: string | null;
  secondaryImageUrls?: string[];
  title: string;
}

export default function PostImageCarousel({
  mainImageUrl,
  secondaryImageUrls = [],
  title,
}: Props) {
  const { t } = useTranslation();
  console.log(import.meta.env.VITE_API_URL);
  const validImages = [mainImageUrl, ...secondaryImageUrls].filter(
    Boolean
  ) as string[];

  const [currentImage, setCurrentImage] = useState(validImages[0] || "");
  const [currentIndex, setCurrentIndex] = useState(0);
  const [imageLoaded, setImageLoaded] = useState<{ [key: string]: boolean }>(
    {}
  );
  const [imageErrors, setImageErrors] = useState<Set<string>>(new Set());

  const hasMultipleImages = validImages.length > 1;
  const hasImages = validImages.length > 0;

  const handleImageLoad = (imageUrl: string) => {
    setImageLoaded((prev) => ({ ...prev, [imageUrl]: true }));
  };

  const handleImageError = (imageUrl: string) => {
    setImageErrors((prev) => new Set(prev).add(imageUrl));
  };

  const selectImage = (imageUrl: string, index: number) => {
    setCurrentImage(imageUrl);
    setCurrentIndex(index);
  };

  const nextImage = () => {
    if (!hasImages) return;
    const nextIndex = (currentIndex + 1) % validImages.length;
    selectImage(validImages[nextIndex], nextIndex);
  };

  const prevImage = () => {
    if (!hasImages) return;
    const prevIndex =
      currentIndex === 0 ? validImages.length - 1 : currentIndex - 1;
    selectImage(validImages[prevIndex], prevIndex);
  };

  const handleFullscreen = () => {
    console.log("Fullscreen view for:", currentImage);
  };

  if (!hasImages) {
    return (
      <Box
        display="flex"
        alignItems="center"
        justifyContent="center"
        flexDirection="column"
        sx={{
          width: "100%",
          height: { xs: 250, sm: 350, md: 400 },
          mb: 3,
          borderRadius: "16px",
          backgroundColor: "grey.100",
          border: "1px solid",
          borderColor: "divider",
          gap: 2,
          color: "text.secondary",
        }}
      >
        <ImageIcon sx={{ fontSize: 80, color: "grey.400" }} />
        <Box sx={{ fontSize: "1rem", fontWeight: 500 }}>
          {t("noImagesAvailable")}
        </Box>
      </Box>
    );
  }

  return (
    <Box
      display="flex"
      flexDirection={{ xs: "column", md: "row" }}
      gap={2}
      sx={{
        width: "100%",
        mb: 3,
        borderRadius: "16px",
        overflow: "hidden",
        backgroundColor: "background.paper",
        border: "1px solid",
        borderColor: "divider",
        boxShadow: (theme) => theme.shadows[4],
        transition: "box-shadow 0.3s ease",
        "&:hover": {
          boxShadow: (theme) => theme.shadows[8],
        },
      }}
    >
      <Box
        sx={{
          position: "relative",
          flex: 1,
          maxHeight: { xs: 250, sm: 350, md: 400 },
          borderRadius: { xs: "0", md: "12px" },
          overflow: "hidden",
          backgroundColor: "grey.100",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        {!imageLoaded[currentImage] &&
          !imageErrors.has(currentImage) &&
          currentImage && (
            <Skeleton
              variant="rectangular"
              width="100%"
              height="100%"
              animation="wave"
              sx={{
                position: "absolute",
                top: 0,
                left: 0,
                borderRadius: "12px",
              }}
            />
          )}

        {imageErrors.has(currentImage) ? (
          <Fade in timeout={300}>
            <Box
              display="flex"
              alignItems="center"
              justifyContent="center"
              flexDirection="column"
              gap={2}
              sx={{ color: "text.secondary", textAlign: "center", px: 2 }}
            >
              <PhotoLibraryIcon sx={{ fontSize: 64, color: "grey.400" }} />
              <Box sx={{ fontSize: "0.875rem" }}>{t("imageLoadError")}</Box>
            </Box>
          </Fade>
        ) : (
          currentImage && (
            <Fade in={imageLoaded[currentImage]} timeout={300}>
              <img
                src={`${import.meta.env.VITE_API_URL}/${currentImage}`}
                alt={title}
                onLoad={() => handleImageLoad(currentImage)}
                onError={() => handleImageError(currentImage)}
                style={{
                  width: "100%",
                  height: "100%",
                  objectFit: "cover",
                  objectPosition: "center",
                }}
              />
            </Fade>
          )
        )}

        {/* Navigation Controls */}
        {hasMultipleImages && imageLoaded[currentImage] && (
          <>
            <Tooltip title={t("previousImage")} placement="right">
              <IconButton
                onClick={prevImage}
                sx={{
                  position: "absolute",
                  left: 12,
                  top: "50%",
                  transform: "translateY(-50%)",
                  backgroundColor: alpha("#000", 0.6),
                  color: "white",
                  backdropFilter: "blur(10px)",
                  border: "1px solid rgba(255,255,255,0.2)",
                  transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                  "&:hover": {
                    backgroundColor: alpha("#000", 0.8),
                    transform: "translateY(-50%) scale(1.1)",
                    boxShadow: `0px 8px 25px ${alpha("#000", 0.4)}`,
                  },
                  "&:active": {
                    transform: "translateY(-50%) scale(1.05)",
                  },
                }}
              >
                <ArrowBackIosIcon sx={{ fontSize: 20 }} />
              </IconButton>
            </Tooltip>

            <Tooltip title={t("nextImage")} placement="left">
              <IconButton
                onClick={nextImage}
                sx={{
                  position: "absolute",
                  right: 12,
                  top: "50%",
                  transform: "translateY(-50%)",
                  backgroundColor: alpha("#000", 0.6),
                  color: "white",
                  backdropFilter: "blur(10px)",
                  border: "1px solid rgba(255,255,255,0.2)",
                  transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                  "&:hover": {
                    backgroundColor: alpha("#000", 0.8),
                    transform: "translateY(-50%) scale(1.1)",
                    boxShadow: `0px 8px 25px ${alpha("#000", 0.4)}`,
                  },
                  "&:active": {
                    transform: "translateY(-50%) scale(1.05)",
                  },
                }}
              >
                <ArrowForwardIosIcon sx={{ fontSize: 20 }} />
              </IconButton>
            </Tooltip>
          </>
        )}

        {/* Fullscreen Button */}
        {imageLoaded[currentImage] && (
          <Tooltip title={t("fullscreen")}>
            <IconButton
              onClick={handleFullscreen}
              sx={{
                position: "absolute",
                top: 12,
                right: 12,
                backgroundColor: alpha("#000", 0.6),
                color: "white",
                backdropFilter: "blur(10px)",
                border: "1px solid rgba(255,255,255,0.2)",
                transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                "&:hover": {
                  backgroundColor: alpha("#000", 0.8),
                  transform: "scale(1.1)",
                  boxShadow: `0px 8px 25px ${alpha("#000", 0.4)}`,
                },
                "&:active": {
                  transform: "scale(1.05)",
                },
              }}
            >
              <FullscreenIcon sx={{ fontSize: 20 }} />
            </IconButton>
          </Tooltip>
        )}

        {hasMultipleImages && (
          <Fade in timeout={500}>
            <Box
              sx={{
                position: "absolute",
                bottom: 12,
                left: 12,
                backgroundColor: alpha("#000", 0.8),
                color: "white",
                px: 2,
                py: 0.5,
                borderRadius: "16px",
                fontSize: "0.75rem",
                fontWeight: 600,
                backdropFilter: "blur(10px)",
                border: "1px solid rgba(255,255,255,0.2)",
                boxShadow: `0px 4px 12px ${alpha("#000", 0.3)}`,
              }}
            >
              {currentIndex + 1} / {validImages.length}
            </Box>
          </Fade>
        )}
      </Box>

      {hasMultipleImages && (
        <Box
          display="flex"
          flexDirection={{ xs: "row", md: "column" }}
          alignItems="center"
          gap={1.5}
          sx={{
            maxWidth: { xs: "100%", md: "120px" },
            maxHeight: { xs: "120px", md: "400px" },
            overflowX: { xs: "auto", md: "hidden" },
            overflowY: { xs: "hidden", md: "auto" },
            p: 2,
            "&::-webkit-scrollbar": {
              width: 6,
              height: 6,
            },
            "&::-webkit-scrollbar-track": {
              backgroundColor: alpha("#000", 0.1),
              borderRadius: 3,
            },
            "&::-webkit-scrollbar-thumb": {
              backgroundColor: alpha("#000", 0.3),
              borderRadius: 3,
              "&:hover": {
                backgroundColor: alpha("#000", 0.5),
              },
            },
          }}
        >
          {validImages.map((imageUrl, index) => (
            <Box
              key={index}
              onClick={() => selectImage(imageUrl, index)}
              sx={{
                width: { xs: "80px", md: "100px" },
                height: { xs: "80px", md: "80px" },
                minWidth: { xs: "80px", md: "100px" },
                overflow: "hidden",
                borderRadius: "12px",
                cursor: "pointer",
                border: "3px solid",
                borderColor:
                  currentImage === imageUrl ? "primary.main" : "transparent",
                transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                backgroundColor: "grey.100",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                boxShadow:
                  currentImage === imageUrl
                    ? (theme) =>
                        `0px 4px 20px ${alpha(theme.palette.primary.main, 0.4)}`
                    : "none",
                "&:hover": {
                  borderColor:
                    currentImage === imageUrl
                      ? "primary.main"
                      : "primary.light",
                  transform: "scale(1.05)",
                  boxShadow: (theme) =>
                    `0px 4px 20px ${alpha(theme.palette.primary.main, 0.2)}`,
                },
                "&:active": {
                  transform: "scale(1.02)",
                },
              }}
            >
              {imageErrors.has(imageUrl) ? (
                <PhotoLibraryIcon sx={{ color: "grey.400", fontSize: 24 }} />
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
                  <Fade in={imageLoaded[imageUrl]} timeout={200}>
                    <img
                      src={`${import.meta.env.VITE_API_URL}/${imageUrl}`}
                      alt={`${title} thumbnail ${index + 1}`}
                      onLoad={() => handleImageLoad(imageUrl)}
                      onError={() => handleImageError(imageUrl)}
                      style={{
                        width: "100%",
                        height: "100%",
                        objectFit: "cover",
                        objectPosition: "center",
                      }}
                    />
                  </Fade>
                </>
              )}
            </Box>
          ))}
        </Box>
      )}
    </Box>
  );
}
