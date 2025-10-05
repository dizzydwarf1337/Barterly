import React, { useEffect, useState, useCallback, useMemo } from "react";
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
  Select,
  MenuItem,
  CircularProgress,
  FormControl,
  InputLabel,
  Switch,
  FormControlLabel,
  Chip,
  Theme,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import PostAddIcon from "@mui/icons-material/PostAdd";
import BasicTable from "../../../../app/components/basicTable";
import { useDebounce } from "../../../../app/hooks/useDebounce";
import { usePagination } from "../../../../app/hooks/usePagination";
import useStore from "../../../../app/stores/store";
import { useTranslation } from "react-i18next";
import { GetPostsRequestDto } from "../dto/postsDto";
import { observer } from "mobx-react-lite";
import { SortingState } from "@tanstack/react-table";
import { PostPreview } from "../../../posts/types/postTypes";
import postsApi from "../api/adminPostApi";
import { postsColumns } from "../components/postColumns";
import { Category, SubCategory } from "../../categories/types/categoryTypes";
import categoriesApi from "../../categories/api/adminCategoriesApi";

interface FiltersSectionProps {
  search: string;
  onSearchChange: (value: string) => void;
  categories: Category[];
  category: Category | null;
  onCategoryChange: (categoryId: string) => void;
  subCategory: SubCategory | null;
  onSubCategoryChange: (subCategoryId: string) => void;
  isActive: boolean | null;
  onIsActiveChange: (value: boolean | null) => void;
  isDeleted: boolean | null;
  onIsDeletedChange: (value: boolean | null) => void;
  onClearFilters: () => void;
  activeFiltersCount: number;
  theme: Theme;
  t: Function;
}
const FiltersSection = React.memo<FiltersSectionProps>(
  ({
    search,
    onSearchChange,
    categories,
    category,
    onCategoryChange,
    subCategory,
    onSubCategoryChange,
    isActive,
    onIsActiveChange,
    isDeleted,
    onIsDeletedChange,
    onClearFilters,
    activeFiltersCount,
    theme,
    t,
  }) => (
    <Paper
      elevation={0}
      sx={{
        p: 3,
        mb: 3,
        borderRadius: 2,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        backgroundColor: alpha(theme.palette.background.paper, 0.8),
      }}
    >
      <Box
        display="flex"
        gap={2}
        justifyContent={"space-between"}
        alignItems="center"
      >
        {/* Search Field */}
        <Box>
          <TextField
            fullWidth
            placeholder={t("adminPosts:searchPlaceholder") || "Search posts..."}
            value={search}
            onChange={(e) => onSearchChange(e.target.value)}
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
          />
        </Box>

        {/* Category Select */}
        <Box minWidth={200}>
          <FormControl fullWidth>
            <InputLabel>
              {t("adminPosts:searchByCategory") || "Category"}
            </InputLabel>
            <Select
              value={category?.id || ""}
              label={t("adminPosts:searchByCategory") || "Category"}
              onChange={(e) => onCategoryChange(e.target.value)}
              sx={{ borderRadius: 2 }}
            >
              <MenuItem value="">
                <em>{t("adminPosts:allCategories") || "All Categories"}</em>
              </MenuItem>
              {categories.map((cat: Category) => (
                <MenuItem key={cat.id} value={cat.id}>
                  {cat.nameEN}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>

        {/* SubCategory Select */}
        <Box minWidth={200}>
          <FormControl fullWidth>
            <InputLabel>
              {t("adminPosts:searchBySubCategory") || "SubCategory"}
            </InputLabel>
            <Select
              value={subCategory?.id || ""}
              disabled={!category || !category.subCategories?.length}
              label={t("adminPosts:searchBySubCategory") || "SubCategory"}
              onChange={(e) => onSubCategoryChange(e.target.value)}
              sx={{ borderRadius: 2 }}
            >
              <MenuItem value="">
                <em>
                  {t("adminPosts:allSubCategories") || "All SubCategories"}
                </em>
              </MenuItem>
              {category?.subCategories?.map((subCat: SubCategory) => (
                <MenuItem key={subCat.id} value={subCat.id}>
                  {subCat.nameEN}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>

        {/* Status Switches */}
        <Box>
          <Box display="flex" flexDirection="column" gap={1}>
            <FormControlLabel
              control={
                <Switch
                  checked={isActive === true}
                  onChange={(e) =>
                    onIsActiveChange(e.target.checked ? true : null)
                  }
                  color="success"
                  size="small"
                />
              }
              label={t("adminPosts:activeOnly") || "Active Only"}
              sx={{ margin: 0 }}
            />
            <FormControlLabel
              control={
                <Switch
                  checked={isDeleted === true}
                  onChange={(e) =>
                    onIsDeletedChange(e.target.checked ? true : null)
                  }
                  color="error"
                  size="small"
                />
              }
              label={t("adminPosts:deletedOnly") || "Deleted Only"}
              sx={{ margin: 0 }}
            />
          </Box>
        </Box>

        {/* Clear Filters Button */}
        <Box>
          <Box display="flex" flexDirection="column" gap={1}>
            <Button
              variant="outlined"
              onClick={onClearFilters}
              disabled={activeFiltersCount === 0}
              sx={{
                borderRadius: 2,
                textTransform: "none",
                fontWeight: 600,
              }}
              fullWidth
            >
              {t("adminPosts:clearFilters") || "Clear Filters"}
            </Button>
            {activeFiltersCount > 0 && (
              <Chip
                label={`${activeFiltersCount} ${
                  t("adminPosts:filtersActive") || "filters active"
                }`}
                size="small"
                color="primary"
                sx={{ alignSelf: "center" }}
              />
            )}
          </Box>
        </Box>
      </Box>
    </Paper>
  )
);

export const PostsPage = observer(() => {
  const { pagination } = usePagination();
  const [sorting, setSorting] = useState<SortingState>([]);
  const [search, setSearch] = useState("");
  const [categories, setCategories] = useState<Category[]>([]);
  const [category, setCategory] = useState<Category | null>(null);
  const [subCategory, setSubCategory] = useState<SubCategory | null>(null);
  const [isActive, setIsActive] = useState<boolean | null>(null);
  const [isDeleted, setIsDeleted] = useState<boolean | null>(null);
  const debouncedSearch = useDebounce(search);
  const theme = useTheme();
  const { uiStore } = useStore();
  const { t } = useTranslation();

  const [posts, setPosts] = useState<PostPreview[]>([]);
  const [isInitialLoading, setIsInitialLoading] = useState<boolean>(true);
  const [isTableLoading, setIsTableLoading] = useState<boolean>(false);

  const handleClearFilters = useCallback(() => {
    setSearch("");
    setCategory(null);
    setSubCategory(null);
    setIsActive(null);
    setIsDeleted(null);
  }, []);

  const handlePageChange = useCallback(
    (newPage: number) => {
      pagination.page.setPage(newPage);
    },
    [pagination.page]
  );

  const handlePageSizeChange = useCallback(
    (newPageSize: number) => {
      pagination.page.setPageSize(newPageSize);
      pagination.page.setPage(1);
    },
    [pagination.page]
  );

  const handleCategoryChange = useCallback(
    (categoryId: string) => {
      if (categoryId === "") {
        setCategory(null);
        setSubCategory(null);
      } else {
        const selectedCategory = categories.find(
          (cat) => cat.id === categoryId
        );
        setCategory(selectedCategory || null);
        setSubCategory(null);
      }
    },
    [categories]
  );

  const handleSubCategoryChange = useCallback(
    (subCategoryId: string) => {
      if (subCategoryId === "") {
        setSubCategory(null);
      } else {
        const selectedSubCategory = category?.subCategories.find(
          (sub) => sub.id === subCategoryId
        );
        setSubCategory(selectedSubCategory || null);
      }
    },
    [category]
  );

  const fetchPosts = useCallback(
    async (showLoading = true) => {
      if (showLoading) {
        setIsTableLoading(true);
      }

      try {
        let sortField = "createdAt";
        let isDescending = true;

        if (sorting.length > 0) {
          const sortConfig = sorting[0];
          sortField = sortConfig.id;
          isDescending = sortConfig.desc;
        }

        const request: GetPostsRequestDto = {
          filterBy: {
            search: debouncedSearch,
            pageNumber: pagination.page.pageNumber,
            pageSize: pagination.page.pageSize,
            categoryId: category?.id ?? null,
            subCategoryId: subCategory?.id ?? null,
            userId: null,
            isActive: isActive,
            isDeleted: isDeleted,
          },
          sortBy: {
            sortBy: sortField,
            isDescending: isDescending,
          },
        };

        const response = await postsApi.getPosts(request);
        if (response.isSuccess) {
          setPosts(response.value.posts);
          pagination.total.setTotalCount(response.value.totalCount);
          pagination.total.setTotalPagesCount(response.value.totalPages);
        }
      } catch {
        uiStore.showSnackbar(
          t("adminPosts:fetchError") || "Error loading posts",
          "error"
        );
      } finally {
        if (showLoading) {
          setIsTableLoading(false);
        }
        if (isInitialLoading) {
          setIsInitialLoading(false);
        }
      }
    },
    [
      debouncedSearch,
      pagination.page.pageNumber,
      pagination.page.pageSize,
      sorting,
      category,
      subCategory,
      isActive,
      isDeleted,
      pagination.total,
      uiStore,
      t,
      isInitialLoading,
    ]
  );

  const fetchCategories = useCallback(async () => {
    try {
      const response = await categoriesApi.getCategories({
        filterBy: {
          search: undefined,
          pageSize: 1000,
          pageNumber: 1,
          id: "",
          nameEN: "",
          namePL: "",
          description: "",
          subCategories: [],
        },
      });
      if (response.isSuccess) {
        setCategories(response.value.categories);
      }
    } catch {
      setCategories([]);
      uiStore.showSnackbar("Failed to load categories", "error");
    }
  }, [uiStore]);

  const activeFiltersCount = useMemo(() => {
    let count = 0;
    if (search) count++;
    if (category) count++;
    if (subCategory) count++;
    if (isActive !== null) count++;
    if (isDeleted !== null) count++;
    return count;
  }, [search, category, subCategory, isActive, isDeleted]);

  const tableColumns = useMemo(() => postsColumns(() => {}), []);

  useEffect(() => {
    fetchPosts();
  }, [fetchPosts]);

  useEffect(() => {
    fetchCategories();
  }, [fetchCategories]);

  if (isInitialLoading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="400px"
      >
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Fade in timeout={300}>
      <Box sx={{ p: { xs: 2, sm: 3 } }}>
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
                <PostAddIcon sx={{ color: "white", fontSize: 28 }} />
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
                  {t("adminPosts:title") || "Posts Management"}
                </Typography>
                <Typography
                  variant="body1"
                  color="text.secondary"
                  sx={{ mt: 0.5 }}
                >
                  {t("adminPosts:subtitle") || "Manage and moderate posts"}
                </Typography>
              </Box>
            </Box>
          </Box>
        </Paper>

        <FiltersSection
          search={search}
          onSearchChange={setSearch}
          categories={categories}
          category={category}
          onCategoryChange={handleCategoryChange}
          subCategory={subCategory}
          onSubCategoryChange={handleSubCategoryChange}
          isActive={isActive}
          onIsActiveChange={setIsActive}
          isDeleted={isDeleted}
          onIsDeletedChange={setIsDeleted}
          onClearFilters={handleClearFilters}
          activeFiltersCount={activeFiltersCount}
          theme={theme}
          t={t}
        />

        <Paper
          elevation={0}
          sx={{
            borderRadius: 2,
            overflow: "hidden",
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            background: theme.palette.background.paper,
            boxShadow: `0 4px 24px ${alpha(theme.palette.common.black, 0.06)}`,
          }}
        >
          <BasicTable
            data={posts}
            columns={tableColumns}
            loading={isTableLoading}
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

export default PostsPage;
