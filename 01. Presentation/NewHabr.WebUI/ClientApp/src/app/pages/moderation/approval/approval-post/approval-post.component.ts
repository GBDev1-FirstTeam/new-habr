import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';
import { Publication } from 'src/app/core/models/Publication';
import { ApproveState } from 'src/app/core/models/Structures';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';
import { AppStoreProvider } from 'src/app/core/store/store';

@Component({
  selector: 'app-approval-post',
  templateUrl: './approval-post.component.html',
  styleUrls: ['./approval-post.component.scss']
})
export class ApprovalPostComponent implements OnInit, OnDestroy {

  subscribtions: Subscription[] = [];
  post: Publication;
  postId: string;

  succesfulApprove: boolean = false;
  succesfulRefuse: boolean = false;

  constructor(
    private http: HttpRequestService,
    private activeRoute: ActivatedRoute,
    private store: AppStoreProvider) { }


  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      this.postId = params.id;
      const postSubscribtion = this.http.getPublicationById(this.postId).subscribe(p => {
        if (p) {
          this.post = p;
        }
      })

      this.subscribtions.push(postSubscribtion);
    })
  }

  ngOnDestroy(): void {
    this.subscribtions.forEach(element => element.unsubscribe());
  }

  approve = (post: Publication) => {
    lastValueFrom(this.http.approvePublication(this.postId, ApproveState.Approved)).then(() => {
      this.succesfulApprove = true;
    })
  }

  refuse = (post: Publication) => {
    lastValueFrom(this.http.approvePublication(this.postId, ApproveState.NotApproved)).then(() => {
      this.succesfulRefuse = true;
    })
  }
}
