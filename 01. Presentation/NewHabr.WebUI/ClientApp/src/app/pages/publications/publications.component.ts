import { Component, OnInit } from '@angular/core';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.scss']
})
export class PublicationsComponent implements OnInit {

  publications: Array<Publication>;

  constructor(private http: HttpRequestService) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getPublications().subscribe(publications => {
      if (publications) {
        this.publications = publications;
        publicationsSubscribtion.unsubscribe();
      }
    });
  }

  getTime(utc: number): string {
    return new Date(utc).toLocaleString();
  }
}
