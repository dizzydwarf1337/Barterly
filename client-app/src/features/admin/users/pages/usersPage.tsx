import { useEffect, useState } from "react";
import {
  Box,
  Paper,
  Typography,
  TextField,
  InputAdornment,
  IconButton,
  Tooltip,
  Chip,
  alpha,
  useTheme,
  Fade,
  Button,
  Stack,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import RefreshIcon from "@mui/icons-material/Refresh";
import GroupIcon from "@mui/icons-material/Group";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import FilterListIcon from "@mui/icons-material/FilterList";
import BasicTable from "../../../../app/components/basicTable";
import usersApi from "../api/usersApi";
import { useSorting } from "../../../../app/hooks/useSorting";
import { usePagination } from "../../../../app/hooks/usePagination";
import { useDebounce } from "../../../../app/hooks/useDebounce";
import useStore from "../../../../app/stores/store";
import { useTranslation } from "react-i18next";
import { UserPreview } from "../types/userTypes";
import { usersColumns } from "../components/table/userColumns";
import { observer } from "mobx-react-lite";

export const UsersPage = observer(() => {
  const { pagination } = usePagination();
  const sorting = useSorting();
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search);
  const theme = useTheme();
  const { uiStore } = useStore();
  const { t } = useTranslation();

  const [users, setUsers] = useState<UserPreview[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isRefreshing, setIsRefreshing] = useState<boolean>(false);

  const handleDelete = async (userId: string) => {
    try {
      await usersApi.deleteUser({ id: userId });
      uiStore.showSnackbar("User has been deleted", "success");
      fetchUsers(); 
    } catch {
      uiStore.showSnackbar("Error deleting user", "error");
    }
  };

  const fetchUsers = async () => {
    setIsLoading(true);
    try {
      const response = await usersApi.getUsers({
        filterBy: {
          search: debouncedSearch,
          pageNumber: pagination.page.pageNumber,
          pageSize: pagination.page.pageSize,
        },
        sortBy: {
          sortBy: sorting.sortBy.sortBy,
          isDescending: sorting.isDescending.isDescending,
        },
      });
      pagination.total.setTotalCount(response.value.totalCount);
      pagination.total.setTotalPagesCount(response.value.totalPages);
      setUsers(response.value.items);
    } catch {
      uiStore.showSnackbar(t("errorLoadingUsers"), "error", "right");
    } finally {
      setIsLoading(false);
    }
  };

  const handleRefresh = async () => {
    setIsRefreshing(true);
    await fetchUsers();
    setTimeout(() => setIsRefreshing(false), 500);
  };

  useEffect(() => {
    fetchUsers();
  }, [debouncedSearch, pagination.page, sorting]);

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
                <GroupIcon sx={{ color: "white", fontSize: 28 }} />
              </Box>
              <Box>
                <Typography
                  variant="h4"
                  fontWeight="bold"
                  sx={{
                    background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                    backgroundClip: "text",
                    WebkitBackgroundClip: "text",
                    WebkitTextFillColor: "transparent",
                  }}
                >
                  {t("users") || "Users Management"}
                </Typography>
                <Box display="flex" alignItems="center" gap={1} mt={0.5}>
                  <Chip
                    label={`${pagination.total.totalCount || 0} ${
                      t("totalUsers") || "Total Users"
                    }`}
                    size="small"
                    color="primary"
                    variant="outlined"
                    sx={{ fontWeight: 600 }}
                  />
                  <Chip
                    label={`${pagination.total.totalPagesCount || 0} ${
                      t("pages") || "Pages"
                    }`}
                    size="small"
                    variant="outlined"
                    sx={{ fontWeight: 600 }}
                  />
                </Box>
              </Box>
            </Box>

            <Stack direction="row" spacing={1}>
              <Tooltip title={t("addUser") || "Add New User"}>
                <Button
                  variant="contained"
                  startIcon={<PersonAddIcon />}
                  sx={{
                    borderRadius: 2,
                    textTransform: "none",
                    fontWeight: 600,
                    background: `linear-gradient(135deg, ${theme.palette.success.main} 0%, ${theme.palette.success.dark} 100%)`,
                    boxShadow: `0 4px 12px ${alpha(
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
                  }}
                >
                  {t("addUser") || "Add User"}
                </Button>
              </Tooltip>

              <Tooltip title={t("filters") || "Filters"}>
                <IconButton
                  sx={{
                    borderRadius: 2,
                    border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
                    "&:hover": {
                      backgroundColor: alpha(theme.palette.primary.main, 0.08),
                    },
                  }}
                >
                  <FilterListIcon />
                </IconButton>
              </Tooltip>
            </Stack>
          </Box>
        </Paper>

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
              placeholder={t("searchUsers") || "Search users by name, email..."}
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

            <Tooltip title={t("refresh") || "Refresh"}>
              <IconButton
                onClick={handleRefresh}
                disabled={isRefreshing}
                sx={{
                  borderRadius: 2,
                  border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
                  transition: "all 0.3s ease",
                  transform: isRefreshing ? "rotate(360deg)" : "rotate(0deg)",
                  "&:hover": {
                    backgroundColor: alpha(theme.palette.primary.main, 0.08),
                    transform: "rotate(180deg)",
                  },
                }}
              >
                <RefreshIcon />
              </IconButton>
            </Tooltip>
          </Box>
        </Paper>

        <Paper
          elevation={0}
          sx={{
            borderRadius: 3,
            overflow: "hidden",
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            background: theme.palette.background.paper,
            boxShadow: `0 4px 24px ${alpha(theme.palette.common.black, 0.06)}`,
          }}
        >
          <BasicTable
            data={users}
            loading={isLoading}
            columns={usersColumns(handleDelete)}
            page={pagination.page.pageNumber}
            pageSize={pagination.page.pageSize}
            count={pagination.total.totalCount as number}
            handlePageChange={pagination.page.setPage}
            handlePageSizeChange={pagination.page.setPageSize}
          />
        </Paper>
      </Box>
    </Fade>
  );
});

export default UsersPage;