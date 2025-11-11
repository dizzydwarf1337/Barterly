import { observer } from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import { Box, Fade, Typography } from "@mui/material";
import { PostPreview } from "../types/postTypes";
import PostSmallItem from "./PostSmallItem";
import PostItem from "./PostItem";
import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";

interface FeedPostListProps {
  posts: PostPreview[];
}

export default observer(function FeedPostList({ posts }: FeedPostListProps) {
  const { uiStore } = useStore();
  const { t } = useTranslation();
  const [visiblePosts, setVisiblePosts] = useState<PostPreview[]>([]);
  const [_, setAnimationDelay] = useState(0);

  useEffect(() => {
    setVisiblePosts([]);
    setAnimationDelay(0);

    posts.forEach((post, index) => {
      setTimeout(() => {
        setVisiblePosts((prev) => [...prev, post]);
      }, index * 100);
    });
  }, [posts]);

  if (!posts || posts.length === 0) {
    return (
      <Fade in timeout={500}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="center"
          justifyContent="center"
          py={8}
          px={4}
          sx={{
            backgroundColor: "background.paper",
            borderRadius: "20px",
            border: "1px solid",
            borderColor: "divider",
            textAlign: "center",
          }}
        >
          <Typography
            variant="h6"
            color="text.secondary"
            sx={{
              fontWeight: 600,
              mb: 1,
            }}
          >
            {t("noPosts")}
          </Typography>
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{ opacity: 0.7 }}
          >
            {t("noPostsDescription")}
          </Typography>
        </Box>
      </Fade>
    );
  }

  return (
    <Box
      display="flex"
      flexDirection="column"
      gap={{ xs: "12px", sm: "16px" }}
      sx={{
        width: "100%",
        position: "relative",
      }}
    >
      {posts.map((post, index) => {
        const isVisible = visiblePosts.some((vp) => vp.id === post.id);

        return (
          <Fade
            key={post.id}
            in={isVisible}
            timeout={600}
            style={{
              transitionDelay: isVisible ? `${index * 50}ms` : "0ms",
            }}
          >
            <Box
              sx={{
                transform: isVisible ? "translateY(0)" : "translateY(20px)",
                transition: "transform 0.6s cubic-bezier(0.4, 0, 0.2, 1)",
                transitionDelay: `${index * 50}ms`,
              }}
            >
              {uiStore.isMobile ? (
                <PostSmallItem post={post} />
              ) : (
                <PostItem post={post} />
              )}
            </Box>
          </Fade>
        );
      })}

      {visiblePosts.length < posts.length && (
        <Box display="flex" justifyContent="center" py={2}>
          <Box
            sx={{
              width: 32,
              height: 32,
              border: "3px solid",
              borderColor: "primary.main",
              borderTopColor: "transparent",
              borderRadius: "50%",
              animation: "spin 1s linear infinite",
              "@keyframes spin": {
                "0%": { transform: "rotate(0deg)" },
                "100%": { transform: "rotate(360deg)" },
              },
            }}
          />
        </Box>
      )}
    </Box>
  );
});
