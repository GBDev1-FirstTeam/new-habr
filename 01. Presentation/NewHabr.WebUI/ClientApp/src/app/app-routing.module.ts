import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";

const defaultRoutePath = "home";
const rootRoutes: Routes = [
    { path: "home", component: HomeComponent },

    { path: "**", redirectTo: defaultRoutePath }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            rootRoutes,
            { enableTracing: true, relativeLinkResolution: 'legacy', scrollPositionRestoration: 'enabled' }
        )
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule {
    getDefaultRoutePath() {
        return defaultRoutePath;
    }
}
