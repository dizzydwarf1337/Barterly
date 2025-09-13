import { useEffect, useState } from "react";
import {
  Box,
  Paper,
  Typography,
  TextField,
  InputAdornment,
  alpha,
  useTheme,
  Fade,
  Button,
  Stack,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import CategoryIcon from "@mui/icons-material/Category";
import AddIcon from "@mui/icons-material/Add";
import BasicTable from "../../../../app/components/basicTable";
import categoriesApi from "../api/adminCategoriesApi";
import { useDebounce } from "../../../../app/hooks/useDebounce";
import { usePagination } from "../../../../app/hooks/usePagination";
import useStore from "../../../../app/stores/store";
import { useTranslation } from "react-i18next";
import {  GetAllCategoriesRequestDto } from "../dto/categoriesDto";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router";
import { SortingState } from "@tanstack/react-table";
import { categoriesColumns } from "../components/categoryColumns";
import { Category } from "../types/categoryTypes";

export const CategoriesPage = observer(() => {
  const { pagination } = usePagination();
  const [sorting, setSorting] = useState<SortingState>([]);
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search);
  const theme = useTheme();
  const { uiStore } = useStore();
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleDelete = async (categoryId: string) => {
    try {
      await categoriesApi.deleteCategory({ id: categoryId });
      uiStore.showSnackbar(
        t("adminCategories:categoryDeletedSuccess") || "Category has been deleted",
        "success"
      );
    } catch {
      uiStore.showSnackbar(
        t("adminCategories:categoryDeleteError") || "Error deleting category",
        "error"
      );
    }
  };

  const handleClearFilters = () => {
    setSearch("");
  }

  const handlePageChange = (newPage: number) => {
    pagination.page.setPage(newPage);
  };

  const handlePageSizeChange = (newPageSize: number) => {
    pagination.page.setPageSize(newPageSize);
    pagination.page.setPage(1); 
  };

  useEffect(() => {
    const fetchCategories = async () => {
    setIsLoading(true);
    try {
      let sortField = "namePL";
      let isDescending = false;

      if (sorting.length > 0) {
        const sortConfig = sorting[0];
        sortField = sortConfig.id;
        isDescending = sortConfig.desc;
      }

      const request: GetAllCategoriesRequestDto = {
        filterBy: {
            search: debouncedSearch,
            pageNumber: pagination.page.pageNumber,
            pageSize: pagination.page.pageSize,
            id: "",
            nameEN:"",
            namePL:"",
            description:"",
            subCategories:[],
        },
        sortBy: {
          sortBy: sortField,
          isDescending: isDescending,
        },
      };

      const response = await categoriesApi.getCategories(request);
      if (response.isSuccess) {
        setCategories(response.value.categories);
        pagination.total.setTotalCount(response.value.totalCount);
        pagination.total.setTotalPagesCount(response.value.totalPages);
      }
    } catch {
      uiStore.showSnackbar(
        t("adminCategories:fetchError") || "Error loading categories",
        "error"
      );
    } finally {
      setIsLoading(false);
    }
  };
      fetchCategories();
  }, [debouncedSearch, pagination.page.pageNumber, pagination.page.pageSize, sorting]);

  return (
    <Fade in timeout={300}>
      <Box sx={{ p: { xs: 2, sm: 3 } }}>
        {/* Header */}
        <Paper
          elevation={0}
          sx={{
            p: 3,
            mb: 3,
            borderRadius: 3,
            background: `linear-gradient(135deg, ${alpha(
              theme.palette.primary.main,
              0.1
            )} 0%, ${alpha(theme.palette.secondary.main, 0.05)} 100%)`,
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            backdropFilter: "blur(10px)",
          }}
        >
          <Box
            display="flex"
            flexDirection={{ xs: "column", md: "row" }}
            justifyContent="space-between"
            alignItems={{ xs: "stretch", md: "center" }}
            gap={2}
          >
            <Box display="flex" alignItems="center" gap={2}>
              <Box
                sx={{
                  p: 1.5,
                  borderRadius: 2,
                  background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
                  boxShadow: `0 8px 16px ${alpha(
                    theme.palette.primary.main,
                    0.3
                  )}`,
                }}
              >
                <CategoryIcon sx={{ color: "white", fontSize: 28 }} />
              </Box>
              <Box>
                <Typography
                  variant="h4"
                  fontWeight={700}
                  color="text.primary"
                  sx={{
                    background: `linear-gradient(45deg, ${theme.palette.primary.main} 30%, ${theme.palette.secondary.main} 90%)`,
                    backgroundClip: "text",
                    WebkitBackgroundClip: "text",
                    WebkitTextFillColor: "transparent",
                  }}
                >
                  {t("adminCategories:title")}
                </Typography>
                <Typography variant="body1" color="text.secondary" sx={{ mt: 0.5 }}>
                  {t("adminCategories:subtitle")}
                </Typography>
              </Box>
            </Box>

            <Stack direction="row" spacing={2}>
              <Button
                variant="contained"
                color="success"
                startIcon={<AddIcon />}
                onClick={() => navigate("new")}
                sx={{
                  borderRadius: 2,
                  textTransform: "none",
                  fontWeight: 600,
                  px: 3,
                  py: 1.5,
                  boxShadow: `0 4px 16px ${alpha(
                    theme.palette.success.main,
                    0.3
                  )}`,
                  "&:hover": {
                    transform: "translateY(-2px)",
                    boxShadow: `0 6px 20px ${alpha(
                      theme.palette.success.main,
                      0.4
                    )}`,
                  },
                  transition: "all 0.2s ease",
                }}
              >
                {t("adminCategories:addCategory")}
              </Button>
            </Stack>
          </Box>
        </Paper>

        {/* Search and Filters */}
        <Paper
          elevation={0}
          sx={{
            p: 2,
            mb: 3,
            borderRadius: 2,
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            backgroundColor: alpha(theme.palette.background.paper, 0.8),
          }}
        >
          <Box
            display="flex"
            flexDirection={{ xs: "column", sm: "row" }}
            gap={2}
            alignItems="center"
          >
            <TextField
              fullWidth
              placeholder={t("adminCategories:searchPlaceholder")}
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon color="action" />
                  </InputAdornment>
                ),
                sx: {
                  borderRadius: 2,
                  "& .MuiOutlinedInput-notchedOutline": {
                    borderColor: alpha(theme.palette.divider, 0.2),
                  },
                  "&:hover .MuiOutlinedInput-notchedOutline": {
                    borderColor: theme.palette.primary.main,
                  },
                  "&.Mui-focused .MuiOutlinedInput-notchedOutline": {
                    borderColor: theme.palette.primary.main,
                  },
                },
              }}
              sx={{ maxWidth: { sm: 400 } }}
            />

            <Box marginLeft={"auto"}>
              <Button
                variant="contained"
                onClick={handleClearFilters}
                disabled={!search}
                sx={{
                  borderRadius: 2,
                  textTransform: "none",
                  fontWeight: 600,
                }}
              >
                {t("adminCategories:clearFilters") || "Clear Filters"}
              </Button>
            </Box>
          </Box>
        </Paper>

        {/* Table */}
        <Paper
          elevation={0}
          sx={{
            borderRadius: 2,
            overflow: "hidden",
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            background: theme.palette.background.paper,
            boxShadow: `0 4px 24px ${alpha(
              theme.palette.common.black,
              0.06
            )}`,
          }}
        >
          <BasicTable
            data={categories}
            columns={categoriesColumns(handleDelete)}
            loading={isLoading}
            sorting={sorting}
            setSorting={setSorting}
            page={pagination.page.pageNumber}
            pageSize={pagination.page.pageSize}
            count={pagination.total.totalCount || 0}
            handlePageChange={handlePageChange}
            handlePageSizeChange={handlePageSizeChange}
          />
        </Paper>
      </Box>
    </Fade>
  );
});

export default CategoriesPage;