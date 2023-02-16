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

export interface PublicationRequest {
    Title: string;
    Content: string;
    ImgURL: string;
    Categories: CT[];
    Tags: CT[];
}

export interface CT {
    Name: string;
}
