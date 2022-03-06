import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { CustomerListComponent } from './components/customer/customer-list/customer-list.component';
import { CustomerManagerComponent } from './components/customer/customer-manager/customer-manager.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FilterService, GridAllModule, GridModule, GroupService, PageService, SortService } from '@syncfusion/ej2-angular-grids';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CustomerListComponent,
    CustomerManagerComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    GridModule,
    GridAllModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ])
  ],
  providers: [
    PageService,
    SortService,
    FilterService,
    GroupService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
