import { NgModule } from '@angular/core';
import { PagesRoutingModule } from './pages-routing.module'

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';


import { DashboardComponent } from '../pages/dashboard/dashboard.component';
import { TablesComponent } from '../pages/tables/tables.component';
import { FormsComponent } from '../pages/forms/forms.component';


const nzModules = [
  NzButtonModule,
  NzCardModule,
  NzGridModule
]


@NgModule({
  declarations: [
    DashboardComponent,
    FormsComponent,
    TablesComponent
  ],
  imports: [
    PagesRoutingModule,
    ...nzModules],
  exports: [PagesRoutingModule]
})
export class PagesModule { }