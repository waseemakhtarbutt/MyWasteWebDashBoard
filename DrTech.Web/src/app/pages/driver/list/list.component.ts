import { Component, OnInit, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import {DriverService} from '../service/driver.service'
import { Router } from '@angular/router';
import { CompositeFilterDescriptor, filterBy, compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { ExcelService } from '../../../common/service/excel.service';
import { DriverRequestDto } from '../dto/driver.dto';

@Component({
  selector: 'ngx-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {
  listViewModel: any[] = [];
  driverBadge : any ;
  loading=false;
  public driverId = 0;
  public driverName:string = ""

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  IsSuspendable :boolean = false;
  public SuspensionMessage :string ="Do you realy want to suspend this driver?"
  public sort: SortDescriptor[] = [{
    field: 'fullName',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  public driverRequestDto = new DriverRequestDto();
  public range = { start: null, end: null };
  constructor(public driverService: DriverService, private excelService: ExcelService,private router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.LoadData();
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.LoadData();
  }

  LoadData() {
    this.loading = true;

    this.driverService.GetDrivers(this.driverRequestDto).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;

        this.loadItems();
      }
    });

    this.loading = false
  }
  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
}
  private loadItems(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
       data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
        total: this.listViewModel.length
    };

    if(this.listViewModel){
      this.driverBadge=  this.listViewModel.length;
    }
   else
   this.driverBadge = '';
  }
  exportAsXLSX(): void {
    debugger
    this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
  }
  onComplaintAction(dialog: TemplateRef<any>, event: any, id: any) {
    this.driverName = this.listViewModel.find(x=>x.id==id).fullName;
    this.deleteDriver(id)
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

      this.driverId = id;
  }

  async deleteDriver(id)
  {
    var response = await this.driverService.CheckDriverAssignments(id);
    if(response.statusCode == 0){
      console.log(response.data)


      if(response.data == false)
      {
      this.IsSuspendable = false;
       this.SuspensionMessage = "Unassign all tasks before deactivating the driver!";
      }
      else{
          this.IsSuspendable = true;
          this.SuspensionMessage = "Do you realy want to suspend this driver?";
      }
    }
    // var delResponse = await this.gpnRequestService.InactiveStgSchool(id);
    // if(delResponse.statusCode == 0)
    // {
    //   var response = await this.gpnRequestService.GetStgSchoolList();
    //   if (response.statusCode == 0) {
    //     this.listViewModel = response.data;
    //     this.loadItems();
    //   }
    // }

  }
  async SuspendDriver(dialog: NbDialogRef<any>) {

    var response = await this.driverService.SuspendDriver(this.driverId);

    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();
    }
  }

  public onSelect(e) {
    this.router.navigate(["/pages/driver/tasklist/" + this.gridView.data[e.index % this.pageSize].id])
  }
  searchGrid(search) {

    const predicate = compileFilter(
      { logic: "or",
       filters: [
         { field: "fullName", operator: "contains", value: search },
         { field: "phone", operator: "contains", value: search},
         { field: "vehicleName", operator: "contains", value: search},
         { field: "regNumber", operator: "contains", value: search},
        ]});

        if(search)
        {
          this.gridView = {
            data: this.listViewModel.filter(predicate),
             total: this.listViewModel.length
         };
        }
        else
        {
          this.gridView = {
            data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
             total: this.listViewModel.length
         };
        }
 }

 filterDateRange():void{
  debugger;
  if(this.range.start != null && this.range.end != null)
  {
    this.driverRequestDto.startDate = this.range.start.toLocaleDateString();;
    this.driverRequestDto.endDate = this.range.end.toLocaleDateString();;
    this.skip = 0;
    this.LoadData();
  }
}
clearDateRange():void{
  this.range.start = null;
  this.range.end = null;
  this.driverRequestDto.startDate = null;
  this.driverRequestDto.endDate = null;
  this.skip = 0;
  this.LoadData();
}
}
