import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LikeData } from 'src/app/core/models/Like';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { LikeService } from 'src/app/core/services/LikeService';
import { AppStoreProvider } from 'src/app/core/store/store';
import { Metadata } from '../../core/models/Structures';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.scss']
})
export class PublicationsComponent implements OnInit {

  subscribtions: Subscription[] = [];
  isAuth: boolean;

  metaData = {
              CurrentPage: 1,
              PageSize: 10,
              TotalCount: 1,
              TotalPages: 1
            } as Metadata;
  
  publications: Array<Publication>;

  constructor(private http: HttpRequestService, private router: Router, private store: AppStoreProvider, private likeService: LikeService) { }

  ngOnInit(): void {
    this.loadItems();
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);

    this.subscribtions.push(isAuthSubscribtion);
  }

  loadItems() {
    const publicationsSubscribtion = this.http.getPublications({ pageNumber: this.metaData.CurrentPage, pageSize: this.metaData.PageSize }).subscribe(p => {
      if (p) {
        this.publications = p.Articles;
        this.metaData = p.Metadata;
        publicationsSubscribtion.unsubscribe();
      }
    })
  }

    pageChanged(event: any) {
      this.metaData.CurrentPage = event.page;
      this.metaData.PageSize = event.itemsPerPage;
      this.loadItems();
    }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  navigate = (id: string | undefined) => this.router.navigate(['publications', id]);

  like = (likeData: LikeData, post: Publication) => this.likeService.like(likeData, post.Id, this.http.likeArticle.bind(this.http));
}
