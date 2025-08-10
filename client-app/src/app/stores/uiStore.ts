import { Theme } from "@mui/material";
import { makeAutoObservable } from "mobx";
import lightTheme from "../theme/LightTheme";
import i18n from "../locales/i18n";
import darkTheme from "../theme/DarkTheme";

export default class UiStore {
  // Application theme using for choosing colors and so on
  theme!: Theme;
  // String representing application theme mode "dark" or "light"
  themeMode!: "dark" | "light";
  // App Language
  lang: string = "pl";
  // NavBar user profile settings dialog
  userSettingIsOpen: boolean = false;
  // Menu controll stuff
  menuElement: HTMLElement | undefined;
  // Snackbar feature
  snackbarOpen: boolean = false;
  snackbarMessage: string = "";
  snackbarSeverity: "success" | "error" | "warning" | "info" = "info";
  snackbarPosition: "right" | "left" | "center" = "right";
  // var for controlling user interface
  isMobile = window.innerWidth < 480;
  isMobileMenuOpen: boolean = false;

  constructor() {
    makeAutoObservable(this);
    let theme = localStorage.getItem("brt_theme");
    this.setTheme(theme === "dark" ? darkTheme : lightTheme);
    this.setThemeMode(theme === "dark" ? "dark" : "light");

    // Инициализация языка
    let savedLang = localStorage.getItem("brt_lng") || "pl";
    this.setLanguage(savedLang);

    window.addEventListener("resize", this.updateIsMobile);
  }

  // ✅ ИСПРАВЛЕННЫЙ метод обновления мобильного режима
  updateIsMobile = () => {
    this.isMobile = window.innerWidth < 480;
  };

  setTheme = (theme: Theme) => (this.theme = theme);

  getTheme = () => this.theme;

  // ✅ ИСПРАВЛЕННЫЙ метод смены темы
  changeTheme = () => {
    if (this.themeMode === "dark") {
      localStorage.setItem("brt_theme", "light");
      this.setThemeMode("light");
      this.setTheme(lightTheme);
    } else {
      localStorage.setItem("brt_theme", "dark");
      this.setThemeMode("dark");
      this.setTheme(darkTheme);
    }
  };

  setThemeMode = (themeMode: "dark" | "light") => (this.themeMode = themeMode);

  getThemeMode = () => this.themeMode;

  setLanguage = (language: string) => (this.lang = language);

  getLanguage = () => this.lang;

  // ✅ ИСПРАВЛЕННЫЙ метод смены языка
  changeLanguage = () => {
    const newLanguage = this.getLanguage() === "en" ? "pl" : "en";
    this.setLanguage(newLanguage);
    i18n.changeLanguage(newLanguage);
    localStorage.setItem("brt_lng", newLanguage);
  };

  // User settings dialog methods
  setUserSettingIsOpen = (isOpen: boolean) => (this.userSettingIsOpen = isOpen);

  getUserSettingIsOpen = () => this.userSettingIsOpen;

  setMenuElement = (element: HTMLElement | undefined) =>
    (this.menuElement = element);

  // Mobile menu methods
  setIsMobileMenuOpen = (isOpen: boolean) => (this.isMobileMenuOpen = isOpen);

  // Snackbar methods
  showSnackbar = (
    message: string,
    severity: "success" | "error" | "warning" | "info" = "info",
    position: "right" | "left" | "center" = "right"
  ) => {
    this.snackbarMessage = message;
    this.snackbarSeverity = severity;
    this.snackbarPosition = position;
    this.snackbarOpen = true;
  };

  closeSnackbar = () => {
    this.snackbarOpen = false;
  };
}
