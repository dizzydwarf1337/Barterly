import { createColumnHelper } from "@tanstack/react-table";
import {
  Typography,
  Tooltip,
  IconButton,
  Stack,
  Chip,
  Box,
} from "@mui/material";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import CategoryIcon from "@mui/icons-material/Category";
import { useNavigate } from "react-router";
import { DeletionDialog } from "../../../../app/components/deletionDialog";
import { Category } from "../types/categoryTypes";

const columnHelper = createColumnHelper<Category>();

const ActionsCell = ({
  category,
  onDelete,
}: {
  category: Category;
  onDelete: (categoryId: string) => void;
}) => {
  const { t } = useTranslation();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const navigate = useNavigate();
  
  const handleDelete = () => {
    onDelete(category.id);
    setDeleteDialogOpen(false);
  };

  const handleEdit = () => {
    navigate(`${category.id}`, { state: { category } });
  };

  return (
    <>
      <Stack direction="row" spacing={0.5}>
        <Tooltip title={t("adminCategories:edit")}>
          <IconButton
            size="small"
            color="primary"
            sx={{
              "&:hover": {
                transform: "scale(1.1)",
              },
              transition: "all 0.2s",
            }}
            onClick={handleEdit}
          >
            <EditIcon fontSize="small" />
          </IconButton>
        </Tooltip>
        <Tooltip title={t("adminCategories:delete")}>
          <IconButton
            size="small"
            color="error"
            onClick={() => setDeleteDialogOpen(true)}
            sx={{
              "&:hover": {
                transform: "scale(1.1)",
              },
              transition: "all 0.2s",
            }}
          >
            <DeleteIcon fontSize="small" />
          </IconButton>
        </Tooltip>
      </Stack>

      <DeletionDialog
        open={deleteDialogOpen}
        message={t("adminCategories:deleteConfirmation.message")}
        title={t("adminCategories:deleteConfirmation.title")}
        onAccept={handleDelete}
        onClose={() => setDeleteDialogOpen(false)}
      />
    </>
  );
};

export const categoriesColumns = (onDelete: (categoryId: string) => void) => [
  // Category Name (Polish) column
  columnHelper.accessor("namePL", {
    header: "Name (PL)",
    cell: (info) => (
      <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
        <CategoryIcon fontSize="small" color="primary" />
        <Typography variant="body2" fontWeight={600}>
          {info.getValue()}
        </Typography>
      </Box>
    ),
    size: 200,
    enableSorting: true,
  }),

  // Category Name (English) column
  columnHelper.accessor("nameEN", {
    header: "Name (EN)",
    cell: (info) => (
      <Typography variant="body2" fontWeight={600}>
        {info.getValue()}
      </Typography>
    ),
    size: 200,
    enableSorting: true,
  }),

  // Description column
  columnHelper.accessor("description", {
    header: "Description",
    cell: (info) => {
      const description = info.getValue();
      
      if (!description) {
        return (
          <Typography variant="body2" color="text.secondary">
            —
          </Typography>
        );
      }

      return (
        <Tooltip title={description}>
          <Typography 
            variant="body2" 
            noWrap 
            sx={{ maxWidth: 250 }}
          >
            {description}
          </Typography>
        </Tooltip>
      );
    },
    size: 250,
    enableSorting: true,
  }),

  // Subcategories count column
  columnHelper.accessor("subCategories", {
    header: "Subcategories",
    cell: (info) => {
      const count = info.getValue()?.length || 0;

      return (
        <Chip
          label={`${count} subcategories`}
          size="small"
          color={count > 0 ? "primary" : "default"}
          variant={count > 0 ? "filled" : "outlined"}
          sx={{ fontWeight: 500, minWidth: 120 }}
        />
      );
    },
    size: 150,
    enableSorting: true,
  }),

  // Actions column
  columnHelper.display({
    id: "actions",
    header: "Actions",
    cell: ({ row }) => (
      <ActionsCell
        category={row.original}
        onDelete={() => onDelete(row.original.id)}
      />
    ),
    size: 100,
    enableSorting: false,
  }),
];