import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostContainerComponent } from './post-container/post-container.component';



@NgModule({
  declarations: [
    PostContainerComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    PostContainerComponent
  ]
})
export class ComponentsLibraryModule { }
