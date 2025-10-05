import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import {
  PostCurrency,
  PostPreview,
  PostPriceType,
  PostPromotionType,
  PostType,
} from "../types/postTypes";
import {
  Box,
  Typography,
  Chip,
  Divider,
  Avatar,
  alpha,
  IconButton,
  Tooltip,
} from "@mui/material";
import ImagesPreview from "./imagesPreview";
import HomeIcon from "@mui/icons-material/Home";
import AccountBalanceWalletIcon from "@mui/icons-material/AccountBalanceWallet";
import MoneyIcon from "@mui/icons-material/Money";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import ScheduleIcon from "@mui/icons-material/Schedule";
import VisibilityIcon from "@mui/icons-material/Visibility";
import PersonIcon from "@mui/icons-material/Person";
import TrendingUpIcon from "@mui/icons-material/TrendingUp";
import WorkIcon from "@mui/icons-material/Work";
import BusinessIcon from "@mui/icons-material/Business";
import FavoriteIcon from "@mui/icons-material/Favorite";
import postApi from "../api/postApi";
import useStore from "../../../app/stores/store";
import userPostApi from "../api/userPostApi";

interface Props {
  post: PostPreview;
}

export default observer(function PostItem({ post }: Props) {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { uiStore, authStore } = useStore();
  const renderPriceAndType = () => {
    const currencySymbol = post.currency ? PostCurrency[post.currency] : "";
    const priceTypeTranslation =
      post.priceType != null ? t(PostPriceType[post.priceType]) : "";

    let priceText = "";
    let icon = MoneyIcon;
    let isSuccessColor = true;

    if (post.postType === PostType.Work) {
      icon = AccountBalanceWalletIcon;
      if (post.minSalary && post.maxSalary) {
        priceText = `${post.minSalary} - ${post.maxSalary}`;
      } else if (post.minSalary) {
        priceText = `${t("from")} ${post.minSalary}`;
      } else if (post.maxSalary) {
        priceText = `${t("upTo")} ${post.maxSalary}`;
      } else {
        priceText = t("negotiable");
      }
    } else if (post.postType === PostType.Rent) {
      priceText = post.price != null ? `${post.price}` : t("negotiable");
    } else {
      if (
        post.price != null &&
        post.price > 0 &&
        post.priceType !== PostPriceType.Free
      ) {
        priceText = `${post.price}`;
      } else {
        priceText = t("Free");
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
        <Box>
          <Typography
            variant="body1"
            sx={{
              fontWeight: 700,
              color: isSuccessColor ? "success.main" : "primary.main",
            }}
          >
            {priceText} {priceText !== t("Free") ? currencySymbol : ""}
            {priceTypeTranslation &&
              priceText !== t("Free") &&
              ` / ${priceTypeTranslation}`}
          </Typography>
        </Box>
      </Box>
    );
  };

  const getPostTypeInfo = () => {
    switch (post.postType) {
      case PostType.Work:
        return { icon: WorkIcon, label: t("work"), color: "primary" };
      case PostType.Rent:
        return { icon: HomeIcon, label: t("rent"), color: "secondary" };
      default:
        return { icon: BusinessIcon, label: t("service"), color: "info" };
    }
  };

  const postTypeInfo = getPostTypeInfo();
  const isPromoted =
    post.postPromotionType !== null &&
    post.postPromotionType !== PostPromotionType.None;

  return (
    <Box
      onClick={() => navigate(`posts/${post.id}`)}
      sx={{
        display: "flex",
        flexDirection: "row",
        gap: 3,
        p: 3,
        backgroundColor: "background.paper",
        borderRadius: "20px",
        border: "1px solid",
        borderColor: isPromoted ? "primary.main" : "divider",
        cursor: "pointer",
        position: "relative",
        overflow: "hidden",
        transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
        "&:hover": {
          transform: "translateY(-6px)",
          boxShadow: (theme) =>
            `0 20px 40px ${alpha(theme.palette.primary.main, 0.15)}`,
          borderColor: "primary.main",
          "& .post-image": {
            transform: "scale(1.05)",
          },
          "& .post-actions": {
            opacity: 1,
            transform: "translateY(0)",
          },
        },
        "&:active": {
          transform: "translateY(-3px)",
          transition: "all 0.1s ease",
        },
      }}
    >
      {/* Promotion Badge */}
      {isPromoted && (
        <Box
          sx={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0,
            height: "4px",
            background: (theme) =>
              `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
            zIndex: 2,
          }}
        />
      )}

      {post.mainImageUrl && (
        <Box
          className="post-image"
          sx={{
            flexShrink: 0,
            overflow: "hidden",
            borderRadius: "16px",
            transition: "transform 0.3s ease",
          }}
        >
          <ImagesPreview mainImageUrl={post.mainImageUrl} postId={post.id} />
        </Box>
      )}

      {/* Content Section */}
      <Box display="flex" flexDirection="column" flex={1} gap={2}>
        {/* Header */}
        <Box
          display="flex"
          justifyContent="space-between"
          alignItems="flex-start"
        >
          <Box flex={1}>
            <Box display="flex" alignItems="center" gap={2} mb={1}>
              <Chip
                icon={<postTypeInfo.icon />}
                label={postTypeInfo.label}
                color={postTypeInfo.color as any}
                size="small"
                variant="outlined"
                sx={{
                  fontWeight: 600,
                  fontSize: "0.75rem",
                }}
              />
              {isPromoted && (
                <Chip
                  icon={<TrendingUpIcon />}
                  label={t(
                    `promotion.${PostPromotionType[post.postPromotionType!]}`
                  )}
                  color="primary"
                  size="small"
                  sx={{
                    fontWeight: 700,
                    fontSize: "0.7rem",
                    textTransform: "uppercase",
                  }}
                />
              )}
            </Box>

            <Typography
              variant="h5"
              sx={{
                fontWeight: 700,
                fontSize: "1.5rem",
                lineHeight: 1.3,
                mb: 1,
                display: "-webkit-box",
                WebkitLineClamp: 2,
                WebkitBoxOrient: "vertical",
                overflow: "hidden",
                color: "text.primary",
              }}
            >
              {post.title}
            </Typography>

            {/* Location */}
            <Box display="flex" alignItems="center" gap={1} mb={2}>
              <LocationOnIcon color="primary" sx={{ fontSize: 18 }} />
              <Typography
                variant="body2"
                color="text.secondary"
                sx={{ fontWeight: 500 }}
              >
                {post.city}
              </Typography>
            </Box>
          </Box>

          {/* Quick Actions */}
          <Box
            className="post-actions"
            display="flex"
            flexDirection="column"
            gap={1}
            sx={{
              opacity: 0,
              transform: "translateY(-10px)",
              transition: "all 0.3s ease",
            }}
          >
            <Tooltip title={t("favorite")}>
              <IconButton
                size="small"
                onClick={async (e) => {
                  e.stopPropagation();
                  try {
                    const isFav = authStore.user?.favPostIds.includes(post.id);
                    if (isFav) {
                      await userPostApi.addFavPost({ id: post.id });
                      authStore.setUser({
                        ...authStore.user!,
                        favPostIds: authStore.user!.favPostIds.filter(
                          (x) => x !== post.id
                        ),
                      });
                      uiStore.showSnackbar(
                        t("postRemovedFromFavorites"),
                        "info"
                      );
                    } else {
                      await userPostApi.addFavPost({ id: post.id });
                      authStore.setUser({
                        ...authStore.user!,
                        favPostIds: [...authStore.user!.favPostIds, post.id],
                      });
                      uiStore.showSnackbar(
                        t("postAddedToFavorites"),
                        "success"
                      );
                    }
                  } catch (error) {
                    uiStore.showSnackbar(t("errorUpdatingFavorites"), "error");
                  }
                }}
                sx={{
                  backgroundColor: alpha("#000", 0.04),
                  color: (theme) =>
                    authStore.user?.favPostIds.includes(post.id)
                      ? theme.palette.error.main
                      : theme.palette.primary,

                  "&:hover": {
                    backgroundColor: alpha("#000", 0.08),
                    transform: "scale(1.1)",
                  },
                }}
              >
                <FavoriteIcon fontSize="small" />
              </IconButton>
            </Tooltip>
          </Box>
        </Box>

        {/* Description */}
        <Typography
          variant="body1"
          color="text.secondary"
          sx={{
            display: "-webkit-box",
            WebkitLineClamp: 2,
            WebkitBoxOrient: "vertical",
            overflow: "hidden",
            lineHeight: 1.6,
            fontSize: "0.95rem",
          }}
        >
          {post.shortDescription}
        </Typography>

        <Divider sx={{ my: 1 }} />

        {/* Footer */}
        <Box display="flex" justifyContent="space-between" alignItems="center">
          {/* Price */}
          {renderPriceAndType()}

          {/* Meta Info */}
          <Box display="flex" alignItems="center" gap={3}>
            <Box display="flex" alignItems="center" gap={0.5}>
              <VisibilityIcon
                sx={{
                  fontSize: 16,
                  color: "text.secondary",
                  opacity: 0.7,
                }}
              />
              <Typography
                variant="caption"
                color="text.secondary"
                sx={{ fontSize: "0.75rem" }}
              >
                {post.viewsCount || 0} {t("views")}
              </Typography>
            </Box>

            <Box display="flex" alignItems="center" gap={0.5}>
              <ScheduleIcon
                sx={{
                  fontSize: 16,
                  color: "text.secondary",
                  opacity: 0.7,
                }}
              />
              <Typography
                variant="caption"
                color="text.secondary"
                sx={{ fontSize: "0.75rem" }}
              >
                {new Date(post.createdAt ?? "").toLocaleDateString()}
              </Typography>
            </Box>

            <Box display="flex" alignItems="center" gap={0.5}>
              <Avatar
                sx={{
                  width: 24,
                  height: 24,
                  backgroundColor: "primary.main",
                  fontSize: "0.75rem",
                }}
              >
                <PersonIcon sx={{ fontSize: 14 }} />
              </Avatar>
              <Typography
                variant="caption"
                color="text.secondary"
                sx={{ fontSize: "0.75rem" }}
              >
                {post.ownerName || t("anonymous")}
              </Typography>
            </Box>
          </Box>
        </Box>
      </Box>
    </Box>
  );
});
