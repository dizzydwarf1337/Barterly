import React, { useState, useEffect } from "react";
import {
  Box,
  Typography,
  TextField,
  Button,
  CardContent,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
  IconButton,
  FormControlLabel,
  Switch,
  alpha,
  useTheme,
  CircularProgress,
  Stack,
  Paper,
  Container,
} from "@mui/material";
import { observer } from "mobx-react-lite";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import { useForm, Controller, SubmitHandler } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import useStore from "../../../app/stores/store";

// Icons
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import AddPhotoAlternateIcon from "@mui/icons-material/AddPhotoAlternate";
import DeleteIcon from "@mui/icons-material/Delete";
import BusinessCenterIcon from "@mui/icons-material/BusinessCenter";
import HomeIcon from "@mui/icons-material/Home";
import CategoryIcon from "@mui/icons-material/Category";
import SaveIcon from "@mui/icons-material/Save";
import LocalOfferIcon from "@mui/icons-material/LocalOffer";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import DescriptionIcon from "@mui/icons-material/Description";
import PriceCheckIcon from "@mui/icons-material/PriceCheck";
import ImageIcon from "@mui/icons-material/Image";
import ApartmentIcon from "@mui/icons-material/Apartment";

// Types and API
import {
  PostType,
  PostPriceType,
  RentObjectType,
  WorkloadType,
  WorkLocationType,
  PostCurrency,
  PostFormData,
} from "../types/postTypes";
import { Category, SubCategory } from "../../categories/types/categoryTypes";
import categoryApi from "../../categories/api/categoriesApi";
import userPostApi from "../api/userPostApi";

export const PostCreatePage = observer(() => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const theme = useTheme();
  const { uiStore } = useStore();

  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>("");
  const [availableSubCategories, setAvailableSubCategories] = useState<SubCategory[]>([]);
  const [newTag, setNewTag] = useState("");
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  // Yup validation schema
  const validationSchema: Yup.ObjectSchema<PostFormData> = Yup.object().shape({
    title: Yup.string()
      .min(2, t('validation.nameMinLength'))
      .max(50, 'Title must not exceed 50 characters')
      .required(t('validation.required')),
    
    shortDescription: Yup.string()
      .min(2, 'Short description must be at least 2 characters')
      .max(200, 'Short description must not exceed 200 characters')
      .required(t('validation.required')),
    
    fullDescription: Yup.string()
      .min(10, 'Full description must be at least 10 characters')
      .max(10000, 'Full description must not exceed 10000 characters')
      .required(t('validation.required')),
    
    subCategoryId: Yup.string()
      .required(t('validation.required')),
    
    postType: Yup.mixed<PostType>()
      .oneOf(Object.values(PostType))
      .required(t('validation.required')),
    
    city: Yup.string()
      .max(100, 'City name must not exceed 100 characters')
      .optional(),
    
    region: Yup.string()
      .max(100, 'Region name must not exceed 100 characters')
      .optional(),
    
    country: Yup.string()
      .max(100, 'Country name must not exceed 100 characters')
      .optional(),
    
    street: Yup.string()
      .max(100, 'Street name must not exceed 100 characters')
      .optional(),
    
    price: Yup.number()
      .nullable()
      .defined()
      .min(0, 'Price must be positive'),
    
    minSalary: Yup.number()
      .nullable()
      .defined()
      .min(0, 'Minimum salary must be positive'),
    
    maxSalary: Yup.number()
      .nullable()
      .defined()
      .min(0, 'Maximum salary must be positive')
      .test('max-greater-than-min', 'Maximum salary must be greater than minimum salary', function(value) {
        const { minSalary } = this.parent;
        if (minSalary && value && value < minSalary) {
          return false;
        }
        return true;
      }),
    
    numberOfRooms: Yup.number()
      .nullable()
      .defined()
      .min(1, 'Number of rooms must be at least 1'),
    
    area: Yup.number()
      .nullable()
      .defined()
      .min(1, 'Area must be at least 1 m²'),
    
    floor: Yup.number()
      .nullable()
      .defined()
      .min(0, 'Floor must be 0 or higher'),
    
    currency: Yup.mixed<PostCurrency>().required(),
    postPriceType: Yup.mixed<PostPriceType>().nullable().defined(),
    rentObjectType: Yup.mixed<RentObjectType>().nullable().defined(),
    workload: Yup.mixed<WorkloadType>().nullable().defined(),
    workLocation: Yup.mixed<WorkLocationType>().nullable().defined(),
    buildingNumber: Yup.string()
      .max(10, 'Building number must not exceed 10 characters')
      .optional(),
    experienceRequired: Yup.boolean()
      .required(),
    tags: Yup.array()
      .of(Yup.string().required())
      .required(),
    mainImage: Yup.mixed<File>()
        .nullable()
        .test('fileSize', 'File is empty', (file) => !file || file.size > 0),
    secondaryImages: Yup.array()
      .of(
        Yup.mixed<File>()
          .test('fileSize', 'File is empty', (file) => !!file && file.size > 0)
          .required()
      )
      .required()
      .default([]),
  });

  const {
    control,
    handleSubmit,
    watch,
    getValues,
    setValue,
    formState: { errors }
  } = useForm<PostFormData>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      subCategoryId: "",
      postType: PostType.Common,
      title: "",
      city: undefined,
      region: undefined,
      country: undefined,
      street: undefined,
      fullDescription: "",
      shortDescription: "",
      price: null,
      postPriceType: null,
      currency: PostCurrency["Zł"],
      tags: [],
      mainImage: null,
      secondaryImages: [],
      // Rent fields
      rentObjectType: null,
      numberOfRooms: null,
      area: null,
      floor: null,
      // Work fields
      workload: null,
      workLocation: null,
      minSalary: null,
      maxSalary: null,
      buildingNumber: undefined,
      experienceRequired: false,
    },
  });

  const watchedPostType = watch("postType");
  const watchedTags = watch("tags");
  const selectedPriceType = watch("postPriceType");
  const isFree = selectedPriceType === PostPriceType.Free;  
  useEffect(() => {
    if (isFree) {
      setValue("minSalary", null);
      setValue("maxSalary", null);
      setValue("price", null);
    }
  }, [isFree]);
  // Load categories
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        setLoading(true);
        const response = await categoryApi.getCategories();
        setCategories(response.value || []);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  // Update subcategories when category changes
  useEffect(() => {
    if (selectedCategory) {
      const category = categories.find(c => c.id === selectedCategory);
      setAvailableSubCategories(category?.subCategories || []);
      setValue("subCategoryId", "");
    } else {
      setAvailableSubCategories([]);
    }
  }, [selectedCategory, categories, setValue]);

  const handleAddTag = () => {
    if (newTag.trim() && !watchedTags.includes(newTag.trim())) {
      setValue("tags", [...watchedTags, newTag.trim()]);
      setNewTag("");
    }
  };

  const handleRemoveTag = (tagToRemove: string) => {
    setValue(
      "tags",
      watchedTags.filter((tag) => tag !== tagToRemove)
    );
  };

  const handleImageUpload = (
    event: React.ChangeEvent<HTMLInputElement>,
    isMain: boolean = false
  ) => {
    const files = event.target.files;
    if (files) {
      if (isMain) {
        setValue("mainImage", files[0]);
      } else {
        const newImages = Array.from(files);
        const currentImages = watch("secondaryImages");
        setValue("secondaryImages", [...currentImages, ...newImages]);
      }
    }
  };

  const handleRemoveMainImage = () => {
    var mainImage = getValues('mainImage');
    if(mainImage)
      setValue('mainImage',null);
  }

  const handleRemoveSecondaryImage = (index: number) => {
    const currentImages = watch("secondaryImages");
    const newImages = currentImages.filter((_, i) => i !== index);
    setValue("secondaryImages", newImages);
  };

  const onSubmit: SubmitHandler<PostFormData> = async (data) => {
    setSubmitting(true);
    try {
      await userPostApi.createPost(data);
      uiStore.showSnackbar("Post created successfully!", "success");
      navigate("/posts");
    } catch (error) {
      console.error("Error submitting form:", error);
      uiStore.showSnackbar("Error creating post", "error");
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ py: 8 }}>
        <Box 
          display="flex" 
          justifyContent="center" 
          alignItems="center" 
          minHeight="50vh"
          flexDirection="column"
          gap={3}
        >
          <CircularProgress size={60} thickness={4} />
          <Typography variant="h6" color="text.secondary" sx={{ fontWeight: 500 }}>
            {t("loading")}
          </Typography>
        </Box>
      </Container>
    );
  }

  const renderPostTypeSelector = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <CategoryIcon sx={{ color: 'primary.main' }} />
          {t("postType")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          Wybierz typ ogłoszenia, które chcesz utworzyć
        </Typography>
      </Box>
      
      <Controller
        name="postType"
        control={control}
        render={({ field }) => (
          <Stack 
            direction={{ xs: "column", sm: "row" }} 
            spacing={3}
            sx={{ width: "100%" }}
          >
            {Object.values(PostType).map((type) => (
              <Box key={type} sx={{ flex: 1 }}>
                <Paper
                  elevation={0}
                  sx={{
                    cursor: "pointer",
                    border: `2px solid ${
                      field.value === type
                        ? theme.palette.primary.main
                        : alpha(theme.palette.divider, 0.2)
                    }`,
                    backgroundColor: field.value === type
                      ? alpha(theme.palette.primary.main, 0.08)
                      : alpha(theme.palette.background.paper, 0.6),
                    transition: "all 0.4s cubic-bezier(0.4, 0, 0.2, 1)",
                    borderRadius: 3,
                    overflow: 'hidden',
                    position: 'relative',
                    "&:hover": {
                      borderColor: theme.palette.primary.main,
                      transform: "translateY(-6px)",
                      boxShadow: `0 20px 40px ${alpha(theme.palette.primary.main, 0.15)}`,
                      backgroundColor: alpha(theme.palette.primary.main, 0.05),
                    },
                    "&::before": {
                      content: '""',
                      position: 'absolute',
                      top: 0,
                      left: 0,
                      right: 0,
                      height: '4px',
                      background: field.value === type 
                        ? `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`
                        : 'transparent',
                      transition: 'all 0.3s ease',
                    }
                  }}
                  onClick={() => field.onChange(type)}
                >
                  <CardContent sx={{ textAlign: "center", py: 4, px: 3 }}>
                    {type === PostType.Work && (
                      <BusinessCenterIcon
                        sx={{ 
                          fontSize: 56, 
                          mb: 2, 
                          color: field.value === type ? "primary.main" : "text.secondary",
                          transition: 'color 0.3s ease'
                        }}
                      />
                    )}
                    {type === PostType.Rent && (
                      <HomeIcon
                        sx={{ 
                          fontSize: 56, 
                          mb: 2, 
                          color: field.value === type ? "primary.main" : "text.secondary",
                          transition: 'color 0.3s ease'
                        }}
                      />
                    )}
                    {type === PostType.Common && (
                      <CategoryIcon
                        sx={{ 
                          fontSize: 56, 
                          mb: 2, 
                          color: field.value === type ? "primary.main" : "text.secondary",
                          transition: 'color 0.3s ease'
                        }}
                      />
                    )}
                    <Typography 
                      variant="h6"
                      sx={{
                        fontWeight: 700,
                        color: field.value === type ? "primary.main" : "text.primary",
                        mb: 1,
                        transition: 'color 0.3s ease'
                      }}
                    >
                      {type === PostType.Common && t("common")}
                      {type === PostType.Work && t("work")}
                      {type === PostType.Rent && t("rent")}
                    </Typography>
                    <Typography 
                      variant="body2" 
                      color="text.secondary"
                      sx={{ fontWeight: 500 }}
                    >
                      {type === PostType.Common && "Ogłoszenia ogólne"}
                      {type === PostType.Work && "Oferty pracy"}
                      {type === PostType.Rent && "Wynajem nieruchomości"}
                    </Typography>
                  </CardContent>
                </Paper>
              </Box>
            ))}
          </Stack>
        )}
      />
      {errors.postType && (
        <Typography variant="caption" color="error" sx={{ mt: 2, display: 'block', fontWeight: 500 }}>
          {errors.postType.message}
        </Typography>
      )}
    </Paper>
  );

  const renderCategorySelector = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <CategoryIcon sx={{ color: 'primary.main' }} />
          {t("category")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          Wybierz kategorię i podkategorię dla swojego ogłoszenia
        </Typography>
      </Box>
      
      <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
        <Box sx={{ flex: 1 }}>
          <FormControl fullWidth>
            <InputLabel 
              sx={{ 
                color: 'text.secondary',
                fontWeight: 500,
                '&.Mui-focused': {
                  color: 'primary.main'
                }
              }}
            >
              {t("selectCategory")}
            </InputLabel>
            <Select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              label={t("selectCategory")}
              sx={{
                borderRadius: 3,
                '& .MuiSelect-select': {
                  color: 'text.primary',
                  fontWeight: 500,
                },
                '& .MuiOutlinedInput-notchedOutline': {
                  borderColor: alpha(theme.palette.divider, 0.3),
                  transition: 'border-color 0.3s ease',
                },
                '&:hover .MuiOutlinedInput-notchedOutline': {
                  borderColor: theme.palette.primary.main,
                },
                '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                  borderColor: theme.palette.primary.main,
                  borderWidth: '2px',
                },
              }}
              MenuProps={{
                PaperProps: {
                  sx: {
                    borderRadius: 3,
                    mt: 1,
                    maxHeight: 300,
                    border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                    boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                    '& .MuiMenuItem-root': {
                      color: 'text.primary',
                      fontWeight: 500,
                      py: 1.5,
                      '&:hover': {
                        backgroundColor: alpha(theme.palette.primary.main, 0.08),
                      },
                      '&.Mui-selected': {
                        backgroundColor: alpha(theme.palette.primary.main, 0.12),
                        '&:hover': {
                          backgroundColor: alpha(theme.palette.primary.main, 0.16),
                        },
                      },
                    },
                  },
                },
              }}
            >
              {categories.map((category) => (
                <MenuItem key={category.id} value={category.id}>
                  {uiStore.lang === "en" ? category.nameEN : category.namePL}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>
        
        <Box sx={{ flex: 1 }}>
          <Controller
            name="subCategoryId"
            control={control}
            render={({ field }) => (
              <FormControl 
                fullWidth 
                error={!!errors.subCategoryId}
                disabled={!selectedCategory}
              >
                <InputLabel 
                  sx={{ 
                    color: 'text.secondary',
                    fontWeight: 500,
                    '&.Mui-focused': {
                      color: 'primary.main'
                    }
                  }}
                >
                  {t("selectSubcategory")}
                </InputLabel>
                <Select
                  {...field}
                  label={t("selectSubcategory")}
                  sx={{
                    borderRadius: 3,
                    '& .MuiSelect-select': {
                      color: 'text.primary',
                      fontWeight: 500,
                    },
                    '& .MuiOutlinedInput-notchedOutline': {
                      borderColor: alpha(theme.palette.divider, 0.3),
                      transition: 'border-color 0.3s ease',
                    },
                    '&:hover .MuiOutlinedInput-notchedOutline': {
                      borderColor: theme.palette.primary.main,
                    },
                    '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                      borderColor: theme.palette.primary.main,
                      borderWidth: '2px',
                    },
                  }}
                  MenuProps={{
                    PaperProps: {
                      sx: {
                        borderRadius: 3,
                        mt: 1,
                        maxHeight: 300,
                        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                        boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                        '& .MuiMenuItem-root': {
                          color: 'text.primary',
                          fontWeight: 500,
                          py: 1.5,
                          '&:hover': {
                            backgroundColor: alpha(theme.palette.primary.main, 0.08),
                          },
                          '&.Mui-selected': {
                            backgroundColor: alpha(theme.palette.primary.main, 0.12),
                            '&:hover': {
                              backgroundColor: alpha(theme.palette.primary.main, 0.16),
                            },
                          },
                        },
                      },
                    },
                  }}
                >
                  {availableSubCategories.map((subCategory) => (
                    <MenuItem key={subCategory.id} value={subCategory.id}>
                      {uiStore.getLanguage() === "pl" ? subCategory.namePL : subCategory.nameEN}
                    </MenuItem>
                  ))}
                </Select>
                {errors.subCategoryId && (
                  <Typography variant="caption" color="error" sx={{ mt: 1, fontWeight: 500 }}>
                    {errors.subCategoryId.message}
                  </Typography>
                )}
              </FormControl>
            )}
          />
        </Box>
      </Stack>
    </Paper>
  );

  const renderBasicFields = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <DescriptionIcon sx={{ color: 'primary.main' }} />
          {t("basicInformation")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          Podaj podstawowe informacje o swoim ogłoszeniu
        </Typography>
      </Box>
      
      <Stack spacing={3}>
        <Controller
          name="title"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t("title")}
              error={!!errors.title}
              helperText={errors.title?.message}
              fullWidth
              required
              sx={{
                '& .MuiOutlinedInput-root': {
                  borderRadius: 3,
                  backgroundColor: alpha(theme.palette.background.paper, 0.5),
                  '& fieldset': {
                    borderColor: alpha(theme.palette.divider, 0.3),
                    transition: 'border-color 0.3s ease',
                  },
                  '&:hover fieldset': {
                    borderColor: theme.palette.primary.main,
                  },
                  '&.Mui-focused fieldset': {
                    borderColor: theme.palette.primary.main,
                    borderWidth: '2px',
                  },
                },
                '& .MuiInputLabel-root': {
                  fontWeight: 500,
                  '&.Mui-focused': {
                    color: 'primary.main',
                  },
                },
              }}
            />
          )}
        />
        
        <Controller
          name="shortDescription"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t("shortDescription")}
              error={!!errors.shortDescription}
              helperText={errors.shortDescription?.message}
              fullWidth
              required
              multiline
              rows={3}
              sx={{
                '& .MuiOutlinedInput-root': {
                  borderRadius: 3,
                  backgroundColor: alpha(theme.palette.background.paper, 0.5),
                  '& fieldset': {
                    borderColor: alpha(theme.palette.divider, 0.3),
                    transition: 'border-color 0.3s ease',
                  },
                  '&:hover fieldset': {
                    borderColor: theme.palette.primary.main,
                  },
                  '&.Mui-focused fieldset': {
                    borderColor: theme.palette.primary.main,
                    borderWidth: '2px',
                  },
                },
                '& .MuiInputLabel-root': {
                  fontWeight: 500,
                  '&.Mui-focused': {
                    color: 'primary.main',
                  },
                },
              }}
            />
          )}
        />
        
        <Controller
          name="fullDescription"
          control={control}
          render={({ field }) => (
            <TextField
              {...field}
              label={t("fullDescription")}
              error={!!errors.fullDescription}
              helperText={errors.fullDescription?.message}
              fullWidth
              required
              multiline
              rows={6}
              sx={{
                '& .MuiOutlinedInput-root': {
                  borderRadius: 3,
                  backgroundColor: alpha(theme.palette.background.paper, 0.5),
                  '& fieldset': {
                    borderColor: alpha(theme.palette.divider, 0.3),
                    transition: 'border-color 0.3s ease',
                  },
                  '&:hover fieldset': {
                    borderColor: theme.palette.primary.main,
                  },
                  '&.Mui-focused fieldset': {
                    borderColor: theme.palette.primary.main,
                    borderWidth: '2px',
                  },
                },
                '& .MuiInputLabel-root': {
                  fontWeight: 500,
                  '&.Mui-focused': {
                    color: 'primary.main',
                  },
                },
              }}
            />
          )}
        />
      </Stack>
    </Paper>
  );

  const renderLocationFields = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <LocationOnIcon sx={{ color: 'primary.main' }} />
          {t("location")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          Podaj lokalizację dla swojego ogłoszenia
        </Typography>
      </Box>
      
      <Stack spacing={3}>
        <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
          <Box sx={{ flex: 1 }}>
            <Controller
              name="country"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  value={field.value || ""}
                  label={t("country")}
                  error={!!errors.country}
                  helperText={errors.country?.message}
                  fullWidth
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& fieldset': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover fieldset': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused fieldset': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    },
                    '& .MuiInputLabel-root': {
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main',
                      },
                    },
                  }}
                />
              )}
            />
          </Box>
          <Box sx={{ flex: 1 }}>
            <Controller
              name="city"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  value={field.value || ""}
                  label={t("city")}
                  error={!!errors.city}
                  helperText={errors.city?.message}
                  fullWidth
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& fieldset': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover fieldset': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused fieldset': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    },
                    '& .MuiInputLabel-root': {
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main',
                      },
                    },
                  }}
                />
              )}
            />
          </Box>
        </Stack>
        
        <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
          <Box sx={{ flex: 1 }}>
            <Controller
              name="region"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  value={field.value || ""}
                  label={t("region")}
                  error={!!errors.region}
                  helperText={errors.region?.message}
                  fullWidth
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& fieldset': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover fieldset': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused fieldset': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    },
                    '& .MuiInputLabel-root': {
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main',
                      },
                    },
                  }}
                />
              )}
            />
          </Box>
          <Box sx={{ flex: 1 }}>
            <Controller
              name="street"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  value={field.value || ""}
                  label={t("street")}
                  error={!!errors.street}
                  helperText={errors.street?.message}
                  fullWidth
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& fieldset': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover fieldset': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused fieldset': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    },
                    '& .MuiInputLabel-root': {
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main',
                      },
                    },
                  }}
                />
              )}
            />
          </Box>
        </Stack>

        {/* Building Number - moved from Work Details */}
        <Box sx={{ maxWidth: { xs: "100%", md: "50%" } }}>
          <Controller
            name="buildingNumber"
            control={control}
            render={({ field }) => (
              <TextField
                {...field}
                value={field.value || ""}
                label={t("houseNumber")}
                error={!!errors.buildingNumber}
                helperText={errors.buildingNumber?.message}
                fullWidth
                sx={{
                  '& .MuiOutlinedInput-root': {
                    borderRadius: 3,
                    backgroundColor: alpha(theme.palette.background.paper, 0.5),
                    '& fieldset': {
                      borderColor: alpha(theme.palette.divider, 0.3),
                      transition: 'border-color 0.3s ease',
                    },
                    '&:hover fieldset': {
                      borderColor: theme.palette.primary.main,
                    },
                    '&.Mui-focused fieldset': {
                      borderColor: theme.palette.primary.main,
                      borderWidth: '2px',
                    },
                  },
                  '& .MuiInputLabel-root': {
                    fontWeight: 500,
                    '&.Mui-focused': {
                      color: 'primary.main',
                    },
                  },
                }}
              />
            )}
          />
        </Box>
      </Stack>
    </Paper>
  );

  const renderPriceFields = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <PriceCheckIcon sx={{ color: 'primary.main' }} />
          {watchedPostType === PostType.Work ? t("salary") : t("price")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          {watchedPostType === PostType.Work ? 
            "Określ widełki wynagrodzenia" : 
            "Ustaw cenę dla swojego ogłoszenia"
          }
        </Typography>
      </Box>
      
      <Stack spacing={3}>
        {watchedPostType === PostType.Work ? (
          
          <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
            {!isFree && (
              <>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="minSalary"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    value={field.value || ""}
                    onChange={(e) => {
                      const value = e.target.value;
                      field.onChange(value === "" ? null : Number(value));
                    }}
                    label={t("minSalary")}
                    type="number"
                    error={!!errors.minSalary}
                    helperText={errors.minSalary?.message}
                    fullWidth
                    sx={{
                      '& .MuiOutlinedInput-root': {
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& fieldset': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover fieldset': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused fieldset': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      },
                      '& .MuiInputLabel-root': {
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main',
                        },
                      },
                    }}
                  />
                )}
              />
            </Box>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="maxSalary"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    value={field.value || ""}
                    onChange={(e) => {
                      const value = e.target.value;
                      field.onChange(value === "" ? null : Number(value));
                    }}
                    label={t("maxSalary")}
                    type="number"
                    error={!!errors.maxSalary}
                    helperText={errors.maxSalary?.message}
                    fullWidth
                    sx={{
                      '& .MuiOutlinedInput-root': {
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& fieldset': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover fieldset': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused fieldset': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      },
                      '& .MuiInputLabel-root': {
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main',
                        },
                      },
                    }}
                  />
                )}
              />
            </Box>
            </>
       )}
          </Stack>
   
        ) : 
          !isFree && (
          <Box sx={{ maxWidth: "100%" }}>
 
            <Controller
              name="price"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  value={field.value || ""}
                  onChange={(e) => {
                    const value = e.target.value;
                    field.onChange(value === "" ? null : Number(value));
                  }}
                  label={t("price")}
                  type="number"
                  error={!!errors.price}
                  helperText={errors.price?.message}
                  fullWidth
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& fieldset': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover fieldset': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused fieldset': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    },
                    '& .MuiInputLabel-root': {
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main',
                      },
                    },
                  }}
                />
              )}
            />
            
          </Box>
          )}

        <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
          <Box sx={{ flex: 1 }}>
            <Controller
              name="postPriceType"
              control={control}
              render={({ field }) => (
                <FormControl fullWidth>
                  <InputLabel 
                    sx={{ 
                      color: 'text.secondary',
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main'
                      }
                    }}
                  >
                    {t("priceType")}
                  </InputLabel>
                  <Select
                    {...field}
                    value={field.value !== null && field.value !== undefined ? field.value : ""}
                    onChange={(e) => {
                      const value = e.target.value;
                      // Fixed enum parsing - ensure it's parsed as integer
                      field.onChange(value === "" ? null : parseInt(value.toString(), 10));
                    }}
                    label={t("priceType")}
                    sx={{
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& .MuiSelect-select': {
                        color: 'text.primary',
                        fontWeight: 500,
                      },
                      '& .MuiOutlinedInput-notchedOutline': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover .MuiOutlinedInput-notchedOutline': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    }}
                    MenuProps={{
                      PaperProps: {
                        sx: {
                          borderRadius: 3,
                          mt: 1,
                          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                          boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                          '& .MuiMenuItem-root': {
                            color: 'text.primary',
                            fontWeight: 500,
                            py: 1.5,
                            '&:hover': {
                              backgroundColor: alpha(theme.palette.primary.main, 0.08),
                            },
                            '&.Mui-selected': {
                              backgroundColor: alpha(theme.palette.primary.main, 0.12),
                              '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.16),
                              },
                            },
                          },
                        },
                      },
                    }}
                  >
                    {Object.entries(PostPriceType)
                      .filter(([key]) => isNaN(Number(key)))
                      .map(([key, value]) => (
                        <MenuItem key={value} value={value}>
                          {t(key)}
                        </MenuItem>
                      ))}
                  </Select>
                </FormControl>
              )}
            />
          </Box>
                  {!isFree && (
          <Box sx={{ flex: 1 }}>
            <Controller
              name="currency"
              control={control}
              render={({ field }) => (
                <FormControl fullWidth>
                  <InputLabel 
                    sx={{ 
                      color: 'text.secondary',
                      fontWeight: 500,
                      '&.Mui-focused': {
                        color: 'primary.main'
                      }
                    }}
                  >
                    {t("currency")}
                  </InputLabel>
                  <Select
                    {...field}
                    label={t("currency")}
                    sx={{
                      borderRadius: 3,
                      backgroundColor: alpha(theme.palette.background.paper, 0.5),
                      '& .MuiSelect-select': {
                        color: 'text.primary',
                        fontWeight: 500,
                      },
                      '& .MuiOutlinedInput-notchedOutline': {
                        borderColor: alpha(theme.palette.divider, 0.3),
                        transition: 'border-color 0.3s ease',
                      },
                      '&:hover .MuiOutlinedInput-notchedOutline': {
                        borderColor: theme.palette.primary.main,
                      },
                      '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                        borderColor: theme.palette.primary.main,
                        borderWidth: '2px',
                      },
                    }}
                    MenuProps={{
                      PaperProps: {
                        sx: {
                          borderRadius: 3,
                          mt: 1,
                          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                          boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                          '& .MuiMenuItem-root': {
                            color: 'text.primary',
                            fontWeight: 500,
                            py: 1.5,
                            '&:hover': {
                              backgroundColor: alpha(theme.palette.primary.main, 0.08),
                            },
                            '&.Mui-selected': {
                              backgroundColor: alpha(theme.palette.primary.main, 0.12),
                              '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.16),
                              },
                            },
                          },
                        },
                      },
                    }}
                  >
                    {Object.entries(PostCurrency)
                      .filter(([key]) => isNaN(Number(key)))
                      .map(([key, value]) => (
                        <MenuItem key={value} value={value}>
                          {key}
                        </MenuItem>
                      ))}
                  </Select>
                </FormControl>
              )}
            />
          </Box>
                  )}
        </Stack>
      </Stack>
    </Paper>
  );

  const renderRentFields = () => {
    if (watchedPostType !== PostType.Rent) return null;

    return (
      <Paper 
        elevation={0}
        sx={{ 
          p: 4,
          borderRadius: 4,
          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
          background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
          backdropFilter: 'blur(20px)',
          mb: 3,
        }}
      >
        <Box sx={{ mb: 4 }}>
          <Typography 
            variant="h5" 
            sx={{ 
              fontWeight: 700,
              color: 'text.primary',
              mb: 1,
              display: 'flex',
              alignItems: 'center',
              gap: 2
            }}
          >
            <ApartmentIcon sx={{ color: 'primary.main' }} />
            {t("propertyDetails")}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
            Podaj szczegóły dotyczące wynajmowanej nieruchomości
          </Typography>
        </Box>
        
        <Stack spacing={3}>
          <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="rentObjectType"
                control={control}
                render={({ field }) => (
                  <FormControl fullWidth>
                    <InputLabel 
                      sx={{ 
                        color: 'text.secondary',
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main'
                        }
                      }}
                    >
                      {t("propertyType")}
                    </InputLabel>
                    <Select
                      {...field}
                      value={field.value !== null && field.value !== undefined ? field.value : ""}
                      onChange={(e) => {
                        const value = e.target.value;
                        // Fixed enum parsing - ensure it's parsed as integer
                        field.onChange(value === "" ? null : parseInt(value.toString(), 10));
                      }}
                      label={t("propertyType")}
                      sx={{
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& .MuiSelect-select': {
                          color: 'text.primary',
                          fontWeight: 500,
                        },
                        '& .MuiOutlinedInput-notchedOutline': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover .MuiOutlinedInput-notchedOutline': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      }}
                      MenuProps={{
                        PaperProps: {
                          sx: {
                            borderRadius: 3,
                            mt: 1,
                            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                            boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                            '& .MuiMenuItem-root': {
                              color: 'text.primary',
                              fontWeight: 500,
                              py: 1.5,
                              '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.08),
                              },
                              '&.Mui-selected': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.12),
                                '&:hover': {
                                  backgroundColor: alpha(theme.palette.primary.main, 0.16),
                                },
                              },
                            },
                          },
                        },
                      }}
                    >
                      {Object.entries(RentObjectType)
                        .filter(([key]) => isNaN(Number(key)))
                        .map(([key, value]) => (
                          <MenuItem key={value} value={value}>
                            {t(key)}
                          </MenuItem>
                        ))}
                    </Select>
                  </FormControl>
                )}
              />
            </Box>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="numberOfRooms"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    value={field.value || ""}
                    onChange={(e) => {
                      const value = e.target.value;
                      field.onChange(value === "" ? null : Number(value));
                    }}
                    label={t("numberOfRooms")}
                    type="number"
                    error={!!errors.numberOfRooms}
                    helperText={errors.numberOfRooms?.message}
                    fullWidth
                    sx={{
                      '& .MuiOutlinedInput-root': {
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& fieldset': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover fieldset': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused fieldset': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      },
                      '& .MuiInputLabel-root': {
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main',
                        },
                      },
                    }}
                  />
                )}
              />
            </Box>
          </Stack>
          
          <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="area"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    value={field.value || ""}
                    onChange={(e) => {
                      const value = e.target.value;
                      field.onChange(value === "" ? null : Number(value));
                    }}
                    label={t("areaSquareMeters")}
                    type="number"
                    error={!!errors.area}
                    helperText={errors.area?.message}
                    fullWidth
                    sx={{
                      '& .MuiOutlinedInput-root': {
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& fieldset': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover fieldset': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused fieldset': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      },
                      '& .MuiInputLabel-root': {
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main',
                        },
                      },
                    }}
                  />
                )}
              />
            </Box>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="floor"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    value={field.value || ""}
                    onChange={(e) => {
                      const value = e.target.value;
                      field.onChange(value === "" ? null : Number(value));
                    }}
                    label={t("floor")}
                    type="number"
                    error={!!errors.floor}
                    helperText={errors.floor?.message}
                    fullWidth
                    sx={{
                      '& .MuiOutlinedInput-root': {
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& fieldset': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover fieldset': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused fieldset': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      },
                      '& .MuiInputLabel-root': {
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main',
                        },
                      },
                    }}
                  />
                )}
              />
            </Box>
          </Stack>
        </Stack>
      </Paper>
    );
  };

  const renderWorkFields = () => {
    if (watchedPostType !== PostType.Work) return null;

    return (
      <Paper 
        elevation={0}
        sx={{ 
          p: 4,
          borderRadius: 4,
          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
          background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
          backdropFilter: 'blur(20px)',
          mb: 3,
        }}
      >
        <Box sx={{ mb: 4 }}>
          <Typography 
            variant="h5" 
            sx={{ 
              fontWeight: 700,
              color: 'text.primary',
              mb: 1,
              display: 'flex',
              alignItems: 'center',
              gap: 2
            }}
          >
            <BusinessCenterIcon sx={{ color: 'primary.main' }} />
            {t("workDetails")}
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
            Określ szczegóły oferty pracy
          </Typography>
        </Box>
        
        <Stack spacing={3}>
          <Stack direction={{ xs: "column", md: "row" }} spacing={3}>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="workload"
                control={control}
                render={({ field }) => (
                  <FormControl fullWidth>
                    <InputLabel 
                      sx={{ 
                        color: 'text.secondary',
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main'
                        }
                      }}
                    >
                      {t("workload")}
                    </InputLabel>
                    <Select
                      {...field}
                      value={field.value !== null && field.value !== undefined ? field.value : ""}
                      onChange={(e) => {
                        const value = e.target.value;
                        // Fixed enum parsing - ensure it's parsed as integer
                        field.onChange(value === "" ? null : parseInt(value.toString(), 10));
                      }}
                      label={t("workload")}
                      sx={{
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& .MuiSelect-select': {
                          color: 'text.primary',
                          fontWeight: 500,
                        },
                        '& .MuiOutlinedInput-notchedOutline': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover .MuiOutlinedInput-notchedOutline': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      }}
                      MenuProps={{
                        PaperProps: {
                          sx: {
                            borderRadius: 3,
                            mt: 1,
                            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                            boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                            '& .MuiMenuItem-root': {
                              color: 'text.primary',
                              fontWeight: 500,
                              py: 1.5,
                              '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.08),
                              },
                              '&.Mui-selected': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.12),
                                '&:hover': {
                                  backgroundColor: alpha(theme.palette.primary.main, 0.16),
                                },
                              },
                            },
                          },
                        },
                      }}
                    >
                      {Object.entries(WorkloadType)
                        .filter(([key]) => isNaN(Number(key)))
                        .map(([key, value]) => (
                          <MenuItem key={value} value={value}>
                            {t(key)}
                          </MenuItem>
                        ))}
                    </Select>
                  </FormControl>
                )}
              />
            </Box>
            <Box sx={{ flex: 1 }}>
              <Controller
                name="workLocation"
                control={control}
                render={({ field }) => (
                  <FormControl fullWidth>
                    <InputLabel 
                      sx={{ 
                        color: 'text.secondary',
                        fontWeight: 500,
                        '&.Mui-focused': {
                          color: 'primary.main'
                        }
                      }}
                    >
                      {t("workLocation")}
                    </InputLabel>
                    <Select
                      {...field}
                      value={field.value !== null && field.value !== undefined ? field.value : ""}
                      onChange={(e) => {
                        const value = e.target.value;
                        // Fixed enum parsing - ensure it's parsed as integer
                        field.onChange(value === "" ? null : parseInt(value.toString(), 10));
                      }}
                      label={t("workLocation")}
                      sx={{
                        borderRadius: 3,
                        backgroundColor: alpha(theme.palette.background.paper, 0.5),
                        '& .MuiSelect-select': {
                          color: 'text.primary',
                          fontWeight: 500,
                        },
                        '& .MuiOutlinedInput-notchedOutline': {
                          borderColor: alpha(theme.palette.divider, 0.3),
                          transition: 'border-color 0.3s ease',
                        },
                        '&:hover .MuiOutlinedInput-notchedOutline': {
                          borderColor: theme.palette.primary.main,
                        },
                        '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                          borderColor: theme.palette.primary.main,
                          borderWidth: '2px',
                        },
                      }}
                      MenuProps={{
                        PaperProps: {
                          sx: {
                            borderRadius: 3,
                            mt: 1,
                            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                            boxShadow: `0 10px 40px ${alpha('#000000', 0.1)}`,
                            '& .MuiMenuItem-root': {
                              color: 'text.primary',
                              fontWeight: 500,
                              py: 1.5,
                              '&:hover': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.08),
                              },
                              '&.Mui-selected': {
                                backgroundColor: alpha(theme.palette.primary.main, 0.12),
                                '&:hover': {
                                  backgroundColor: alpha(theme.palette.primary.main, 0.16),
                                },
                              },
                            },
                          },
                        },
                      }}
                    >
                      {Object.entries(WorkLocationType)
                        .filter(([key]) => isNaN(Number(key)))
                        .map(([key, value]) => (
                          <MenuItem key={value} value={value}>
                            {t(key)}
                          </MenuItem>
                        ))}
                    </Select>
                  </FormControl>
                )}
              />
            </Box>
          </Stack>

          {/* Experience Required Switch */}
          <Box sx={{ 
            display: 'flex', 
            alignItems: 'center', 
            justifyContent: 'center',
            p: 3,
            backgroundColor: alpha(theme.palette.background.paper, 0.5),
            borderRadius: 3,
            border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
          }}>
            <Controller
              name="experienceRequired"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      onChange={field.onChange}
                      sx={{
                        '& .MuiSwitch-switchBase.Mui-checked': {
                          color: 'primary.main',
                          '& + .MuiSwitch-track': {
                            backgroundColor: 'primary.main',
                          },
                        },
                        '& .MuiSwitch-track': {
                          backgroundColor: alpha(theme.palette.divider, 0.3),
                        },
                      }}
                    />
                  }
                  label={
                    <Typography variant="body1" sx={{ fontWeight: 600, color: 'text.primary' }}>
                      {t("experienceRequired")}
                    </Typography>
                  }
                />
              )}
            />
          </Box>
        </Stack>
      </Paper>
    );
  };

  const renderTagsField = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <LocalOfferIcon sx={{ color: 'primary.main' }} />
          {t("tags")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          Dodaj tagi, które pomogą znaleźć Twoje ogłoszenie
        </Typography>
      </Box>
      
      <Stack spacing={3}>
        <Stack direction={{ xs: "column", sm: "row" }} spacing={2} alignItems="flex-start">
          <Box sx={{ flex: 1, minWidth: { xs: "100%", sm: "200px" } }}>
            <TextField
              label={t("addTag")}
              value={newTag}
              onChange={(e) => setNewTag(e.target.value)}
              size="small"
              fullWidth
              sx={{ 
                '& .MuiOutlinedInput-root': {
                  borderRadius: 3,
                  backgroundColor: alpha(theme.palette.background.paper, 0.5),
                  '& fieldset': {
                    borderColor: alpha(theme.palette.divider, 0.3),
                    transition: 'border-color 0.3s ease',
                  },
                  '&:hover fieldset': {
                    borderColor: theme.palette.primary.main,
                  },
                  '&.Mui-focused fieldset': {
                    borderColor: theme.palette.primary.main,
                    borderWidth: '2px',
                  },
                },
                '& .MuiInputLabel-root': {
                  fontWeight: 500,
                  '&.Mui-focused': {
                    color: 'primary.main',
                  },
                },
              }}
            />
          </Box>
          <Button 
            variant="contained" 
            onClick={handleAddTag}
            sx={{ 
              borderRadius: 3,
              minHeight: "40px",
              fontWeight: 600,
              px: 3,
              background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
              boxShadow: `0 4px 12px ${alpha(theme.palette.primary.main, 0.3)}`,
              '&:hover': {
                transform: "translateY(-1px)",
                boxShadow: `0 6px 16px ${alpha(theme.palette.primary.main, 0.4)}`,
              },
            }}
          >
            {t("add")}
          </Button>
        </Stack>
        
        {watchedTags.length > 0 && (
          <Box 
            sx={{ 
              p: 3, 
              backgroundColor: alpha(theme.palette.background.paper, 0.5),
              borderRadius: 3,
              border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
            }}
          >
            <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 2, fontWeight: 600 }}>
              Dodane tagi ({watchedTags.length})
            </Typography>
            <Box display="flex" gap={1} flexWrap="wrap">
              {watchedTags.map((tag) => (
                <Chip
                  key={tag}
                  label={tag}
                  onDelete={() => handleRemoveTag(tag)}
                  icon={<LocalOfferIcon />}
                  sx={{
                    borderRadius: 2,
                    fontWeight: 500,
                    backgroundColor: alpha(theme.palette.primary.main, 0.1),
                    color: 'primary.main',
                    border: `1px solid ${alpha(theme.palette.primary.main, 0.2)}`,
                    '& .MuiChip-deleteIcon': {
                      color: 'primary.main',
                      '&:hover': {
                        color: 'primary.dark',
                      },
                    },
                    '&:hover': {
                      backgroundColor: alpha(theme.palette.primary.main, 0.15),
                    },
                  }}
                />
              ))}
            </Box>
          </Box>
        )}
      </Stack>
    </Paper>
  );

  const renderImageUpload = () => (
    <Paper 
      elevation={0}
      sx={{ 
        p: 4,
        borderRadius: 4,
        border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
        backdropFilter: 'blur(20px)',
        mb: 3,
      }}
    >
      <Box sx={{ mb: 4 }}>
        <Typography 
          variant="h5" 
          sx={{ 
            fontWeight: 700,
            color: 'text.primary',
            mb: 1,
            display: 'flex',
            alignItems: 'center',
            gap: 2
          }}
        >
          <ImageIcon sx={{ color: 'primary.main' }} />
          {t("images")}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
          Dodaj zdjęcia do swojego ogłoszenia
        </Typography>
      </Box>
      
      <Stack spacing={2}>
        {/* Main Image */}
        <Box display="flex" flexDirection={"column"}>
          <Box display="flex" flexDirection={"row"} justifyContent={"space-between"}>
          <Typography variant="h6" sx={{ mb: 2, fontWeight: 600, color: 'text.primary' }}>
            {t("mainImage")}
          </Typography>
          {getValues('mainImage') &&
          <IconButton
                      size="small"
                      onClick={() => handleRemoveMainImage()}
                      sx={{
                        color: 'error.main',
                        '&:hover': {
                          backgroundColor: alpha(theme.palette.error.main, 0.1),
                        },
                      }}
                    >
                      <DeleteIcon />
          </IconButton>
          }
          </Box>
          <Paper
            elevation={0}
            sx={{
              border: `2px dashed ${alpha(theme.palette.primary.main, 0.3)}`,
              borderRadius: 4,
              p: 4,
              textAlign: "center",
              cursor: "pointer",
              transition: "all 0.3s ease",
              backgroundColor: watch("mainImage") 
                ? alpha(theme.palette.primary.main, 0.05)
                : alpha(theme.palette.background.paper, 0.5),
              "&:hover": {
                borderColor: theme.palette.primary.main,
                backgroundColor: alpha(theme.palette.primary.main, 0.08),
                transform: "translateY(-2px)",
                boxShadow: `0 8px 24px ${alpha(theme.palette.primary.main, 0.15)}`,
              },
            }}
            component="label"
          >
            <input
              type="file"
              hidden
              accept="image/*"
              onChange={(e) => handleImageUpload(e, true)}
            />
            <AddPhotoAlternateIcon
              sx={{ 
                fontSize: 64, 
                color: watch("mainImage") ? "primary.main" : "text.secondary", 
                mb: 2 
              }}
            />
            <Typography 
              variant="h6" 
              color={watch("mainImage") ? "primary.main" : "text.secondary"}
              sx={{ fontWeight: 600, mb: 1 }}
            >
              {watch("mainImage")
                ? watch("mainImage")?.name
                : t("selectMainImage")
              }
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
              {t("clickToSelectImage")}
            </Typography>
          </Paper>
        </Box>

        {/* Secondary Images */}
        <Box display="flex" flexDirection={"column"}>
          <Typography variant="h6" sx={{ mb: 2, fontWeight: 600, color: 'text.primary' }}>
            {t("additionalImages")}
          </Typography>
          <Paper
            elevation={0}
            sx={{
              border: `2px dashed ${alpha(theme.palette.primary.main, 0.3)}`,
              borderRadius: 4,
              p: 4,
              textAlign: "center",
              cursor: "pointer",
              transition: "all 0.3s ease",
              backgroundColor: watch("secondaryImages").length > 0 
                ? alpha(theme.palette.primary.main, 0.05)
                : alpha(theme.palette.background.paper, 0.5),
              "&:hover": {
                borderColor: theme.palette.primary.main,
                backgroundColor: alpha(theme.palette.primary.main, 0.08),
                transform: "translateY(-2px)",
                boxShadow: `0 8px 24px ${alpha(theme.palette.primary.main, 0.15)}`,
              },
            }}
            component="label"
          >
            <input
              type="file"
              hidden
              accept="image/*"
              multiple
              onChange={(e) => handleImageUpload(e, false)}
            />
            <AddPhotoAlternateIcon
              sx={{ 
                fontSize: 64, 
                color: watch("secondaryImages").length > 0 ? "primary.main" : "text.secondary", 
                mb: 2 
              }}
            />
            <Typography 
              variant="h6" 
              color={watch("secondaryImages").length > 0 ? "primary.main" : "text.secondary"}
              sx={{ fontWeight: 600, mb: 1 }}
            >
              {t("selectAdditionalImages")}
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 500 }}>
              {t("clickToSelectMultipleImages")}
            </Typography>
          </Paper>

          {watch("secondaryImages").length > 0 && (
            <Box sx={{ mt: 3 }}>
              <Typography variant="subtitle2" color="text.secondary" sx={{ mb: 2, fontWeight: 600 }}>
                {t("selectedImages")} ({watch("secondaryImages").length})
              </Typography>
              <Stack spacing={1}>
                {watch("secondaryImages").map((file, index) => (
                  <Paper
                    key={index}
                    elevation={0}
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "space-between",
                      p: 2,
                      backgroundColor: alpha(theme.palette.background.paper, 0.6),
                      borderRadius: 2,
                      border: `1px solid ${alpha(theme.palette.divider, 0.2)}`,
                    }}
                  >
                    <Typography variant="body2" color="text.primary" sx={{ fontWeight: 500 }}>
                      {file.name}
                    </Typography>
                    <IconButton
                      size="small"
                      onClick={() => handleRemoveSecondaryImage(index)}
                      sx={{
                        color: 'error.main',
                        '&:hover': {
                          backgroundColor: alpha(theme.palette.error.main, 0.1),
                        },
                      }}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Paper>
                ))}
              </Stack>
            </Box>
          )}
        </Box>
      </Stack>
    </Paper>
  );

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      {/* Header */}
      <Paper 
        elevation={0}
        sx={{ 
          p: 4,
          mb: 4,
          borderRadius: 4,
          background: `linear-gradient(135deg, ${alpha(theme.palette.primary.main, 0.05)} 0%, ${alpha(theme.palette.secondary.main, 0.05)} 100%)`,
          border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
        }}
      >
        <Stack direction="row" alignItems="center" spacing={3}>
          <IconButton 
            onClick={() => navigate(-1)} 
            sx={{ 
              color: 'primary.main',
              backgroundColor: alpha(theme.palette.primary.main, 0.1),
              borderRadius: 2,
              '&:hover': {
                backgroundColor: alpha(theme.palette.primary.main, 0.2),
                transform: 'translateY(-2px)',
              },
              transition: 'all 0.3s ease',
            }}
          >
            <ArrowBackIcon />
          </IconButton>
          <Box>
            <Typography 
              variant="h3" 
              component="h1"
              sx={{ 
                fontWeight: 800,
                background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.secondary.main} 100%)`,
                backgroundClip: 'text',
                WebkitBackgroundClip: 'text',
                WebkitTextFillColor: 'transparent',
                mb: 1,
              }}
            >
              {t("addPost")}
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ fontWeight: 500 }}>
              Utwórz nowe ogłoszenie w kilku prostych krokach
            </Typography>
          </Box>
        </Stack>
      </Paper>

      <form onSubmit={handleSubmit(onSubmit)}>
        <Stack spacing={0}>
          {/* Post Type Selector */}
          {renderPostTypeSelector()}

          {/* Category Selector */}
          {renderCategorySelector()}

          {/* Basic Information */}
          {renderBasicFields()}

          {/* Location */}
          {renderLocationFields()}

          {/* Price/Salary */}
          {renderPriceFields()}

          {/* Rent Specific Fields */}
          {renderRentFields()}

          {/* Work Specific Fields */}
          {renderWorkFields()}

          {/* Tags */}
          {renderTagsField()}

          {/* Image Upload */}
          {renderImageUpload()}

          {/* Submit Button */}
          <Paper 
            elevation={0}
            sx={{ 
              p: 4,
              borderRadius: 4,
              border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
              background: `linear-gradient(145deg, ${alpha(theme.palette.background.paper, 0.8)} 0%, ${alpha(theme.palette.background.default, 0.4)} 100%)`,
              backdropFilter: 'blur(20px)',
            }}
          >
            <Stack direction="row" justifyContent="flex-end" spacing={3}>
              <Button
                variant="outlined"
                onClick={() => navigate(-1)}
                size="large"
                disabled={submitting}
                sx={{
                  borderRadius: 3,
                  fontWeight: 600,
                  px: 4,
                  py: 1.5,
                  borderColor: alpha(theme.palette.divider, 0.5),
                  color: 'text.primary',
                  '&:hover': {
                    borderColor: theme.palette.primary.main,
                    backgroundColor: alpha(theme.palette.primary.main, 0.05),
                  },
                }}
              >
                {t("cancel")}
              </Button>
              <Button
                type="submit"
                variant="contained"
                startIcon={submitting ? <CircularProgress size={20} color="inherit" /> : <SaveIcon />}
                size="large"
                disabled={submitting}
                sx={{
                  borderRadius: 3,
                  fontWeight: 700,
                  px: 6,
                  py: 1.5,
                  background: `linear-gradient(135deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.dark} 100%)`,
                  boxShadow: `0 8px 24px ${alpha(theme.palette.primary.main, 0.3)}`,
                  '&:hover': {
                    transform: "translateY(-2px)",
                    boxShadow: `0 12px 32px ${alpha(theme.palette.primary.main, 0.4)}`,
                  },
                  '&:disabled': {
                    background: alpha(theme.palette.action.disabled, 0.3),
                    color: 'text.disabled',
                  },
                }}
              >
                {submitting ? t("creating") : t("createPost")}
              </Button>
            </Stack>
          </Paper>
        </Stack>
      </form>
    </Container>
  );
});