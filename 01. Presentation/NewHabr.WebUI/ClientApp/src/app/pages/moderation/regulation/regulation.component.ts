import { Component, OnInit } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { UserInfo } from 'src/app/core/models/User';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-regulation',
  templateUrl: './regulation.component.html',
  styleUrls: ['./regulation.component.scss']
})
export class RegulationComponent implements OnInit {

  users: Array<UserInfo>;
  showUsers: Array<UserInfo>;
  selection: string;

  succesfulBan: boolean = false;

  constructor(private http: HttpRequestService) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getUsers().subscribe(users => {
      if (users) {
        this.users = users;
        this.showUsers = this.users;
        publicationsSubscribtion.unsubscribe();
      }
    })
  }

  ban = (user: UserInfo, reason: string) => {
    if (!!reason) {
      lastValueFrom(this.http.banUser(user.Id, { BanReason: reason })).then(() => {
        this.succesfulBan = true;
        user.Banned = true;
      })
    }
  }

  disableBan = (user: UserInfo) => { }

  changeSelection(event: any) {
    switch (event.target.value) {
      case '1':
        this.showUsers = this.users; break;
      case '2':
        this.showUsers = this.users?.filter(x => !x.Banned); break;
      case '3':
        this.showUsers = this.users?.filter(x => x.Banned); break;
    }
  }
}
