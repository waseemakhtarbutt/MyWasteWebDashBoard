import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { RequestService } from '../service/request-service';
import { CommonService } from '../../../common/service/common-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { ReuseDTO } from '../dto';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { debug } from 'util';
import { LocationLinkComponent, UserDetailLinkComponent } from '../../../common/custom-control';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ExcelService } from '../../../common/service/excel.service';

@Component({
  selector: 'ngx-regift-request',
  templateUrl: './regift-request.component.html',
  styleUrls: ['./regift-request.component.scss'],
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
export class RegiftRequestComponent implements OnInit {

  showMessage(va) {
    alert(va);
  }

  source: LocalDataSource = new LocalDataSource();
  userId: any;
  statusId: any;
  badge: any;
  points: number = 0;
  listViewModel: any[] = [];
  loading = false;

  public gridView: GridDataResult;
  public pageSize = 12;
  public skip = 0;
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  // public sort: SortDescriptor[] = [{
  //   field: 'description',
  //   dir: 'asc'
  // }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public requestService: RequestService,private excelService: ExcelService, public commonService: CommonService, private dialogService: NbDialogService, private route: ActivatedRoute, private router: Router) {
    this.userId = route.snapshot.paramMap.get("id");
    this.statusId = 1;
  }

  ngOnInit() {
    this.LoadData();
    this.commonService.GetStatusList().subscribe(result => {
      this.statusList = result.data;
    });
  }

  LoadData() {
    this.loading = true;

    this.requestService.GetRegiftList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;
        this.loadItems();
        this.loading = false
      }
    });


  }
  exportAsXLSX(): void {
    debugger
    this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
  }
  public onSelect(e) {
    this.router.navigate(["/pages/driver/assign-driver/regift-assign/" + this.gridView.data[e.index % this.pageSize].id]);
  }
  public sortChange(sort: SortDescriptor[]): void {
    //this.sort = sort;
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
          { field: "statusDescription", operator: "contains", value: search }

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
    var datalist = this.requestService.GetRegiftList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = [];
        this.skip = 0;
        this.listViewModel = result.data;
        this.loadItems();
      }
    });

  }
}
