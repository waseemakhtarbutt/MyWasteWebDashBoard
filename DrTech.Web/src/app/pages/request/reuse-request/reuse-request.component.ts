import { Component, OnInit, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Router } from '@angular/router';
import { LocalDataSource } from 'ng2-smart-table';
import { RequestService } from '../service/request-service';
import { CommonService } from '../../../common/service/common-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { ReuseDTO } from '../dto';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { LocationLinkComponent, UserDetailLinkComponent } from '../../../common/custom-control';
import { ActivatedRoute } from '@angular/router';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ExcelService } from '../../../common/service/excel.service';

@Component({
  selector: 'ngx-reuse-request',
  templateUrl: './reuse-request.component.html',
  styleUrls: ['./reuse-request.component.scss']
})
export class ReuseRequestComponent implements OnInit {
  listViewModel: any[] = [];
  loading = false;
  public gridView: GridDataResult;
  public pageSize = 9;
  public skip = 0;
  source: LocalDataSource = new LocalDataSource();
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  updatedStatus: number = -1;
  points: number = 0;
  userId: any;
  statusId: any;
  refuseModel: ReuseDTO[] = [];
  IsDisabled: boolean = true;
  IsGPTextBoxDisabled: boolean = true;
  public DefaultGreenPoints: number = 0;
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public requestService: RequestService,private excelService: ExcelService, public commonService: CommonService, private router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {

    // this.LoadData();
    this.commonService.GetStatusList().subscribe(result => {
      this.statusList = result.data;
      console.log(this.statusList);
    });
    this.loading = true;

    var response = await this.requestService.GetAllReuseList();
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();
    }
    this.loading = false
    var response = await this.commonService.GetDefaultGreenPoints();
    if (response.statusCode == 0) {
      this.DefaultGreenPoints = response.data.greenPointValue;

    }
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
  async updateStatus(data: ReuseDTO, ref: NbDialogRef<any>) {
    this.requestService.updateReuseStatusById(data).subscribe(result => {
      this.reloadGrid();
      //this.loadItems();
      ref.close();
    });
  }
  async reloadGrid() {
    this.loading = true;

    var response = await this.requestService.GetAllReuseList();
    console.log(response.data);
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();

    }
    this.loading = false

  }
  LoadData() {
    this.requestService.GetReuseList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        result.data.forEach(p => p.pinType = "reuse");
        this.source.load(result.data);
      }
    });
  }
  async onComplaintAction(dialog: TemplateRef<any>, event: any, id: number) {
    debugger
    var response = await this.requestService.GetReuseById(id);
    this.IsGPTextBoxDisabled = false;
    if (response.statusCode == 0) {
      this.resuleModel = response.data[0];
      if (response.data[0].statusID == 3 || response.data[0].statusID == 5) {
        this.IsDisabled = false;
      }
      else {
        this.IsDisabled = true;
      }
    }

    this.dialogService.open(
      dialog,
      {
        context: this.resuleModel,
        closeOnBackdropClick: false,
        closeOnEsc: false,

      });
  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }
  public onSelect(e) {
    // this.router.navigate(["/pages/driver/tasklist/" + this.gridView.data[e.index % this.pageSize].id])
  }
  resuleModel: ReuseDTO
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
  async getSelectedOptionText(event: Event) {
    let selectElementText = event.target['options']
    [event.target['options'].selectedIndex].text;
    if (selectElementText == "Resolved") {
      this.IsGPTextBoxDisabled = true;
      // var response = await this.commonService.GetDefaultGreenPoints();
      //if (response.statusCode == 0) {
      this.resuleModel.greenPoints = this.DefaultGreenPoints;

      // }
    }
    else {

      this.resuleModel.greenPoints = 0;
      this.IsGPTextBoxDisabled = false;
    }
  }
  public filterChanged(event: Event) {
    let selectElementText = event.target['value'];
    debugger
    this.statusId = selectElementText;
    var datalist = this.requestService.GetReuseList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        //this.listViewModel=[];
        this.skip = 0;
        debugger
        this.listViewModel = result.data;
        this.loadItems();
      }
    });

  }
}
