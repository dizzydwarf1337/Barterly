import { Box, Collapse, Typography } from "@mui/material"
import { observer } from "mobx-react-lite"
import useStore from "../../app/stores/store";
import CategoryListItem from "./categoryListItem";
import { TransitionGroup } from "react-transition-group";
import { lazy, Suspense } from "react";
import useAdminStore from "../../app/stores/adminStores/adminStore";


const AddCategoryModal = lazy(() => import('./addCategoryModal'));


export default observer(function CategoryList() {

    const { categoryStore, uiStore,userStore } = useStore();
    const { adminCategoryStore } = useAdminStore();
    const handleOpenModal = () => {
        adminCategoryStore.setOpenModal(true);
    }
    const handleCloseModal = () => {
        adminCategoryStore.setOpenModal(false);
    }
    return (
        <>
        <Box >
            <TransitionGroup>
                    {categoryStore.categories && !categoryStore.categoryLoading
                        ? (
                            <Box
                                display="grid"
                                gridTemplateColumns={uiStore.isMobile ? "repeat(auto-fill, minmax(120px, 1fr))" : "repeat(auto-fill, minmax(200px, 1fr))"}
                                gap= "10px"
                        >
                            {categoryStore.categories!.map(category => (
                                <Collapse key={category.id} in={true}>
                                    <CategoryListItem category={category} />
                                </Collapse>
                            ))}
                        </Box>
                    )
            : (
                <Typography variant="body1">Loading...</Typography>
                    )}
                {(userStore.user?.role === "Admin" || userStore.user?.role === "Moderator") &&
                        <Collapse>
                            <Box display="flex" mt="10px" onClick={handleOpenModal} 
                                sx={{
                                
                                    backgroundColor: "success.main", width: "150px", height: "40px",
                                    alignItems: "center", justifyContent: "center", borderRadius: "12px",
                                    transition: "0.2s ease-out",
                                    ':hover': {
                                        boxShadow: `0px 2px 4px ${uiStore.theme.palette.primary.contrastText}`,
                                        translate:'0px -2px'
                                    },
                                    cursor: "pointer"
                                }}>
                                <Typography>
                                    Add category
                                </Typography>
                          
                            </Box>  
                        </Collapse>          
                    }
                </TransitionGroup>
                <Suspense>
                    <AddCategoryModal open={adminCategoryStore.openModal} onClose={handleCloseModal} category={adminCategoryStore.category} />    
                </Suspense>
            </Box>
           
        </>
    )
    
})