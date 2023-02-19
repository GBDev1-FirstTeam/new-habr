import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConvertDatePipe } from './convert-date.pipe';



@NgModule({
  declarations: [
    ConvertDatePipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ConvertDatePipe
  ]
})
export class PipesModule { }
