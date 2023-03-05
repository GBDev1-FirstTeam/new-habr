import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { CommentRequest } from 'src/app/core/models/Commentary';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { Authorization } from 'src/app/core/models/Authorization';
import { AppStoreProvider } from 'src/app/core/store/store';
import { LikeData } from 'src/app/core/models/Like';
import { UserInfo } from 'src/app/core/models/User';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];
  post: Publication;
  postId: string;
  commentText: string;
  userInfo: UserInfo | null;
  
  auth: Authorization | null;
  isAuth: boolean | null;

  constructor(
    private http: HttpRequestService,
    private activeRoute: ActivatedRoute,
    private store: AppStoreProvider) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      this.postId = params.id;
      const postSubscribtion = this.http.getPublicationById(this.postId).subscribe(p => {
        if (p) {
          this.post = p;
          postSubscribtion.unsubscribe();
        }
      })

      const userSubscribtion = this.store.getUserInfo().subscribe(userInfo => {
        if (userInfo) {
          this.userInfo = userInfo;
          userSubscribtion.unsubscribe();
        }
      })
    })

    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(isAuthSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  addComment() {
    if (!this.commentText) {
      return;
    }

    const comment: CommentRequest = {
      Text: this.commentText,
      ArticleId: this.postId
    }
    
    this.post.Comments?.push({
      Id: undefined,
      UserId: this.auth!.User.Id,
      Username: this.auth!.User.UserName,
      ArticleId: this.postId,
      Text: this.commentText,
      ModifiedAt: Date.now()
    });
    
    lastValueFrom(this.http.addComment(comment));

    this.commentText = '';
  }

  like(likeData: LikeData) {
    if(likeData.isLiked) {
      lastValueFrom(this.http.likeArticle(this.postId || '', 'like'))
    }
    else {
      lastValueFrom(this.http.likeArticle(this.postId || '', 'unlike'))
    }
  }
}
