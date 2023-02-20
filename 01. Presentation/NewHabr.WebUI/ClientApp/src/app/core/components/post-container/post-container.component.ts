import { Component, Input, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Publication, PublicationUser } from 'src/app/core/models/Publication';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PostContainerComponent {

  @Input() post: Publication | PublicationUser | null;
  userId?: string;
  userName?: string;
  
  constructor(private router: Router, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.userId = (this.post as Publication)?.UserId;
    this.userName = (this.post as Publication)?.Username;
  }

  getInnerHtml() {
    return this.sanitizer.bypassSecurityTrustHtml(this.post?.Content as any);
  }

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);
}
