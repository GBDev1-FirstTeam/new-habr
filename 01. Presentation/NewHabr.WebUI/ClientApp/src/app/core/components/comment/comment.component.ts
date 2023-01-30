import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Commentary } from '../../models/Commentary';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent {

  @Input() comment: Commentary;

  constructor(private router: Router) { }

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);
}
