import ApiResponse from "../../../app/API/apiResponse";
import { User } from "../types/authTypes";



export interface LoginRequestDTO {
    email: string;
    password: string;
}

export interface LoginResponseDTO extends ApiResponse<User> {}

export interface RegisterRequestDTO  {
    firstName:string;
    lastName:string;
    email:string;
    password:string;
}


export interface LoginWithGoogleRequestDTO {
    token:string;
}

export interface LoginWithGoogleResponseDTO extends ApiResponse<User> {}

export interface ResendEmailConfirmationRequestDTO {
    email:string;
}

export interface ConfirmEmailRequestDTO  {
    userMail:string;
    token:string;
}

export interface ResendEmailConfirmationRequestDTO {
    email:string;
}