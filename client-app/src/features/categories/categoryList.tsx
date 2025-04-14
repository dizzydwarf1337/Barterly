import { Box, Collapse, Typography } from "@mui/material"
import { observer } from "mobx-react-lite"
import useStore from "../../app/stores/store";
import CategoryListItem from "./categoryListItem";
import { TransitionGroup } from "react-transition-group";

export default observer(function CategoryList() {

    const { categoryStore, uiStore,userStore } = useStore();

    return (

        <Box >
            <TransitionGroup>
                {categoryStore.categories && !categoryStore.categoryLoading
                    ? (
                        <Box
                            display="grid"
                            gridTemplateColumns="repeat(auto-fill, minmax(200px, 1fr))" 
                            gap="10px"
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
                        <Box display="flex" mt="10px"
                            sx={{
                                backgroundColor: "success.main", width: "150px", height: "40px",
                                alignItems: "center", justifyContent: "center", borderRadius: "10px",
                                transition: "0.4s ease-out",
                                ':hover': {
                                    boxShadow: `2px 2px  ${uiStore.theme.palette.primary.contrastText}`
                                },
                                cursor: "pointer"
                            }}>
                            <Typography >
                                Add category
                            </Typography>
                        </Box>
                    </Collapse>
                }
            </TransitionGroup>
       </Box>
       
    )

})