import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { DriverService } from '../service/driver.service';
import { ExcelService } from '../../../common/service/excel.service';

@Component({
  selector: 'ngx-delivered-driverlist',
  templateUrl: './delivered-driverlist.component.html',
  styleUrls: ['./delivered-driverlist.component.scss']
})
export class DeliveredDriverlistComponent implements OnInit {
  @Output() messageEvent = new EventEmitter<string>();
  @Input() DriverId;
  listViewModel: any[] = [];
  deliveredBadge : any = "";
  loading=false;

  public deliveredGridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;

  constructor(public driverService: DriverService,private excelService: ExcelService) { }
  async ngOnInit() {
    this.loading = true;
  
    var response = await this.driverService.GetDelivered(this.DriverId);
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
  this.messageEvent.emit(this.deliveredBadge)
}

public pageChange(event: PageChangeEvent): void {
  this.skip = event.skip;
  this.loadItems();
}

private loadItems(): void { 
  this.deliveredGridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
  };
  if(this.listViewModel){
    this.deliveredBadge = this.listViewModel.length;
  }
 else 
 this.deliveredBadge ="";
 this.badgeCount();
}

}
