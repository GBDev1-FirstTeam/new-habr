import { Category } from "./Category";
import { Metadata, NameStruct } from "./Structures";
import { Tag } from "./Tag";
import { Commentary } from "./Commentary";

export interface Publication {
    Id?: string;
    UserId?: string;
    Title: string;
    Categories: Array<Category>;
    Tags: Array<Tag>;
    Comments?: Array<Commentary>;
    Content: string;
    CreatedAt?: number;
    ModifiedAt?: number;
    PublishedAt?: number;
    Published?: boolean;
    ApproveState?: string;
    ImgURL: string;
    Username?: string;
    CommentsCount?: number;
    IsLiked?: boolean;
    LikesCount?: number;
}

export interface PublicationUser {
    Id?: string;
    Title: string;
    Categories: Array<Category>;
    Tags: Array<Tag>;
    ApproveState?: string;
    CommentsCount: number;
    Content: string;
    CreatedAt?: number;
    ModifiedAt?: number;
    PublishedAt?: number;
    Published?: boolean;
    ImgURL: string;
    LikesCount: number;
}

export interface PublicationRequest {
    Title: string;
    Content: string;
    ImgURL: string;
    Categories: NameStruct[];
    Tags: NameStruct[];
}

export interface PublicationsResponse {
    Articles: Array<Publication>;
    Metadata: Metadata;
}

export interface PublicationsResponseUser {
    Articles: Array<PublicationUser>;
    Metadata: Metadata;
}
