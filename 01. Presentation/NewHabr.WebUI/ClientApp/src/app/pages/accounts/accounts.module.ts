import { Injectable, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountsComponent } from './accounts.component';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterModule, RouterStateSnapshot, Routes, UrlTree } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppStoreProvider } from 'src/app/core/store/store';
import { Observable } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { ArticlesComponent } from './articles/articles.component';
import { ArticleComponent } from './article/article.component';
import { ComponentsLibraryModule } from 'src/app/core/components/components-library.module';

@Injectable()
class LoginGuard implements CanActivate {
  
  constructor(private store: AppStoreProvider, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean|UrlTree>|Promise<boolean|UrlTree>|boolean|UrlTree {  
    return this.store.getIsAuth();
  }
}

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: ':id',
    component: AccountsComponent,
    children: [
      {
        path: 'articles',
        component: ArticlesComponent
      },
      {
        path: 'articles/edit/:id',
        component: ArticleComponent
      },
      {
        path: 'articles/create',
        component: ArticleComponent
      }
    ],
    canActivate: [LoginGuard]
  }
];

@NgModule({
  declarations: [
    AccountsComponent,
    LoginComponent,
    ArticlesComponent,
    ArticleComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ComponentsLibraryModule
  ],
  providers: [{
    provide: LoginGuard
  }]
})
export class AccountsModule { }