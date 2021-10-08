import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { PageChangeEvent, GridDataResult } from '@progress/kendo-angular-grid';
import { DriverService } from '../service/driver.service';
import { ExcelService } from '../../../common/service/excel.service';

@Component({
  selector: 'ngx-regift-driverlist',
  templateUrl: './regift-driverlist.component.html',
  styleUrls: ['./regift-driverlist.component.scss']
})
export class RegiftDriverlistComponent implements OnInit {

  @Output() messageEvent = new EventEmitter<string>();
  @Input() DriverId;
  listViewModel: any[] = [];
  regiftBadge : any = "";
  loading=false;

  public regiftGridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;

  constructor(public driverService: DriverService, private excelService: ExcelService) { }
  async ngOnInit() {
    this.loading = true;
  
    var response = await this.driverService.GetRegift(this.DriverId);
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
  this.messageEvent.emit(this.regiftBadge)
}

public pageChange(event: PageChangeEvent): void {
  this.skip = event.skip;
  this.loadItems();
}

private loadItems(): void { 
  this.regiftGridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
  };
  if(this.listViewModel){
    this.regiftBadge = this.listViewModel.length;
  }
 else 
 this.regiftBadge ="";
 this.badgeCount();
}

}
