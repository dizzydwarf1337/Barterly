import { observer } from "mobx-react-lite";
import useStore from "../../app/stores/store";
import PostItem from "../posts/PostItem";
import { Box } from "@mui/material";
import PostSmallItem from "../posts/PostSmallItem";

export default observer(function FeedPostList() {


    const { postStore, uiStore } = useStore();

    return (
        <>
            <Box display="flex" flexDirection="column" gap="10px">
                {postStore.feedPosts.map((post) => (
                    <Box>
                        {uiStore.isMobile ?
                            <PostSmallItem key={post.id} post={post} />
                            : (
                                <PostItem key={post.id} post={post} />
                            )}
                    </Box>
                ))}
            </Box>
        </>
    )
})