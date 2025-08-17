import { makeAutoObservable } from "mobx";
import { User } from "../types/authTypes";
import apiClient from "../../../app/API/apiClient";

export default class authStore {
  user: User | null = null;
  isLoggedIn: boolean = false;
  userLoading: boolean = false;
  googleLoading: boolean = false;
  token: string | null = null;
  roles: string[] = [];
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

  setRoles = (roles: string[]) => (this.roles = roles);

  getRoles = () => this.roles;

  setToken = (token: string | null) => (this.token = token);

  login = async (token: string, roles: string[]) => {
    this.setUserIsLoggedIn(true);
    localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
    this.setRoles(roles);
    this.setToken(token);
    apiClient.setToken(token);
  };
  loginWithGoogle = (token: string, roles: string[]) => {
    this.setUserIsLoggedIn(true);
    localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
    this.setRoles(roles);
    this.setToken(token);
    apiClient.setToken(token);
  };
  logout = async () => {
    this.clearUser();
    this.setUserIsLoggedIn(false);
    localStorage.removeItem("brt_login");
    localStorage.removeItem("brt_user");
    apiClient.setToken(null);
  };

  setMe = async (user: User) => {
    this.setUser(user);
    localStorage.setItem("brt_user", JSON.stringify(this.user));
  };
}
