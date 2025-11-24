import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import useStore from "../../../app/stores/store";
import userApi from "../api/userApi";
import { UserData } from "../types/userTypes";
import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import {
  Box,
  TextField,
  Button,
  Avatar,
  IconButton,
  Paper,
  Typography,
  Grid,
  CircularProgress,
} from "@mui/material";
import { PhotoCamera, Delete } from "@mui/icons-material";

type ProfileFormData = yup.InferType<typeof profileSchema>;

const profileSchema = yup.object({
  firstName: yup
    .string()
    .required("user:FirstNameRequired")
    .max(50, "user:FirstNameTooLong")
    .default(""),
  lastName: yup
    .string()
    .required("user:LastNameRequired")
    .max(50, "user:LastNameTooLong")
    .default(""),
  bio: yup.string().max(500, "user:BioTooLong").default(""),
  country: yup.string().max(100, "user:CountryTooLong").default(""),
  city: yup.string().max(100, "user:CityTooLong").default(""),
  street: yup.string().max(200, "user:StreetTooLong").default(""),
  houseNumber: yup.string().max(20, "user:HouseNumberTooLong").default(""),
  postalCode: yup.string().max(20, "user:PostalCodeTooLong").default(""),
  file: yup.mixed<FileList>().optional(),
});

const UserProfilePage = () => {
  const [userData, setUserData] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(false);
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [isPhotoDeleted, setIsPhotoDeleted] = useState(false);
  const { authStore, uiStore } = useStore();
  const { t } = useTranslation();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    watch,
    setValue,
  } = useForm<ProfileFormData>({
    resolver: yupResolver(profileSchema) as unknown as any,
  });

  const fileWatch = watch("file");

  const loadUserData = async () => {
    if (!authStore?.user?.id) return;
    try {
      const result = await userApi.getUserData({ userId: authStore?.user?.id });
      if (result.isSuccess) {
        setUserData(result.value);
        reset({
          firstName: result.value.firstName || "",
          lastName: result.value.lastName || "",
          bio: result.value.bio || "",
          country: result.value.country || "",
          city: result.value.city || "",
          street: result.value.street || "",
          houseNumber: result.value.houseNumber || "",
          postalCode: result.value.postalCode || "",
        });
        setImagePreview(result.value.profilePicturePath || null);
        setIsPhotoDeleted(false);
      } else {
        uiStore.showSnackbar(t("user:UserNotFound"), "error");
      }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    }
  };

  useEffect(() => {
    loadUserData();
  }, [authStore.user]);

  useEffect(() => {
    if (fileWatch && fileWatch.length > 0) {
      const file = fileWatch[0];
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result as string);
        setIsPhotoDeleted(false);
      };
      reader.readAsDataURL(file);
    }
  }, [fileWatch]);

  const handleDeletePhoto = () => {
    setImagePreview(null);
    setIsPhotoDeleted(true);
    setValue("file", undefined);
    const fileInput = document.getElementById("profile-picture-upload") as HTMLInputElement;
    if (fileInput) {
      fileInput.value = "";
    }
  };

  const onSubmit = async (data: ProfileFormData) => {
    setLoading(true);
    try {
      const formData = new FormData();
      formData.append("firstName", data.firstName);
      formData.append("lastName", data.lastName);
      if (data.bio) formData.append("bio", data.bio);
      if (data.country) formData.append("country", data.country);
      if (data.city) formData.append("city", data.city);
      if (data.street) formData.append("street", data.street);
      if (data.houseNumber) formData.append("houseNumber", data.houseNumber);
      if (data.postalCode) formData.append("postalCode", data.postalCode);

      if (isPhotoDeleted) {
        formData.append("profilePicturePath", "");
      } else if (data.file && data.file.length > 0) {
        formData.append("file", data.file[0]);
      } else if (userData?.profilePicturePath) {
        formData.append("profilePicturePath", userData.profilePicturePath);
      }

      const result = await userApi.updateProfile(formData);

      if (result.isSuccess) {
        uiStore.showSnackbar(t("user:ProfileUpdatedSuccessfully"), "success");
        authStore.setUser({
          ...authStore.user!,
          profilePicturePath: result.value.profilePicturePath
        });
        loadUserData();
      } else {
        uiStore.showSnackbar(
          result.error || t("common:ErrorOccurred"),
          "error"
        );
      }
    } catch (err) {
      console.log(err);
      uiStore.showSnackbar(t("common:ErrorOccurred"), "error");
    } finally {
      setLoading(false);
    }
  };

  if (!userData) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="400px"
      >
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Paper sx={{ p: 3, maxWidth: 800, mx: "auto" }}>
      <Typography variant="h5" gutterBottom>
        {t("user:EditProfile")}
      </Typography>

      <form onSubmit={handleSubmit(onSubmit)}>
        <Box display="flex" flexDirection="column" alignItems="center" mb={3}>
          <Avatar
            src={
              imagePreview?.startsWith("data:")
                ? imagePreview
                : imagePreview
                ? `${import.meta.env.VITE_API_URL}/${imagePreview}`
                : undefined
            }
            sx={{ width: 120, height: 120, mb: 2 }}
          />
          <Box display="flex" gap={1}>
            <input
              accept="image/*"
              style={{ display: "none" }}
              id="profile-picture-upload"
              type="file"
              {...register("file")}
            />
            <label htmlFor="profile-picture-upload">
              <IconButton color="primary" component="span">
                <PhotoCamera />
              </IconButton>
            </label>
            {imagePreview && (
              <IconButton color="error" onClick={handleDeletePhoto}>
                <Delete />
              </IconButton>
            )}
          </Box>
        </Box>

        <Grid container spacing={2}>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label={t("user:FirstName")}
              {...register("firstName")}
              error={!!errors.firstName}
              helperText={errors.firstName ? t(errors.firstName.message!) : ""}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label={t("user:LastName")}
              {...register("lastName")}
              error={!!errors.lastName}
              helperText={errors.lastName ? t(errors.lastName.message!) : ""}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              multiline
              rows={3}
              label={t("user:Bio")}
              {...register("bio")}
              error={!!errors.bio}
              helperText={errors.bio ? t(errors.bio.message!) : ""}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label={t("user:Country")}
              {...register("country")}
              error={!!errors.country}
              helperText={errors.country ? t(errors.country.message!) : ""}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label={t("user:City")}
              {...register("city")}
              error={!!errors.city}
              helperText={errors.city ? t(errors.city.message!) : ""}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label={t("user:Street")}
              {...register("street")}
              error={!!errors.street}
              helperText={errors.street ? t(errors.street.message!) : ""}
            />
          </Grid>
          <Grid item xs={12} sm={3}>
            <TextField
              fullWidth
              label={t("user:HouseNumber")}
              {...register("houseNumber")}
              error={!!errors.houseNumber}
              helperText={
                errors.houseNumber ? t(errors.houseNumber.message!) : ""
              }
            />
          </Grid>
          <Grid item xs={12} sm={3}>
            <TextField
              fullWidth
              label={t("user:PostalCode")}
              {...register("postalCode")}
              error={!!errors.postalCode}
              helperText={
                errors.postalCode ? t(errors.postalCode.message!) : ""
              }
            />
          </Grid>
        </Grid>

        <Box mt={3} display="flex" justifyContent="flex-end" gap={2}>
          <Button variant="outlined" onClick={() => reset()} disabled={loading}>
            {t("common:Cancel")}
          </Button>
          <Button type="submit" variant="contained" disabled={loading}>
            {loading ? <CircularProgress size={24} /> : t("common:Save")}
          </Button>
        </Box>
      </form>
    </Paper>
  );
};

export default observer(UserProfilePage);