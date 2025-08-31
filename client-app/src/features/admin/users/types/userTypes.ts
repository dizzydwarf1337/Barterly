

export enum UserRoles {
    User = 1,
    Moderator = 2,
    Admin = 3
}

export interface UserPreview {
    id:string;
    firstName: string;
    lastName: string;
    email:string;
    bio: string | null;
    country: string | null;
    role:UserRoles;
    city: string | null;
    street: string | null;
    houseNumber: string | null;
    postalCode: string | null;
    createdAt: Date;
    lastSeen: Date;
}

export interface UserData {
    firstName: string;
    lastName: string;
    email: string;
    bio: string | null;
    country: string | null;
    city: string | null;
    street: string | null;
    houseNumber: string | null;
    postalCode: string | null;
    profilePicturePath: string | null;
    createdAt: string;
    lastSeen: string;
}

export interface UserSettings {
    id: string;
    isHidden: boolean;
    isDeleted: boolean;
    isBanned: boolean;
    isPostRestricted: boolean;
    isOpinionRestricted: boolean;
    isChatRestricted: boolean;
}