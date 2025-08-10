import {Theme} from "@mui/material";
import {makeAutoObservable, runInAction} from "mobx";
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
    snackbarSeverity: 'success' | 'error' | 'warning' | 'info' = 'info'
    snackbarPosition: 'right' | 'left' | 'center' = 'right'
    // var for contolling user interface
    isMobile = window.innerWidth < 480;
    isMobileMenuOpen: boolean = false;

    constructor() {
        makeAutoObservable(this);
        let theme = localStorage.getItem("brt_theme")
        this.setTheme(theme === "dark" ? darkTheme : lightTheme);
        this.setThemeMode(theme === "dark" ? "dark" : "light");
        window.addEventListener("resize", this.updateIsMobile);
    }

    setTheme = (theme: Theme) => this.theme = theme;

    getTheme = () => this.theme;

    changeTheme = () => {
        if (localStorage.getItem("brt_theme") == "dark") {
            localStorage.setItem("brt_theme", "light");
            this.setThemeMode("light");
            this.setTheme(lightTheme);
        } else {
            localStorage.setItem("brt_theme", "dark");
            this.setThemeMode("dark");
            this.setTheme(darkTheme);
        }
    }

    setThemeMode = (themeMode: "dark" | "light") => this.themeMode = themeMode;

    getThemeMode = () => this.themeMode;

    setLanguage = (language: string) => this.lang = language;

    getLanguage = () => this.lang;

    changeLanguage = () => {
        const newLanguage = this.getLanguage() === 'en' ? 'pl' : 'en';
        i18n.changeLanguage(newLanguage);
        this.setLanguage(newLanguage);
        localStorage.setItem("brt_lng", newLanguage);
    }

    setUserSettingIsOpen = (value: boolean) => this.userSettingIsOpen = value;

    getUserSettingIsOpen = () => this.userSettingIsOpen;

    setMenuElement = (el: HTMLElement | undefined) => this.menuElement = el;

    getMenuElement = () => this.menuElement;

    setSnackbarOpen = (value: boolean) => this.snackbarOpen = value;

    showSnackbar(message: string, severity: 'success' | 'error' | 'warning' | 'info', position: 'left' | 'center' | 'right') {
        this.snackbarMessage = message;
        this.snackbarSeverity = severity;
        this.snackbarPosition = position;
        runInAction(() => {
            this.setSnackbarOpen(true);
        })
    }

    closeSnackbar() {
        runInAction(() => {
            this.setSnackbarOpen(false);
        })
    }

    // Mobile nav menu controll

    updateIsMobile = () => {
        runInAction(() => {
            this.isMobile = window.innerWidth < 480;
        })
    }

    setIsMobileMenuOpen = (value: boolean) => {
        this.isMobileMenuOpen = value;
    }

}