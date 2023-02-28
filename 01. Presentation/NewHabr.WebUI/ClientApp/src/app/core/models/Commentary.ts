export interface Commentary {
    Id?: string;
    UserId: string;
    UserName: string;
    ArticleId: string;
    Text: string;
    CreatedAt?: number;
    ModifiedAt: number;
}

export interface CommentRequest {
    Text: string;
    ArticleId: string;
}
