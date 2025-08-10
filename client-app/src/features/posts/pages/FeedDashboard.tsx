import { Box, Typography } from "@mui/material";
import { observer } from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { PostPreview } from "../types/postTypes";
import postApi from "../api/postApi";
import FeedPostList from "../components/feedPostList";

export default observer(function FeedDashboard() {
  const { uiStore } = useStore();
  const { t } = useTranslation();
  const [posts, setPosts] = useState<PostPreview[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchFeedPosts = async () => {
      try {
        setLoading(true);
        const response = await postApi.getFeed({
          filterBy: {
            pageNumber: "1",
            pageSize: "10",
          },
        });
        setPosts(response.value.items);
      } catch (error) {
        console.error("Failed to fetch feed posts:", error);
        uiStore.showSnackbar(t("failedToLoadPosts"), "error", "right");
      } finally {
        setLoading(false);
      }
    };

    fetchFeedPosts();
  }, []);

  return (
    <Box width="100%" display="flex" flexDirection="column" gap="20px">
      <Box
        display="flex"
        justifyContent={uiStore.isMobile ? "center" : "flex-start"}
        alignItems="center"
        width="100%"
        pl={!uiStore.isMobile ? "20px" : undefined}
      >
        <Typography variant="h3">{t("feed")}</Typography>
      </Box>
      <Box>
        {loading ? (
          <Typography>{t("loading")}...</Typography>
        ) : (
          <FeedPostList posts={posts} />
        )}
      </Box>
    </Box>
  );
});
