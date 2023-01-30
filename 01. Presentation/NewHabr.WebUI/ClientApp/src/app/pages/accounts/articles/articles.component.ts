import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom, Observable } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.scss']
})
export class ArticlesComponent implements OnInit {

  accountId: string;
  publications$: Observable<Array<Publication> | null>;

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.parent?.params.subscribe(params => {
      this.accountId = params.id;
      this.publications$ = this.http.getAccountPublications(this.accountId);
    })
  }

  read = (postId: string | undefined) => this.router.navigate(['publications', postId]);
  edit = (postId: string | undefined) => this.router.navigate(['accounts', this.accountId, 'articles', 'edit', postId]);
  create = () => this.router.navigate(['accounts', this.accountId, 'articles', 'create']);
  publish = (post: Publication) => {
    post.PublishedAt = Date.now();
    post.IsPublished = true;
    lastValueFrom(this.http.postPublication(post));
  };
}
