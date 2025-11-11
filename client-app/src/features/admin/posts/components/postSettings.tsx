import {
  Box,
  Button,
  Card,
  CardContent,
  Chip,
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  Select,
  Switch,
  TextField,
  Typography,
} from "@mui/material";
import { PostSettings as Settings, PostStatusType } from "../types/postTypes";

import * as Yup from "yup";
import { useTranslation } from "react-i18next";
import useStore from "../../../../app/stores/store";
import { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { observer } from "mobx-react-lite";
import SaveIcon from "@mui/icons-material/Save";
import SettingsIcon from "@mui/icons-material/Settings";
import { getStatusColor, getStatusText } from "../utils/postStatusTypeUtil";
import postsApi from "../api/adminPostApi";

interface Props {
  settings: Settings;
  onUpdate: () => void;
}

const validationSchema = Yup.object().shape({
  id: Yup.string(),
  isHidden: Yup.boolean(),
  isDeleted: Yup.boolean(),
  postStatusType: Yup.number().required(),
  rejectionMessage: Yup.string().when("postStatusType", {
    is: PostStatusType.Rejected,
    then: (schema) =>
      schema.required("Rejection message is required when rejecting post"),
    otherwise: (schema) => schema.optional(),
  }),
});

export const PostSettings = observer(({ settings, onUpdate }: Props) => {
  const { t } = useTranslation();
  const { uiStore } = useStore();
  const [submitting, setSubmitting] = useState(false);

  const {
    control,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      id: settings.id,
      isHidden: settings.isHidden,
      isDeleted: settings.isDeleted,
      postStatusType: settings.postStatusType,
      rejectionMessage: settings.rejectionMessage || "",
    },
  });

  const watchedStatus = watch("postStatusType");

  const onSubmit = async (data: any) => {
    try {
      setSubmitting(true);
      await postsApi.updatePostSettings(data);
      uiStore.showSnackbar("Post settings updated successfully", "success");
      onUpdate();
    } catch {
      uiStore.showSnackbar("Failed to update post settings", "error");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Card sx={{ borderRadius: "16px" }}>
      <CardContent sx={{ p: 3 }}>
        <Typography
          variant="h6"
          gutterBottom
          sx={{ display: "flex", alignItems: "center", gap: 1 }}
        >
          <SettingsIcon color="primary" />
          {t("adminPosts:postSettings")}
        </Typography>

        <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ mt: 2 }}>
          <Box
            sx={{
              mb: 3,
              p: 2,
              bgcolor: "background.surface",
              borderRadius: "8px",
            }}
          >
            <Typography variant="body2" color="text.secondary" gutterBottom>
              Current Status
            </Typography>
            <Chip
              label={getStatusText(settings.postStatusType)}
              color={getStatusColor(settings.postStatusType)}
              variant="filled"
            />
          </Box>

          <Controller
            name="postStatusType"
            control={control}
            render={({ field }) => (
              <FormControl fullWidth sx={{ mb: 3 }}>
                <InputLabel>Post Status</InputLabel>
                <Select
                  {...field}
                  label="Post Status"
                  error={!!errors.postStatusType}
                >
                  <MenuItem value={PostStatusType.UnderReview}>
                    Under Review
                  </MenuItem>
                  <MenuItem value={PostStatusType.Published}>
                    Published
                  </MenuItem>
                  <MenuItem value={PostStatusType.Rejected}>Rejected</MenuItem>
                  <MenuItem value={PostStatusType.ReSubmitted}>
                    Re-Submitted
                  </MenuItem>
                  <MenuItem value={PostStatusType.Deleted}>Deleted</MenuItem>
                </Select>
              </FormControl>
            )}
          />

          {watchedStatus === PostStatusType.Rejected && (
            <Controller
              name="rejectionMessage"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  fullWidth
                  multiline
                  rows={3}
                  label="Rejection Message"
                  error={!!errors.rejectionMessage}
                  helperText={errors.rejectionMessage?.message}
                  sx={{ mb: 3 }}
                />
              )}
            />
          )}

          {/* Switches */}
          <Box sx={{ mb: 3 }}>
            <Controller
              name="isHidden"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={<Switch {...field} checked={field.value} />}
                  label="Hidden"
                  sx={{ display: "block", mb: 1 }}
                />
              )}
            />

            <Controller
              name="isDeleted"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={<Switch {...field} checked={field.value} />}
                  label="Deleted"
                  sx={{ display: "block" }}
                />
              )}
            />
          </Box>

          {/* Submit Button */}
          <Button
            type="submit"
            variant="contained"
            startIcon={<SaveIcon />}
            disabled={submitting}
            sx={{ textTransform: "none" }}
          >
            {submitting ? "Saving..." : "Save Settings"}
          </Button>
        </Box>
      </CardContent>
    </Card>
  );
});
