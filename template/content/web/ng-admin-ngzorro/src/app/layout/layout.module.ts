import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule } from './layout-routing.module';



import { IconsProviderModule } from '../icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzBadgeModule } from 'ng-zorro-antd/badge';

import { HeaderComponent } from './components/header/header.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { FooterComponent } from './components/footer/footer.component';

import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { HeaderActionComponent } from './components/header-action/header-action.component';

const modules = [
  NzLayoutModule,
  NzBreadCrumbModule,
  NzMenuModule,
  NzMessageModule,
  NzDropDownModule,
  IconsProviderModule,
  NzBadgeModule
]


@NgModule({
  declarations: [
    LayoutComponent,
    HeaderComponent,
    SidebarComponent,
    FooterComponent,
    BreadcrumbComponent,
    HeaderActionComponent,
  ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    ...modules
  ],
  exports: [
    LayoutRoutingModule,
    IconsProviderModule
  ]
})
export class LayoutModule { }
