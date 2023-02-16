import { Component, Input, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Publication } from 'src/app/core/models/Publication';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PostContainerComponent {

  @Input() post: Publication | null;
  
  constructor(private router: Router, private sanitizer: DomSanitizer) { }

getInnerHtml(){
  return this.sanitizer.bypassSecurityTrustHtml(this.post?.Content as any);
}

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);
}
