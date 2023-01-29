import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.scss']
})
export class PublicationsComponent implements OnInit {

  publications$: Observable<Array<Publication> | null>;

  constructor(private store: AppStoreProvider, private router: Router) { }

  ngOnInit(): void {
    this.store.loadPublications();
    this.publications$ = this.store.getPublicationsFromStore();
  }

  getTime = (utc: number): string => new Date(utc).toLocaleString();

  navigate = (id: string | undefined) => this.router.navigate(['publications', id]);
}
