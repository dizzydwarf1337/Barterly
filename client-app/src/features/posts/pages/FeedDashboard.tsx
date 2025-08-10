import {Box, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import useStore from "../../../app/stores/store";
import {useEffect, useState} from "react";
import FeedPostList from "../../home/FeedPostList";
import {useTranslation} from "react-i18next";
import { PostPreview } from "../types/postTypes";
import postApi from "../api/postApi";


export default observer(function FeedPage() {

    const { uiStore } = useStore();
    const {t} = useTranslation();
    const [posts,setPosts] = useState<PostPreview[]>([]);
    useEffect(() => {
        try {
            postApi.getFeed({filterBy:{
                pageNumber:'1',
                pageSize:'10',
            }}).then(response => {
                setPosts(response.value.items);
            });
        } catch {
            
        }
    }, [])

    return (
        <Box width="100%" display="flex" flexDirection="column" gap="20px">
            <Box
                display="flex"
                justifyContent={uiStore.isMobile ? "center" : "flex-start"}
                alignItems="center"
                width="100%"
                pl={!uiStore.isMobile ? "20px" : undefined}
            >
                <Typography variant="h3">
                    {t('feed')}
                </Typography>
            </Box>
            <Box>
                <FeedPostList/>
            </Box>
        </Box>
    )
})

