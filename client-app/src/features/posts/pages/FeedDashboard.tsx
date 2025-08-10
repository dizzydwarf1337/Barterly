import { Box, Typography, Skeleton, Alert, Fade } from "@mui/material";
import { observer } from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { PostPreview } from "../types/postTypes";
import postApi from "../api/postApi";
import FeedPostList from "../components/feedPostList";
import TrendingUpIcon from "@mui/icons-material/TrendingUp";

export default observer(function FeedDashboard() {
  const { uiStore } = useStore();
  const { t } = useTranslation();
  const [posts, setPosts] = useState<PostPreview[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFeedPosts = async () => {
      try {
        setLoading(true);
        setError(null);
        const response = await postApi.getFeed({
          filterBy: {
            pageNumber: "1",
            pageSize: "10",
          },
        });
        setPosts(response.value.items);
      } catch (error) {
        console.error("Failed to fetch feed posts:", error);
        const errorMessage = t("failedToLoadPosts");
        setError(errorMessage);
        uiStore.showSnackbar(errorMessage, "error", "right");
      } finally {
        setLoading(false);
      }
    };

    fetchFeedPosts();
  }, []);

  const renderSkeletons = () => (
    <Box display="flex" flexDirection="column" gap="16px">
      {Array.from({ length: 5 }).map((_, index) => (
        <Box
          key={index}
          sx={{
            backgroundColor: "background.paper",
            borderRadius: "16px",
            p: { xs: 2, sm: 3 },
            border: "1px solid",
            borderColor: "divider",
          }}
        >
          <Box display="flex" gap={2}>
            {!uiStore.isMobile && (
              <Skeleton
                variant="rounded"
                width={150}
                height={150}
                sx={{ borderRadius: "12px", flexShrink: 0 }}
              />
            )}
            <Box flex={1} display="flex" flexDirection="column" gap={1}>
              <Box display="flex" justifyContent="space-between" alignItems="flex-start">
                <Skeleton variant="text" width="60%" height={32} />
                <Skeleton variant="text" width="80px" height={24} />
              </Box>
              <Skeleton variant="text" width="40%" height={20} />
              <Skeleton variant="text" width="100%" height={16} />
              <Skeleton variant="text" width="80%" height={16} />
              <Box display="flex" justifyContent="space-between" alignItems="center" mt={1}>
                <Skeleton variant="text" width="120px" height={24} />
                <Skeleton variant="text" width="100px" height={24} />
              </Box>
            </Box>
          </Box>
        </Box>
      ))}
    </Box>
  );

  return (
    <Box width="100%" display="flex" flexDirection="column" gap="24px">
      {/* Header Section */}
      <Fade in timeout={600}>
        <Box
          display="flex"
          justifyContent="space-between"
          alignItems="center"
          width="100%"
          px={{ xs: 2, sm: 3 }}
          py={1}
        >
          <Box display="flex" alignItems="center" gap={2}>
            <TrendingUpIcon 
              color="primary" 
              sx={{ 
                fontSize: { xs: 28, sm: 32 },
                filter: 'drop-shadow(0 2px 4px rgba(0,0,0,0.1))'
              }} 
            />
            <Box>
              <Typography 
                variant="h3" 
                sx={{
                  background: (theme) => 
                    `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                  backgroundClip: 'text',
                  WebkitBackgroundClip: 'text',
                  WebkitTextFillColor: 'transparent',
                  fontWeight: 700,
                  fontSize: { xs: '1.75rem', sm: '2.125rem' }
                }}
              >
                {t("feed")}
              </Typography>
              <Typography 
                variant="body2" 
                color="text.secondary"
                sx={{ mt: -0.5, fontSize: '0.875rem' }}
              >
                {posts.length > 0 && !loading && `${posts.length} ${t("posts", { count: posts.length })}`}
              </Typography>
            </Box>
          </Box>
          
        </Box>
      </Fade>

      <Box px={{ xs: 1, sm: 0 }}>
        {error ? (
          <Fade in timeout={400}>
            <Alert 
              severity="error" 
              sx={{ 
                borderRadius: "12px",
                border: "1px solid",
                borderColor: "error.light"
              }}
            >
              {error}
            </Alert>
          </Fade>
        ) : loading ? (
          <Fade in timeout={300}>
            <Box>{renderSkeletons()}</Box>
          </Fade>
        ) : posts.length === 0 ? (
          <Fade in timeout={500}>
            <Alert 
              severity="info" 
              sx={{ 
                borderRadius: "12px",
                border: "1px solid",
                borderColor: "info.light"
              }}
            >
              {t("noPosts")}
            </Alert>
          </Fade>
        ) : (
          <Fade in timeout={600}>
            <FeedPostList posts={posts} />
          </Fade>
        )}
      </Box>
    </Box>
  );
});