import { useEffect, useState } from "react";
import { Box, Skeleton, Alert } from "@mui/material";
import { observer } from "mobx-react-lite";

import { GetPostResponseDto } from "../dto/postsDto";
import postsApi from "../api/adminPostApi";
import useStore from "../../../../app/stores/store";
import { useParams } from "react-router";
import { PostOwner } from "../components/postOwner";
import { Post } from "../components/post";
import { PostSettings } from "../components/postSettings";
import { PostOpinions } from "../components/postOpinions";

export const PostPage = observer(() => {
  const { uiStore } = useStore();
  const params = useParams();
  const [postResponse, setPostResponse] = useState<GetPostResponseDto | null>(
    null
  );
  const [loading, setLoading] = useState(true);

  const fetchPost = async () => {
    try {
      setLoading(true);
      const response = await postsApi.getPost(params.id ?? "");
      if (response.isSuccess) {
        setPostResponse(response.value);
      }
    } catch {
      uiStore.showSnackbar("Failed to load post, check id", "error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPost();
  }, [params]);

  if (loading) {
    return (
      <Box sx={{ maxWidth: 1200, mx: "auto", p: 3 }}>
        <Skeleton variant="text" width={200} height={40} sx={{ mb: 2 }} />
        <Skeleton
          variant="rectangular"
          height={300}
          sx={{ mb: 3, borderRadius: "16px" }}
        />
        <Box
          display="grid"
          gridTemplateColumns={{ xs: "1fr", md: "2fr 1fr" }}
          gap={3}
        >
          <Box>
            <Skeleton
              variant="rectangular"
              height={200}
              sx={{ mb: 2, borderRadius: "16px" }}
            />
            <Skeleton
              variant="rectangular"
              height={150}
              sx={{ borderRadius: "16px" }}
            />
          </Box>
          <Skeleton
            variant="rectangular"
            height={200}
            sx={{ borderRadius: "16px" }}
          />
        </Box>
      </Box>
    );
  }

  if (!postResponse) {
    return (
      <Box sx={{ maxWidth: 1200, mx: "auto", p: 3 }}>
        <Alert severity="error" sx={{ borderRadius: "12px" }}>
          Post not found
        </Alert>
      </Box>
    );
  }

  return (
    <Box sx={{ maxWidth: 1200, mx: "auto", p: 3 }}>
      <Post post={postResponse.post} />
      <Box sx={{ mt: 3 }}>
        <PostOwner owner={postResponse.owner} />
      </Box>
      <Box sx={{ mt: 3 }}>
        <PostSettings settings={postResponse.settings} onUpdate={fetchPost} />
      </Box>
      <Box sx={{ mt: 3 }}>
        <PostOpinions opinions={postResponse.opinions} />
      </Box>
    </Box>
  );
});

export default PostPage;
