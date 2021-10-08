import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { DriverService } from '../service/driver.service';
import { ExcelService } from '../../../common/service/excel.service';


@Component({
  selector: 'ngx-alljobs',
  templateUrl: './alljobs.component.html',
  styleUrls: ['./alljobs.component.scss']
})
export class AlljobsComponent implements OnInit {
  @Output() messageEvent = new EventEmitter<string>();
  @Input() DriverId;
  listViewModel: any[] = [];
  jobBadge : any = "";
  loading=false;
  mindate = new Date();
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public min: Date;
  public max: Date;
  constructor(public driverService: DriverService,private excelService: ExcelService) { }
  async ngOnInit() {
    this.loading = true;
  
    var response = await this.driverService.GetAllJobs(this.DriverId);
    if (response.statusCode == 0) {  
      this.listViewModel = response.data;
      this.loadItems();      
  }
  this.loading = false
}
exportAsXLSX(): void {
  debugger
  this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
}
badgeCount() {
  this.messageEvent.emit(this.jobBadge)
}

public pageChange(event: PageChangeEvent): void {
  this.skip = event.skip;
  this.loadItems();
}

private loadItems(): void { 
  this.gridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
  };
  if(this.listViewModel){
    this.jobBadge = this.listViewModel.length;
  }
 else 
 this.jobBadge ="";
 this.badgeCount();
}


}
