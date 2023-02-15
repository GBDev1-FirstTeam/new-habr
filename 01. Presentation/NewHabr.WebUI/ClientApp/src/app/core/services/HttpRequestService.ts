import { HttpClient } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { Backend } from '../models/Configuration';
import { Publication } from '../models/Publication';
import { Commentary } from '../models/Commentary';
import { User } from '../models/User';
import { ConfigurationService } from './ConfigurationService';
import { LikeRequest } from '../models/Like';
import { Registration, RegistrationRequest } from '../models/Registration';
import { Recovery, RecoveryChangePassword, RecoveryQuestion, RecoveryRequestAnswer, RecoveryRequestLogin } from '../models/Recovery';
import { Authorization, LoginRequest, RegisterRequest } from '../models/Authorization';
import { SecureQuestion } from '../models/SecureQuestion';
import { AppStoreProvider } from '../store/store';

@Injectable({
  providedIn: 'root',
})
export class HttpRequestService {

  backend: Backend;
  auth: Authorization;

  constructor(
    private http: HttpClient,
    private configService: ConfigurationService,
    private injector: Injector) {
    this.backend = this.configService.configuration.backend;
    
    setTimeout(() => {
      const store = this.injector.get(AppStoreProvider);
      store.getAuth().subscribe(auth => {
        if (auth) {
          this.auth = auth;
        }
      })
    });
  }

  private get<Type>(url: string): Observable<Type> {
    return this.http.get<Type>(url, {
      headers: {
        "Authorization": `Bearer ${this.auth?.Token}`
      }
    });
  }
  
  private post<InType, OutType>(url: string, body: InType): Observable<OutType> {
    return this.http.post<OutType>(url, body, {
      headers: {
        "Authorization": `Bearer ${this.auth?.Token}`
      }
    });
  }
  
  private put<InType, OutType>(url: string, body: InType): Observable<OutType> {
    return this.http.put<OutType>(url, body, {
      headers: {
        "Authorization": `Bearer ${this.auth?.Token}`
      }
    });
  }


  getPublications(): Observable<Publication[]> {
    const url = this.backend.baseURL + `/articles/published/10`; //ToDo продумать, откуда брать количество
    return this.get<Array<Publication>>(url);
  }

  publishArticle(id: string) {
    const url = this.backend.baseURL + `/articles/${id}/publish`;
    return this.put<any, any>(url, undefined);
  }
  
  getAccountPublications(id: string): Observable<Publication[]> {
    const url = this.backend.baseURL + `/users/${id}/articles`;
    return this.get<Array<Publication>>(url);
  }
  
  getPostById(id: string): Observable<Publication> {
    const url = this.backend.baseURL + `/articles/${id}`;
    return this.get<Publication>(url);
  }
  
  getUserById(id: string): Observable<User> {
    const url = this.backend.baseURL + `/users/${id}`;
    return this.get<User>(url);
  }
  
  getCommentsByPostId(id: string): Observable<Array<Commentary>> {
    const url = this.backend.baseURL + `/comments/${id}`;
    return this.get<Array<Commentary>>(url);
  }

  postRegistration(body: RegistrationRequest) {
    const url = this.backend.baseURL + `/register`;
    return this.post<RegistrationRequest, Registration>(url, body);
  }

  postRecoveryLogin(body: RecoveryRequestLogin) {
    const url = this.backend.baseURL + `/recovery/login`;
    return this.post<RecoveryRequestLogin, RecoveryQuestion>(url, body);
  }

  postRecoveryAnswer(body: RecoveryRequestAnswer) {
    const url = this.backend.baseURL + `/recovery/answer`;
    return this.post<RecoveryRequestAnswer, Recovery>(url, body);
  }

  postRecoveryChangePassword(body: RecoveryChangePassword) {
    const url = this.backend.baseURL + `/recovery/password`;
    return this.post<RecoveryChangePassword, any>(url, body);
  }
  
  postComment(body: Commentary) {
    const url = this.backend.baseURL + `/comments/add`;
    return this.post<Commentary, any>(url, body);
  }
  
  postCreatePublication(body: Publication) {
    const url = this.backend.baseURL + `/articles`;
    return this.post<Publication, any>(url, body);
  }
  
  postUpdatePublication(id: string, body: Publication) {
    const url = this.backend.baseURL + `/articles/${id}`;
    return this.put<Publication, any>(url, body);
  }
  
  postLike(body: LikeRequest, path: string) {
    let url = this.backend.baseURL + `/${path}/like`;
    return this.post<LikeRequest, any>(url, body);
  }

  // #region /auth
  register(body: RegisterRequest) {
    const url = this.backend.baseURL + `/auth`;
    return this.post<RegisterRequest, Authorization>(url, body);
  }
  login(body: LoginRequest) {
    const url = this.backend.baseURL + `/auth/login`;
    return this.post<LoginRequest, Authorization>(url, body);
  }
  // #endregion

  // #region /SecureQuestions
  getAllQuestions() {
    const url = this.backend.baseURL + `/SecureQuestions`;
    return this.get<Array<SecureQuestion>>(url);
  }
  // #endregion
}
