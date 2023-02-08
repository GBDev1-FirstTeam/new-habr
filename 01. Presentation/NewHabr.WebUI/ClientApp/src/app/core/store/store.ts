import { Publication } from "../models/Publication";
import { createStore, withProps, Store, StoreDef, select } from '@ngneat/elf';
import { Injectable } from "@angular/core";
import { HttpRequestService } from "../services/HttpRequestService";
import { Authorization } from "../models/Authorization";
import { RegistrationRequest } from "../models/Registration";
import { RecoveryRequestAnswer } from "../models/Recovery";

export interface AppStore {
    publications: Array<Publication> | null,
    post: Publication | null,
    auth: Authorization | null,
    isAuth: boolean
}

@Injectable({
    providedIn: 'root',
})
export class AppStoreProvider {

    private authObjectName: string = 'auth-object';
    private store: Store<StoreDef<AppStore>>;

    constructor(private http: HttpRequestService) {
        
        const auth = this.readFromLocalStorage();

        this.store = createStore(
            { name: 'appStore' },

            withProps<AppStore>({
                publications: null,
                post: null,
                auth: auth,
                isAuth: !!auth?.User && !!auth?.Token && !!auth?.RefreshToken
            })
        );
    }

    loadPublications() {
        if (this.store.getValue().publications == null) {
            const publicationsSubscribtion = this.http.getPublications().subscribe(publications => {
                if (publications) {
                    this.updatePublications(publications);
                    publicationsSubscribtion.unsubscribe();
                }
            });
        }
    }
    
    loadPublicationById(id: string) {
        const post = this.store.getValue().post;
        if (post == null || post.Id !== id) {
            const postSubscribtion = this.http.getPostById(id).subscribe(post => {
                if (post) {
                    this.updatePost(post);
                    postSubscribtion.unsubscribe();
                }
            });
        }
    }

    getPublicationsFromStore = () => this.store.pipe(select((state => state.publications)));
    getPostFromStore = () => this.store.pipe(select((state => state.post)));
    
    private updatePublications(publications: Array<Publication>) {
        this.store.update(st => ({
            ...st,
            publications: publications
        }))
    }
    
    private updatePost(post: Publication) {
        this.store.update(st => ({
            ...st,
            post: post
        }))
    }

    private authSubscribtion = (auth: Authorization) => {
        const isAuth = !!auth?.User && !!auth?.Token && !!auth?.RefreshToken;
        if (isAuth) {
            this.store.update(st => ({
                ...st,
                auth: auth,
                isAuth: isAuth
            }))
        } else {
            this.store.update(st => ({
                ...st,
                auth: null,
                isAuth: false
            }))
        }
        this.saveToLocalStorage(auth);
    }

    authentication(login: string, password: string) {
        const authenticationSubscribtion = this.http.postAuthentication({
            Login: login,
            Password: password
        }).subscribe(auth => {
            this.authSubscribtion(auth);
            authenticationSubscribtion.unsubscribe();
        });
    }
    
    register(registerData: RegistrationRequest) {
        const registrationSubscribtion =
            this.http.postRegistration(registerData).subscribe(auth => {
            this.authSubscribtion(auth);
            registrationSubscribtion.unsubscribe();
        });
    }

    recovery(recoveryData: RecoveryRequestAnswer) {
        const recoverySubscribtion =
            this.http.postRecoveryAnswer(recoveryData).subscribe(auth => {
            this.authSubscribtion(auth);
            recoverySubscribtion.unsubscribe();
        });
    }

    logout() {
        this.store.update(st => ({
            ...st,
            auth: null,
            isAuth: false
        }))
        localStorage.removeItem(this.authObjectName)
    }

    getAuth = () => this.store.pipe(select((state => state.auth)));
    getIsAuth = () => this.store.pipe(select((state => state.isAuth)));

    private saveToLocalStorage(auth: Authorization) {
        localStorage.setItem(this.authObjectName, JSON.stringify(auth))
    }
    
    private readFromLocalStorage(): Authorization | null {
        return JSON.parse(localStorage.getItem(this.authObjectName)!) as Authorization | null;
    }
}
