import { Component } from '@angular/core';
import { ThemeService } from './theme.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'ng-admin-ngzorro';

  constructor(private themeService: ThemeService) { }
  toggleTheme(): void {
    this.themeService.toggleTheme().then();
  }
}
