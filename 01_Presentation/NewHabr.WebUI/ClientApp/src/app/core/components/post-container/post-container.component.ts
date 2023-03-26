import { Component, ElementRef, Input, OnChanges, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Publication, PublicationUser } from 'src/app/core/models/Publication';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PostContainerComponent implements OnChanges {

  userId?: string;
  userName?: string;
  
  @Input() post: Publication | PublicationUser | null;

  @ViewChild('htmlContainer', { static: true }) viewMe: ElementRef;
  
  constructor(private router: Router) { }

  ngOnChanges(changes: SimpleChanges): void {
    const post = changes['post'].currentValue;

    if (post) {
      this.userId = (post as Publication)?.UserId;
      this.userName = (post as Publication)?.Username;
      this.viewMe.nativeElement.innerHTML = (post as Publication)?.Content;
    }
  }

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);

  categoryNavigate(id: number | undefined) {
    this.router.navigate(['search'], { queryParams: { Category: id }})
  }

  tagNavigate(id: number | undefined) {
    this.router.navigate(['search'], { queryParams: { Tag: id }})
  }
}
