import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchComponent } from './search.component';
import { RouterModule, Routes } from '@angular/router';
import { ComponentsLibraryModule } from 'src/app/core/components/components-library.module';

const routes: Routes = [
  {
    path: '',
    component: SearchComponent
  }
];

@NgModule({
  declarations: [
    SearchComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ComponentsLibraryModule
  ]
})
export class SearchModule { }
