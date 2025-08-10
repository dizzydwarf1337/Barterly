import {observer} from "mobx-react-lite";
import useStore from "../../app/stores/store";
import {Box} from "@mui/material";
import PostSmallItem from "../posts/components/PostSmallItem";
import PostItem from "../posts/components/PostItem";

export default observer(function FeedPostList() {


    const {uiStore} = useStore();

    return (
        <>
            <Box display="flex" flexDirection="column" gap="10px">
                {postStore.feedPosts.map((post) => (
                    <Box>
                        {uiStore.isMobile ?
                            <PostSmallItem key={post.id} post={post}/>
                            : (
                                <PostItem key={post.id} post={post}/>
                            )}
                    </Box>
                ))}
            </Box>
        </>
    )
})