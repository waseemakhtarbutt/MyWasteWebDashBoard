import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';

import { ActivatedRoute, Router } from '@angular/router';
import { ExcelService } from '../../../../common/service/excel.service';

import { LocalDataSource } from 'ng2-smart-table';
import { DriverService } from '../../service/driver.service';
import { NbTokenService } from '../../../../common/auth';
import { MywasteserviceService } from '../../../my-waste/mywasteservice.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { guiforecycleRequest } from '../dto/guiforecycleRequest-dto';
@Component({
  selector: 'ngx-gui-for-recycle-list',
  templateUrl: './gui-for-recycle-list.component.html',
  styleUrls: ['./gui-for-recycle-list.component.scss']
})
export class GuiForRecycleListComponent implements OnInit {
  GridHeaderText : string = "Collection Schedule";
  loading: boolean = false;
  listViewModel: any[] = [];
  _viewModel: any = {};
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  source: LocalDataSource = new LocalDataSource();
  updatedStatus: number = -1;
  points: number = 10;
  userId: any;
  statusId: any;
  companyFullName :string =""
  public CompanyId:number =0
  IsDisabled: boolean = true;
  public ID
  ShowBAdmin : boolean = false;
  ShowSAdmin : boolean = false;
  ShowLocation:boolean =false;
  ShowIfSuperAdmin : boolean =false;
  companyList : any;
  HeadBusinessesList :any;
  HideChartsIfSuperAdmin:boolean = false ;
  BusinessName :string;
  //Set Charts height based on screen size:
  ScreenSize :number;
  barChartWidth:number = 300;
  barChartCircularWidth:number = 250;
  public range = { start: null, end: null };
  barChartMonthWidth:number = 200;
  barChartYearWidth:number = 200;
  barChartDetailWidth:number = 350;
  //I am using this property to hide the location Admin of the previous dashboard- (In future if needed just make this property to true and set the LocationAdmin directive on the html Div to show accordingly.)
  HideLocationAdminPreviousDashboard = true;
  public guiforecycleRequest = new guiforecycleRequest();
  constructor(
    private formBuilder: FormBuilder,
    public driverservice: DriverService,
    private service: MywasteserviceService,
    private tokenService: NbTokenService,
    private excelService: ExcelService) 
  {

  }

  async ngOnInit() {
    this.ScreenSize = window.innerWidth;
    if(this.ScreenSize < 1400){
    this.barChartWidth = 120;
    this.barChartCircularWidth = 160;

    this.barChartMonthWidth = 120;
    this.barChartYearWidth = 120;
    this.barChartDetailWidth = 160;

    }


    this.reloadGrid();
    let role = this.tokenService.getRole();
    if (role === 4){
      this.ShowBAdmin = true;
      this.ShowSAdmin = false;
      this.ShowLocation = true;
      this.ShowIfSuperAdmin = false;
      this.HideChartsIfSuperAdmin = false;
      this.GridHeaderText = "Collection Schedule (By Locations)";
    }
    else{
      this.ShowBAdmin = false;
      this.ShowSAdmin = true;
      this.ShowLocation = false;
      this.ShowIfSuperAdmin = false;
      this.HideChartsIfSuperAdmin = false;
      this.GridHeaderText = "Collection Schedule";
    }

    if(role == 1)
    {
      this.ShowIfSuperAdmin = true;
      this.ShowLocation = true;
      this.HideChartsIfSuperAdmin = true;
      this.GridHeaderText = "Collection Schedule (By Locations)";
    }

    // var response = await this.service.GetCompanyByAdminRole();
    // if(response.statusCode == 0){
    //   this.companyList = response.data;
    //   //console.log(this.companyList)
    //  }

     var Headresponse = await this.service.GetHeadOfficesOFBusinessForGOI();
    if(Headresponse.statusCode == 0){
      this.HeadBusinessesList = Headresponse.data;
     }

     var BusinessNameresponse = await this.service.GetLoggedInAdminBusinessName();
     if(BusinessNameresponse.statusCode == 0){
         this.BusinessName = BusinessNameresponse.data;
      }

  }
  async filterDateRange(){
    debugger;
    if(this.range.start != null && this.range.end != null)
    {
      this.guiforecycleRequest.startDate = this.range.start.toLocaleDateString();
      this.guiforecycleRequest.endDate = this.range.end.toLocaleDateString();
      var response = await this.driverservice.GetGOIListForSuperAdminByDate(this.guiforecycleRequest);
      if(response.statusCode == 0){
        this.listViewModel = response.data;
        this.skip = 0;
        this.loadItems();
        //console.log(this.companyList)
       }
    }
  }
  clearDateRange():void{
    this.range.start = null;
    this.range.end = null;
    this.guiforecycleRequest.startDate = null;
    this.guiforecycleRequest.endDate = null;
    this.reloadGrid();
  }
  async reloadGrid() {
    this.loading = true;

    var response = await this.driverservice.GetGUIList();
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();
    }
    this.loading = false

  }
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  public loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
    };

  }

  async onChangeBusiness(event) {
    this.companyList =[];
    var response = await this.service.GetBusinessBranchesByIdForGOI(event.target.value);
    if(response.statusCode == 0){
      this.companyList = response.data;
      //console.log(this.companyList)
     }
}
exportAsXLSX(): void {
  debugger
  this.excelService.exportAsExcelFile(this.listViewModel, 'sample');
}

async onChangeBusinessGOI(event) {

 // this.listViewModel =[];
 this.guiforecycleRequest.branchID =event.target.value

  var response =await this.driverservice.GetGOIListForSuperAdmin(event.target.value);
  if(response.statusCode == 0){
    this.listViewModel = response.data;
    this.skip = 0;
    this.loadItems();
    //console.log(this.companyList)
   }
}


  searchGrid(search) {

    const predicate = compileFilter(
      { logic: "or",
       filters: [
         { field: "locationName", operator: "contains", value: search },
        //  { field: "weight", operator: "contains", value: search},
        //  { field: "greenPoints", operator: "contains", value: search},

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
