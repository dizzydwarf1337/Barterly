import { Box, Collapse, Typography, useTheme } from "@mui/material";
import { observer } from "mobx-react-lite";
import { TransitionGroup } from "react-transition-group";
import categoryApi from "../api/categoriesApi";
import { useEffect, useState } from "react";
import { Category } from "../types/categoryTypes";
import CategoryListItem from "./categoryListItem";

export default observer(function CategoryList() {
  const theme = useTheme();
  const isMobile = theme.breakpoints.down("sm");
  const [categories, setCategories] = useState<Category[]>([]);
  useEffect(() => {
    categoryApi.getCategories().then((response) => {
      setCategories(response.value);
    });
  }, []);
  return (
    <>
      <Box>
        <TransitionGroup>
          {categories && categories.length > 0 ? (
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
                <Collapse key={category.id} in={true}>
                  <CategoryListItem category={category} />
                </Collapse>
              ))}
            </Box>
          ) : (
            <Typography variant="body1">Loading...</Typography>
          )}
        </TransitionGroup>
      </Box>
    </>
  );
});
