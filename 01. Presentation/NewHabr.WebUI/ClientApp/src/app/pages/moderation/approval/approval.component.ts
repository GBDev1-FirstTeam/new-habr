import { Component, OnInit } from '@angular/core';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-approval',
  templateUrl: './approval.component.html',
  styleUrls: ['./approval.component.scss']
})
export class ApprovalComponent implements OnInit {

  publications: Array<Publication>;

  constructor(private http: HttpRequestService) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getUnpublishedPublications(1, 10).subscribe(p => {
      if (p) {
        this.publications = p?.Articles;
        publicationsSubscribtion.unsubscribe();
      }
    })
  }
}
