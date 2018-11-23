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
    ProductIntervalComponent
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
    NgMultiSelectDropDownModule.forRoot()
  ],
  entryComponents: [ ProductIntervalComponent ],
  providers: [ { provide: NZ_I18N, useValue: en_US } ],
  bootstrap: [ AppComponent ]
})
export class AppModule {}
