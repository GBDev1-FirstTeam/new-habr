import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationsComponent } from './publications.component';
import { RouterModule, Routes } from '@angular/router';
import { PostComponent } from './post/post.component';
import { ComponentsLibraryModule } from 'src/app/core/components/components-library.module';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path: '',
    component: PublicationsComponent
  },
  {
    path: ':id',
    component: PostComponent
  }
];

@NgModule({
  declarations: [
    PublicationsComponent,
    PostComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ComponentsLibraryModule,
    FormsModule
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PublicationsModule { }
