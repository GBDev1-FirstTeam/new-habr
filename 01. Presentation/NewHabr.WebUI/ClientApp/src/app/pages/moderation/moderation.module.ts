import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreationComponent } from './creation/creation.component';
import { RouterModule, Routes } from '@angular/router';
import { ModerationComponent } from './moderation.component';
import { ApprovalComponent } from './approval/approval.component';
import { RegulationComponent } from './regulation/regulation.component';
import { ComponentsLibraryModule } from 'src/app/core/components/components-library.module';
import { PipesModule } from 'src/app/core/pipes/pipes.module';
import { ApprovalPostComponent } from './approval/approval-post/approval-post.component';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path: '',
    component: ModerationComponent,
    children: [
      {
        path:'',
        redirectTo: 'creation',
        pathMatch: 'full' 
      },
      {
        path: 'creation',
        component: CreationComponent
      },
      {
        path: 'regulation',
        component: RegulationComponent
      },
      {
        path: 'approval',
        component: ApprovalComponent
      },
      {
        path: 'approval/:id',
        component: ApprovalPostComponent
      }
    ]
  }
];

@NgModule({
  declarations: [
    CreationComponent,
    ModerationComponent,
    ApprovalComponent,
    RegulationComponent,
    ApprovalPostComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ComponentsLibraryModule,
    PipesModule
  ]
})
export class ModerationModule { }
