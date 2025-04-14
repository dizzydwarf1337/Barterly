import { observer } from "mobx-react-lite";
import Category from "../../app/models/category";
import { Box, IconButton, Typography } from "@mui/material";
import useStore from "../../app/stores/store";
import CloseIcon from '@mui/icons-material/Close';
import EditIcon from '@mui/icons-material/Edit';
interface Props {
    category:Category
}


export default observer(function CategoryListItem({ category } : Props) {

    const { uiStore, userStore, categoryStore } = useStore();

    const handleDeleteIcon = async (categoryId:string) => {
        try {
            await categoryStore.deleteCategory(categoryId);
            uiStore.showSnackbar("Category deleted successfully", "success", "right");
        }
        catch {
            uiStore.showSnackbar("Error while deleting category", "error", "right");
        }
    }
    return (
        <Box display="flex"
            sx={{
                backgroundColor: "background.default", width: "150px", height: "40px",
                alignItems: "center", justifyContent: "center", borderRadius: "10px",
                transition: "0.2s ease-out",
                ':hover': {
                    boxShadow: `2px 2px  ${uiStore.theme.palette.primary.contrastText}`
                },
                cursor: "pointer",
                position:"relative"
            }}>
            <Typography >
                {uiStore.lang === "en" ? category.nameEN : category.namePL}
            </Typography>
            {(userStore.user?.role === "Admin" || userStore.user?.role === "Moderator")
                &&
                <Box display="flex" flexDirection="row" position="absolute" top="-5px" right="10px" gap="20px">
                    <Box display="flex" flexDirection="row" width="10px" height="10px" borderRadius="50%"  justifyContent="center" alignItems="center">
                        <IconButton size="small">
                            <EditIcon fontSize="small" color="warning" />
                        </IconButton>
                    </Box>
                    <Box display="flex" flexDirection="row" width="10px" height="10px" borderRadius="50%" justifyContent="center" alignItems="center">
                        <IconButton size="small" onClick={() => { handleDeleteIcon(category.id) }}>
                            <CloseIcon fontSize="small" color="error" />
                        </IconButton>
                    </Box>
                </Box>
            }
        </Box>
       
    )
})
