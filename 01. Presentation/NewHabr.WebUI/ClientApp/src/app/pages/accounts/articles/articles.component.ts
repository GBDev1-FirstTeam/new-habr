import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { PublicationUser } from 'src/app/core/models/Publication';
import { ApproveState } from 'src/app/core/models/Structures';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
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

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.parent?.params.subscribe(params => {
      this.accountId = params.id;
       const publicationsSubscribtion = this.http.getUserPublications(this.accountId).subscribe(p => {
        if (p) {
          this.publications = _.orderBy(p, ['CreatedAt'], ['desc']);
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

  publicate = (post: PublicationUser) => {
    lastValueFrom(this.http.publishArticle(post.Id!)).then(() => {
      this.succesfulPublicate = true;
      post.Published = true;
    });
  };

  moderate = (post: PublicationUser) => {
    lastValueFrom(this.http.setPublicationState(post.Id!, ApproveState.WaitApproval)).then(() => {
      this.succesfulModerate = true;
      post.ApproveState = 'WaitApproval';
    });
  };

  mayBePublish = (post: PublicationUser) => (!post.Published && post.ApproveState === 'Approved');
  mayBeModerate = (post: PublicationUser) => (!post.Published && post.ApproveState === 'NotApproved');
}
