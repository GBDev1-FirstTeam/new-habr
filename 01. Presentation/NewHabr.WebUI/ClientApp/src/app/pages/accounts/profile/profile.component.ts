import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { UserInfo } from 'src/app/core/models/User';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  user: UserInfo;
  userId: string;
  plug: any;

  constructor(
    private http: HttpRequestService,
    private activeRoute: ActivatedRoute,
    private store: AppStoreProvider) { }

  ngOnInit(): void {
    const paramsSubscribtion = this.activeRoute.parent!.params.subscribe(params => {
      this.userId = params.id;
      this.store.loadUserInfo(this.userId);
    });

    const userSubscribtion = this.store.getUserInfo().subscribe(user => {
      if (user) {
        this.user = user;
        userSubscribtion?.unsubscribe();
        paramsSubscribtion?.unsubscribe();
      }
    })
  }

  save() {
    console.log(this.user)
    lastValueFrom(this.http.putUserInfo(this.userId, {
      FirstName: this.user.FirstName,
      LastName: this.user.LastName,
      Patronymic: this.user.Patronymic,
      BirthDay: this.user.BirthDay,
      Description: this.user.Description
    }))
  }
}
