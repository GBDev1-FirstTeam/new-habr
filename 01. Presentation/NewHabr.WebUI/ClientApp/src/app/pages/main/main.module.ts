import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { AppRoutingModule } from 'src/app/app-routing.module';



@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    AppRoutingModule
  ],
  exports: [
    MainComponent
  ]
})
export class MainModule { }
