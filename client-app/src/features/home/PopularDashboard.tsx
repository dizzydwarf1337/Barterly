import {Box, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import useStore from "../../app/stores/store";
import {useEffect} from "react";
import {useTranslation} from "react-i18next";
import PopularPostList from "./PopularPostList";


export default observer(function PopularDashboard() {

    const {postStore, uiStore} = useStore();
    const {t} = useTranslation();
    useEffect(() => {
        try {
            postStore.getPopularPostsApi(5);
        } catch (e: Error) {
            uiStore.showSnackbar(e.Message, "error", "right");
        }
    }, [postStore.feedPage]);

    return (
        <Box pl="20px" display="flex" flexDirection="column" gap="25px">
            <Box>
                <Typography variant="h3">
                    {t('popular')}
                </Typography>
            </Box>
            <Box>
                <PopularPostList/>
            </Box>
        </Box>
    )
})

