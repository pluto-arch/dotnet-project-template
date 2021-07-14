import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { DefaultModule } from "@src/app/layout/default/default.module";
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressRouterModule } from 'ngx-progressbar/router';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    DefaultModule,
    NgProgressModule,
    NgProgressRouterModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
