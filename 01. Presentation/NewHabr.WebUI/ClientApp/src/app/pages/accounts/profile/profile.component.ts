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
  birthday: string;

  succesfulSend: boolean = false;

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
        this.birthday = (new Date(this.user.BirthDay)).toLocaleString('fr-CA', {
          year: "numeric",
          month: "2-digit",
          day: "2-digit"
        } as Intl.DateTimeFormatOptions);

        userSubscribtion?.unsubscribe();
      }
    })
  }

  save() {
    lastValueFrom(this.http.putUserInfo(this.userId, {
      FirstName: this.user.FirstName,
      LastName: this.user.LastName,
      Patronymic: this.user.Patronymic,
      BirthDay: this.user.BirthDay,
      Description: this.user.Description
    })).then(x => {
      this.succesfulSend = true;
    })
  }

  changeBirthday($event: any) {
    this.user.BirthDay = $event.target.valueAsNumber;
  }
}
