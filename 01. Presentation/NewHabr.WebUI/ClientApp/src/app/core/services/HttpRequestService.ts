import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Backend } from '../models/Configuration';
import { Publication } from '../models/Publication';
import { Commentary } from '../models/Commentary';
import { User } from '../models/User';
import { ConfigurationService } from './ConfigurationService';
import { AuthorizationRequest, Authorization } from '../models/Authorization';

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
    const url = this.backend.baseURL + this.backend.children.publications;
    return this.get<Array<Publication>>(url);
  }
  
  getAccountPublications(id: string): Observable<Publication[]> {
    const url = this.backend.baseURL + this.backend.children.userPublications.format(id);
    return this.get<Array<Publication>>(url);
  }
  
  getPostById(id: string): Observable<Publication> {
    const url = this.backend.baseURL + this.backend.children.publication.format(id);
    return this.get<Publication>(url);
  }
  
  getUserById(id: string): Observable<User> {
    const url = this.backend.baseURL + this.backend.children.user.format(id);
    return this.get<User>(url);
  }
  
  getCommentsByPostId(id: string): Observable<Array<Commentary>> {
    const url = this.backend.baseURL + this.backend.children.comments.format(id);
    return this.get<Array<Commentary>>(url);
  }

  postAuthentication(body: AuthorizationRequest) {
    const url = this.backend.baseURL + this.backend.children.login;
    return this.post<AuthorizationRequest, Authorization>(url, body);
  }
  
  postComment(body: Commentary) {
    const url = this.backend.baseURL + this.backend.children.addComment;
    return this.post<Commentary, any>(url, body);
  }
  
  postPublication(body: Publication) {
    const url = this.backend.baseURL + this.backend.children.addPublication;
    return this.post<Publication, any>(url, body);
  }
}
