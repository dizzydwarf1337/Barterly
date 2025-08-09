import {Box} from "@mui/material";
import {observer} from "mobx-react-lite";
import CategoriesDashboard from "../categories/categoriesDashboard";
import FeedDashboard from "./FeedDashboard";
import PopularDashboard from "./PopularDashboard";
import useStore from "../../app/stores/store";

export default observer(function MainPage() {

    const {uiStore} = useStore();


    return (
        <>
            <Box display="flex" flexDirection="column" gap="20px">
                <Box display="flex" flexDirection="column" gap="10px">
                    <CategoriesDashboard/>
                </Box>
                <Box display="grid" gridTemplateColumns={!uiStore.isMobile && "3fr 1fr"} gap="20px">
                    <Box display="flex" flexDirection="column" gap="10px">
                        <FeedDashboard/>
                    </Box>
                    {!uiStore.isMobile &&
                        <Box display="flex" flexDirection="column" gap="10px">
                            <PopularDashboard/>
                        </Box>
                    }
                </Box>
            </Box>
        </>
    )
})