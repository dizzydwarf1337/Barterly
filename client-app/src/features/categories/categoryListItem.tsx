import { observer } from "mobx-react-lite";
import Category from "../../app/models/category";
import { Box, IconButton, Typography } from "@mui/material";
import useStore from "../../app/stores/store";
import CloseIcon from '@mui/icons-material/Close';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import useAdminStore from "../../app/stores/adminStores/adminStore";
interface Props {
    category:Category
}


export default observer(function CategoryListItem({ category } : Props) {

    const { uiStore, userStore, categoryStore } = useStore();
    const { adminCategoryStore } = useAdminStore();
    const handleDelete= async (categoryId: string) => {
        try {
            await adminCategoryStore.deleteCategory(categoryId);
            categoryStore.setCategories(categoryStore.categories!.filter(x => x.id !== categoryId));
            uiStore.showSnackbar("Category deleted successfully", "success", "right");
        }
        catch {
            uiStore.showSnackbar("Error while deleting category", "error", "right");
        }
    }
    const handleEdit = async (category: Category | null) => {
        adminCategoryStore.openModalFunc(category);
    }
    
    return (
        <Box display="flex"
            sx={{
                backgroundColor: "background.default", width: "150px", height: "40px",
                alignItems: "center", justifyContent: "center", borderRadius: "10px",
                transition: "0.15s ease-out",
                ':hover': {
                    boxShadow: `2px 2px  ${uiStore.theme.palette.primary.contrastText}`,
                    translate:"-2px -2px",
                },
                cursor: "pointer",

            }}>
            <Typography >
                {uiStore.lang === "en" ? category.nameEN : category.namePL}
            </Typography>
            {(userStore.user?.role === "Admin" || userStore.user?.role === "Moderator")
                &&
                <Box display="flex" flexDirection="row" alignItems="center" gap="10px">
    
                    <IconButton size="small" onClick={()=>handleEdit(category!) }>
                        <EditOutlinedIcon fontSize="small" color="warning" />
                    </IconButton>
                    <IconButton size="small" onClick={() => { handleDelete(category.id) }}>
                        <CloseIcon fontSize="small" color="error" />
                    </IconButton>
                </Box>
            }
        </Box>
       
    )
})
