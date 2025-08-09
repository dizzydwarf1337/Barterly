import {Box, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import useStore from "../../app/stores/store";
import {useEffect} from "react";
import FeedPostList from "./FeedPostList";
import {useTranslation} from "react-i18next";


export default observer(function FeedDashboard() {

    const {userStore, postStore, uiStore} = useStore();
    const {t} = useTranslation();
    useEffect(() => {
        try {
            postStore.getFeedApi();
        } catch (e: Error) {
            uiStore.showSnackbar(e.Message, "error", "right");
        }
    }, [postStore.feedPage, userStore.user!]);

    return (
        <Box width="100%" display="flex" flexDirection="column" gap="20px">
            <Box
                display="flex"
                justifyContent={uiStore.isMobile ? "center" : "flex-start"}
                alignItems="center"
                width="100%"
                pl={!uiStore.isMobile && "20px"}
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

