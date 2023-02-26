import { Component, OnInit } from '@angular/core';
import { lastValueFrom, Subscription } from 'rxjs';
import { UserInfo } from 'src/app/core/models/User';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { UserRole } from 'src/app/core/static/UserRole';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-regulation',
  templateUrl: './regulation.component.html',
  styleUrls: ['./regulation.component.scss']
})
export class RegulationComponent implements OnInit {

  subscribtions: Subscription[] = [];
  users: Array<UserInfo>;
  showUsers: Array<UserInfo>;
  selection: string;

  isAdmin: boolean;

  succesfulBan: boolean = false;
  succesfulSetRole: boolean = false;

  constructor(private store: AppStoreProvider, private http: HttpRequestService) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getUsers().subscribe(users => {
      if (users) {
        this.users = users;
        this.showUsers = this.users;
        publicationsSubscribtion.unsubscribe();
      }
    })
    const isAdminSubscribtion = this.store.getIsAdmin().subscribe(isAdmin => this.isAdmin = isAdmin);

    this.subscribtions.push(isAdminSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
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
  
  setRole(select: string, user: UserInfo) {
    const setRoleLocal = (id: string, role: string) => lastValueFrom(this.http.setUserRole(id, { Roles: [role] })).then(() => {
      this.succesfulSetRole = true;
    })

    switch (select) {
      case '1':
        setRoleLocal(user.Id, UserRole.User); break;
      case '2':
        setRoleLocal(user.Id, UserRole.Moderator); break;
      case '3':
        setRoleLocal(user.Id, UserRole.Administrator); break;
    }
  }
}
