import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { LikeData } from 'src/app/core/models/Like';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
  
  publications: Array<Publication>;
  isLoading: boolean = false;

  constructor(private http: HttpRequestService, private router: Router) { }

  navigate = (id: string | undefined) => this.router.navigate(['publications', id]);

  like(likeData: LikeData, post: Publication) {
    if(likeData.isLiked) {
      lastValueFrom(this.http.likeArticle(post.Id || '', 'like'))
    }
    else {
      lastValueFrom(this.http.likeArticle(post.Id || '', 'unlike'))
    }
  }

  searchArticles(input: HTMLInputElement) {
    this.isLoading = true;
    this.publications = [];
    const timeoutId = setTimeout(() => this.isLoading = false, 15000);

    const publicationsSubscribtion = this.http.getPublications({ search: input.value }).subscribe(p => {
      if (p) {
        this.publications = p.Articles;
        this.isLoading = false;
        input.value = '';
        clearTimeout(timeoutId);
        publicationsSubscribtion.unsubscribe();
      }
    })
  }
}
