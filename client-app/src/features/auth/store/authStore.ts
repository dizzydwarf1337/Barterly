import {makeAutoObservable} from "mobx";
import { User } from "../types/authTypes";



export default class authStore {
    user: User | null = null;
    isLoggedIn: boolean = false;
    userLoading: boolean = false;
    googleLoading: boolean = false;

    constructor() {
        makeAutoObservable(this);
        const user = localStorage.getItem("brt_user");
        const login = localStorage.getItem("brt_login");
        this.setUser(user ? JSON.parse(user) : null);
        this.setUserIsLoggedIn(login ? JSON.parse(login) : false);
    }

    getToken = () => this.user?.token;

    setUser = (user: User | null) => this.user = user;

    clearUser = () => this.user = null;

    getUser = () => this.user;

    setUserIsLoggedIn = (value: boolean) => this.isLoggedIn = value;

    getUserIsLoggedIn = () => this.isLoggedIn;

    setLoading = (value: boolean) => this.userLoading = value;

    getLoading = () => this.userLoading;

    setGoogleLoading = (value: boolean) => this.googleLoading = value;

    getGoogleLoading = () => this.googleLoading;

    login = async (user:User) => {
        this.setUser(user);
        localStorage.setItem("brt_user", JSON.stringify(this.user));
        this.setUserIsLoggedIn(true);
        localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
      
    }
    loginWithGoogle = (user:User) => {
        this.setUser(user);
        localStorage.setItem("brt_user", JSON.stringify(this.user));
        this.setUserIsLoggedIn(true);
        localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
    }
    logout = async () => {
        this.clearUser();
        this.setUserIsLoggedIn(false);
        localStorage.removeItem("brt_login");
        localStorage.removeItem("brt_user");
    }


}