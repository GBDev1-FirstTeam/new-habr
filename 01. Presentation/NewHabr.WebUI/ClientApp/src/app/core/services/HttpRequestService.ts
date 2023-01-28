import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Backend } from '../models/Configuration';
import { Publication } from '../models/Publication';
import { User } from '../models/User';
import { ConfigurationService } from './ConfigurationService';

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


  getPublications(): Observable<Publication[]> {
    const url = this.backend.baseURL + this.backend.children.publications;
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
}
