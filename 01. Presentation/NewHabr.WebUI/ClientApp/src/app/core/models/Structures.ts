export interface NameStruct {
    Name: string;
}

export interface QuestionStruct {
    Question: string;
}

export interface BanStruct {
    BanReason: string;
}

export interface RoleStruct {
    Roles: string[];
}

export interface Metadata {
    CurrentPage: number;
    PageSize: number;
    TotalCount: number;
    TotalPages: number;
}

export interface ArticleQueryParameters {
    from?: number;
    to?: number;
    orderBy?: number;
    search?: string;
    byRating?: number;
    pageNumber?: number;
    pageSize?: number;
    Category?: number;
    Tag?: number;
}
