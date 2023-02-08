import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Backend } from '../models/Configuration';
import { Publication } from '../models/Publication';
import { Commentary } from '../models/Commentary';
import { User } from '../models/User';
import { ConfigurationService } from './ConfigurationService';
import { AuthorizationRequest, Authorization } from '../models/Authorization';
import { LikeRequest } from '../models/Like';
import { Registration, RegistrationRequest } from '../models/Registration';
import { Recovery, RecoveryChangePassword, RecoveryQuestion, RecoveryRequestAnswer, RecoveryRequestLogin } from '../models/Recovery';

@Injectable({
  providedIn: 'root',
})
export class HttpRequestService {

  backend: Backend;

  constructor(private http: HttpClient, private configService: ConfigurationService) {
    this.backend = this.configService.configuration.backend;
  }

  private get<Type>(url: string): Observable<Type> {
    return this.http.get<Type>(url);
  }
  
  private post<InType, OutType>(url: string, body: InType): Observable<OutType> {
    return this.http.post<OutType>(url, body);
  }


  getPublications(): Observable<Publication[]> {
    const url = this.backend.baseURL + `/publications`;
    return this.get<Array<Publication>>(url);
  }
  
  getAccountPublications(id: string): Observable<Publication[]> {
    const url = this.backend.baseURL + `/users/${id}/publications`;
    return this.get<Array<Publication>>(url);
  }
  
  getPostById(id: string): Observable<Publication> {
    const url = this.backend.baseURL + `/publications/${id}`;
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

  postAuthentication(body: AuthorizationRequest) {
    const url = this.backend.baseURL + `/login`;
    return this.post<AuthorizationRequest, Authorization>(url, body);
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
  
  postPublication(body: Publication) {
    const url = this.backend.baseURL + `/publications/add`;
    return this.post<Publication, any>(url, body);
  }
  
  postLike(body: LikeRequest, path: string) {
    let url = this.backend.baseURL + `/${path}/like`;
    return this.post<LikeRequest, any>(url, body);
  }
}
