import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LikeData } from 'src/app/core/models/Like';
import { UserInfo } from 'src/app/core/models/User';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { LikeService } from 'src/app/core/services/LikeService';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  user: UserInfo;

  constructor(
    private http: HttpRequestService,
    private activeRoute: ActivatedRoute,
    private likeService: LikeService) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      const userSubscribtion = this.http.getUserInfo(params.id).subscribe(user => {
        if (user) {
          this.user = user;
          userSubscribtion.unsubscribe();
        }
      })
    })
  }
  
  like = (likeData: LikeData) => this.likeService.like(likeData, this.user.Id, this.http.likeUser.bind(this.http));
}
