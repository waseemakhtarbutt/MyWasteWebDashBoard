import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataResult } from '@progress/kendo-angular-grid';

@Component({
  selector: 'ngx-adminlist',
  templateUrl: './adminlist.component.html',
  styleUrls: ['./adminlist.component.scss']
})
export class AdminListComponent implements OnInit {
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
