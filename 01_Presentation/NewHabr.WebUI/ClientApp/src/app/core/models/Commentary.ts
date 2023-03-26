export interface Commentary {
    Id?: string;
    UserId: string;
    Username: string;
    ArticleId: string;
    Text: string;
    CreatedAt?: number;
    ModifiedAt: number;
    IsLiked?: boolean;
    LikesCount?: number;
}

export interface CommentRequest {
    Text: string;
    ArticleId: string;
}
