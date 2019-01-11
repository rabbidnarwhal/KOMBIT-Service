import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { LayoutComponent } from './components/layout/layout.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { LayoutPublicComponent } from './components/layout-public/layout-public.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ChartjsModule } from '@ctrl/ngx-chartjs';
import { ProductPostComponent } from './components/product-post/product-post.component';
import { NumbersOnly } from './directives/number-only.directive';
import { QuillModule } from 'ngx-quill';
import { ProductListTableComponent } from './components/product-list-table/product-list-table.component';
import { ProductIntervalComponent } from './components/product-interval/product-interval.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { MainMenuComponent } from './components/main-menu/main-menu.component';
import { MainSubmenuComponent } from './components/main-submenu/main-submenu.component';
import { ProductFilterComponent } from './components/product-filter/product-filter.component';
import { ProductMineComponent } from './components/product-mine/product-mine.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HeaderModalComponent } from './components/header-modal/header-modal.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { DashboardTableViewComponent } from './components/dashboard-table-view/dashboard-table-view.component';
import { ScheduleDetailComponent } from './components/schedule-detail/schedule-detail.component';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    LoginComponent,
    LayoutComponent,
    LayoutPublicComponent,
    ProductListComponent,
    ProductPostComponent,
    NumbersOnly,
    ProductListTableComponent,
    ProductIntervalComponent,
    ProductDetailComponent,
    MainMenuComponent,
    MainSubmenuComponent,
    ProductFilterComponent,
    ProductMineComponent,
    ScheduleComponent,
    ProfileComponent,
    HeaderModalComponent,
    DashboardTableViewComponent,
    ScheduleDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    NgZorroAntdModule,
    ReactiveFormsModule,
    ChartjsModule,
    QuillModule,
    NgMultiSelectDropDownModule.forRoot(),
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    })
  ],
  entryComponents: [ ProductIntervalComponent ],
  providers: [ { provide: NZ_I18N, useValue: en_US } ],
  bootstrap: [ AppComponent ]
})
export class AppModule {}
