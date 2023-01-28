export interface Configuration {
    backend: Backend;
}

export interface Backend {
    baseURL: string;
    children: {
        publications: string;
        publication: string;
        user: string;
        comments: string;
    }
}
