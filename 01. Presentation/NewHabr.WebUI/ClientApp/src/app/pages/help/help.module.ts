import { ComponentsLibraryModule } from 'src/app/core/components/components-library.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HelpComponent } from './help.component';

const routes: Routes = [
  {
    path: '',
    component: HelpComponent
  }
];

@NgModule({
  declarations: [
    HelpComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ComponentsLibraryModule
  ]
})
export class HelpModule { }
