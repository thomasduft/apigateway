import { HomeModule } from './home/home.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { SidebarModule } from './sidebar/sidebar.module';
import { WorkspaceModule } from './workspace/workspace.module';

import { HomeComponent } from './home/home.component';

import { CoreModule } from './core/core.module';

import { CatalogsModule } from './catalogs/catalogs.module';
import { OrdersModule } from './orders/orders.module';

const ROUTES: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', component: HomeComponent }
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(ROUTES),
    CoreModule,
    HomeModule,
    CatalogsModule,
    OrdersModule,
    SidebarModule,
    WorkspaceModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
