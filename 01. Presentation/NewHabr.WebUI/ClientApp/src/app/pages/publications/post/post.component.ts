import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { Commentary } from 'src/app/core/models/Commentary';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {

  post$: Observable<Publication>;
  postId: string;
  comments: Array<Commentary>;
  commentText: string;

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      this.postId = params.id;
      this.post$ = this.http.getPostById(this.postId);
      
      const commentsSubscribtion = this.http.getCommentsByPostId(this.postId).subscribe(comments => {
        if (comments) {
          this.comments = comments;
          commentsSubscribtion.unsubscribe();
        }
      })
    })
  }

  addComment() {
    const comment: Commentary = {
      Id: 'sdfsd',
      UserId: 'sdvsvdvd',
      ArticleId: this.postId,
      Text: this.commentText,
      CreatedAt: Date.now()
    }

    this.commentText = '';
    this.comments.push(comment);
  }
}
