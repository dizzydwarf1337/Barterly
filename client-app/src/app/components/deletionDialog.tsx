import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Typography,
  Button,
  Box,
  IconButton,
  Fade,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { observer } from "mobx-react-lite";

import WarningIcon from "@mui/icons-material/Warning";
import CloseIcon from "@mui/icons-material/Close";
import DeleteIcon from "@mui/icons-material/Delete";

interface Props {
  open: boolean;
  message?: string;
  onAccept: () => void;
  onClose: () => void;
}

export const DeletionDialog = observer(({ open, message, onAccept, onClose }: Props) => {
  const { t } = useTranslation();

  const handleAccept = () => {
    onAccept();
    onClose();
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      TransitionComponent={Fade}
      TransitionProps={{ timeout: 300 }}
      maxWidth="xs"
      fullWidth
      PaperProps={{
        sx: {
          borderRadius: 3,
          overflow: 'hidden',
        },
      }}
    >
      <DialogTitle sx={{ pb: 1 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
          <WarningIcon color="warning" sx={{ fontSize: 28 }} />
          <Typography variant="h6" component="span" sx={{ fontWeight: 600 }}>
            {t('deleteConfirmation.title')}
          </Typography>
          <IconButton
            onClick={onClose}
            sx={{
              position: 'absolute',
              right: 8,
              top: 8,
              color: (theme) => theme.palette.grey[500],
            }}
          >
            <CloseIcon />
          </IconButton>
        </Box>
      </DialogTitle>

      <DialogContent sx={{ pt: 1, pb: 2 }}>
        <Typography variant="body1" color="text.secondary">
          {message || t('deleteConfirmation.defaultMessage')}
        </Typography>
      </DialogContent>

      <DialogActions sx={{ px: 3, pb: 3, gap: 1 }}>
        <Button
          onClick={onClose}
          variant="outlined"
          color="inherit"
          sx={{ minWidth: 100 }}
        >
          {t('cancel')}
        </Button>
        <Button
          onClick={handleAccept}
          variant="contained"
          color="error"
          startIcon={<DeleteIcon />}
          sx={{ minWidth: 100 }}
        >
          {t('delete')}
        </Button>
      </DialogActions>
    </Dialog>
  );
});