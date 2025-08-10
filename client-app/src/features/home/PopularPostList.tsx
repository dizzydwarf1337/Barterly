import {observer} from "mobx-react-lite";
import {Box} from "@mui/material";
import PostSmallItem from "../posts/components/PostSmallItem";
import { useEffect, useState } from "react";
import { PostPreview } from "../posts/types/postTypes";

export default observer(function PopularPostList() {

    const [posts,setPosts] = useState<PostPreview[]>([]);

    useEffect(() => {
        setPosts(postStore.popularPosts);
    }, [postStore.popularPosts]);

    return (
        <>
            <Box display="flex" flexDirection="column" gap="10px">
                {postStore.popularPosts.map((post) => (
                    <Box>
                        <PostSmallItem key={post.id} post={post}/>
                    </Box>
                ))}
            </Box>
        </>
    )
})