import { Component, OnInit, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Router } from '@angular/router';
import { UserService } from '../../service/user-service';
import { LocalDataSource } from 'ng2-smart-table';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ExcelService } from '../../../../common/service/excel.service';
import { UserRequestDto } from '../../dto/user-dto';
@Component({
  selector: 'ngx-user-list-registered', 
  templateUrl: './registered.component.html',
  styleUrls: ['./registered.component.scss']
})
export class RegisteredComponent implements OnInit {
  listViewModel: any[] = [];
  loading = false;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  source: LocalDataSource = new LocalDataSource();
  updatedStatus: number = -1;
  points: number = 0;
  userId: any;
  statusId: any;
  // public sort: SortDescriptor[] = [{
  //   field: 'fullName',
  //   dir: 'asc'
  // }];
  public multiple = false;
  public allowUnsort = true;
  userRequestDto : UserRequestDto = new UserRequestDto();
  public range = { start: null, end: null };
  constructor(public userService: UserService,private excelService: ExcelService, private router: Router) { 
    this.userRequestDto.type = 'registered';
  }

  async ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading = true;
    this.userService.GetUserList(this.userRequestDto).subscribe(result => {
      if (result.statusCode == 0) {
        result.data.forEach(p => p.pinType = "user");
        this.source.load(result.data);
        this.listViewModel = [];
        this.listViewModel = result.data;
        this.loadItems();
      }
    });
    this.loading = false
  }
  exportAsXLSX(): void {
    debugger
    this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
  }
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  //   public sortChange(sort: SortDescriptor[]): void {
  //     this.sort = sort;
  //     this.loadItems();
  // }
  private loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
    };

  }
  public onSelect(e) {
    // this.router.navigate(["/pages/driver/tasklist/" + this.gridView.data[e.index % this.pageSize].id])
  }
  // xyz:RefuseDTO

  searchGrid(search) {

    const predicate = compileFilter(
      {
        logic: "or",
        filters: [
          { field: "fullName", operator: "contains", value: search },
          { field: "email", operator: "contains", value: search },
          { field: "phone", operator: "contains", value: search },

        ]
      });
    if (search) {
      this.gridView = {
        data: this.listViewModel.filter(predicate),
        total: this.listViewModel.length
      };
    }
    else {
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
      this.userRequestDto.startDate = this.range.start.toLocaleDateString();;
      this.userRequestDto.endDate = this.range.end.toLocaleDateString();;
      this.LoadData();
    }
  }
  clearDateRange():void{
    this.range.start = null;
    this.range.end = null;
    this.userRequestDto.startDate = null;
    this.userRequestDto.endDate = null;
    this.LoadData();
  }
}
