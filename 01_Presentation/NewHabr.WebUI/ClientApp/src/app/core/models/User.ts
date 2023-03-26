export interface User {
    Id: string;
    Login: string;
    FirstName: string;
    LastName: string;
    Patronymic: string;
    Role: string;
    Age: number;
    Description: string;
}

export interface PutUserInfo {
    FirstName: string;
    LastName: string;
    Patronymic: string;
    BirthDay: number;
    Description: string;
}

export interface UserInfo {
    Id: string;
    UserName: string;
    Age: number;
    Banned: boolean;
    FirstName: string;
    LastName: string;
    Patronymic: string;
    BirthDay: number;
    Description: string;
    Roles: string[];

    BannedAt: number;
    BanExpiratonDate: number;
    BanReason: number;

    IsLiked?: boolean;
    LikesCount?: number;
}

export interface AuthUser {
    Id: string;
    UserName: string;
    Roles: string[];
}
