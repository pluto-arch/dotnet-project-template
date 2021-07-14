import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';



@NgModule({
  imports: [RouterModule.forChild([
    {
      path: 'login',
      component: LoginComponent
    },
    {
      path: 'register',
      component: RegisterComponent
    },
    {
      path: 'reset-password',
      component: ResetPasswordComponent
    }
  ])],
  exports: [RouterModule]
})
export class AuthRoutingModule { }