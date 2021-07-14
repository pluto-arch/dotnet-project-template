import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'


const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./layout/default/default.module').then(m => m.DefaultModule)
  }
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }

