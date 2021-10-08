import { Component, Input, OnInit } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';

@Component({
  template: `
    <a [routerLink]="['/pages/user/detail/', rowData.userId]">{{rowData.userName}}</a>
  `,
})
export class UserDetailLinkComponent implements ViewCell, OnInit {
  @Input() value: string | number;   // This hold the cell value
  @Input() rowData: any;  // This holds the entire row object

  ngOnInit() {
    //console.log(this.rowData);
  }
}
