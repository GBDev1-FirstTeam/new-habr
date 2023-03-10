import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Commentary } from '../../models/Commentary';
import { LikeData } from '../../models/Like';
import { HttpRequestService } from '../../services/HttpRequestService';
import { LikeService } from '../../services/LikeService';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent {

  @Input() comment: Commentary;

  constructor(private router: Router, private http: HttpRequestService, private likeService: LikeService) { }

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);

  like = (likeData: LikeData) => this.likeService.like(likeData, this.comment.Id, this.http.likeComment.bind(this.http));
}
