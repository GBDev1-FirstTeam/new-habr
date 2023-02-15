import { Like } from "./Like";

export interface User extends Like {
    Id: string;
    Login: string;
    FirstName: string;
    LastName: string;
    Patronymic: string;
    Role: string;
    Age: number;
    Description: string;
}

export interface AuthUser {
    Id: string;
    UserName: string;
    Roles: string[];
}
