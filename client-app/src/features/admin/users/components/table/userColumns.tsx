import { createColumnHelper } from "@tanstack/react-table";
import { UserPreview, UserRoles } from "../../types/userTypes";
import { Chip, Box, Typography, Tooltip, IconButton, Stack } from "@mui/material";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import dayjs from "dayjs";
import relativeTime from "dayjs/plugin/relativeTime";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import SupervisorAccountIcon from "@mui/icons-material/SupervisorAccount";
import PersonIcon from "@mui/icons-material/Person";
import { DeletionDialog } from "../../../../../app/components/deletionDialog";
import { useNavigate } from "react-router";

dayjs.extend(relativeTime);

const columnHelper = createColumnHelper<UserPreview>();

const roleConfig = {
  [UserRoles.Admin]: {
    label: 'Admin',
    color: 'error' as const,
    icon: <AdminPanelSettingsIcon fontSize="small" />
  },
  [UserRoles.Moderator]: {
    label: 'Moderator',
    color: 'warning' as const,
    icon: <SupervisorAccountIcon fontSize="small" />
  },
  [UserRoles.User]: {
    label: 'User',
    color: 'primary' as const,
    icon: <PersonIcon fontSize="small" />
  }
};

const ActionsCell = ({ user, onDelete }: { user: UserPreview; onDelete: (userId: string) => void }) => {
  const { t } = useTranslation();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const navigate = useNavigate();
  const handleDelete = () => {
    onDelete(user.id);
    setDeleteDialogOpen(false);
  };

  return (
    <>
      <Stack direction="row" spacing={0.5}>
        <Tooltip title={t('edit')}>
          <IconButton 
            size="small" 
            color="primary"
            sx={{ 
              '&:hover': { 
                transform: 'scale(1.1)' 
              },
              transition: 'all 0.2s'
            }}
            onClick={()=>navigate(`${user.id}`)}
          >
            <EditIcon fontSize="small" />
          </IconButton>
        </Tooltip>
        <Tooltip title={t('delete')}>
          <IconButton 
            size="small" 
            color="error"
            onClick={() => setDeleteDialogOpen(true)}
            sx={{ 
              '&:hover': { 
                transform: 'scale(1.1)' 
              },
              transition: 'all 0.2s'
            }}
          >
            <DeleteIcon fontSize="small" />
          </IconButton>
        </Tooltip>
      </Stack>

      <DeletionDialog
        open={deleteDialogOpen}
        message={t('deleteConfirmation.userMessage', { name: `${user.firstName} ${user.lastName}` })}
        onAccept={handleDelete}
        onClose={() => setDeleteDialogOpen(false)}
      />
    </>
  );
};

export const usersColumns = (onDelete: (userId: string) => void) => [
  columnHelper.accessor('firstName', {
    header: 'First Name',
    cell: (info) => (
      <Typography variant="body2" fontWeight={600}>
        {info.getValue()}
      </Typography>
    ),
    size: 150,
  }),

  // Last Name column
  columnHelper.accessor('lastName', {
    header: 'Last Name',
    cell: (info) => (
      <Typography variant="body2" fontWeight={600}>
        {info.getValue()}
      </Typography>
    ),
    size: 150,
  }),

  columnHelper.accessor('email', {
    header: 'Email',
    cell: (info) => (
      <Typography variant="body2" fontWeight={600}>
        {info.getValue()}
      </Typography>
    ),
    size: 150,
  }),

  // Address column
  columnHelper.display({
    id: 'address',
    header: 'Address',
    cell: ({ row }) => {
      const { street, houseNumber, city, postalCode, country } = row.original;
      
      if (!street && !city && !country) {
        return <Typography variant="body2" color="text.secondary">—</Typography>;
      }
      
      const addressParts = [
        street && houseNumber ? `${street} ${houseNumber}` : street,
        postalCode && city ? `${postalCode} ${city}` : city,
        country
      ].filter(Boolean);
      
      return (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <LocationOnIcon fontSize="small" color="action" />
          <Typography variant="body2" noWrap sx={{ maxWidth: 250 }}>
            {addressParts.join(', ')}
          </Typography>
        </Box>
      );
    },
    size: 280,
  }),

  // Role column
  columnHelper.accessor('role', {
    header: 'Role',
    cell: (info) => {
      const role = info.getValue();
      const config = roleConfig[role];
      
      return (
        <Chip
          label={config.label}
          color={config.color}
          icon={config.icon}
          size="small"
          sx={{ fontWeight: 600 }}
        />
      );
    },
    size: 120,
  }),

  // Created At column
  columnHelper.accessor('createdAt', {
    header: 'Created At',
    cell: (info) => (
      <Tooltip title={dayjs(info.getValue()).format('MMMM DD, YYYY HH:mm:ss')}>
        <Typography variant="body2">
          {dayjs(info.getValue()).format('MMM DD, YYYY')}
        </Typography>
      </Tooltip>
    ),
    size: 120,
  }),

  // Last Seen column
  columnHelper.accessor('lastSeen', {
    header: 'Last Seen',
    cell: (info) => {
      const lastSeen = dayjs(info.getValue());
      const now = dayjs();
      const diffMinutes = now.diff(lastSeen, 'minute');
      const diffHours = now.diff(lastSeen, 'hour');
      const diffDays = now.diff(lastSeen, 'day');
      
      let status = 'default';
      let label = '';
      
      if (diffMinutes < 5) {
        status = 'success';
        label = 'Online';
      } else if (diffHours < 1) {
        label = `${diffMinutes}m ago`;
      } else if (diffHours < 24) {
        label = `${diffHours}h ago`;
      } else if (diffDays < 7) {
        label = `${diffDays}d ago`;
      } else {
        label = lastSeen.format('MMM DD, YYYY');
      }
      
      return (
        <Chip
          label={label}
          size="small"
          color={status === 'success' ? 'success' : 'default'}
          variant={status === 'success' ? 'filled' : 'outlined'}
          sx={{ fontWeight: 500, minWidth: 80 }}
        />
      );
    },
    size: 130,
  }),

  // Actions column
  columnHelper.display({
    id: 'actions',
    header: 'Actions',
    cell: ({ row }) => (
      <ActionsCell 
        user={row.original} 
        onDelete={() => onDelete(row.original.id)} 
      />
    ),
    size: 100,
  }),
];