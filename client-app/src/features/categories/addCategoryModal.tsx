import { Box, Button, Dialog, FormControl, FormHelperText, IconButton, Input, InputLabel, Typography } from "@mui/material";
import Category from "../../app/models/category";
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import SubCategory from "../../app/models/subCategory";
import { v4 as uuid } from 'uuid';
import { useEffect, useState } from "react";
import useStore from "../../app/stores/store";
import useAdminStore from "../../app/stores/adminStores/adminStore";
import { observer } from "mobx-react-lite";
interface Props {
    open: boolean,
    onClose: () => void;
    category:Category | null,
}

export default observer(function AddCategoryModal({open, onClose, category} : Props) {


    const { categoryStore, uiStore } = useStore();
    const { adminCategoryStore } = useAdminStore();
    const [error, setError] = useState<string>("");
    const [categoryToAdd, setCategoryToAdd] = useState<Category>( {
        id: uuid(),
        nameEN: "",
        namePL: "",
        subCategories: [],
        description:"",
    });
    const [subCategoriesToAdd, setSubCategories] = useState<SubCategory[]>([]);
    useEffect(() => {
        if (category) {
            setCategoryToAdd(category);
            setSubCategories(category.subCategories || []);
        } else {
            setCategoryToAdd({
                id: uuid(),
                nameEN: "",
                namePL: "",
                subCategories: [],
                description: "",
            });
            setSubCategories([]);
        }
    }, [category]);
   
    const handleSubCategoryChange = (index: number, field: keyof SubCategory, value: string) => {
        setSubCategories(prev =>
            prev.map((sub, i) =>
                i === index ? { ...sub, [field]: value } : sub
            )
        );
    };
    const handleCategoryChange = ({ target }: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = target;
        setCategoryToAdd(prev => ({
            ...prev,
            [name]: value
        }));
    };
    const handleSubmit = async () => {
        const updatedCategory = {
            ...categoryToAdd,
            subCategories: subCategoriesToAdd,
        };
        if (categoryToAdd.nameEN.trim() === "" || categoryToAdd.namePL.trim() === "") {
            setError("Name PL and Name EN field is required");
            return;
        }
        try {
            if (!category) {
                await adminCategoryStore.addCategory(updatedCategory);
                uiStore.showSnackbar("Category added", "success", "right");
                categoryStore.setCategories([...categoryStore.categories!, categoryToAdd]);
            }
            else {
                await adminCategoryStore.editCategory(updatedCategory);
                uiStore.showSnackbar("Category updated", "success", "right");
                categoryStore.setCategories([
                    ...categoryStore.categories!.filter(x => x.id !== updatedCategory.id),
                    updatedCategory
                ]);
            }

            
        }
        catch (error) {
            console.log(error);
            uiStore.showSnackbar("Error while adding category", "error", "right")
        }
        finally {
            onClose();
            setSubCategories([]);
            setCategoryToAdd(({
                id: uuid(),
                nameEN: "",
                namePL: "",
                subCategories: [],
                description: "",
            }));
            setError("");
        }
    };
    return (
        <Dialog open={open} onClose={onClose} fullWidth>
            <Box display="flex" flexDirection="column" padding="20px" gap="20px" sx={{backgroundColor:"background.default"} }>
                <Box display="flex" justifyContent="center">
                    <Typography variant="h4">
                        Add Category
                    </Typography>
                </Box>
                    <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "center", padding: "20px", gap: "20px", backgroundColor: "background.paper" }}>
                        <Box sx={{ display: "flex", flexDirection: "column",width:"100%", padding: "20px", gap: "20px", backgroundColor: "background.paper" }}>
                        <FormControl fullWidth>
                            <InputLabel htmlFor="nameEN-input" required sx={{ color: "primary.contrastText" }}>
                               Name EN
                            </InputLabel>
                            <Input id="nameEn-input" name="nameEN" onChange={(e) => handleCategoryChange(e)} value={categoryToAdd.nameEN} aria-describedby="nameEn-helper" sx={{ color: "primary.contrastText" }} />
                            <FormHelperText id="nameEn=hepler" error={true}>{error}</FormHelperText>
                        </FormControl>
                        <FormControl fullWidth>
                            <InputLabel htmlFor="namePl-input" required sx={{ color: "primary.contrastText" }}>
                              Name PL
                            </InputLabel>
                            <Input id="namePl-input" name="namePL" onChange={(e) => handleCategoryChange(e)} value={categoryToAdd.namePL} aria-describedby="namePl-helper" sx={{ color: "primary.contrastText" }} />
                            <FormHelperText id="namePl-helper" error={true}>{error}</FormHelperText>
                        </FormControl>
                        <FormControl fullWidth>
                            <InputLabel htmlFor="desc-input" sx={{ color: "primary.contrastText" }}>
                              Description
                            </InputLabel>
                            <Input id="desc-input" name="description" onChange={(e) => handleCategoryChange(e)} value={categoryToAdd.description} sx={{ color: "primary.contrastText" }}   />
                        </FormControl>
                    </Box>
                    <Box display="flex" justifyContent="flex-end" alignItems="center" gap="10px" width="100%" >
                        <Typography variant="subtitle2">Add SubCategory</Typography>
                        <IconButton onClick={() => setSubCategories(prev => [...prev, { categoryId: categoryToAdd.id, titleEN: "", titlePL: "", id: uuid() }])}>
                            <AddIcon color="success"/>
                        </IconButton>
                    </Box>
                    <Box display="flex" flexDirection="column" gap="20px">
                        {subCategoriesToAdd.map((sub, index) => (
                            <Box key={sub.id} display="flex" flexDirection="row" justifyContent="center" alignItems="center" gap="20px">
                                <FormControl required>
                                    <InputLabel htmlFor={`subCategory-namePl-${index}`} sx={{ color: "primary.contrastText" }}>Sub category {index + 1} name PL</InputLabel>
                                    <Input sx={{ color: "primary.contrastText" }}
                                        id={`subCategory-namePl-${index}`}
                                        value={sub.titlePL}
                                        onChange={(e) => handleSubCategoryChange(index, "titlePL", e.target.value)}

                                    />
                                </FormControl>
                                <FormControl required sx={{color:"primary.contrastText"} }>
                                    <InputLabel sx={{ color: "primary.contrastText" }}>Sub category {index + 1} name EN</InputLabel>
                                    <Input sx={{ color: "primary.contrastText" }}
                                        id={`subCategory-nameEn-${index}`}
                                        value={sub.titleEN}
                                        
                                        onChange={(e) => handleSubCategoryChange(index, "titleEN", e.target.value)}

                                    />
                                </FormControl>
                                <IconButton onClick={() =>
                                    setSubCategories(prev => prev.filter((_, i) => i !== index))
                                }>
                                    <RemoveIcon color="error" />
                                </IconButton>
                            </Box>
                        ))}

                    </Box>
                    <Box display="flex" flexDirection="row" justifyContent="space-between" width="100%">
                        <Button variant="contained" color="success" loading={categoryStore.categoryLoading} onClick={() => handleSubmit()}>Submit</Button>
                        <Button variant="outlined" color="error" onClick={() => {
                            onClose(); setSubCategories([]), setError(""); setCategoryToAdd(({
                                id: uuid(),
                                nameEN: "",
                                namePL: "",
                                subCategories: [],
                                description: "",
                            }));
                        }}
                        >
                            Cancel
                        </Button>
                    </Box>
                </Box>
            </Box>
        </Dialog>
    );
})

