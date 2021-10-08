import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataResult } from '@progress/kendo-angular-grid';

@Component({
  selector: 'ngx-admin-suspended-list',
  templateUrl: './admin-suspended-list.component.html',
  styleUrls: ['./admin-suspended-list.component.scss']
})


export class AdminSuspendedListComponent implements OnInit {
  loading=false;

  listViewModel: any[] = [];
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public DriverId : any;
  constructor(private route: ActivatedRoute, private router: Router) {
}

  async ngOnInit() {    
  }
}

