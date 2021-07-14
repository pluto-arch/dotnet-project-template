import { Component, OnInit } from '@angular/core';
import { ThemeService } from '@src/app/theme.service';

@Component({
  selector: 'app-header-action',
  templateUrl: './header-action.component.html',
  styleUrls: ['./header-action.component.less']
})
export class HeaderActionComponent implements OnInit {

  skinTheme: any = "outline";

  constructor(private _themeService: ThemeService) { }

  ngOnInit(): void {
  }

  triggleTheme() {
    this._themeService.toggleTheme().then();
  }
}
