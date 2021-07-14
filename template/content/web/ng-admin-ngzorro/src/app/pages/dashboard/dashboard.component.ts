import { Component, OnInit } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less']
})
export class DashboardComponent implements OnInit {

  constructor(private _nzMessageService: NzMessageService) { }

  ngOnInit(): void {
  }

  showMessage(): void {
    this._nzMessageService.error("error")
    this._nzMessageService.info("info")
    this._nzMessageService.success("success")
    this._nzMessageService.warning("warning")
  }

}
