import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { RequestService } from '../service/request-service';
import { CommonService } from '../../../common/service/common-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { RecycleDTO } from '../dto';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { LocationLinkComponent, UserDetailLinkComponent } from '../../../common/custom-control';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ExcelService } from '../../../common/service/excel.service';
import { RecycleRequest } from '../dto/recycleRequest-dto';

@Component({
  selector: 'ngx-recycleall-request',
  templateUrl: './recycleall-request.component.html',
  styleUrls: ['./recycleall-request.component.scss'],
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
    :host
    /deep/
    i.nb-compose:hover
    {
      background-color: #91C747 !important;
      border-radius: 50px;
      padding:3px 3px 3px 3px;
      color: #ffffff !important;
    }
    :host
    /deep/
    ng2-st-tbody-custom a.ng2-smart-action.ng2-smart-action-custom-custom {
      display: inline-block;
      width: 35px;
      padding-top: 12px;
    }
  `],
})
export class RecycleallRequestComponent implements OnInit {
  source: LocalDataSource = new LocalDataSource();
  userId: any;
  statusId: any;
  badge: any;
  points: number = 0;
  listViewModel: any[] = [];
  loading = false;
  ExcellistViewModel: any[] = [];
  public gridView: GridDataResult;
  public pageSize = 9;
  public skip = 0;
  // public sort: SortDescriptor[] = [{
  //   field: 'updatedDate',
  //   dir: 'desc'
  // }];
  public multiple = false;
  public allowUnsort = true;
  public recycleRequest = new RecycleRequest();
  public range = { start: null, end: null };
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  constructor(public requestService: RequestService,private excelService: ExcelService, public commonService: CommonService, private dialogService: NbDialogService, private route: ActivatedRoute, private router: Router) {
    this.userId = route.snapshot.paramMap.get("id");
    this.recycleRequest.statusId = 1;
  }

  ngOnInit() {
    this.LoadData();
    this.commonService.GetStatusList().subscribe(result => {
      this.statusList = result.data;

    });
  }

  LoadData() {
    this.loading = true;

    this.requestService.GetRecycleAllList(this.recycleRequest).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = [];
        this.listViewModel = result.data;
        this.loadItems();
        this.loading = false
      }
    });

    // this.loading = false
  }
  exportAsXLSX(): void {
    debugger
    this.loading = true;

    this.requestService.GetRecycleAllListExcel(this.recycleRequest).subscribe(result => {
      if (result.statusCode == 0) {
        this.ExcellistViewModel = [];
        this.ExcellistViewModel = result.data;
        this.loading = false;
        this.excelService.exportAsExcelFile(this.ExcellistViewModel, 'sample');

      }
    });
  }
  public onSelect(e) {
    this.router.navigate(["/pages/driver/assign-driver/update-recycle-assign/" + this.gridView.data[e.index % this.pageSize].id]);
  }
  public sortChange(sort: SortDescriptor[]): void {
    //  this.sort = sort;
    this.loadItems();
  }
  private loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
    };
    if (this.listViewModel) {
      this.badge = this.listViewModel.length;
    }
    else
      this.badge = '';
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }

  searchGrid(search) {

    const predicate = compileFilter(
      {
        logic: "or",
        filters: [
          { field: "idea", operator: "contains", value: search },
          { field: "userName", operator: "contains", value: search },
          { field: "statusDescription", operator: "contains", value: search },
          { field: "cityName", operator: "contains", value: search },
          { field: "areaName", operator: "contains", value: search },
          { field: "address", operator: "contains", value: search },
          { field: "collectorDateTime", operator: "contains", value: search }

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
  public filterChanged(event: Event) {
    let selectElementText = event.target['value'];
    this.statusId = selectElementText;
    var datalist = this.requestService.GetRecycleList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        //this.listViewModel=[];
        this.skip = 0;
        debugger
        this.listViewModel = result.data;
        this.loadItems();
      }
    });

  }
  filterDateRange():void{
    debugger;
    if(this.range.start != null && this.range.end != null)
    {

      
      this.recycleRequest.startDate = this.range.start.toLocaleDateString();
      this.recycleRequest.endDate =  this.range.end.toLocaleDateString();
      this.LoadData();
    }
  }
  clearDateRange():void{
    this.range.start = null;
    this.range.end = null;
    this.recycleRequest.startDate = null;
    this.recycleRequest.endDate = null;
    this.LoadData();
  }
}
