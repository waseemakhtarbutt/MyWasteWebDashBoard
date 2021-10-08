import { Component, OnInit } from '@angular/core';
import { ScheduleDTO } from '../dto/schedule-dto';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { MywasteserviceService } from '../mywasteservice.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DriverDTO } from '../../driver/dto/driver.dto';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ExcelService } from '../../../common/service/excel.service';

@Component({
  selector: 'ngx-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  public gridView: GridDataResult;
  public pageSize = 10;
  public skip = 0;
  listViewModel: any[] = [];
  driverList: Array<DriverDTO> = new Array<DriverDTO>();
  areaList: Array<any> = new Array<any>();
  CityList: Array<any> = new Array<any>();
  _scheduleModel: ScheduleDTO = new ScheduleDTO();
  listScheduleModel: Array<ScheduleDTO> = new Array<ScheduleDTO>();
  _scheduleModel1: ScheduleDTO = new ScheduleDTO();
  importSchedule: ScheduleDTO;

  loadBtn: boolean = true;
  saveBtn: boolean = false;
  exportBtn: boolean = false;
  importBtn: boolean = true;
  DublicateErrorMessage: string;
  loading: boolean = false;

  fileUrl;
  size: any;
  _viewModel: any = {};
  file: File[] = [];
  fileToUpload: File;
  constructor(
    private formBuilder: FormBuilder,
     public myWasteService: MywasteserviceService,
      private route: ActivatedRoute,
       private router: Router,
       public excelService: ExcelService) {

  }

  ngOnInit() {
    this.myWasteService.GetAllDrivers().subscribe(result => {
      var driver = new DriverDTO();
      this.driverList = result.data;
    });

    this.myWasteService.GetAllCitys().subscribe(result => {
      this.CityList = result.data;
    });

  }
  async onChangeAreasByID(event) {
    if (this._scheduleModel.CityID == null) {
      this.areaList = [];
    }
    var response = await this.myWasteService.GetAreasByID(event.target.value);
    if (response.statusCode == 0) {
      this.areaList = response.data;
    }
  }
  private loadItems(): void {
    if (this.skip == this.listScheduleModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: this.listScheduleModel.slice(0, 0 + this.listScheduleModel.length),
      total: this.listScheduleModel.length
    };

  }

  onSubmit() {
    if (this._scheduleModel.AreaID == undefined || this._scheduleModel.DriverID == undefined || this._scheduleModel.Date == undefined || this._scheduleModel.fTime == undefined || this._scheduleModel.tTime == undefined)
      return;
    this.saveBtn = true;
    this.importBtn = false;
    this.DublicateErrorMessage = "";
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";

    var date = new Date(this._scheduleModel.Date.getFullYear(), this._scheduleModel.Date.getMonth(), 1);
    var days = [];
    this.listScheduleModel = [];
debugger
    while (date.getMonth() === this._scheduleModel.Date.getMonth()) {
      this._scheduleModel1 = new ScheduleDTO();
      this._scheduleModel1.Date = new Date(date);
      this._scheduleModel1.Day = weekday[new Date(date).getDay()]
      this._scheduleModel1.FromTime = this._scheduleModel.fTime.toLocaleTimeString().replace(/:\d{2}\s/, ' ');
      this._scheduleModel1.ToTime = this._scheduleModel.tTime.toLocaleTimeString().replace(/:\d{2}\s/, ' ');
      this._scheduleModel1.Active = true;
      this._scheduleModel1.Status = "Active";
      this.listScheduleModel.push(this._scheduleModel1);
      date.setDate(date.getDate() + 1);
    }
    console.log(days);

    this.loadItems();
  }
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }


  async saveGrid() {
    debugger
    this.loading = true;
    this.listScheduleModel = this.gridView.data.filter(x => x.Active == true);
    this.listScheduleModel.push(this._scheduleModel);
    var formResponse = await this.myWasteService.SaveSchedule(this.listScheduleModel);
    if (formResponse.statusCode == 0)
      this.loading = false;
    if (formResponse.data == false) {
      this.DublicateErrorMessage = formResponse.statusMessage;
      this.gridView.data = [];
      this.saveBtn = false;

    }
    else {
      this.listScheduleModel = this.listScheduleModel.filter(x => x.Active == true);
      this.loadItems();
      this.exportBtn = true;
      this.saveBtn = false;
      this.loadBtn = false;
      this.importBtn = false;
      this.DublicateErrorMessage = "";
    }
  }

  statusChange(): void {
    setTimeout(() =>
      this.gridView.data.forEach(element => {
        if (element.Active != true)
          element.Status = "Inactive";
        else
          element.Status = "Active";
      }), 100);
  }

  clear(): void {
    this.loadBtn = true;
    this.saveBtn = false;
    this.exportBtn = false;
    this.importBtn = true;
    this._scheduleModel = new ScheduleDTO();
    this.listScheduleModel = [];
    this.gridView.data = [];
    this.DublicateErrorMessage = "";
  }

  exportAsXLSX(): void {
    this.excelService.exportAsExcelFile(this.listScheduleModel, 'sample');
  }

  async importExcel() {
    var formResponse = await this.myWasteService.ImportExcel(this.fileToUpload, "");
    this.loading = true;
    if (formResponse.statusCode == 0) {
      this.listScheduleModel = formResponse.data.map(item => {
        this.importSchedule = new ScheduleDTO();
        this.importSchedule.Date = new Date(item.date),
          this.importSchedule.FromTime = item.fromTime,
          this.importSchedule.ToTime = item.toTime,
          this.importSchedule.Active = item.active,
          this.importSchedule.Status = item.status
        //this.listScheduleModel.push(this.listScheduleModel);
        // item.Date = new Date(item.Date); 
        return this.importSchedule;
      });;
      this.loadItems();
      this.saveBtn = true;
      this.importBtn = false;
    }
    this.loading = false;
  }

  public uploadFile(event) {
    if (event.target.files.length == 0) {
      console.log("No file selected!");
      return;
    }
    var mimeType = event.target.files[0].type;
    // if (mimeType.match(/xlxs\/*/) == null) {

    //   return;
    // }
    this.size = event.target.files[0].size;
    this.size = this.size / 1000 / 1000;
    let file: File = event.target.files[0];
    this.fileToUpload = event.target.files[0];
    var reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onload = (_event) => {
      this._viewModel.fileName = reader.result;
    }
    this.importExcel();
  }

}
