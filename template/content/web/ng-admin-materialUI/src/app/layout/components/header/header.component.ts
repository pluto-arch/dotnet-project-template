import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DOCUMENT } from '@angular/common'
import { Inject } from '@angular/core'
import { SideService } from '../side/side.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Output() onTriggleSideBar: EventEmitter<any> = new EventEmitter<any>()
  currentTheme: string = "styles";
  isDark: boolean = false;
  themelist = [
    "styles",
    "indigo-pink",
    "deeppurple-amber",
    "blue-yellow",
    "pink-bluegrey",
    "purple-green",
  ]
  constructor(@Inject(DOCUMENT) private document: Document, private _sidebarServce: SideService) { }

  ngOnInit(): void {

  }

  changeTheme(theme: string): void {
    this.currentTheme = theme;
    const id = 'lazy-load-theme';
    const link = this.document.getElementById(id);
    let themeCss = `${theme}.css`;
    if (this.isDark) {
      themeCss = `${theme}-dark.css`;
    }
    if (!link) {
      const linkEl = document.createElement('link');
      linkEl.setAttribute('rel', 'stylesheet');
      linkEl.setAttribute('type', 'text/css');
      linkEl.setAttribute('id', id);
      linkEl.setAttribute('href', themeCss);
      document.head.appendChild(linkEl);
    } else {
      // 替换 link 元素的 href 地址 
      (link as HTMLLinkElement).href = themeCss;
    }
  }

  switchDark() {
    this.changeTheme(this.currentTheme);
  }

  triggleSideBar() {
    this._sidebarServce.toggle()
    // setTimeout(() => {
    //   window.dispatchEvent(new Event('resize'))
    // }, 300);
  }

}
