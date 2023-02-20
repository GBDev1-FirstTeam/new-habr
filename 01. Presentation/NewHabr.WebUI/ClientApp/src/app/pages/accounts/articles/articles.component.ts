import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom, Observable } from 'rxjs';
import { PublicationUser } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlesComponent implements OnInit {

  accountId: string;
  publications$: Observable<Array<PublicationUser> | null>;

  succesfulModerate: boolean = false;
  succesfulPublicate: boolean = false;

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.parent?.params.subscribe(params => {
      this.accountId = params.id;
      this.publications$ = this.http.getUserPublications(this.accountId);
    })
  }

  edit = (postId: string | undefined) => this.router.navigate(['accounts', this.accountId, 'articles', 'edit', postId]);
  create = () => this.router.navigate(['accounts', this.accountId, 'articles', 'create']);
  publicate = (post: PublicationUser) => {
    lastValueFrom(this.http.publishArticle(post.Id!)).then(() => {
      this.succesfulPublicate = true;
    });
  };
  moderate = (post: PublicationUser) => {
    lastValueFrom(this.http.publishArticle(post.Id!)).then(() => {
      this.succesfulPublicate = true;
    });
  };
  
  mayBePublish = (post: PublicationUser) => (!post.Published && post.ApproveState === 'Approved');
  mayBeModerate = (post: PublicationUser) => (!post.Published && post.ApproveState !== 'Approved');
}
