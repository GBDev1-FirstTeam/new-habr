import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom, Observable, Subscription } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { Commentary } from 'src/app/core/models/Commentary';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { Authorization } from 'src/app/core/models/Authorization';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];
  post$: Observable<Publication>;
  postId: string;
  comments: Array<Commentary> = [];
  commentText: string;
  
  auth: Authorization | null;
  isAuth: boolean | null;

  constructor(
    private http: HttpRequestService,
    private activeRoute: ActivatedRoute,
    private store: AppStoreProvider) { }
  

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      this.postId = params.id;
      this.post$ = this.http.getPostById(this.postId);
      
      const commentsSubscribtion = this.http.getCommentsByPostId(this.postId).subscribe(comments => {
        if (comments) {
          this.comments = comments;
          this.subscribtions.push(commentsSubscribtion);
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
    const comment: Commentary = {
      UserId: this.auth?.User.Id!,
      UserLogin: this.auth?.User.Login!,
      ArticleId: this.postId,
      Text: this.commentText,
      CreatedAt: Date.now(),
    }
    
    this.commentText = '';
    this.comments.push(comment);
    
    lastValueFrom(this.http.postComment(comment));
  }
}
