import { Like } from "./Like";

export interface Commentary extends Like {
    Id?: string;
    UserId: string;
    UserLogin: string;
    ArticleId: string;
    Text: string;
    CreatedAt: number;
    LikesCount?: number;
    IsLiked?: boolean;
}
