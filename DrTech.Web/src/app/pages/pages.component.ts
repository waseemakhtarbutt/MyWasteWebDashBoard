import { Component } from '@angular/core';

import { MENU_ITEMS_ADMIN, MENU_ITEMS_SCHOOL, MENU_ITEMS_BUSINESS, MENU_ITEMS_NGO, MENU_ITEMS_WASTE, MENU_ITEMS_BUSINESS_GOI, MENU_ITEMS_DumpRecycle_Staff,MENU_ITEMS_RecycleList_Staff } from './pages-menu';
import { NbTokenService } from '../common/auth';
import { isInteger } from '@ng-bootstrap/ng-bootstrap/util/util';

@Component({
  selector: 'ngx-pages',
  template: `
    <ngx-sample-layout>
      <nb-menu [items]="menu" style="background-color:#005169; color:white;"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-sample-layout>
  `,styleUrls:['./menu.scss']

})
export class PagesComponent {
  menu: any = [];
  type: any;

  constructor(private tokenService: NbTokenService) {

    this.type = localStorage.getItem('type');
    //this.menu = MENU_ITEMS_ADMIN;
     let role = tokenService.getRole();

    console.log(role)
    if (role === 1)
      this.menu = MENU_ITEMS_ADMIN;
    if (role === 2 || role === 7)
      this.menu = MENU_ITEMS_SCHOOL;
    if (role === 3 || role === 8)
      this.menu = MENU_ITEMS_NGO;
    if ((role === 4 || role === 9) && this.type != "G")
      this.menu = MENU_ITEMS_BUSINESS;
    if (role === 10)
    this.menu = MENU_ITEMS_WASTE;
    if ((role === 4 || role === 9) && this.type === "G"){
      this.menu = MENU_ITEMS_BUSINESS_GOI;
    }
    if (role === 1 && this.type ==="S")
      this.menu = MENU_ITEMS_DumpRecycle_Staff;
      if ((role === 11 ) && this.type === "G"){
        this.menu = MENU_ITEMS_RecycleList_Staff;
      }

 

  }

}
