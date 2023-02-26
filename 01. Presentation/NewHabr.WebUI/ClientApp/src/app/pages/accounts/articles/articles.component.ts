import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { PublicationUser } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { ArticleState } from 'src/app/core/static/ArticleState';
import * as _ from 'lodash';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlesComponent implements OnInit {

  subscribtions: Subscription[] = [];

  accountId: string;
  publications: Array<PublicationUser>;

  succesfulModerate: boolean = false;
  succesfulPublicate: boolean = false;
  succesfulUnPublicate: boolean = false;

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.parent?.params.subscribe(params => {
      this.accountId = params.id;
       const publicationsSubscribtion = this.http.getUserPublications(this.accountId).subscribe(p => {
        if (p) {
          this.publications = _.orderBy(p.Articles, ['CreatedAt'], ['desc']);
        }
      })

      this.subscribtions.push(publicationsSubscribtion);
    })
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  edit = (postId: string | undefined) => this.router.navigate(['accounts', this.accountId, 'articles', 'edit', postId]);

  create = () => this.router.navigate(['accounts', this.accountId, 'articles', 'create']);

  publish = (post: PublicationUser) => {
    lastValueFrom(this.http.publishPost(post.Id!)).then(() => {
      if (post.ApproveState === ArticleState.NotApproved) {
        this.succesfulModerate = true;
        post.ApproveState = ArticleState.WaitApproval;
      } else {
        this.succesfulPublicate = true;
        post.Published = true;
      }
    });
  };

  unpublish = (post: PublicationUser) => {
    lastValueFrom(this.http.unpublishPost(post.Id!)).then(() => {
      this.succesfulUnPublicate = true;
      post.Published = false;
    });
  };

  mayBePublish = (post: PublicationUser) => !post.Published && post.ApproveState !== ArticleState.WaitApproval;
  mayBeUnPublish = (post: PublicationUser) => post.Published;
}
