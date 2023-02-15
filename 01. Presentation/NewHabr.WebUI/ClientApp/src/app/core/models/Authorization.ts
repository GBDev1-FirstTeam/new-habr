import { AuthUser } from "./User";

export interface RegisterRequest {
    UserName: string;
    Password: string;
    SecurityQuestionId: number;
    SecurityQuestionAnswer: string;
}

export interface LoginRequest {
    UserName: string;
    Password: string;
}

export interface Authorization {
    Token: string;
    User: AuthUser;
}
