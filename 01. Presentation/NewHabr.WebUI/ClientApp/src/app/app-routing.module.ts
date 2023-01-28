import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const rootRoutes: Routes = [
    {
        path: 'publications',
        loadChildren: () => import('./pages/publications/publications.module').then(m => m.PublicationsModule)
    },
    {
        path: 'users',
        loadChildren: () => import('./pages/users/users.module').then(m => m.UsersModule)
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
    ]
})
export class AppRoutingModule { }
