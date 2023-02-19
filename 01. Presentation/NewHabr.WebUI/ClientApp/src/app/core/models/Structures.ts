export interface NameStruct {
    Name: string;
}

export interface QuestionStruct {
    Question: string;
}

export interface Metadata {
    CurrentPage: number;
    PageSize: number;
    TotalCount: number;
    TotalPages: number;
}

export enum ApproveState {
    NotApproved,
    WaitApproval,
    Approved
}
