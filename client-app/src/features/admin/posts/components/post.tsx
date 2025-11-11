import { observer } from "mobx-react-lite";
import {
  PostCurrency,
  PostDetails,
  PostPriceType,
} from "../../../posts/types/postTypes";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";

import ApartmentIcon from "@mui/icons-material/Apartment";
import MeetingRoomIcon from "@mui/icons-material/MeetingRoom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import MoneyIcon from "@mui/icons-material/Money";
import AccountBalanceWalletIcon from "@mui/icons-material/AccountBalanceWallet";
import CalendarTodayIcon from "@mui/icons-material/CalendarToday";
import VisibilityIcon from "@mui/icons-material/Visibility";
import SquareFootIcon from "@mui/icons-material/SquareFoot";
import CategoryIcon from "@mui/icons-material/Category";
import LocalOfferIcon from "@mui/icons-material/LocalOffer";
import BusinessCenterIcon from "@mui/icons-material/Business";
import {
  alpha,
  Box,
  Button,
  Card,
  CardContent,
  Chip,
  Fade,
  Typography,
} from "@mui/material";
import PostImageCarousel from "../../../posts/components/postImageCarousel";

interface Props {
  post: PostDetails;
}

export const Post = observer(({ post }: Props) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const renderPriceAndType = () => {
    const currencySymbol = post.currency ? PostCurrency[post.currency] : "";
    const priceTypeTranslation =
      post.priceType != null ? t(PostPriceType[post.priceType]) : "";

    let priceText = "";
    let icon = MoneyIcon;
    let isSuccessColor = true;

    if (post.postType === "Work") {
      icon = AccountBalanceWalletIcon;
      if (post.minSalary && post.maxSalary) {
        priceText = `${post.minSalary} - ${post.maxSalary}`;
      } else if (post.minSalary) {
        priceText = `${t("adminPosts:from")} ${post.minSalary}`;
      } else if (post.maxSalary) {
        priceText = `${t("adminPosts:upTo")} ${post.maxSalary}`;
      } else {
        priceText = t("adminPosts:negotiable");
      }
    } else if (post.postType === "Rent") {
      priceText =
        post.price != null ? `${post.price}` : t("adminPosts:negotiable");
    } else {
      if (
        post.price != null &&
        post.price > 0 &&
        post.priceType !== PostPriceType.Free
      ) {
        priceText = `${post.price}`;
      } else {
        priceText = t("adminPosts:free");
        isSuccessColor = false;
      }
    }

    const IconComponent = icon;

    return (
      <Box
        display="flex"
        flexDirection="row"
        alignItems="center"
        gap={1}
        sx={{
          backgroundColor: isSuccessColor
            ? alpha("#4CAF50", 0.1)
            : alpha("#2196F3", 0.1),
          borderRadius: "12px",
          px: 2,
          py: 1,
        }}
      >
        <IconComponent
          color={isSuccessColor ? "success" : "primary"}
          sx={{ fontSize: 20 }}
        />
        <Typography
          variant="body1"
          sx={{
            fontWeight: 700,
            color: isSuccessColor ? "success.main" : "primary.main",
          }}
        >
          {priceText} {priceText !== t("adminPosts:free") ? currencySymbol : ""}
          {priceTypeTranslation &&
            priceText !== t("adminPosts:free") &&
            ` / ${priceTypeTranslation}`}
        </Typography>
      </Box>
    );
  };

  return (
    <Fade in timeout={600}>
      <Card sx={{ borderRadius: "20px", overflow: "hidden" }}>
        <Box sx={{ p: 2 }}>
          <Button
            startIcon={<ArrowBackIcon />}
            onClick={() => navigate(-1)}
            sx={{
              color: "text.secondary",
              textTransform: "none",
              "&:hover": { backgroundColor: alpha("#000", 0.04) },
            }}
          >
            {t("adminPosts:goBack")}
          </Button>
        </Box>

        {post.mainImageUrl && (
          <Box sx={{ height: 400, position: "relative", overflow: "hidden" }}>
            <PostImageCarousel
              title={post.title}
              mainImageUrl={post.mainImageUrl}
              secondaryImageUrls={(post.postImages ?? []).map(
                (x) => x.imageUrl!
              )}
            />
          </Box>
        )}

        <CardContent sx={{ p: 4 }}>
          {/* Header */}
          <Box
            display="flex"
            justifyContent="space-between"
            alignItems="flex-start"
            mb={3}
          >
            <Box>
              <Typography variant="h4" fontWeight="bold" gutterBottom>
                {post.title}
              </Typography>
              <Box display="flex" alignItems="center" gap={2} flexWrap="wrap">
                <Chip
                  icon={<CalendarTodayIcon />}
                  label={new Date(post.createdAt!).toLocaleDateString()}
                  variant="outlined"
                  size="small"
                />
                {post.updatedAt && (
                  <Chip
                    icon={<CalendarTodayIcon />}
                    label={`Updated: ${new Date(
                      post.updatedAt
                    ).toLocaleDateString()}`}
                    variant="outlined"
                    size="small"
                    color="secondary"
                  />
                )}
                {post.viewsCount && (
                  <Chip
                    icon={<VisibilityIcon />}
                    label={`${post.viewsCount} views`}
                    variant="outlined"
                    size="small"
                  />
                )}
                {post.subCategory && (
                  <Chip
                    icon={<CategoryIcon />}
                    label={post.subCategory.namePL || post.subCategory.nameEN}
                    variant="filled"
                    size="small"
                    color="primary"
                  />
                )}
              </Box>
            </Box>
            {renderPriceAndType()}
          </Box>

          {/* Description */}
          {post.shortDescription && (
            <Typography variant="body1" color="text.secondary" paragraph>
              {post.shortDescription}
            </Typography>
          )}

          {/* Full Description */}
          {post.fullDescription && (
            <Box sx={{ mb: 3 }}>
              <Typography
                variant="h6"
                fontWeight="bold"
                gutterBottom
                sx={{ display: "flex", alignItems: "center", gap: 1 }}
              >
                <BusinessCenterIcon color="primary" />
                {t("adminPosts:fullDescription")}
              </Typography>
              <Typography
                variant="body1"
                sx={{ whiteSpace: "pre-wrap", lineHeight: 1.7 }}
              >
                {post.fullDescription}
              </Typography>
            </Box>
          )}

          {/* Tags */}
          {post.tags && post.tags.length > 0 && (
            <Box sx={{ mb: 3 }}>
              <Typography
                variant="h6"
                fontWeight="bold"
                gutterBottom
                sx={{ display: "flex", alignItems: "center", gap: 1 }}
              >
                <LocalOfferIcon color="primary" />
                {t("adminPosts:tags")}
              </Typography>
              <Box display="flex" flexWrap="wrap" gap={1}>
                {post.tags.map((tag, index) => (
                  <Chip
                    key={index}
                    label={tag}
                    variant="outlined"
                    size="small"
                  />
                ))}
              </Box>
            </Box>
          )}

          {/* Details Grid */}
          <Box
            display="grid"
            gridTemplateColumns={{
              xs: "1fr",
              sm: "repeat(2, 1fr)",
              md: "repeat(3, 1fr)",
            }}
            gap={3}
          >
            {/* Location */}
            {(post.city || post.region || post.country) && (
              <Box display="flex" alignItems="center" gap={1}>
                <LocationOnIcon color="primary" />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Location
                  </Typography>
                  <Typography variant="body1" fontWeight="medium">
                    {[
                      post.country,
                      post.region,
                      post.city,
                      post.street,
                      post.buildingNumber || post.houseNumber,
                    ]
                      .filter(Boolean)
                      .join(", ")}
                  </Typography>
                </Box>
              </Box>
            )}

            {/* Area */}
            {post.area && (
              <Box display="flex" alignItems="center" gap={1}>
                <SquareFootIcon color="primary" />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Area
                  </Typography>
                  <Typography variant="body1" fontWeight="medium">
                    {post.area} m²
                  </Typography>
                </Box>
              </Box>
            )}

            {/* Rooms */}
            {post.numberOfRooms && (
              <Box display="flex" alignItems="center" gap={1}>
                <MeetingRoomIcon color="primary" />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Rooms
                  </Typography>
                  <Typography variant="body1" fontWeight="medium">
                    {post.numberOfRooms}
                  </Typography>
                </Box>
              </Box>
            )}

            {/* Floor */}
            {post.floor && (
              <Box display="flex" alignItems="center" gap={1}>
                <ApartmentIcon color="primary" />
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Floor
                  </Typography>
                  <Typography variant="body1" fontWeight="medium">
                    {post.floor}
                  </Typography>
                </Box>
              </Box>
            )}
          </Box>
        </CardContent>
      </Card>
    </Fade>
  );
});
