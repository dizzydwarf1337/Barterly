import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import {
  Box,
  CircularProgress,
  Typography,
  IconButton,
  Menu,
  MenuItem,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  FormControl,
  FormLabel,
  RadioGroup,
  Radio,
  FormControlLabel,
  Divider,
} from "@mui/material";
import { MoreVert } from "@mui/icons-material";
import { useTranslation } from "react-i18next";
import useStore from "../../../app/stores/store";
import { PostPreview, PostPromotionType } from "../../posts/types/postTypes";
import userPostApi from "../../posts/api/userPostApi";
import PostItem from "../../posts/components/PostItem";

const UserPostsPage = () => {
  const [posts, setPosts] = useState<PostPreview[]>([]);
  const [loading, setLoading] = useState(false);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedPost, setSelectedPost] = useState<PostPreview | null>(null);
  const [promotionDialogOpen, setPromotionDialogOpen] = useState(false);
  const [promotionType, setPromotionType] = useState<PostPromotionType>(PostPromotionType.Highlight);
  const { uiStore } = useStore();
  const { t } = useTranslation();

  const loadPosts = async () => {
    setLoading(true);
    try {
      const result = await userPostApi.getMyPosts();
      if (result.isSuccess) {
        setPosts(result.value);
      } else {
        uiStore.showSnackbar(
          result.error || t("common:ErrorOccurred"),
          "error"
        );
      }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadPosts();
  }, []);

  const handleMenuOpen = (
    event: React.MouseEvent<HTMLElement>,
    post: PostPreview
  ) => {
    setAnchorEl(event.currentTarget);
    setSelectedPost(post);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleBuyPromotion = () => {
    setPromotionDialogOpen(true);
    handleMenuClose();
  };

  const handlePromotionDialogClose = () => {
    setPromotionDialogOpen(false);
  };

  const handlePromotionSubmit = async () => {
    if (!selectedPost) return;
    try{
      await userPostApi.buyPromotion({postPromotionType: promotionType as PostPromotionType, postId: selectedPost!.id});
      const updated = { ...selectedPost, postPromotionType: promotionType };
      setSelectedPost(updated);
      setPosts(posts.map(p => p.id === selectedPost.id ? updated : p));
      uiStore.showSnackbar(t("user:PromotionPurchased"), "success");
      handlePromotionDialogClose();
    }
    catch (err){
      console.log(err);
      uiStore.showSnackbar(t("user:PromotionPurchaseFailure"), "error");
    }
  };

  const handleToggleVisibility = async () => {
    if (!selectedPost) return;
    try {
      const result = await userPostApi.changePostVisibility(selectedPost.id);
      if(result.isSuccess){
        uiStore.showSnackbar(
          t(selectedPost.isHidden ? "user:PostShown" : "user:PostHidden"),
          "success"
        );
        const updated = { ...selectedPost, isHidden: !selectedPost.isHidden };

        setSelectedPost(updated);
        setPosts(posts.map(p => p.id === selectedPost.id ? updated : p));
    }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    }
    handleMenuClose();
  };

  const handleDeletePost = async () => {
    if (!selectedPost) return;
    try {
      const result = await userPostApi.deletePost(selectedPost.id);
      if(result.isSuccess){
        uiStore.showSnackbar(t("user:PostDeleted"), "success");
        setPosts((posts.filter(x => x.id !== selectedPost.id)));
      }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    }
    handleMenuClose();
  };

  const handleEditPost = () => {
    if (!selectedPost) return;
    // TODO: Navigate to edit post page or open edit dialog
    console.log("Editing post:", selectedPost.id);
    handleMenuClose();
  };

  if (loading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="400px"
      >
        <CircularProgress />
      </Box>
    );
  }

  const visiblePosts = posts.filter(p => !p.isHidden);
  const hiddenPosts  = posts.filter(p => p.isHidden);


  if (posts.length === 0) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="400px"
      >
        <Typography variant="h6" color="text.secondary">
          {t("user:NoPosts")}
        </Typography>
      </Box>
    );
  }

  const renderPostSection = (
    sectionPosts: PostPreview[],
    titleKey: string
  ) => {
    if (sectionPosts.length === 0) return null;

    return (
      <Box mb={4}>
        <Typography variant="h4" gutterBottom sx={{ mb: 2, color: (theme) => theme.palette.primary.main }}>
          {t(titleKey)}
        </Typography>
        {sectionPosts.map((post) => (
          <Box key={post.id} position="relative" mb={2}>
            <PostItem post={post} />
            <IconButton
              sx={{
                position: "absolute",
                top: "40%",
                right: 8,
              }}
              onClick={(e) => handleMenuOpen(e, post)}
            >
              <MoreVert />
            </IconButton>
          </Box>
        ))}
      </Box>
    );
  };

  return (
    <Box>
      {renderPostSection(visiblePosts, "user:VisiblePosts")}
      
      {visiblePosts.length > 0 && hiddenPosts.length > 0 && (
        <Divider sx={{ my: 4 }} />
      )}
      
      {renderPostSection(hiddenPosts, "user:HiddenPosts")}

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleBuyPromotion}>
          {t("user:BuyPromotion")}
        </MenuItem>
        <MenuItem onClick={handleEditPost}>{t("common:Edit")}</MenuItem>
        <MenuItem onClick={handleToggleVisibility}>
          {selectedPost?.isHidden
            ? t("user:ShowPost")
            : t("user:HidePost")}
        </MenuItem>
        <MenuItem onClick={handleDeletePost}>{t("delete")}</MenuItem>
      </Menu>

      <Dialog
        open={promotionDialogOpen}
        onClose={handlePromotionDialogClose}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>{t("user:SelectPromotionType")}</DialogTitle>
        <DialogContent>
          <FormControl component="fieldset" sx={{ width: "100%", mt: 2 }}>
            <FormLabel component="legend">{t("user:PromotionType")}</FormLabel>
            <RadioGroup
              value={promotionType}
              onChange={(e) => setPromotionType(Number(e.target.value) as PostPromotionType)}
            >
              <FormControlLabel
                value={PostPromotionType.Highlight}
                control={<Radio />}
                label={
                  <Box>
                    <Typography variant="body1">
                      {t("user:StandardPromotion")}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {t("user:StandardPromotionDescription")}
                    </Typography>
                  </Box>
                }
              />
              <FormControlLabel
                value={PostPromotionType.Top}
                control={<Radio />}
                label={
                  <Box>
                    <Typography variant="body1">
                      {t("user:PremiumPromotion")}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {t("user:PremiumPromotionDescription")}
                    </Typography>
                  </Box>
                }
              />
            </RadioGroup>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={handlePromotionDialogClose}>
            {t("common:Cancel")}
          </Button>
          <Button
            onClick={handlePromotionSubmit}
            variant="contained"
            color="primary"
          >
            {t("user:Purchase")}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default observer(UserPostsPage);