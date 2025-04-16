import { Box, Dialog, Typography } from "@mui/material";
import Category from "../../app/models/category";

interface Props {
    open: boolean,
    onClose: ()=>void;
}


export default function AddCategoryModal({open, onClose} : Props) {

    const categoryToAdd: Category = {
        id: "",
        nameEN: "",
        namePL: "",
        subCategories: [],
        description:"",
    }
   

    return (
        <Dialog open={open} onClose={onClose} sx={{ width: "100%", height: "100%", padding:"20px" }}>
            <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "center", alignItems: "center", padding:"20px" }}>
                    
            </Box>
        </Dialog>
    );
}