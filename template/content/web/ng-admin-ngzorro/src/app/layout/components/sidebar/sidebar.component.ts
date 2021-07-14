import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less']
})
export class SidebarComponent implements OnInit {
  navMenu = menus;
  constructor() { }

  ngOnInit(): void {
  }

}

const menus = [
  {
    title: '看板',
    icon: 'dashboard',
    level: 1,
    routerLink: "/dashboard",
  },
  {
    title: '组件',
    icon: 'setting',
    level: 1,
    routerLink: "",
    children: [
      {
        title: '表格',
        icon: 'table',
        level: 2,
        routerLink: "/table",
      },
      {
        title: '表单',
        icon: 'form',
        level: 2,
        routerLink: "/form",
      }
    ]
  }
]



