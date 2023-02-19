import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostContainerComponent } from './post-container/post-container.component';
import { CommentComponent } from './comment/comment.component';
import { LikeComponent } from './like/like.component';
import { SuccesfulSendComponent } from './succesful-send/succesful-send.component';



@NgModule({
  declarations: [
    PostContainerComponent,
    CommentComponent,
    LikeComponent,
    SuccesfulSendComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PostContainerComponent,
    CommentComponent,
    LikeComponent,
    SuccesfulSendComponent
  ]
})
export class ComponentsLibraryModule { }
