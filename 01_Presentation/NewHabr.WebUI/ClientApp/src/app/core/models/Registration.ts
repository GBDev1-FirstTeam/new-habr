import { User } from "./User";

export interface RegistrationRequest {
    Login: string;
    Password: string;
    FirstName: string;
    LastName: string;
    Patronymic: string;
    Role: string;
    Age: number;
    Description: string;
}

export interface Registration {
    Token: string;
    RefreshToken: string;
    User: User;
}
