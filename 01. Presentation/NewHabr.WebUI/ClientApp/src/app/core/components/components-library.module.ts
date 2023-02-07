import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostContainerComponent } from './post-container/post-container.component';
import { CommentComponent } from './comment/comment.component';
import { LikeComponent } from './like/like.component';



@NgModule({
  declarations: [
    PostContainerComponent,
    CommentComponent,
    LikeComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PostContainerComponent,
    CommentComponent,
    LikeComponent
  ]
})
export class ComponentsLibraryModule { }
