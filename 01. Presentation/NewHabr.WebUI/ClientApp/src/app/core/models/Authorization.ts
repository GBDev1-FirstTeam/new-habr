import { User } from "./User";

export interface AuthorizationRequest {
    Login: string;
    Password: string;
}

export interface Authorization {
    Token: string;
    RefreshToken: string;
    User: User;
}
