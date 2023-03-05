import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { UserInfo } from 'src/app/core/models/User';
import { ConvertDatePipe } from 'src/app/core/pipes/convert-date.pipe';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  providers: [ConvertDatePipe]
})
export class MainComponent {

  subscribtions: Subscription[] = [];
  
  auth: Authorization | null;
  isAuth: boolean;
  isModerator: boolean;
  isAdmin: boolean;
  
  userInfo: UserInfo | null;

  blockString: string;
  
  menu = [
    {
      name: 'Главная страница',
      url: 'publications',
      iClass: 'bi bi-columns'
    },
    {
      name: 'Личный кабинет',
      url: 'accounts/login',
      iClass: 'bi bi-person'
    },
    {
      name: 'Помощь',
      url: 'help',
      iClass: 'bi bi-question-circle'
    },
    {
      name: 'Поиск',
      url: 'search',
      iClass: 'bi bi-search'
    }
  ]

  constructor(private store: AppStoreProvider, private router: Router, private convertDate: ConvertDatePipe) { }

  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);
    const isModeratorSubscribtion = this.store.getIsModerator().subscribe(isModerator => this.isModerator = isModerator);
    const isAdminSubscribtion = this.store.getIsAdmin().subscribe(isAdmin => this.isAdmin = isAdmin);
    const userInfoSubscribtion = this.store.getUserInfo().subscribe(userInfo => {
      this.userInfo = userInfo;
      
      if (userInfo?.Banned) {
        this.blockString = `Аккаунт заблокирован с ${this.convertDate.transform(userInfo?.BannedAt)} по ${this.convertDate.transform(userInfo?.BanExpiratonDate)}. Причина: ${userInfo?.BanReason}`;
        setTimeout(() => {
          const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]') as any;
          const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
        }, 1000);
      }
    });

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(isAuthSubscribtion);
    this.subscribtions.push(isModeratorSubscribtion);
    this.subscribtions.push(isAdminSubscribtion);
    this.subscribtions.push(userInfoSubscribtion);
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  navigate = (path: any[]) => this.router.navigate(path);

  logout() {
    this.store.logout();
    this.router.navigate([this.menu[0].url]);
  }
}
