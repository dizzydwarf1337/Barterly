import { Box, Typography, useTheme } from "@mui/material";
import { observer } from "mobx-react-lite";
import categoryApi from "../api/categoriesApi";
import { useEffect, useState } from "react";
import { Category } from "../types/categoryTypes";
import CategoryListItem from "./categoryListItem";

export default observer(function CategoryList() {
  const theme = useTheme();
  const isMobile = theme.breakpoints.down("sm");
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await categoryApi.getCategories();
        setCategories(response.value);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <Typography variant="body1">Loading...</Typography>
      </Box>
    );
  }

  if (!categories || categories.length === 0) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <Typography variant="body1">No categories found</Typography>
      </Box>
    );
  }

  return (
    <Box
      display="grid"
      gridTemplateColumns={
        isMobile
          ? "repeat(auto-fill, minmax(120px, 1fr))"
          : "repeat(auto-fill, minmax(200px, 1fr))"
      }
      gap="10px"
    >
      {categories.map((category) => (
        <CategoryListItem key={category.id} category={category} />
      ))}
    </Box>
  );
});