import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzTabsModule } from 'ng-zorro-antd/tabs';

import { AuthRoutingModule } from './auth-routing.module';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { AccountComponent } from './components/account/account.component';
import { MobileComponent } from './components/mobile/mobile.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    ResetPasswordComponent,
    AccountComponent,
    MobileComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzCardModule,
    NzDividerModule,
    NzFormModule,
    NzInputModule,
    NzCheckboxModule,
    NzTabsModule,
    AuthRoutingModule
  ],
  exports: [
    AuthRoutingModule
  ]
})
export class AuthModule { }
