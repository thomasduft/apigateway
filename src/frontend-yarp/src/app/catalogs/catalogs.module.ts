import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';

import { CatalogsComponent } from './catalogs.component';
import { CoreModule } from '../core/core.module';

const routes: Routes = [
  { path: 'catalogs', component: CatalogsComponent }
];

@NgModule({
  declarations: [
    CatalogsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    CoreModule
  ]
})
export class CatalogsModule { }
