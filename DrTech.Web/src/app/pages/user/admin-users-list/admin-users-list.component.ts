import { Component, OnInit, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Router } from '@angular/router';
import { CompositeFilterDescriptor, filterBy, compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { LocalDataSource } from 'ng2-smart-table';
import { UserService } from '../service/user-service';
import { ExcelService } from '../../../common/service/excel.service';


@Component({
  selector: 'ngx-admin-users-list',
  templateUrl: './admin-users-list.component.html',
  styleUrls: ['./admin-users-list.component.scss']
})

export class AdminUsersListComponent implements OnInit {
  listViewModel: any[] = [];
  loading = false;

  public gridView: GridDataResult;
  public pageSize = 9;
  public skip = 0;
  source: LocalDataSource = new LocalDataSource();
  updatedStatus: number = -1;
  points: number = 0;
  userId: any;
  statusId: any;
  IsDisabled: boolean = true;
  IsGPTextBoxDisabled: boolean = true;
  public DefaultGreenPoints: number = 0;
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public userService: UserService, private excelService: ExcelService,private router: Router) { }
  async ngOnInit() {

    this.reloadGrid();
   // this.userService.GetStatusList().subscribe(result => {
     // this.statusList = result.data;
   // }
    //);
    this.loading = true;

    // var response = await this.requestService.GetAllReportList();
    // if (response.statusCode == 0) {
    //   this.listViewModel = response.data;
    //   this.loadItems();
    // }
    this.loading = false
    //Set Default GP
    // var response = await this.commonService.GetDefaultGreenPoints();
    // if (response.statusCode == 0) {
    //   this.DefaultGreenPoints = response.data.greenPointValue;
    // }
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
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize), this.sort),
      total: this.listViewModel.length
    };

  }

  async reloadGrid() {
    this.loading = true;
    var response = await this.userService.GetAdminUserList();
    console.log(response.data);
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();

    }
    this.loading = false

  }

}
