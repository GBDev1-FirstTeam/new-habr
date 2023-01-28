import { Component, Input } from '@angular/core';
import { Publication } from 'src/app/core/models/Publication';


@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss']
})
export class PostContainerComponent {

  @Input() post: Publication | null;

  getTime = (utc: number | undefined): string => new Date(utc || 0).toLocaleString();
}
