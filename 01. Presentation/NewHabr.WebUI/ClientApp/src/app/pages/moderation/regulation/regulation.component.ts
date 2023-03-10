import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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
  succesfulUnban: boolean = false;
  succesfulSetRole: boolean = false;

  constructor(private store: AppStoreProvider, private http: HttpRequestService, private cdr: ChangeDetectorRef) { }

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

  disableBan = (user: UserInfo) => {
    lastValueFrom(this.http.unbanUser(user.Id)).then(() => {
      this.succesfulUnban = true;
      user.Banned = false;
    })
  }

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
  
  setRole(userId: string, isUser: boolean, isModerator: boolean, isAdmin: boolean) {
    if ((isUser || isModerator || isAdmin) === false)
      return;
    
    const roles = [];

    if (isUser) roles.push(UserRole.User);
    if (isModerator) roles.push(UserRole.Moderator);
    if (isAdmin) roles.push(UserRole.Administrator);

    lastValueFrom(this.http.setUserRole(userId, { Roles: roles })).then(() => {
      this.succesfulSetRole = true;
    })
  }

  isUser = (roles: string[]) => roles.includes(UserRole.User);
  isModerator = (roles: string[]) => roles.includes(UserRole.Moderator);
  isAdministrator = (roles: string[]) => roles.includes(UserRole.Administrator);

  selectRole = () => this.cdr.detectChanges();
}
