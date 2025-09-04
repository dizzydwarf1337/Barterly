import { useEffect, useState } from "react";
import {
  Box,
  Paper,
  Typography,
  TextField,
  InputAdornment,
  Tooltip,
  Chip,
  alpha,
  useTheme,
  Fade,
  Button,
  Stack,
  Switch,
  FormControlLabel,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import GroupIcon from "@mui/icons-material/Group";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
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
import { AddUserDialog } from "../components/AddUserDialog";

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

  const [isBanned, setIsBanned] = useState<boolean | null>(null);
  const [isDeleted, setIsDeleted] = useState<boolean | null>(null);
  const [isEmailConfirmed, setIsEmailConfirmed] = useState<boolean | null>(
    null
  );

  const [isDialogOpen, setIsDialogOpen] = useState(false);

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
          isBanned,
          isDeleted,
          isEmailConfirmed,
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
      uiStore.showSnackbar(t("adminUsers:errorLoadingUsers"), "error", "right");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, [
    debouncedSearch,
    pagination.page,
    sorting,
    isBanned,
    isDeleted,
    isEmailConfirmed,
  ]);

  return (
    <>
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
                    {t("adminUsers:adminUsers") || "Users Management"}
                  </Typography>
                  <Box display="flex" alignItems="center" gap={1} mt={0.5}>
                    <Chip
                      label={`${pagination.total.totalCount || 0} ${
                        t("adminUsers:totalUsers") || "Total Users"
                      }`}
                      size="small"
                      color="primary"
                      variant="outlined"
                      sx={{ fontWeight: 600 }}
                    />
                    <Chip
                      label={`${pagination.total.totalPagesCount || 0 + 1} ${
                        t("adminUsers:pages") || "Pages"
                      }`}
                      size="small"
                      variant="outlined"
                      sx={{ fontWeight: 600 }}
                    />
                  </Box>
                </Box>
              </Box>

              <Stack direction="row" spacing={1}>
                <Tooltip title={t("adminUsers:addUser") || "Add New User"}>
                  <Button
                    variant="contained"
                    startIcon={<PersonAddIcon />}
                    onClick={() => setIsDialogOpen(true)}
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
                    {t("adminUsers:addUser") || "Add User"}
                  </Button>
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
                placeholder={
                  t("adminUsers:searchUsers") ||
                  "Search users by name, email..."
                }
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
              <FormControlLabel
                control={
                  <Switch
                    checked={isBanned ?? false}
                    onChange={(e) => setIsBanned(e.target.checked)}
                  />
                }
                label={t("adminUsers:filterBanned")}
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={isDeleted ?? false}
                    onChange={(e) => setIsDeleted(e.target.checked)}
                  />
                }
                label={t("adminUsers:filterDeleted")}
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={isEmailConfirmed ?? false}
                    onChange={(e) => setIsEmailConfirmed(e.target.checked)}
                  />
                }
                label={t("adminUsers:filterEmailConfirmed")}
              />
              <Box marginLeft={"auto"} sx={{ color: "success" }}>
                <Button
                  variant="contained"
                  onClick={() => {
                    setIsBanned(false);
                    setIsDeleted(false);
                    setIsEmailConfirmed(false);
                    setSearch("");
                  }}
                >
                  {t("adminUsers:clearFilters") || "Clear Filters"}
                </Button>
              </Box>
            </Box>
          </Paper>

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
      <AddUserDialog
        open={isDialogOpen}
        onClose={() => setIsDialogOpen(false)}
        onSuccess={() => {
          setIsDialogOpen(false);
          fetchUsers();
        }}
      />
    </>
  );
});

export default UsersPage;
