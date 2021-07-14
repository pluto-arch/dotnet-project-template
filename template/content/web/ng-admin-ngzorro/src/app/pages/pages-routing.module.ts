import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { DashboardComponent } from '@src/app/pages/dashboard/dashboard.component';
import { FormsComponent } from '@src/app/pages/forms/forms.component';
import { TablesComponent } from '@src/app/pages/tables/tables.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: DashboardComponent
  },
  {
    path: 'table',
    component: TablesComponent
  },
  {
    path: 'form',
    component: FormsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }