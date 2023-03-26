import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { Authorization } from '../../models/Authorization';
import { LikeData } from '../../models/Like';
import { UserInfo } from '../../models/User';
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
  userInfo: UserInfo | null;

  @Input() isLiked: boolean;
  @Input() count: number;
  @Input() likedObject: any;

  @Output() doLike: EventEmitter<LikeData> = new EventEmitter<LikeData>()

  constructor(private store: AppStoreProvider) { }

  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);
    const userSubscribtion = this.store.getUserInfo().subscribe(userInfo => {
      if (userInfo) {
        this.userInfo = userInfo;
      }
    })

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(isAuthSubscribtion);
    this.subscribtions.push(userSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  like() {
    if (!this.isAuth || this.userInfo?.Banned) return;
    if (this.likedObject?.UserId === this.userInfo?.Id) return;
    if (this.likedObject?.Id === this.userInfo?.Id) return;

    this.count = this.isLiked ? this.count - 1 : this.count + 1;
    this.isLiked = !this.isLiked;
    
    this.doLike.emit({
      count: this.count,
      isLiked: this.isLiked
    })
  }
}
