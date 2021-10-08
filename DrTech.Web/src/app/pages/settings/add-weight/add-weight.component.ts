import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalDataSource } from 'ng2-smart-table';
import { ScheduleDTO } from '../../my-waste/dto/schedule-dto';
import { SettingsService } from '../settings.service';
import { MywasteserviceService } from '../../my-waste/mywasteservice.service';

@Component({
  selector: 'ngx-add-weight',
  templateUrl: './add-weight.component.html',
  styleUrls: ['./add-weight.component.scss']
})
export class AddWeightComponent implements OnInit {
  headerInfo: String = "Add Weight";
  uploadTxt: string = "Upload Banner";
  areaList: any[];
  AddTypeList: any[];
  AreaID: number;
  AdTypeID: number;
  _viewModel: any = {};
  fileType: boolean;
  rowButtonClass: boolean;
  size: any;

  fileToUpload: File;
  imageToUpload: File;
  fileSize: boolean;
  picType: boolean;
  addFile: boolean = true;
  rowClass: boolean;
  picSize: boolean;
  loading: boolean;
  file: File[] = [];
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  listViewModel: any;
  CityList: any[];
  constructor(public myAddWeightService: SettingsService, public myWasteService: MywasteserviceService) { }
  ngOnInit() {
    this.reloadGrid();

    this.myWasteService.GetAllCitys().subscribe(result => {
      this.CityList = result.data;
    });

    this.myAddWeightService.GetAllWeight().subscribe(result => {
      debugger
      this.AddTypeList = result.data;
    });

  }

  async onChangeAreasByID(event) {
    if (this._viewModel.CityID == null) {
      this.areaList = [];
    }
    var response = await this.myWasteService.GetAreasByID(event.target.value);
    if (response.statusCode == 0) {
      this.areaList = response.data;
    }
  }
 
  async onSubmit() {
    this.loading = true;
    debugger
    var formResponse = await this.myAddWeightService.SaveWeight(this._viewModel);
    if (formResponse.statusCode == 0)
      this.headerInfo = "Add Weight";
    this._viewModel = {};
    this.loading = false;
    this.reloadGrid()
  }
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }

  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
  }
  public loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    debugger
    this.gridView = {
      data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize), this.sort),
      total: this.listViewModel.length
    };

  }
  async reloadGrid() {
    // this.loading = true;

    // var response = await this.myAddWeightService.GetWeight();
    // debugger
    // console.log(response.data);
    // if (response.statusCode == 0) {
    //   this.listViewModel = response.data;
    //   this.loadItems();

    // }
    // this.loading = false

  }
}
