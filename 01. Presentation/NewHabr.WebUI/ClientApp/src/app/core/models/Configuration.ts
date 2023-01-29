export interface Configuration {
    backend: Backend;
}

export interface Backend {
    baseURL: string;
    children: {
        publications: string;
        publication: string;
        addPublication: string;
        user: string;
        userPublications: string;
        comments: string;
        addComment: string;
        login: string;
    }
}
