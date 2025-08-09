import {makeAutoObservable, runInAction} from "mobx";
import User from "../models/user";
import LoginDto from "../models/loginDto";
import agent from "../API/agent";
import ApiResponse from "../models/apiResponse";
import GoogleLoginDto from "../models/googleLoginDto";
import RegisterDto from "../models/registerDto";
import Email from "../models/Email";
import ConfirmEmail from "../models/confirmEmail";
import ResetPassword from "../models/ResetPassword";


export default class UserStore {
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

    login = async (loginDto: LoginDto) => {
        this.setLoading(true);
        try {
            let res: ApiResponse<User> = await agent.Auth.Login(loginDto);
            if (res.isSuccess) {
                runInAction(() => {
                    this.setUser(res.value);
                    localStorage.setItem("brt_user", JSON.stringify(this.user));
                    this.setUserIsLoggedIn(true);
                    localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
                })
            } else console.log(res.error);
        } catch (e) {
            console.log("Login failed ", e);
            throw e;
        } finally {
            this.setLoading(false)
        }
    }
    loginWithGoogle = async (token: string) => {
        this.setGoogleLoading(true);
        try {
            let googleLoginDto: GoogleLoginDto = {token};
            let res: ApiResponse<User> = await agent.Auth.LoginGoogle(googleLoginDto);
            if (res.isSuccess) {
                runInAction(() => {
                    this.setUser(res.value);
                    localStorage.setItem("brt_user", JSON.stringify(this.user));
                    this.setUserIsLoggedIn(true);
                    localStorage.setItem("brt_login", JSON.stringify(this.isLoggedIn));
                })
            }
        } catch (e) {
            console.log("Login with google failed", e);
            throw e;
        } finally {
            this.setGoogleLoading(false);
        }
    }
    logout = async () => {
        this.setLoading(true);
        try {
            const res: ApiResponse<boolean> = await agent.Auth.Logout({email: this.user!.email});
            if (res.isSuccess) {
                runInAction(() => {
                    this.clearUser();
                    this.setUserIsLoggedIn(false);
                    localStorage.removeItem("brt_login");
                    localStorage.removeItem("brt_user");
                })
            }
        } catch (e) {
            console.log("Logout failed", e);
            throw e;
        } finally {
            this.setLoading(false);
        }
    }
    register = async (registerDto: RegisterDto) => {
        this.setLoading(true);
        try {
            const regRes: ApiResponse<void> = await agent.Auth.Register(registerDto);
            if (!regRes.isSuccess) throw regRes.error && "Error while register new user";
        } catch (e) {
            console.error(`${e}`);
            throw e;
        } finally {
            this.setLoading(false);
        }
    }
    resendEmailConfirmation = async (email: Email) => {
        this.setLoading(true);
        try {
            const res: ApiResponse<void> = await agent.Auth.ResendEmailConfirm(email);
            if (!res.isSuccess) throw res.error && "Error while sending email confirmation";
        } catch (e) {
            console.error(e);
            throw e
        } finally {
            this.setLoading(false);
        }
    }
    confirmEmail = async (confirmEmail: ConfirmEmail) => {
        this.setLoading(true);
        try {
            const res: ApiResponse<void> = await agent.UserAPI.ConfirmEmail(confirmEmail);
            if (!res.isSuccess) throw res.error && "Error while confirming email";
        } catch (e) {
            console.error(e);
            throw e;
        } finally {
            this.setLoading(false);
        }
    }
    resetPassword = async (resetPassword: ResetPassword) => {
        this.setLoading(true);
        try {
            const res: ApiResponse<void> = await agent.UserAPI.ResetPassword(resetPassword);
            if (!res.isSuccess) throw res.error && "Error while resetting password";
        } catch (e) {
            console.log(e);
            throw e;
        } finally {
            this.setLoading(true);
        }
    }


}