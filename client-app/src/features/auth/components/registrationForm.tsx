import {
  Box,
  Button,
  CircularProgress,
  FormControl,
  FormHelperText,
  Input,
  InputLabel,
  Typography,
} from "@mui/material";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router";
import { observer } from "mobx-react-lite";
import { useForm, Controller } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import useStore from "../../../app/stores/store";
import { RegisterRequestDTO } from "../dto/authDto";
import authApi from "../api/authApi";

interface FormData extends RegisterRequestDTO {
  passwordConfirm: string;
}

export default observer(function RegistrationForm() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { uiStore } = useStore();
  const [showPassword, setShowPassword] = useState<boolean>(false);

  const validationSchema = yup.object().shape({
    firstName: yup
      .string()
      .required(t("firstNameRequired") || "First name is required")
      .min(
        2,
        t("firstNameMinLength") || "First name must be at least 2 characters"
      ),

    lastName: yup
      .string()
      .required(t("lastNameRequired") || "Last name is required")
      .min(
        2,
        t("lastNameMinLength") || "Last name must be at least 2 characters"
      ),

    email: yup
      .string()
      .required(t("emailRequired") || "Email is required")
      .email(t("emailInvalid") || "Please enter a valid email address"),

    password: yup
      .string()
      .required(t("passwordRequired") || "Password is required")
      .min(
        6,
        t("passwordMinLength") || "Password must be at least 6 characters"
      )
      .matches(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/,
        t("passwordPattern") ||
          "Password must contain at least one uppercase letter, one lowercase letter, and one number"
      ),

    passwordConfirm: yup
      .string()
      .required(t("passwordConfirmRequired") || "Please confirm your password")
      .oneOf(
        [yup.ref("password")],
        t("passwordConfirmFailed") || "Passwords must match"
      ),
  });

  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    setError: _,
  } = useForm<FormData>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      email: "",
      password: "",
      passwordConfirm: "",
      firstName: "",
      lastName: "",
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      const registerData: RegisterRequestDTO = {
        email: data.email,
        password: data.password,
        firstName: data.firstName,
        lastName: data.lastName,
      };
      const response = await authApi.register(registerData);
      if (response.isSuccess) {
        uiStore.showSnackbar(t("signInSuccess"), "success", "right");
        navigate(`/email-confirm/${registerData.email}`);
      } else {
        uiStore.showSnackbar(t("signInFailed"), "error", "right");
      }
    } catch (error) {
      uiStore.showSnackbar(t("signInFailed"), "error", "right");
    }
  };

  return (
    <Box
      display="flex"
      flexDirection="column"
      gap="10px"
      sx={{
        backgroundColor: "background.paper",
        p: "50px",
        borderRadius: "20px",
        boxShadow: "5px 5px 1px 0px #22333B",
      }}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <Box display="flex" flexDirection="column" gap="20px">
          <Box
            display="flex"
            flexDirection="row"
            width="100%"
            gap={uiStore.isMobile ? "10px" : "60px"}
          >
            <Box display="flex" flexDirection="column" gap="20px">
              <Controller
                name="email"
                control={control}
                render={({ field }) => (
                  <FormControl error={!!errors.email}>
                    <InputLabel htmlFor="email-input" color="info">
                      <Typography>Email</Typography>
                    </InputLabel>
                    <Input
                      {...field}
                      id="email-input"
                      color="info"
                      type="email"
                      autoComplete="email"
                    />
                    {errors.email && (
                      <FormHelperText>
                        <Typography variant="caption" color="error.main">
                          {errors.email.message}
                        </Typography>
                      </FormHelperText>
                    )}
                  </FormControl>
                )}
              />


              <Box display="flex" flexDirection="column" gap="10px">
                <Controller
                  name="password"
                  control={control}
                  render={({ field }) => (
                    <FormControl error={!!errors.password}>
                      <InputLabel htmlFor="password-input" color="info">
                        <Typography>{t("password")}</Typography>
                      </InputLabel>
                      <Input
                        {...field}
                        id="password-input"
                        color="info"
                        type={showPassword ? "text" : "password"}
                        autoComplete="new-password"
                      />
                      {errors.password && (
                        <FormHelperText>
                          <Typography variant="caption" color="error.main">
                            {errors.password.message}
                          </Typography>
                        </FormHelperText>
                      )}
                    </FormControl>
                  )}
                />
                <Box alignItems="left">
                  <input
                    type="checkbox"
                    checked={showPassword}
                    onChange={() => setShowPassword(!showPassword)}
                  />
                  <Typography variant="caption" sx={{ ml: 1 }}>
                    {t("showPassword") || "Show password"}
                  </Typography>
                </Box>
              </Box>

              <Controller
                name="passwordConfirm"
                control={control}
                render={({ field }) => (
                  <FormControl error={!!errors.passwordConfirm}>
                    <InputLabel htmlFor="passwordConfirm-input" color="info">
                      <Typography>{t("passwordConfirm")}</Typography>
                    </InputLabel>
                    <Input
                      {...field}
                      id="passwordConfirm-input"
                      color="info"
                      type="password"
                      autoComplete="new-password"
                    />
                    {errors.passwordConfirm && (
                      <FormHelperText>
                        <Typography variant="caption" color="error.main">
                          {errors.passwordConfirm.message}
                        </Typography>
                      </FormHelperText>
                    )}
                  </FormControl>
                )}
              />

              <Controller
                name="firstName"
                control={control}
                render={({ field }) => (
                  <FormControl error={!!errors.firstName}>
                    <InputLabel htmlFor="firstName-input" color="info">
                      <Typography>{t("firstName")}</Typography>
                    </InputLabel>
                    <Input {...field} id="firstName-input" color="info" />
                    {errors.firstName && (
                      <FormHelperText>
                        <Typography variant="caption" color="error.main">
                          {errors.firstName.message}
                        </Typography>
                      </FormHelperText>
                    )}
                  </FormControl>
                )}
              />

              <Controller
                name="lastName"
                control={control}
                render={({ field }) => (
                  <FormControl error={!!errors.lastName}>
                    <InputLabel htmlFor="lastName-input" color="info">
                      <Typography>{t("lastName")}</Typography>
                    </InputLabel>
                    <Input {...field} id="lastName-input" color="info" />
                    {errors.lastName && (
                      <FormHelperText>
                        <Typography variant="caption" color="error.main">
                          {errors.lastName.message}
                        </Typography>
                      </FormHelperText>
                    )}
                  </FormControl>
                )}
              />
            </Box>
          </Box>

          <Box display="flex" justifyContent="center" alignItems="center">
            <Button
              type="submit"
              variant="contained"
              color="success"
              size="medium"
              disabled={isSubmitting}
              sx={{ minWidth: "100px" }}
            >
              {isSubmitting ? (
                <CircularProgress color="primary" size={24} />
              ) : (
                <Typography>{t("signIn")}</Typography>
              )}
            </Button>
          </Box>
        </Box>
      </form>
    </Box>
  );
});
