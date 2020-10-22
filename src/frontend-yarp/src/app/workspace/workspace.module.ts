import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { WorkspaceComponent } from './workspace.component';

@NgModule({
  declarations: [
    WorkspaceComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
   exports: [
     WorkspaceComponent
   ]
})
export class WorkspaceModule { }
