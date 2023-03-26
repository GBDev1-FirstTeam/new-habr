import { UserInfo } from "./User";

export interface RecoveryRequest {
    UserName: string;
    SecureQuestionId: number;
    Answer: string;
}

export interface RecoveryResponse {
    Token: string;
    User: UserInfo;
}

export interface ResetPasswordRequest {
    Token: string;
    UserName: string;
    NewPassword: string;
}
