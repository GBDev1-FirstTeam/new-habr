import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { lastValueFrom, Subscription } from 'rxjs';
import { Authorization } from '../../models/Authorization';
import { Like } from '../../models/Like';
import { HttpRequestService } from '../../services/HttpRequestService';
import { AppStoreProvider } from '../../store/store';

@Component({
  selector: 'app-like',
  templateUrl: './like.component.html',
  styleUrls: ['./like.component.scss']
})
export class LikeComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];
  
  auth: Authorization | null;
  isAuth: boolean;

  @Input() likeData: Like;
  @Input() path: string;

  constructor(private http: HttpRequestService, private store: AppStoreProvider) { }

  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(isAuthSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  like() {
    if (!this.isAuth) return;

    if (this.likeData.IsLiked) this.likeData.LikesCount!--;
    else this.likeData.LikesCount!++;
    this.likeData.IsLiked = !this.likeData.IsLiked;

    lastValueFrom(this.http.postLike({
      Id: this.likeData.Id!,
      Login: this.auth?.User.Login!,
      UserId: this.auth?.User.Id!,
      Like: this.likeData.IsLiked
    },
    this.path))
  }
}
