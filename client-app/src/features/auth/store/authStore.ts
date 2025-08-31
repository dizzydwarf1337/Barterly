import { makeAutoObservable, runInAction } from "mobx";
import { User } from "../types/authTypes";
import apiClient from "../../../app/API/apiClient";

export default class authStore {
  user: User | null = null;
  isLoggedIn: boolean = false;
  userLoading: boolean = false;
  googleLoading: boolean = false;
  token: string | null = null;
  constructor() {
    makeAutoObservable(this);
    const user = localStorage.getItem("brt_user");
    const login = localStorage.getItem("brt_login");
    this.setUser(user ? JSON.parse(user) : null);
    this.setUserIsLoggedIn(login ? JSON.parse(login) : false);
  }

  getToken = () => this.token;

  setUser = (user: User | null) => (this.user = user);

  clearUser = () => (this.user = null);

  getUser = () => this.user;

  setUserIsLoggedIn = (value: boolean) => (this.isLoggedIn = value);

  getUserIsLoggedIn = () => this.isLoggedIn;

  setLoading = (value: boolean) => (this.userLoading = value);

  getLoading = () => this.userLoading;

  setGoogleLoading = (value: boolean) => (this.googleLoading = value);

  getGoogleLoading = () => this.googleLoading;

  setToken = (token: string | null) => (this.token = token);

  login = async (token: string) => {
    runInAction(()=>{
    this.setUserIsLoggedIn(true);
    localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
    this.setToken(token);
    apiClient.setToken(token);
    })
  };
  loginWithGoogle = (token: string) => {
    runInAction(()=>{
    this.setUserIsLoggedIn(true);
    localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
    apiClient.setToken(token);
    })
  };
  logout = async () => {
    runInAction(()=>{
    this.clearUser();
    this.setUserIsLoggedIn(false);
    localStorage.removeItem("brt_login");
    localStorage.removeItem("brt_user");
    apiClient.setToken(null);
    })
  };

  setMe = async (user: User) => {
    runInAction(()=>{
    this.setUser(user);
    localStorage.setItem("brt_user", JSON.stringify(this.user));
    })
  };
}
