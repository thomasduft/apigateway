import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { SidebarModule } from './sidebar/sidebar.module';
import { WorkspaceModule } from './workspace/workspace.module';

import { HomeComponent } from './home/home.component';

const ROUTES: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'catalogs', loadChildren: () => import('./catalogs/catalogs.module').then(m => m.CatalogsModule) },
  { path: 'orders', loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule) },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', component: HomeComponent }
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(ROUTES),
    SidebarModule,
    WorkspaceModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
