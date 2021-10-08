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
  selector: 'ngx-create-markitplace',
  templateUrl: './create-markitplace.component.html',
  styleUrls: ['./create-markitplace.component.scss']
})
export class CreatemarkitplaceComponent implements OnInit {
  headerInfo: String = "Add Amal Market Place";
  uploadTxt: string = "Upload Image";
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
  constructor(public mymarkitplaceService: SettingsService, public myWasteService: MywasteserviceService) { }


  ngOnInit() {
    this.reloadGrid();
    this.mymarkitplaceService.GetAllBinDetail().subscribe(result => {
      debugger
      this.AddTypeList = result.data;
    });

  }

  public uploadImage(event) {
    if (event.target.files.length == 0) {
      console.log("No file selected!");
      return;
    }
    var mimeType = event.target.files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.picType = false;
      this.rowClass = false;
      return;
    }
    this.picType = true;
    this.rowClass = true;

    this.size = event.target.files[0].size;
    this.size = this.size / 1000 / 1000;
    if (this.size > 2) {
      this.picSize = false;
      this.rowClass = false;
      return;
    }
    this.picSize = true;
    this.rowClass = true;

    let file: File = event.target.files[0];
    this.fileToUpload = event.target.files[0];
    this._viewModel.image = this.fileToUpload.name;

    var reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onload = (_event) => {
      this._viewModel.fileName = reader.result;
    }
    // let fileToUpload = <File>files[0];
    // const formData = new FormData();
    // formData.append('file', fileToUpload, fileToUpload.name);
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    var mimeType = files[0].type;
    if (mimeType.match(/zip\/*/) == null) {
      this.fileType = false;
      this.rowButtonClass = false;
      return;
    }
    this.fileType = true;
    this.rowButtonClass = true;

    this.size = files[0].size;
    this.size = this.size / 1000 / 1000;
    if (this.size > 10) {
      this.fileSize = false;
      this.rowButtonClass = false;
      return;
    }
    this.fileSize = true;
    this.rowButtonClass = true;

    this.fileToUpload = <File>files[0];
    this._viewModel.document = this.fileToUpload.name;
  }
  async onSubmit() {
    this.loading = true;

    //this._viewModel.imageToUpload =  1;
    if (!this.fileToUpload) this._viewModel.imageToUpload = ""; else this._viewModel.imageToUpload = this.imageToUpload;
    if (!this.fileToUpload) this._viewModel.fileToUpload = ""; else this._viewModel.fileToUpload = this.fileToUpload;
    debugger
    if (this._viewModel.imageToUpload === "" || this.fileToUpload === undefined) {
      this.loading = false;
      return;

    }
    this.file.push(this.fileToUpload);
    // this.file.push(this.fileToUpload);

    var formResponse = await this.mymarkitplaceService.MarkitplaceAdd(this.file, this._viewModel);
    if (formResponse.statusCode == 0)
      this.headerInfo = "Add Amal Market Place";
    this._viewModel = {};
    this.file = [];
    this.loading = false;

    //this.router.navigate(["/pages/settings/create-ad"]);
    this.reloadGrid()
  }
  async InactiveMarketPlace(id)
  {
debugger
    var formResponse = await this.mymarkitplaceService.InactiveMarketPlace(id);

     this.reloadGrid();
  }
  async onUpdate() {
    this.loading = true;

    //this._viewModel.imageToUpload =  1;
   
    if (!this.fileToUpload) this._viewModel.imageToUpload = ""; else this._viewModel.imageToUpload = this.imageToUpload;
    if (!this.fileToUpload) this._viewModel.fileToUpload = ""; else this._viewModel.fileToUpload = this.fileToUpload;
    // debugger
    // if (this._viewModel.imageToUpload === "" || this.fileToUpload === undefined) {
    //   this.loading = false;
    //   return;

    // }
    this.file.push(this.fileToUpload);
    // this.file.push(this.fileToUpload);

    var formResponse = await this.mymarkitplaceService.MarkitplaceUpdate(this.file, this._viewModel);
    if (formResponse.statusCode == 0)
      this.headerInfo = "Add Amal Market Place";
    this._viewModel = {};
    this.file = [];
    this.loading = false;

    //this.router.navigate(["/pages/settings/create-ad"]);
    this.reloadGrid()
  }
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  async editValue(id) { 

    debugger
    var response = await this.mymarkitplaceService.GetBinDetailByID(id);
    this._viewModel.id=response.data.id;
    this._viewModel.fileName=response.data.fileName;
    this._viewModel.binName=response.data.binName;
    this._viewModel.price=response.data.price;
    this._viewModel.capacity=response.data.capacity;
    this._viewModel.desction=response.data.description;
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
    this.loading = true;

    var response = await this.mymarkitplaceService.GetBinDetailList();
    debugger
    console.log(response.data);
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();

    }
    this.loading = false

  }
}
