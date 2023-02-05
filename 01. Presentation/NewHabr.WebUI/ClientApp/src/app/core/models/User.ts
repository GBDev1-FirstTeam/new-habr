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
    LikesCount?: number;
    IsLiked?: boolean;
}
