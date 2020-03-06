import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { CatalogsComponent } from './catalogs.component';


const routes: Routes = [
  { path: '', component: CatalogsComponent }
];

@NgModule({
  declarations: [CatalogsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class CatalogsModule { }
