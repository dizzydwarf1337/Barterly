import { Box } from "@mui/material";
import { observer } from "mobx-react-lite";
import useStore from "../app/stores/store";
import FeedDashboard from "../features/posts/pages/FeedDashboard";
import CategoriesDashboard from "../features/categories/pages/categoriesDashboard";

export default observer(function HomePage() {
  const { uiStore } = useStore();

  return (
    <Box display="flex" flexDirection="column" gap="20px">
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
