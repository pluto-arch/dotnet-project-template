import { Component, OnInit } from '@angular/core';
import { SideService } from '../components/side/side.service';

@Component({
  selector: 'app-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss']
})
export class DefaultComponent implements OnInit {

  sideBarOpene: boolean = true;

  constructor(private _sideBarService: SideService) { }

  ngOnInit(): void {
    this._sideBarService.change.subscribe(isOpen => {
      this.sideBarOpene = isOpen;
    });
  }

}
