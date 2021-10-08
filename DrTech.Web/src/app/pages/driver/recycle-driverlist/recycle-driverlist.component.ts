import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { DriverService } from '../service/driver.service';
import { ExcelService } from '../../../common/service/excel.service';

@Component({
  selector: 'ngx-recycle-driverlist',
  templateUrl: './recycle-driverlist.component.html',
  styleUrls: ['./recycle-driverlist.component.scss']
})
export class RecycleDriverlistComponent implements OnInit {
  @Output() messageEvent = new EventEmitter<string>();
  @Input() DriverId;
  listViewModel: any[] = [];
  recycleBadge : any = "";
  loading=false;

  public recycleGridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;

  constructor(public driverService: DriverService, private excelService: ExcelService) { }
  async ngOnInit() {
    this.loading = true;
  
    var response = await this.driverService.GetRecycle(this.DriverId);
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
  this.messageEvent.emit(this.recycleBadge)
}

public pageChange(event: PageChangeEvent): void {
  this.skip = event.skip;
  this.loadItems();
}

private loadItems(): void { 
  this.recycleGridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
  };
  if(this.listViewModel){
    this.recycleBadge = this.listViewModel.length;
  }
 else 
 this.recycleBadge ="";
 this.badgeCount();
}


}
