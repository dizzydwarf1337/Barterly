import {observer} from "mobx-react-lite";
import useStore from "../../app/stores/store";
import PostSmallItem from "../posts/PostSmallItem";
import {Box} from "@mui/material";

export default observer(function PopularPostList() {


    const {postStore} = useStore();

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