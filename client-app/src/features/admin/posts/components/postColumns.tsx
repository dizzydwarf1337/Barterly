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
import TitleIcon from "@mui/icons-material/Title";
import VisibilityIcon from "@mui/icons-material/Visibility";
import { useNavigate } from "react-router";
import { DeletionDialog } from "../../../../app/components/deletionDialog";
import { PostPreview } from "../../../posts/types/postTypes";

const columnHelper = createColumnHelper<PostPreview>();

const ActionsCell = ({
  post,
  onDelete,
}: {
  post: PostPreview;
  onDelete: (postId: string) => void;
}) => {
  const { t } = useTranslation();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const navigate = useNavigate();
  
  const handleDelete = () => {
    onDelete(post.id);
    setDeleteDialogOpen(false);
  };

  const handleEdit = () => {
    navigate(`${post.id}`, { state: { post } });
  };

  return (
    <>
      <Stack direction="row" spacing={0.5}>
        <Tooltip title={t("adminPosts:edit")}>
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
        <Tooltip title={t("adminPosts:delete")}>
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
        message={t("adminPosts:deleteConfirmation.message")}
        title={t("adminPosts:deleteConfirmation.title")}
        onAccept={handleDelete}
        onClose={() => setDeleteDialogOpen(false)}
      />
    </>
  );
};

export const postsColumns = (onDelete: (postId: string) => void) => [
  columnHelper.accessor("title", {
    header: "Title",
    cell: (info) => (
      <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
        <TitleIcon fontSize="small" color="primary" />
        <Tooltip title={info.getValue()}>
          <Typography 
            variant="body2" 
            fontWeight={600}
            noWrap 
            sx={{ maxWidth: 200 }}
          >
            {info.getValue()}
          </Typography>
        </Tooltip>
      </Box>
    ),
    size: 250,
    enableSorting: true,
  }),

  // Price column
  columnHelper.accessor("price", {
    header: "Price",
    cell: (info) => {
      const price = info.getValue();
      const row = info.row.original;
      
      if (!price) {
        return (
          <Typography variant="body2" color="text.secondary">
            —
          </Typography>
        );
      }

      const currency = row.currency || "PLN";
      
      return (
        <Typography variant="body2" fontWeight={600} color="success.main">
          {price.toLocaleString()} {currency}
        </Typography>
      );
    },
    size: 120,
    enableSorting: true,
  }),

  // Post Type column
  columnHelper.accessor("postType", {
    header: "Post Type",
    cell: (info) => {
      const postType = info.getValue();
      
      if (!postType) {
        return (
          <Typography variant="body2" color="text.secondary">
            —
          </Typography>
        );
      }

      return (
        <Chip
          label={postType}
          size="small"
          color="info"
          variant="outlined"
          sx={{ fontWeight: 500 }}
        />
      );
    },
    size: 120,
    enableSorting: true,
  }),

  // Created At column
  columnHelper.accessor("createdAt", {
    header: "Created At",
    cell: (info) => {
      const createdAt = info.getValue();
      
      if (!createdAt) {
        return (
          <Typography variant="body2" color="text.secondary">
            —
          </Typography>
        );
      }

      const date = new Date(createdAt);
      const formattedDate = date.toLocaleDateString();
      const formattedTime = date.toLocaleTimeString([], { 
        hour: '2-digit', 
        minute: '2-digit' 
      });

      return (
        <Tooltip title={`${formattedDate} ${formattedTime}`}>
          <Typography variant="body2">
            {formattedDate}
          </Typography>
        </Tooltip>
      );
    },
    size: 120,
    enableSorting: true,
  }),

  // Views Count column
  columnHelper.accessor("viewsCount", {
    header: "Views",
    cell: (info) => {
      const viewsCount = info.getValue() || 0;

      return (
        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <VisibilityIcon fontSize="small" color="action" />
          <Typography variant="body2" fontWeight={500}>
            {viewsCount.toLocaleString()}
          </Typography>
        </Box>
      );
    },
    size: 100,
    enableSorting: true,
  }),

  // Actions column
  columnHelper.display({
    id: "actions",
    header: "Actions",
    cell: ({ row }) => (
      <ActionsCell
        post={row.original}
        onDelete={() => onDelete(row.original.id)}
      />
    ),
    size: 100,
    enableSorting: false,
  }),
];