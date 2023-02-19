import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.scss']
})
export class PublicationsComponent implements OnInit {

  publications: Array<Publication>;

  constructor(private http: HttpRequestService, private router: Router) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getPublications(1, 10).subscribe(p => {
      if (p) {
        this.publications = p.Articles;
        publicationsSubscribtion.unsubscribe();
      }
    })
  }

  navigate = (id: string | undefined) => this.router.navigate(['publications', id]);
}
