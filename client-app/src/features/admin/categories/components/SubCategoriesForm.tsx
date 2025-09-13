import {
  Box,
  Paper,
  Typography,
  TextField,
  useTheme,
  alpha,
  IconButton,
  Button,
  Divider,
  Tooltip,
  Fade,
} from "@mui/material";
import { useFieldArray, Controller } from "react-hook-form";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import AccountTreeIcon from "@mui/icons-material/AccountTree";
import AddIcon from "@mui/icons-material/Add";
import DeleteIcon from "@mui/icons-material/Delete";
import DragIndicatorIcon from "@mui/icons-material/DragIndicator";
import SubdirectoryArrowRightIcon from "@mui/icons-material/SubdirectoryArrowRight";

interface SubCategoriesFormProps {
  control: any;
  errors: any;
}

export const SubCategoriesForm = observer(({ control, errors }: SubCategoriesFormProps) => {
  const { t } = useTranslation();
  const theme = useTheme();

  const { fields, append, remove } = useFieldArray({
    control,
    name: "subCategories",
  });

  const addSubCategory = () => {
    append({
      namePL: "",
      nameEN: "",
    });
  };

  return (
    <Paper
      elevation={2}
      sx={{
        p: 3,
        backgroundColor: "background.paper",
        borderRadius: 2,
        border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
      }}
    >
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          mb: 3,
          pb: 2,
          borderBottom: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        }}
      >
        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <AccountTreeIcon color="primary" />
          <Typography
            variant="h6"
            fontWeight={600}
            color="text.primary"
          >
            {t("adminCategories:subCategories")}
          </Typography>
          {fields.length > 0 && (
            <Typography
              variant="body2"
              color="text.secondary"
              sx={{
                backgroundColor: alpha(theme.palette.primary.main, 0.1),
                px: 1,
                py: 0.5,
                borderRadius: 1,
                fontWeight: 500,
              }}
            >
              {fields.length}
            </Typography>
          )}
        </Box>

        <Tooltip title={t("adminCategories:addSubCategory")}>
          <Button
            variant="outlined"
            startIcon={<AddIcon />}
            onClick={addSubCategory}
            size="small"
            sx={{
              borderRadius: 2,
              textTransform: "none",
              fontWeight: 500,
              "&:hover": {
                transform: "translateY(-1px)",
                boxShadow: theme.shadows[4],
              },
              transition: "all 0.2s ease",
            }}
          >
            {t("adminCategories:addSubCategory")}
          </Button>
        </Tooltip>
      </Box>

      {fields.length === 0 ? (
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
            py: 6,
            color: "text.secondary",
          }}
        >
          <AccountTreeIcon sx={{ fontSize: 48, mb: 2, opacity: 0.5 }} />
          <Typography variant="body1" color="text.secondary" textAlign="center">
            {t("adminCategories:noSubCategories")}
          </Typography>
          <Typography variant="body2" color="text.secondary" textAlign="center" sx={{ mt: 1 }}>
            {t("adminCategories:addFirstSubCategory")}
          </Typography>
        </Box>
      ) : (
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          {fields.map((field, index) => (
            <Fade in={true} key={field.id}>
              <Paper
                elevation={1}
                sx={{
                  p: 2,
                  backgroundColor: alpha(theme.palette.background.default, 0.5),
                  border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                  borderRadius: 2,
                  "&:hover": {
                    backgroundColor: alpha(theme.palette.primary.main, 0.02),
                    borderColor: alpha(theme.palette.primary.main, 0.2),
                  },
                  transition: "all 0.2s ease",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "flex-start",
                    gap: 2,
                  }}
                >
                  {/* Drag Handle */}
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      width: 40,
                      height: 40,
                      color: "text.secondary",
                      cursor: "grab",
                      "&:hover": {
                        color: "primary.main",
                      },
                    }}
                  >
                    <DragIndicatorIcon fontSize="small" />
                  </Box>

                  {/* Index Badge */}
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "center",
                      minWidth: 32,
                      height: 32,
                      backgroundColor: theme.palette.primary.main,
                      color: "white",
                      borderRadius: "50%",
                      fontSize: "0.875rem",
                      fontWeight: 600,
                      mt: 0.5,
                    }}
                  >
                    {index + 1}
                  </Box>

                  {/* Form Fields */}
                  <Box sx={{ flex: 1, display: "flex", flexDirection: "column", gap: 2 }}>
                    <Box sx={{ display: "flex", gap: 2 }}>
                      {/* Polish Name */}
                      <Controller
                        name={`subCategories.${index}.namePL`}
                        control={control}
                        render={({ field }) => (
                          <TextField
                            {...field}
                            label={t("adminCategories:subCategoryNamePL")}
                            variant="outlined"
                            fullWidth
                            size="small"
                            error={!!errors?.subCategories?.[index]?.namePL}
                            helperText={errors?.subCategories?.[index]?.namePL?.message}
                            InputProps={{
                              startAdornment: (
                                <SubdirectoryArrowRightIcon 
                                  fontSize="small" 
                                  sx={{ mr: 1, color: "text.secondary" }} 
                                />
                              ),
                            }}
                          />
                        )}
                      />

                      {/* English Name */}
                      <Controller
                        name={`subCategories.${index}.nameEN`}
                        control={control}
                        render={({ field }) => (
                          <TextField
                            {...field}
                            label={t("adminCategories:subCategoryNameEN")}
                            variant="outlined"
                            fullWidth
                            size="small"
                            error={!!errors?.subCategories?.[index]?.nameEN}
                            helperText={errors?.subCategories?.[index]?.nameEN?.message}
                            InputProps={{
                              startAdornment: (
                                <SubdirectoryArrowRightIcon 
                                  fontSize="small" 
                                  sx={{ mr: 1, color: "text.secondary" }} 
                                />
                              ),
                            }}
                          />
                        )}
                      />
                    </Box>
                  </Box>

                  {/* Delete Button */}
                  <Tooltip title={t("adminCategories:removeSubCategory")}>
                    <IconButton
                      onClick={() => remove(index)}
                      color="error"
                      size="small"
                      sx={{
                        mt: 0.5,
                        "&:hover": {
                          transform: "scale(1.1)",
                          backgroundColor: alpha(theme.palette.error.main, 0.1),
                        },
                        transition: "all 0.2s ease",
                      }}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </Tooltip>
                </Box>
              </Paper>
            </Fade>
          ))}
        </Box>
      )}

      {fields.length > 0 && (
        <>
          <Divider sx={{ mt: 3, mb: 2 }} />
          <Typography variant="body2" color="text.secondary" textAlign="center">
            {t("adminCategories:subCategoriesHelper")}
          </Typography>
        </>
      )}
    </Paper>
  );
});

export default SubCategoriesForm;