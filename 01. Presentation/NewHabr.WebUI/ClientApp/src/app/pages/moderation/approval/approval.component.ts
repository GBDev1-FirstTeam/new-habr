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
  showPublications: Array<Publication>;
  selection: string;

  constructor(private http: HttpRequestService) { }

  ngOnInit(): void {
    const publicationsSubscribtion = this.http.getUnpublishedPublications(1, 10).subscribe(p => {
      if (p) {
        this.publications = p?.Articles;
        this.showPublications = this.publications;
        publicationsSubscribtion.unsubscribe();
      }
    })
  }

  changeSelection(event: any) {
    switch (event.target.value) {
      case '1':
        this.showPublications = this.publications; break;
      case '2':
        this.showPublications = this.publications?.filter(x => x.ApproveState === 'WaitApproval'); break;
      case '3':
        this.showPublications = this.publications?.filter(x => x.ApproveState === 'NotApproved'); break;
      case '4':
        this.showPublications = this.publications?.filter(x => x.ApproveState === 'Approved'); break;
    }
  }
}
