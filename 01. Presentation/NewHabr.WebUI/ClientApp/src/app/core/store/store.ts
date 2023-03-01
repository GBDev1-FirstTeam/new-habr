import { UserInfo } from './../models/User';
import { Publication } from "../models/Publication";
import { createStore, withProps, Store, StoreDef, select } from '@ngneat/elf';
import { Injectable } from "@angular/core";
import { HttpRequestService } from "../services/HttpRequestService";
import { RecoveryRequestAnswer } from "../models/Recovery";
import { Authorization, LoginRequest, RegisterRequest } from "../models/Authorization";
import { StorageKeys } from '../static/StorageKeys';
import { UserRole } from '../static/UserRole';

export interface AppStore {
    publications: Array<Publication> | null,
    
    userInfo: UserInfo | null,
    
    auth: Authorization | null,
    isAuth: boolean,
    isUser: boolean,
    isAdmin: boolean,
    isModerator: boolean,
}

@Injectable({
    providedIn: 'root',
})
export class AppStoreProvider {

    private store: Store<StoreDef<AppStore>>;

    constructor(private http: HttpRequestService) {
        
        const auth = this.readFromLocalStorage();

        this.store = createStore(
            { name: 'appStore' },

            withProps<AppStore>({
                publications: null,
                
                userInfo: null,
                
                auth: auth,
                isAuth: this.isAuth(auth),
                isUser: this.isUser(auth),
                isAdmin: this.isAdmin(auth),
                isModerator: this.isModerator(auth),
            })
        );
    }

    /*
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

    getPublicationsFromStore = () => this.store.pipe(select((state => state.publications)));
    
    private updatePublications(publications: Array<Publication>) {
        this.store.update(st => ({
            ...st,
            publications: publications
        }))
    }
    */

    private authSubscribtion = (auth: Authorization) => {
        if (this.isAuth(auth)) {
            this.store.update(st => ({
                ...st,
                auth: auth,
                isAuth: this.isAuth(auth),
                isUser: this.isUser(auth),
                isAdmin: this.isAdmin(auth),
                isModerator: this.isModerator(auth),
            }))
        } else {
            this.store.update(st => ({
                ...st,
                auth: null,
                isAuth: false,
                isUser: false,
                isAdmin: false,
                isModerator: false,
            }))
        }
        this.saveToLocalStorage(auth);
    }

    login(loginData: LoginRequest) {
        const loginSubscribtion = this.http.login(loginData).subscribe(auth => {
            this.authSubscribtion(auth);
            loginSubscribtion.unsubscribe();
        });
    }
    
    register(registerData: RegisterRequest) {
        const registerSubscribtion =
            this.http.register(registerData).subscribe(auth => {
            this.authSubscribtion(auth);
            registerSubscribtion.unsubscribe();
        });
    }

    recovery(recoveryData: RecoveryRequestAnswer) {
        const recoverySubscribtion =
            this.http.postRecoveryAnswer(recoveryData).subscribe(auth => {
            // this.authSubscribtion(auth);
            recoverySubscribtion.unsubscribe();
        });
    }

    logout() {
        this.store.update(st => ({
            ...st,
            userInfo: null,
            auth: null,
            isAuth: false,
            isUser: false,
            isAdmin: false,
            isModerator: false,
        }))
        localStorage.removeItem(StorageKeys.AuthObject)
    }

    getAuth = () => this.store.pipe(select((state => state.auth)));
    getIsAuth = () => this.store.pipe(select((state => state.isAuth)));
    getIsUser = () => this.store.pipe(select((state => state.isUser)));
    getIsAdmin = () => this.store.pipe(select((state => state.isAdmin)));
    getIsModerator = () => this.store.pipe(select((state => state.isModerator)));

    private saveToLocalStorage(auth: Authorization) {
        localStorage.setItem(StorageKeys.AuthObject, JSON.stringify(auth))
    }
    
    private readFromLocalStorage(): Authorization | null {
        return JSON.parse(localStorage.getItem(StorageKeys.AuthObject)!) as Authorization | null;
    }

    loadUserInfo(id: string) {
        if (this.store.getValue().userInfo == null) {
            const userInfoSubscribtion = this.http.getUserInfo(id).subscribe(info => {
                if (info) {
                    this.updateUserInfo(info);
                    userInfoSubscribtion.unsubscribe();
                }
            });
        }
    }
    private updateUserInfo(userInfo: UserInfo) {
        this.store.update(st => ({
            ...st,
            userInfo: userInfo
        }))
    }
    getUserInfo = () => this.store.pipe(select((state => state.userInfo)));
    clearUserInfo() {
        this.store.update(st => ({
            ...st,
            userInfo: null
        }))
    }

    private isAuth = (auth: Authorization | null) : boolean => !!auth?.User && !!auth?.Token;
    private isUser = (auth: Authorization | null) : boolean => auth?.User?.Roles.includes(UserRole.User) || false;
    private isAdmin = (auth: Authorization | null) : boolean => auth?.User?.Roles.includes(UserRole.Administrator) || false;
    private isModerator = (auth: Authorization | null) : boolean => auth?.User?.Roles.includes(UserRole.Moderator) || false;
}
