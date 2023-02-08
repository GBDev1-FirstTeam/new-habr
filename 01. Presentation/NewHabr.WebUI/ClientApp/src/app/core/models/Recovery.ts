import { User } from "./User";

export interface RecoveryRequestLogin {
    Login: string;
}

export interface RecoveryQuestion {
    Question: string;
    TransactionId: string;
}

export interface RecoveryRequestAnswer {
    Answer: string;
    TransactionId: string;
}

export interface Recovery {
    Token: string;
    RefreshToken: string;
    User: User;
}

export interface RecoveryChangePassword {
    UserId: string;
    Password: string;
}
