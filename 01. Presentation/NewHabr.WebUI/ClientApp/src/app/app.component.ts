import { Component } from '@angular/core';
import { HttpRequestService } from './core/services/HttpRequestService';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent {
    title = 'app';

    constructor(private http: HttpRequestService) { }

    ngAfterViewInit(): void {
        const lol = this.http.getPublications().subscribe(publications => {
            if (publications) {
                console.log(publications);

                lol.unsubscribe();
            }
        });
    }
}
