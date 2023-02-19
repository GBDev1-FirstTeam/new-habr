import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom, Observable } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlesComponent implements OnInit {

  accountId: string;
  publications$: Observable<Array<Publication> | null>;

  succesfulModerate: boolean = false;

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
  moderate = (post: Publication) => {
    lastValueFrom(this.http.publishArticle(post.Id!)).then(() => {
      this.succesfulModerate = true;
    });
  };
}
