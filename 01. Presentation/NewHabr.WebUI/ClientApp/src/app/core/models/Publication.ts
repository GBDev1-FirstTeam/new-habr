import { Category } from "./Category";
import { Metadata, NameStruct } from "./Structures";
import { Like } from "./Like";
import { Tag } from "./Tag";

export interface Publication extends Like {
    Id?: string;
    UserId?: string;
    Title: string;
    Categories: Array<Category>;
    Tags: Array<Tag>;
    Comments?: Array<Comment>;
    Content: string;
    CreatedAt?: number;
    ModifiedAt?: number;
    PublishedAt?: number;
    ApproveState?: number;
    ImgURL: string;
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
