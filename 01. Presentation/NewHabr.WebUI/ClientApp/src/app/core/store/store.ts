import { Publication } from "../models/Publication";
import { createStore, withProps, Store, StoreDef, select } from '@ngneat/elf';
import { Injectable } from "@angular/core";
import { HttpRequestService } from "../services/HttpRequestService";

export interface AppStore {
    publications: Array<Publication> | null
}

@Injectable({
    providedIn: 'root',
})
export class AppStoreProvider {

    private store: Store<StoreDef<AppStore>>;

    constructor(private http: HttpRequestService) {
        this.store = createStore(
            { name: 'appStore' },

            withProps<AppStore>({
                publications: null
            })
        );
    }

    loadPublications() {
        if (this.store.getValue().publications == null) {
            const publicationsSubscribtion = this.http.getPublications().subscribe(publications => {
                if (publications) {
                    this.updatePublications(publications);
                    publicationsSubscribtion.unsubscribe();
                }
            });
        }
    }

    getPublicationsFromStore = () => this.store.pipe(select((state => state.publications)));

    private updatePublications(publications: Array<Publication>) {
        this.store.update(st => ({
            ...st,
            publications: publications
        }))
    }
}
