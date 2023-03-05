import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { UserInfo } from 'src/app/core/models/User';
import { Notification } from 'src/app/core/models/Notification';
import { ConvertDatePipe } from 'src/app/core/pipes/convert-date.pipe';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  providers: [ConvertDatePipe],
  animations: [
    trigger('notify', [
      state('left', style({
        transform: 'rotate(30deg)',
        color: 'gray'
      })),
      state('right', style({
        transform: 'rotate(-30deg)'
      })),
      state('neutral', style({
        transform: 'rotate(0deg)',
        color: 'white'
      })),
      transition('neutral => left', [
        animate('0.3s')
      ]),
      transition('left => right', [
        animate('0.6s')
      ]),
      transition('right => neutral', [
        animate('0.3s')
      ])
    ])
  ]
})
export class MainComponent {

  subscribtions: Subscription[] = [];
  
  auth: Authorization | null;
  isAuth: boolean;
  isModerator: boolean;
  isAdmin: boolean;
  
  userInfo: UserInfo | null;

  blockString: string;
  notifyPos: string = 'neutral';
  notifications: Array<Notification> = [];
  notificationsTimer: NodeJS.Timer;
  
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
      name: 'Поиск',
      url: 'search',
      iClass: 'bi bi-search'
    },
    {
      name: 'Помощь',
      url: 'help',
      iClass: 'bi bi-question-circle'
    }
  ]

  constructor(private store: AppStoreProvider, private router: Router, private convertDate: ConvertDatePipe, private http: HttpRequestService) { }

  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => {
      if (auth) {
        this.auth = auth;
        this.getUserNotifications();
      }
    });
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);
    const isModeratorSubscribtion = this.store.getIsModerator().subscribe(isModerator => this.isModerator = isModerator);
    const isAdminSubscribtion = this.store.getIsAdmin().subscribe(isAdmin => this.isAdmin = isAdmin);
    const userInfoSubscribtion = this.store.getUserInfo().subscribe(userInfo => {
      if (userInfo) {
        this.userInfo = userInfo;
      
        if (userInfo?.Banned) {
          this.blockString = `Аккаунт заблокирован с ${this.convertDate.transform(userInfo?.BannedAt)} по ${this.convertDate.transform(userInfo?.BanExpiratonDate)}. Причина: ${userInfo?.BanReason}`;
          setTimeout(() => {
            const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]') as any;
            const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
          }, 1000);
        }
      } else if (this.auth) {
        this.store.loadUserInfo(this.auth!.User.Id);
      }
    });

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(isAuthSubscribtion);
    this.subscribtions.push(isModeratorSubscribtion);
    this.subscribtions.push(isAdminSubscribtion);
    this.subscribtions.push(userInfoSubscribtion);

    this.animateNotify();
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
    clearInterval(this.notificationsTimer);
  }

  navigate = (path: any[]) => this.router.navigate(path);

  logout() {
    this.store.logout();
    this.router.navigate([this.menu[0].url]);
  }

  animateNotify() {
    setInterval(() => {
      this.notifyPos = 'left';

      setTimeout(() => {
        this.notifyPos = 'right';

        setTimeout(() => {
          this.notifyPos = 'neutral';
        }, 600);
      }, 300);
    }, 2500);
  }

  getUserNotifications() {
    const queryNotifications = () => {
      if (this.auth) {
        const userNotifySubscribtion = this.http.getUserNotifications(this.auth.User.Id).subscribe(n => {
          if (n) {
            this.notifications = n;
            userNotifySubscribtion.unsubscribe();
          }
        })
      }
    };

    queryNotifications();
    this.notificationsTimer = setInterval(() => queryNotifications(), 300000);
  }

  deleteNotify(notificationId: string) {
    this.notifications = this.notifications.filter(n => n.Id !== notificationId);

    lastValueFrom(this.http.deleteNotification(notificationId));
  }
}
