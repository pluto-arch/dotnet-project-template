import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { DefaultRoutingModule } from "./default-routing.module";

import { DefaultComponent } from './default.component';
import { HeaderComponent } from '../components/header/header.component';
import { SideComponent } from '../components/side/side.component';
import { FooterComponent } from '../components/footer/footer.component';
import { DashboardModule } from '@src/app/pages/dashboard/dashboard.module';

import { MatSidenavModule } from '@angular/material/sidenav';
import { MatMenuModule } from '@angular/material/menu'
import { MatSlideToggleModule } from '@angular/material/slide-toggle'
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar'

@NgModule({
  declarations: [
    DefaultComponent,
    HeaderComponent,
    SideComponent,
    FooterComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatIconModule,
    MatSidenavModule,
    MatDividerModule,
    MatToolbarModule,
    MatListModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatButtonModule,
    FlexLayoutModule,
    DashboardModule,
    DefaultRoutingModule
  ],
  exports: [
    DefaultRoutingModule
  ],
})
export class DefaultModule { }
