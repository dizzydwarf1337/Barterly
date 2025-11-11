import { useEffect } from "react";
import {
  Box,
  Paper,
  Typography,
  TextField,
  useTheme,
  alpha,
} from "@mui/material";
import { Controller, Control, FieldErrors } from "react-hook-form";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { Category } from "../types/categoryTypes";
import CategoryIcon from "@mui/icons-material/Category";
import DescriptionIcon from "@mui/icons-material/Description";

interface CategoryFormData {
  namePL: string;
  nameEN: string;
  description?: string;
  subCategories: Array<{
    id?: string;
    namePL: string;
    nameEN: string;
  }>;
}

interface CategoryFormProps {
  category?: Category | null;
  control: Control<CategoryFormData>;
  errors: FieldErrors<CategoryFormData>;
  reset: (values: CategoryFormData) => void;
}

export const CategoryForm = observer(({ category, control, errors, reset }: CategoryFormProps) => {
  const { t } = useTranslation();
  const theme = useTheme();

  useEffect(() => {
    if (category) {
      reset({
        namePL: category.namePL,
        nameEN: category.nameEN,
        description: category.description || "",
        subCategories: category.subCategories,
      });
    }
  }, [category, reset]);

  return (
    <Paper
      elevation={2}
      sx={{
        p: 3,
        backgroundColor: "background.paper",
        borderRadius: 2,
        border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
        mb: 3,
      }}
    >
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          gap: 1,
          mb: 3,
          pb: 2,
          borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        }}
      >
        <CategoryIcon color="primary" />
        <Typography
          variant="h6"
          fontWeight={600}
          color="text.primary"
        >
          {t("adminCategories:categoryDetails")}
        </Typography>
      </Box>

      <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
        {/* Polish Name */}
        <Controller
          name="namePL"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t("adminCategories:namePl")}
              variant="outlined"
              fullWidth
              error={!!errors.namePL}
              helperText={errors.namePL?.message}
              InputProps={{
                startAdornment: (
                  <CategoryIcon 
                    fontSize="small" 
                    sx={{ mr: 1, color: "text.secondary" }} 
                  />
                ),
              }}
              sx={{
                "& .MuiOutlinedInput-root": {
                  "&:hover fieldset": {
                    borderColor: theme.palette.primary.main,
                  },
                },
              }}
            />
          )}
        />

        {/* English Name */}
        <Controller
          name="nameEN"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t("adminCategories:nameEn")}
              variant="outlined"
              fullWidth
              error={!!errors.nameEN}
              helperText={errors.nameEN?.message}
              InputProps={{
                startAdornment: (
                  <CategoryIcon 
                    fontSize="small" 
                    sx={{ mr: 1, color: "text.secondary" }} 
                  />
                ),
              }}
              sx={{
                "& .MuiOutlinedInput-root": {
                  "&:hover fieldset": {
                    borderColor: theme.palette.primary.main,
                  },
                },
              }}
            />
          )}
        />

        {/* Description */}
        <Controller
          name="description"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t("adminCategories:description")}
              variant="outlined"
              fullWidth
              multiline
              rows={4}
              error={!!errors.description}
              helperText={errors.description?.message || t("adminCategories:descriptionHelper")}
              InputProps={{
                startAdornment: (
                  <DescriptionIcon 
                    fontSize="small" 
                    sx={{ mr: 1, color: "text.secondary", alignSelf: "flex-start", mt: 1 }} 
                  />
                ),
              }}
              sx={{
                "& .MuiOutlinedInput-root": {
                  "&:hover fieldset": {
                    borderColor: theme.palette.primary.main,
                  },
                },
              }}
            />
          )}
        />
      </Box>
    </Paper>
  );
});

export default CategoryForm;