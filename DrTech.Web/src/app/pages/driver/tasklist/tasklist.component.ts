import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {DriverService} from '../service/driver.service'
import { GridDataResult } from '@progress/kendo-angular-grid';
import { DriverDTO } from '../dto/driver.dto';

@Component({
  selector: 'ngx-tasklist',
  templateUrl: './tasklist.component.html',
  styleUrls: ['./tasklist.component.scss']
})
export class TasklistComponent implements OnInit {
  regiftBadge: any;
  jobBadge: any;
  recycleBadge: any;
  deliveredBadge: any;
  public _userViewModel: DriverDTO;
  driverBadge : any;
  loading=false;

  listViewModel: any[] = [];
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public DriverId : any;
  constructor(public driverService: DriverService, private route: ActivatedRoute, private router: Router) {
   this._userViewModel = new DriverDTO();

   }

  async ngOnInit() {    
    this.loading = true;
    var id = this.route.snapshot.paramMap.get("id");
    this.DriverId = id;
    var response = await this.driverService.GetDriverByID(id);
    if (response.statusCode == 0) {  
      this._userViewModel = response.data;
    } 
    
    this.loading = false
  }
  allJobsBadgeCount($event) {
    this.jobBadge = $event
  }

  regiftBadgeCount($event) {
    this.regiftBadge = $event
  }
  recycleBadgeCount($event) {
    this.recycleBadge = $event
  }
  deliveredBadgeCount($event) {
    this.deliveredBadge = $event
  }

}
