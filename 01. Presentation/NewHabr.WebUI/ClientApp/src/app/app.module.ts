import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import '../app/core/extensions/prototypes';
import { ConfigurationService } from './core/services/ConfigurationService';
import { tap } from 'rxjs';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        AppRoutingModule
    ],
    providers: [
        {
            provide: APP_INITIALIZER,
            multi: true,
            deps: [ConfigurationService],
            useFactory: (config: ConfigurationService) => {
                return () => {
                    return config.getConfigAsync().pipe(
                        tap(cfg => config.configuration = cfg)
                    );
                };
            }
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
