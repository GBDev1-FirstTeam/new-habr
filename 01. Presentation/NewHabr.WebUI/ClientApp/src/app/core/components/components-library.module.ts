import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostContainerComponent } from './post-container/post-container.component';
import { CommentComponent } from './comment/comment.component';



@NgModule({
  declarations: [
    PostContainerComponent,
    CommentComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PostContainerComponent,
    CommentComponent
  ]
})
export class ComponentsLibraryModule { }
