import { Theme } from "@mui/material";
import { makeAutoObservable } from "mobx";
import lightTheme from "../theme/LightTheme";
import i18n from "../locales/i18n";
import darkTheme from "../theme/DarkTheme";
import { ChatHub } from "../signalR/Hub";

export default class UiStore {

  theme!: Theme;

  themeMode!: "dark" | "light";

  lang: string = "pl";

  userSettingIsOpen: boolean = false;

  menuElement: HTMLElement | undefined;

  chatHub: ChatHub;

  snackbarOpen: boolean = false;
  snackbarMessage: string = "";
  snackbarSeverity: "success" | "error" | "warning" | "info" = "info";
  snackbarPosition: "right" | "left" | "center" = "right";

  notificationsOpen: boolean = false;

  isMobile = window.innerWidth < 480;
  isMobileMenuOpen: boolean = false;

  isMessagesWidgetOpen: boolean = false;

  constructor(chatHub: ChatHub) {
    makeAutoObservable(this);
    this.chatHub = chatHub;
    let theme = localStorage.getItem("brt_theme");
    this.setTheme(theme === "dark" ? darkTheme : lightTheme);
    this.setThemeMode(theme === "dark" ? "dark" : "light");

    let savedLang = localStorage.getItem("brt_lng") || "pl";
    this.setLanguage(savedLang);

    this.chatHub.setHandler("OrderExistsError", this.handleOrderExistsMessage);

    window.addEventListener("resize", this.updateIsMobile);
  }

  updateIsMobile = () => {
    this.isMobile = window.innerWidth < 480;
  };

  setTheme = (theme: Theme) => (this.theme = theme);

  getTheme = () => this.theme;

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

  changeLanguage = () => {
    const newLanguage = this.getLanguage() === "en" ? "pl" : "en";
    this.setLanguage(newLanguage);
    i18n.changeLanguage(newLanguage);
    localStorage.setItem("brt_lng", newLanguage);
  };

  setUserSettingIsOpen = (isOpen: boolean) => (this.userSettingIsOpen = isOpen);

  getUserSettingIsOpen = () => this.userSettingIsOpen;

  setMenuElement = (element: HTMLElement | undefined) =>
    (this.menuElement = element);

  setIsMobileMenuOpen = (isOpen: boolean) => (this.isMobileMenuOpen = isOpen);

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

  setNotificationsOpen = () => {
    this.notificationsOpen = !this.notificationsOpen;
  };

  setIsMessagesWidgetOpen = (isOpen: boolean) => {
    this.isMessagesWidgetOpen = isOpen;
  };

  handleOrderExistsMessage = () => {
    this.showSnackbar("Active order still pending", "error");
  }
}
