import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Backend } from '../models/Configuration';
import { Publication, PublicationRequest, PublicationsResponse, PublicationsResponseUser } from '../models/Publication';
import { CommentRequest } from '../models/Commentary';
import { Notification } from '../models/Notification';
import { PutUserInfo, UserInfo } from '../models/User';
import { ConfigurationService } from './ConfigurationService';
import { Registration, RegistrationRequest } from '../models/Registration';
import { Recovery, RecoveryChangePassword, RecoveryQuestion, RecoveryRequestAnswer, RecoveryRequestLogin } from '../models/Recovery';
import { Authorization, LoginRequest, RegisterRequest } from '../models/Authorization';
import { SecureQuestion } from '../models/SecureQuestion';
import { Category } from '../models/Category';
import { Tag } from '../models/Tag';
import { ArticleQueryParameters, BanStruct, NameStruct, QuestionStruct, RoleStruct } from '../models/Structures';
import { StorageKeys } from '../static/StorageKeys';

@Injectable({
  providedIn: 'root',
})
export class HttpRequestService {

  backend: Backend;

  constructor(
    private http: HttpClient,
    private configService: ConfigurationService) {
    this.backend = this.configService.configuration.backend;
  }

  private get<Type>(url: string): Observable<Type> {
    const auth = this.getAuth();

    return this.http.get<Type>(url, {
      headers: {
        "Authorization": `Bearer ${auth?.Token}`
      }
    });
  }
  
  private post<InType, OutType>(url: string, body: InType): Observable<OutType> {
    const auth = this.getAuth();

    return this.http.post<OutType>(url, body, {
      headers: {
        "Authorization": `Bearer ${auth?.Token}`
      }
    });
  }
  
  private put<InType, OutType>(url: string, body: InType): Observable<OutType> {
    const auth = this.getAuth();

    return this.http.put<OutType>(url, body, {
      headers: {
        "Authorization": `Bearer ${auth?.Token}`
      }
    });
  }

  private delete(url: string): Observable<Object> {
    const auth = this.getAuth();

    return this.http.delete(url, {
      headers: {
        "Authorization": `Bearer ${auth?.Token}`
      }
    });
  }

  private getAuth = () => JSON.parse(localStorage.getItem(StorageKeys.AuthObject)!) as Authorization | null;

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
  addQuestion(body: QuestionStruct) {
    const url = this.backend.baseURL + `/SecureQuestions`;
    return this.post<QuestionStruct, any>(url, body);
  }
  // #endregion

  // #region /Users
  putUserInfo(id: string, body: PutUserInfo) {
    const url = this.backend.baseURL + `/Users/${id}`;
    return this.put<PutUserInfo, any>(url, body);
  }
  getUsers(): Observable<Array<UserInfo>> {
    const url = this.backend.baseURL + `/Users`;
    return this.get<Array<UserInfo>>(url);
  }
  banUser(id: string, body: BanStruct): Observable<Array<UserInfo>> {
    const url = this.backend.baseURL + `/Users/${id}/ban`;
    return this.put<BanStruct, any>(url, body);
  }
  setUserRole(id: string, body: RoleStruct): Observable<any> {
    const url = this.backend.baseURL + `/Users/${id}/setroles`;
    return this.put<RoleStruct, any>(url, body);
  }
  getUserInfo(id: string) {
    const url = this.backend.baseURL + `/Users/${id}`;
    return this.get<UserInfo>(url);
  }
  getUserPublications(id: string): Observable<PublicationsResponseUser> {
    const url = this.backend.baseURL + `/Users/${id}/articles`;
    return this.get<PublicationsResponseUser>(url);
  }
  getUserNotifications(id: string): Observable<Array<Notification>> {
    const url = this.backend.baseURL + `/Users/${id}/notifications`;
    return this.get<Array<Notification>>(url);
  }
  // #endregion

  // #region /Articles
  createPublication(body: PublicationRequest) {
    const url = this.backend.baseURL + `/Articles`;
    return this.post<PublicationRequest, any>(url, body);
  }
  updatePublication(id: string, body: PublicationRequest) {
    const url = this.backend.baseURL + `/articles/${id}`;
    return this.put<PublicationRequest, any>(url, body);
  }
  publishArticle(id: string) {
    const url = this.backend.baseURL + `/Articles/${id}/publish`;
    return this.put<any, any>(url, undefined);
  }
  publishPost(id: string): Observable<any> {
    const url = this.backend.baseURL + `/Articles/${id}/publish`;
    return this.put<any, any>(url, null);
  }
  unpublishPost(id: string): Observable<any> {
    const url = this.backend.baseURL + `/Articles/${id}/unpublish`;
    return this.put<any, any>(url, null);
  }
  approvePost(id: string): Observable<any> {
    const url = this.backend.baseURL + `/Articles/${id}/approve`;
    return this.put<any, any>(url, null);
  }
  disapprovePost(id: string): Observable<any> {
    const url = this.backend.baseURL + `/Articles/${id}/disapprove`;
    return this.put<any, any>(url, null);
  }
  getPublications(queryParams: ArticleQueryParameters): Observable<PublicationsResponse> {
    let endpoint = '/Articles?';
    for (let key in queryParams) {
      endpoint = endpoint.concat(key, '=', (queryParams as any)[key], '&')
    }

    const url = this.backend.baseURL + endpoint;
    return this.get<PublicationsResponse>(url);
  }
  getPublicationById(id: string): Observable<Publication> {
    const url = this.backend.baseURL + `/Articles/${id}`;
    return this.get<Publication>(url);
  }
  getUnpublishedPublications(step: number, count: number): Observable<PublicationsResponse> {
    const url = this.backend.baseURL + `/Articles/unpublished?PageNumber=${step}&PageSize=${count}`;
    return this.get<PublicationsResponse>(url);
  }
  likeArticle(id: string, mode: string): Observable<any> {
    const url = this.backend.baseURL + `/Articles/${id}/${mode}`;
    return this.put<any, any>(url, null);
  }
  // #endregion

  // #region /Categories
  getCategories() {
    const url = this.backend.baseURL + `/Categories`;
    return this.get<Array<Category>>(url);
  }
  addCategory(body: NameStruct) {
    const url = this.backend.baseURL + `/Categories`;
    return this.post<NameStruct, any>(url, body);
  }
  // #endregion

  // #region /Tags
  getTags() {
    const url = this.backend.baseURL + `/Tags`;
    return this.get<Array<Tag>>(url);
  }
  addTag(body: NameStruct) {
    const url = this.backend.baseURL + `/Tags`;
    return this.post<NameStruct, any>(url, body);
  }
  // #endregion

  // #region /Comments
  addComment(body: CommentRequest) {
    const url = this.backend.baseURL + `/Comments`;
    return this.post<CommentRequest, any>(url, body);
  }
  // #endregion

  // #region /Notifications
  deleteNotification(id: string) {
    const url = this.backend.baseURL + `/Notifications/${id}`;
    // return this.put<any, any>(url, null);
    return this.delete(url);
  }
  // #endregion
}
