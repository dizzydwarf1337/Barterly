import {observer} from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import {Box} from "@mui/material";
import { PostPreview } from "../types/postTypes";
import PostSmallItem from "./PostSmallItem";
import PostItem from "./PostItem";

interface FeedPostListProps {
    posts: PostPreview[];
}

export default observer(function FeedPostList({ posts }: FeedPostListProps) {
    const {uiStore} = useStore();

    return (
        <Box display="flex" flexDirection="column" gap="10px">
            {(posts ?? []).map((post) => (
                <Box key={post.id}>
                    {uiStore.isMobile ?
                        <PostSmallItem post={post}/>
                        : (
                            <PostItem post={post}/>
                        )}
                </Box>
            ))}
        </Box>
    )
})