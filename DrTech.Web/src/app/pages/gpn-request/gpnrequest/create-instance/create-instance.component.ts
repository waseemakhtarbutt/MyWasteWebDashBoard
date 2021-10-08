import { Component, OnInit, Input, ViewChild, AfterViewInit, Output, EventEmitter  } from '@angular/core';
import { GpnRequestService } from '../../service/gpn-request.service';
@Component({
  selector: 'ngx-create-instance',
  templateUrl: './create-instance.component.html',
  styleUrls: ['./create-instance.component.scss']
})
export class CreateInstanceComponent implements OnInit {
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
