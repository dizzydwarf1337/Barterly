import { useParams, useNavigate, useLocation } from "react-router";
import { useEffect, useState } from "react";
import { Category } from "../types/categoryTypes";
import { AddCategoryRequestDto, UpdateCategoryRequestDto } from "../dto/categoriesDto";
import useStore from "../../../../app/stores/store";
import categoriesApi from "../api/adminCategoriesApi";
import {
  Box,
  Skeleton,
  Paper,
  Typography,
  IconButton,
  alpha,
  useTheme,
  Breadcrumbs,
  Link,
  Button,
  Stack,
  Fade,
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import SaveIcon from "@mui/icons-material/Save";
import CategoryIcon from "@mui/icons-material/Category";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import CategoryForm from "../components/CategoryForm";
import SubCategoriesForm from "../components/SubCategoriesForm";

// Form data type for react-hook-form
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

const validationSchema: Yup.ObjectSchema<CategoryFormData> = Yup.object().shape({
  namePL: Yup.string()
    .required("Polish name is required")
    .min(2, "Polish name must be at least 2 characters")
    .max(100, "Polish name must be less than 100 characters"),
  nameEN: Yup.string()
    .required("English name is required")
    .min(2, "English name must be at least 2 characters")
    .max(100, "English name must be less than 100 characters"),
  description: Yup.string()
    .optional()
    .max(500, "Description must be less than 500 characters"),
  subCategories: Yup.array()
    .of(
      Yup.object().shape({
        id: Yup.string().optional(),
        namePL: Yup.string()
          .required("Polish subcategory name is required")
          .min(2, "Polish subcategory name must be at least 2 characters")
          .max(100, "Polish subcategory name must be less than 100 characters"),
        nameEN: Yup.string()
          .required("English subcategory name is required")
          .min(2, "English subcategory name must be at least 2 characters")
          .max(100, "English subcategory name must be less than 100 characters"),
      })
    )
    .default([]),
});

export const CategoryPage = observer(() => {
  const { uiStore } = useStore();
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const { t } = useTranslation();

  const categoryFromState = location.state?.category as Category | undefined;
  const [category, setCategory] = useState<Category | null>(null);
  const [isLoading, _] = useState<boolean>(false);
  const [isSaving, setIsSaving] = useState<boolean>(false);

  const isNewCategory = id === "new";

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors, isDirty },
  } = useForm<CategoryFormData>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      namePL: "",
      nameEN: "",
      description: "",
      subCategories: [] as Array<{
        id?: string;
        namePL: string;
        nameEN: string;
      }>,
    },
  });

  const fetchCategory = async () => {
    if (isNewCategory) return;

    // Если категория уже передана через state, используем её
    if (categoryFromState) {
      setCategory(categoryFromState);
      reset({
        namePL: categoryFromState.namePL, // Используем правильные поля
        nameEN: categoryFromState.nameEN, // Используем правильные поля
        description: categoryFromState.description || "",
        subCategories: categoryFromState.subCategories.map((sub: { id: string; namePL: string; nameEN: string }) => ({
          id: sub.id,
          namePL: sub.namePL,
          nameEN: sub.nameEN,
        })),
      });
      return;
    }
    uiStore.showSnackbar(
      t("adminCategories:categoryNotFound") || "Category not found",
      "error"
    );
    navigate("/admin/categories");
  };

  const onSubmit = async (data: CategoryFormData) => {
    setIsSaving(true);
    try {
      if (isNewCategory) {
        const createData: AddCategoryRequestDto = {
          namePL: data.namePL,
          nameEN: data.nameEN,
          description: data.description,
          subCategories: data.subCategories.map((sub: { namePL: string; nameEN: string }) => ({
            namePL: sub.namePL,
            nameEN: sub.nameEN,
          })),
        };
        await categoriesApi.addCategory(createData);
        uiStore.showSnackbar(
          t("adminCategories:categoryCreatedSuccess") || "Category created successfully",
          "success"
        );
      } else {
        const updateData: UpdateCategoryRequestDto = {
          id: id!,
          namePL: data.namePL,
          nameEN: data.nameEN,
          description: data.description,
          subCategories: data.subCategories.map((sub: { id?: string; namePL: string; nameEN: string }) => ({
            id: sub.id,
            namePL: sub.namePL,
            nameEN: sub.nameEN,
          })),
        };
        await categoriesApi.updateCategory(updateData);
        uiStore.showSnackbar(
          t("adminCategories:categoryUpdatedSuccess") || "Category updated successfully",
          "success"
        );
      }
      navigate("/admin/categories");
    } catch (err: any) {
      const errorMessage =
        err?.response?.data?.error ||
        err?.message ||
        (isNewCategory ? "Failed to create category" : "Failed to update category");
      uiStore.showSnackbar(errorMessage, "error");
    } finally {
      setIsSaving(false);
    }
  };

  useEffect(() => {
    fetchCategory();
  }, [id]);

  if (isLoading) {
    return (
      <Box sx={{ p: 3 }}>
        <Skeleton variant="text" width={200} height={40} sx={{ mb: 2 }} />
        <Skeleton variant="rectangular" width="100%" height={300} sx={{ mb: 2 }} />
        <Skeleton variant="rectangular" width="100%" height={400} />
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3 }}>
      <Fade in timeout={600}>
        <Box>
          {/* Header with Breadcrumbs */}
          <Paper
            elevation={1}
            sx={{
              p: 2,
              mb: 3,
              backgroundColor: "background.paper",
              borderRadius: 2,
              border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
            }}
          >
            <Stack
              direction={{ xs: "column", sm: "row" }}
              justifyContent="space-between"
              alignItems={{ xs: "stretch", sm: "center" }}
              spacing={2}
            >
              <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
                <IconButton
                  onClick={() => navigate("/admin/categories")}
                  sx={{
                    backgroundColor: alpha(theme.palette.primary.main, 0.1),
                    color: theme.palette.primary.main,
                    "&:hover": {
                      backgroundColor: alpha(theme.palette.primary.main, 0.2),
                      transform: "translateX(-2px)",
                    },
                    transition: "all 0.2s ease",
                  }}
                >
                  <ArrowBackIcon />
                </IconButton>

                <Box>
                  <Breadcrumbs
                    aria-label="breadcrumb"
                    separator={<NavigateNextIcon fontSize="small" />}
                    sx={{ mb: 1 }}
                  >
                    <Link
                      underline="hover"
                      color="inherit"
                      href="#"
                      onClick={() => navigate("/admin/categories")}
                      sx={{
                        display: "flex",
                        alignItems: "center",
                        gap: 0.5,
                        "&:hover": { color: theme.palette.primary.main },
                      }}
                    >
                      <CategoryIcon fontSize="small" />
                      {t("adminCategories:title")}
                    </Link>
                    <Typography color="text.primary" fontWeight={600}>
                      {isNewCategory
                        ? t("adminCategories:newCategory")
                        : category?.namePL || t("adminCategories:editCategory")}
                    </Typography>
                  </Breadcrumbs>

                  <Typography
                    variant="h5"
                    fontWeight={700}
                    color="text.primary"
                  >
                    {isNewCategory
                      ? t("adminCategories:createNewCategory")
                      : t("adminCategories:editCategory")}
                  </Typography>
                </Box>
              </Box>

              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSubmit(onSubmit)}
                disabled={isSaving || (!isDirty && !isNewCategory)}
                sx={{
                  borderRadius: 2,
                  textTransform: "none",
                  fontWeight: 600,
                  px: 3,
                  py: 1,
                  minWidth: 140,
                  "&:hover": {
                    transform: "translateY(-2px)",
                    boxShadow: theme.shadows[8],
                  },
                  transition: "all 0.2s ease",
                }}
              >
                {isSaving
                  ? t("adminCategories:saving")
                  : isNewCategory
                  ? t("adminCategories:createCategory")
                  : t("adminCategories:saveChanges")}
              </Button>
            </Stack>
          </Paper>

          {/* Forms */}
          <form onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={3}>
              {/* Category Form */}
              <CategoryForm
                category={category}
                control={control}
                errors={errors}
                reset={reset}
              />

              {/* SubCategories Form */}
              <SubCategoriesForm
                control={control}
                errors={errors}
              />
            </Stack>
          </form>
        </Box>
      </Fade>
    </Box>
  );
});

export default CategoryPage;