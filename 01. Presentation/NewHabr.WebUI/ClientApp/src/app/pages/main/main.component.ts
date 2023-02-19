import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Authorization } from 'src/app/core/models/Authorization';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent {

  subscribtions: Subscription[] = [];
  
  auth: Authorization | null;
  isAuth: boolean;
  isModerator: boolean;

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
      url: 'find',
      iClass: 'bi bi-search'
    }
  ]

  constructor(private store: AppStoreProvider, private router: Router) { }

  ngOnInit(): void {
    const authSubscribtion = this.store.getAuth().subscribe(auth => this.auth = auth);
    const isAuthSubscribtion = this.store.getIsAuth().subscribe(isAuth => this.isAuth = isAuth);
    const isModeratorSubscribtion = this.store.getIsModerator().subscribe(isModerator => this.isModerator = isModerator);

    this.subscribtions.push(authSubscribtion);
    this.subscribtions.push(isAuthSubscribtion);
    this.subscribtions.push(isModeratorSubscribtion);
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
