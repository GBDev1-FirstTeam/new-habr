import { Like } from "./Like";

export interface Publication extends Like {
    Id?: string;
    Title: string;
    Content: string;
    UserId?: string;
    UserLogin?: string;
    CreatedAt?: number;
    ModifyAt?: number;
    PublishedAt?: number;
    ImgURL: string;
    IsPublished: boolean;
}
