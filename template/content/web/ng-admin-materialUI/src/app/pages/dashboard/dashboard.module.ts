import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { CardComponent } from '@src/app/pages/dashboard/components/card/card.component';
import { ChartComponent } from '@src/app/pages/dashboard/components/chart/chart.component';
import { DashboardComponent } from '@src/app/pages/dashboard/dashboard.component';

import { MatDividerModule } from '@angular/material/divider'
import { MatIconModule } from '@angular/material/icon'
import { MatButtonModule } from '@angular/material/button'
import { FlexLayoutModule } from '@angular/flex-layout'
import { MatListModule } from '@angular/material/list'
import { MatCardModule } from '@angular/material/card'

import { NgxEchartsModule } from 'ngx-echarts';


@NgModule({
  declarations: [
    CardComponent,
    ChartComponent,
    DashboardComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatDividerModule,
    MatIconModule,
    MatButtonModule,
    FlexLayoutModule,
    MatListModule,
    MatCardModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
    RouterModule
  ]
})
export class DashboardModule { }
