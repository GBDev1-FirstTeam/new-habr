import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Publication } from 'src/app/core/models/Publication';


@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss']
})
export class PostContainerComponent {

  @Input() post: Publication | null;

  constructor(private router: Router) { }

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);
}
