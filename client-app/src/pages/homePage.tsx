import { Box } from "@mui/material";
import { observer } from "mobx-react-lite";
import useStore from "../app/stores/store";
import FeedDashboard from "../features/posts/pages/FeedDashboard";
import CategoriesDashboard from "../features/categories/pages/categoriesDashboard";
import SearchBar from "../features/posts/components/searchBar";
import { useNavigate } from "react-router";

interface SearchFilters {
    search?: string;
    categoryId?: string;
    subCategoryId?: string;
}

export default observer(function HomePage() {
  const { uiStore } = useStore();
  const navigate = useNavigate();
  const handleSearch = (filters:SearchFilters) => {
    const params = new URLSearchParams()

    if (filters.search) params.append('search', filters.search)
    if (filters.categoryId) params.append('categoryId', filters.categoryId)
    if (filters.subCategoryId) params.append('subCategoryId', filters.subCategoryId)

    navigate(`/search?${params.toString()}`)
  }
  return (
    <Box display="flex" flexDirection="column" gap={5}>
      <Box display="flex" flexDirection="column" gap="10px">
        <Box
        >
          <SearchBar onSearch={handleSearch}/>
        </Box>
      </Box>

      <Box display="flex" flexDirection="column" gap="10px">
        <Box
          p="20px"
          sx={{ backgroundColor: "background.paper", borderRadius: "12px" }}
        >
          <CategoriesDashboard />
        </Box>
      </Box>

      <Box
        display="grid"
        gridTemplateColumns={uiStore.isMobile ? undefined : "3fr 1fr"}
        gap="20px"
      >
        <Box display="flex" flexDirection="column" gap="10px">
          <FeedDashboard />
        </Box>
      </Box>
    </Box>
  );
});
