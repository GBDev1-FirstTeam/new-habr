import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Publication } from 'src/app/core/models/Publication';
import { HttpRequestService } from 'src/app/core/services/HttpRequestService';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {

  post: Publication;

  constructor(
    private http: HttpRequestService,
    private router: Router,
    private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe(params => {
      const postSubscribtion = this.http.getPostById(params.id).subscribe(post => {
        if (post) {
          this.post = post;
          postSubscribtion.unsubscribe();
        }
      });
    })
  }
}
