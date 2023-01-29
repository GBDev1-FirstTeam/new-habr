export interface Publication {
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
