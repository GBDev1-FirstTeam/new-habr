import { Component, ElementRef, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { Publication, PublicationUser } from 'src/app/core/models/Publication';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PostContainerComponent implements OnInit {

  @Input() post: Publication | PublicationUser | null;
  userId?: string;
  userName?: string;

  @ViewChild('htmlContainer', { static: true }) viewMe: ElementRef;
  
  constructor(private router: Router) { }

  ngOnInit() {
    this.userId = (this.post as Publication)?.UserId;
    this.userName = (this.post as Publication)?.Username;
    this.viewMe.nativeElement.innerHTML = (this.post as Publication)?.Content
  }

  navigate = (id: string | undefined) => this.router.navigate(['users', id]);
}
