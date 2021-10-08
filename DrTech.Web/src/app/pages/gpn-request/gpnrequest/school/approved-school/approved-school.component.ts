import { Component, OnInit, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import {GpnRequestService} from '../../../service/gpn-request.service'
import { Router } from '@angular/router';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ExcelService } from '../../../../../common/service/excel.service';
import { SchoolRequestDto } from '../../dto/dto';

@Component({
  selector: 'ngx-approved-school',
  templateUrl: './approved-school.component.html',
  styleUrls: ['./approved-school.component.scss']
})

export class ApprovedSchoolComponent implements OnInit {
  listViewModel: any[] = [];
  loading=false;
  schoolBadge: any ="";

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  public schoolRequestDto = new SchoolRequestDto();
  public range = { start: null, end: null };
  constructor(public gpnRequestService: GpnRequestService,private excelService: ExcelService, private router: Router,private dialogService: NbDialogService) { }

  async ngOnInit() {
   this.loadData();
  }
async loadData(){
  this.loading = true;
 this.skip = 0;
  var response = await this.gpnRequestService.GetApprovedSchoolsList(this.schoolRequestDto);
  if (response.statusCode == 0) {
    this.listViewModel = response.data;
    this.schoolBadge = this.listViewModel.length;
    this.loadItems();
  }
  this.loading = false
}
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
}
exportAsXLSX(): void {
  debugger
  this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
}
  private loadItems(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
        total: this.listViewModel.length
    };

  }

  public onSelect(e) {
   // this.router.navigate(["/pages/driver/tasklist/" + this.gridView.data[e.index % this.pageSize].id])
  }

  async deleteSchool(id,dialog: NbDialogRef<any>)
  {
    dialog.close();
    var delResponse = await this.gpnRequestService.SuspendSchool(id);
    if(delResponse.statusCode == 0)
    {
      var response = await this.gpnRequestService.GetApprovedSchoolsList(this.schoolRequestDto);
      if (response.statusCode == 0) {
        this.listViewModel = response.data;
        this.loadItems();
      }
    }
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
  async filterDateRange():Promise<void>{
    debugger;
    if(this.range.start != null && this.range.end != null)
    {
      this.schoolRequestDto.startDate = this.range.start.toLocaleDateString();
      this.schoolRequestDto.endDate = this.range.end.toLocaleDateString();
      this.loading = true;
      this.loadData();
    }
  }
  clearDateRange():void{
    this.range.start = null;
    this.range.end = null;
    this.schoolRequestDto.startDate = null;
    this.schoolRequestDto.endDate = null;
    this.loadData();
  }
}

