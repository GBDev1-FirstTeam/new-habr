import { Injectable, NgModule } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, NavigationCancel, Router, RouterModule, RouterStateSnapshot, Routes, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AppStoreProvider } from "./core/store/store";

@Injectable()
export class ModeratorGuard implements CanActivate {
  
  constructor(private store: AppStoreProvider, private router: Router) {
    this.router.events.subscribe(event => {
        if (event instanceof NavigationCancel) {
            setTimeout(() => this.router.navigate(['publications']))
        }
    })
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean|UrlTree>|Promise<boolean|UrlTree>|boolean|UrlTree {  
    return this.store.getIsModerator();
  }
}

const rootRoutes: Routes = [
    {
        path: 'publications',
        loadChildren: () => import('./pages/publications/publications.module').then(m => m.PublicationsModule)
    },
    {
        path: 'users',
        loadChildren: () => import('./pages/users/users.module').then(m => m.UsersModule)
    },
    {
        path: 'accounts',
        loadChildren: () => import('./pages/accounts/accounts.module').then(m => m.AccountsModule)
    },
    {
        path: 'moderation',
        loadChildren: () => import('./pages/moderation/moderation.module').then(m => m.ModerationModule),
        canActivate: [ModeratorGuard]
    },
    {
        path: '',
        redirectTo: 'publications',
        pathMatch: 'full'
    },
    { path: "**", redirectTo: '/publications' }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            rootRoutes,
            { enableTracing: false, relativeLinkResolution: 'legacy', scrollPositionRestoration: 'enabled' }
        )
    ],
    exports: [
        RouterModule
    ],
    providers: [ModeratorGuard]
})
export class AppRoutingModule { }
