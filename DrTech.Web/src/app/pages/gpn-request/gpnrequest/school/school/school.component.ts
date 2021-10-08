import { Component, OnInit, EventEmitter, Output, TemplateRef } from '@angular/core';
import { GpnRequestService } from '../../../service/gpn-request.service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ExcelService } from '../../../../../common/service/excel.service';

@Component({
  selector: 'ngx-school',
  templateUrl: './school.component.html',
  styleUrls: ['./school.component.scss']
})
export class SchoolComponent implements OnInit {
  @Output() messageEvent = new EventEmitter<string>();
  listViewModel: any[] = [];
  schoolBadge : any ;
  loading=false;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;


  constructor(public gpnRequestService: GpnRequestService,private excelService: ExcelService,private dialogService: NbDialogService) { }
  async ngOnInit() {
    this.loading = true;

    var response = await this.gpnRequestService.GetStgSchoolList();
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();
  }

  this.loading = false
}

badgeCount() {

  if(this.listViewModel){
    this.schoolBadge = (this.listViewModel.length == 0 ? '' : this.listViewModel.length);
  }
 else
 this.schoolBadge = '';
  this.messageEvent.emit(this.schoolBadge)
}
exportAsXLSX(): void {
  debugger
  this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
}
public pageChange(event: PageChangeEvent): void {
  this.skip = event.skip;
  this.loadItems();
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
 this.badgeCount();
}

async deleteSchool(id,dialog: NbDialogRef<any>)
{
  dialog.close();
  this.loading = true;
  var delResponse = await this.gpnRequestService.InactiveStgSchool(id);
  if(delResponse.statusCode == 0)
  {
    var response = await this.gpnRequestService.GetStgSchoolList();
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();
    }
  }
  this.loading = false;
}

onComplaintAction(dialog: TemplateRef<any>, id: any) {
  //this.updatedStatus = event.data.status;
  const dialogRef = this.dialogService.open(
    dialog,
    {
      context: id,
      closeOnBackdropClick: false,
      closeOnEsc: false,
    });
}

searchGrid(search) {

  const predicate = compileFilter(
    { logic: "or",
     filters: [
       { field: "name", operator: "contains", value: search },
       { field: "branchName", operator: "contains", value: search},
       { field: "contactPerson", operator: "contains", value: search},
       { field: "phone", operator: "contains", value: search},
       { field: "email", operator: "contains", value: search},
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
}
