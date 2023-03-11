import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Category } from 'src/app/core/models/Category';
import { LikeData } from 'src/app/core/models/Like';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { LikeService } from 'src/app/core/services/LikeService';
import * as _ from 'lodash';
import { ArticleQueryParameters } from 'src/app/core/models/Structures';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements AfterViewInit {
  
  subscribtions: Subscription[] = [];
  
  categoriesList: Array<Category>;
  activeCategory: Category = { Id: -1, Name: 'Не выбрано' };
  publications: Array<Publication>;
  isLoading: boolean = false;

  @ViewChild('search') searchInput: ElementRef<HTMLInputElement>;

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private likeService: LikeService,
    private activeRoute: ActivatedRoute) { }

  ngAfterViewInit() {
    const paramsSubscribtion = this.activeRoute.queryParams.subscribe(params => {
      const categoryKey = 'Category';
      const tagKey = 'Tag';
      
      if (categoryKey in params) {
        this.searchArticles(undefined, params[categoryKey], undefined);
      }
      if (tagKey in params) {
        this.searchArticles(undefined, undefined, params[tagKey]);
      }
    });

    const questionsSubscribtion = this.http.getCategories().subscribe(categories => {
      if (categories) {
        this.categoriesList = _.concat(this.activeCategory, categories);
      }
    })

    this.subscribtions.push(paramsSubscribtion);
    this.subscribtions.push(questionsSubscribtion);
  }

  navigate = (id: string | undefined) => this.router.navigate(['publications', id]);

  like = (likeData: LikeData, post: Publication) => this.likeService.like(likeData, post.Id, this.http.likeArticle.bind(this.http));

  searchArticles(text: string | undefined, category: number | undefined = undefined, tag: number | undefined = undefined) {
    if (!text && this.activeCategory.Id === -1 && !category && !tag)
      return;

    this.isLoading = true;
    this.publications = [];
    const timeoutId = setTimeout(() => this.isLoading = false, 15000);

    let query: any = { }
    if (text)
      query = { ...query, search: text }
    if (this.activeCategory.Id === -1)
      query = { ...query, Category: this.activeCategory.Id }
    if (category)
      query = { ...query, Category: category }
    if (tag)
      query = { ...query, Tag: tag }

    const publicationsSubscribtion = this.http.getPublications(query as ArticleQueryParameters).subscribe(p => {
      if (p) {
        this.publications = p.Articles;
        this.isLoading = false;
        clearTimeout(timeoutId);
        publicationsSubscribtion.unsubscribe();
      }
    })
  }

  changeCategory = ($event: any) => this.activeCategory = this.categoriesList.find(c => c.Name === $event.target.value)!
}
