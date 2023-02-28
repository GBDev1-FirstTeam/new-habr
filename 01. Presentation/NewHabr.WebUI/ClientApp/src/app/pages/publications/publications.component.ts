import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { LikeData } from 'src/app/core/models/Like';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.scss']
})
export class PublicationsComponent implements OnInit {

  subscribtions: Subscription[] = [];
  isAuth: boolean;
  
  publications: Array<Publication>;

  constructor(private http: HttpRequestService, private router: Router, private store: AppStoreProvider) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getPublications(1, 10).subscribe(p => {
      if (p) {
        this.publications = p.Articles;
        publicationsSubscribtion.unsubscribe();
      }
    })
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);

    this.subscribtions.push(isAuthSubscribtion);
  }

  navigate = (id: string | undefined) => this.router.navigate(['publications', id]);

  like(likeData: LikeData, post: Publication) {
    if(likeData.isLiked) {
      lastValueFrom(this.http.likeArticle(post.Id || '', 'like'))
    }
    else {
      lastValueFrom(this.http.likeArticle(post.Id || '', 'unlike'))
    }
  }
}
