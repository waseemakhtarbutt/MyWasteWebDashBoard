import { Component, OnInit, Input, ViewChild, AfterViewInit, Output, EventEmitter  } from '@angular/core';
import { GpnRequestService } from '../../service/gpn-request.service';
import { SchoolComponent } from '../school/school/school.component';
import { AnyAaaaRecord } from 'dns';

@Component({
  selector: 'ngx-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {
  listViewModel: any[] = [];
  schoolBadge: number;
  businessBadge : any ;
  organizationBadge : any ;
  
  constructor(public gpnRequestService: GpnRequestService) { }

 async ngOnInit() { 
  
  }

schoolBadgeCount($event) {
  this.schoolBadge = $event
}
businessBadgeCount($event) {
  this.businessBadge = $event
}
organizationBadgeCount($event) {
  this.organizationBadge = $event
}
  // ngAfterViewInit () {
  //   this.schoolBadge = this.child.schoolBadge;
  // }

}
