import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DashboardComponent } from '@src/app/pages/dashboard/dashboard.component';
import { FormsComponent } from '@src/app/pages/forms/forms.component';
import { ListsComponent } from '@src/app/pages/lists/lists.component';
import { TablesComponent } from '@src/app/pages/tables/tables.component';
import { DefaultComponent } from './default.component';


const routes: Routes = [
  {
    path: '',
    component: DefaultComponent,
    children: [
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
        path: 'list',
        component: ListsComponent
      },
      {
        path: 'form',
        component: FormsComponent
      }
    ]
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class DefaultRoutingModule { }

