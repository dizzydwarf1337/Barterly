

export interface User {
    id:string;
    firstName:string;
    lastName:string;
    email:string;
    profilePicturePath?:string | null;
    token:string;
    role:string;
}