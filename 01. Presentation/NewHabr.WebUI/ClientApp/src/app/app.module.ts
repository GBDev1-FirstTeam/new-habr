import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import '../app/core/extensions/prototypes';
import { ConfigurationService } from './core/services/ConfigurationService';
import { tap } from 'rxjs';
import { MainModule } from './pages/main/main.module';
import { PublicationsModule } from './pages/publications/publications.module';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        MainModule,
        PublicationsModule
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
