import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];
  post: Publication = {
    Title: 'Заголовок',
    Content: 'Контент',
    ImgURL: 'Ссылка на изображение',
    IsPublished: false
  };
  text: string;
  mode: Mode;
  auth: Authorization | null;

  constructor(
    private http: HttpRequestService,
    private store: AppStoreProvider,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      const postId = params.id;

      if (postId) {
        const postSubscribtion = this.http.getPostById(params.id).subscribe(post => {
          if (post) {
            this.post = post;
            this.subscribtions.push(postSubscribtion);
          }
        })
        this.mode = Mode.Edit;
      } else {
        this.mode = Mode.Create;
      }
    })

    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    this.subscribtions.push(authSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  save() {
    switch (this.mode) {
      case Mode.Edit:
        this.post.ModifyAt = Date.now();
        lastValueFrom(this.http.postPublication(this.post));
        break;
      case Mode.Create:
        const post = {
          Title: this.post.Title,
          Content: this.post.Content,
          UserId: this.auth?.User.Id,
          UserLogin: this.auth?.User.Login,
          CreatedAt: Date.now(),
          ModifyAt: Date.now(),
          ImgURL: this.post.ImgURL,
          IsPublished: false
        }
        lastValueFrom(this.http.postPublication(post));
        break;
    }
  }
}

enum Mode {
  Edit,
  Create
}
