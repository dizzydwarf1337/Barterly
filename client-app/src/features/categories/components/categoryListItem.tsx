import {observer} from "mobx-react-lite";
import {Box, Typography} from "@mui/material";
import useStore from "../../../app/stores/store";
import { Category } from "../types/categoryTypes";

interface Props {
    category: Category
}


export default observer(function CategoryListItem({category}: Props) {

    const {uiStore} = useStore();

    return (
        <Box display="flex"
             sx={{
                 backgroundColor: "background.default", width: "150px", height: "40px",
                 alignItems: "center", justifyContent: "center", borderRadius: "10px",
                 transition: "0.15s ease-out",
                 ':hover': {
                     boxShadow: `0px 2px 4px ${uiStore.theme.palette.primary.contrastText}`,
                     translate: '0px -2px'
                 },
                 cursor: "pointer",

             }}>
            <Typography>
                {uiStore.lang === "en" ? category.nameEN : category.namePL}
            </Typography>
        </Box>

    )
})
