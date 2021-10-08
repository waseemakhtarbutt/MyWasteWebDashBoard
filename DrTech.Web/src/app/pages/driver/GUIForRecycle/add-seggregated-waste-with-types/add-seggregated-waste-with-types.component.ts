import { Component, OnInit, Output, EventEmitter, TemplateRef, ViewChild } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../../../settings/settings.service';
import { MywasteserviceService } from '../../../my-waste/mywasteservice.service';
import { DriverService } from '../../service/driver.service';
import { NbTokenService } from '../../../../common/auth';
import { Location } from '@angular/common';
import { DesegregatedGridComponent } from '../desegregated-grid/desegregated-grid.component';
import { ExcelService } from '../../../../common/service/excel.service';

@Component({

  selector: 'ngx-add-seggregated-waste-with-types',
  templateUrl: './add-seggregated-waste-with-types.component.html',
  styleUrls: ['./add-seggregated-waste-with-types.component.scss'],

})
export class AddSeggregatedWasteWithTypesComponent implements OnInit {

  @ViewChild(DesegregatedGridComponent) child:DesegregatedGridComponent;


  public range = { start: null, end: null };
  public companyID: number;
  public businessID: number;
  public collectDate: Date;
  public tweight: number;
  public recycleD: number;
  public Isdisabled: boolean = true;
  public ismessage: boolean = false;
  public isSucccessmessage: boolean = false;
  public MessageError: string = '';
  public SuccessMessage: string = '';
  loading: boolean = false;
  shownTime : boolean = false;

  //   collectDate: ''
  ///////////////////////////////
  public IsSegregated: boolean = false;
  ShowCreateForm: boolean = false;
  ShowDisplayForm: boolean = false;
  typesList: any;
  companyList: any;
  TypesWithWeightRecycle: any[] = [];
  HeadBusinessesList: any;
  public max: Date;
  public min: Date;
  MainArray: any = [];
  _viewModell: any; 
  public resultss: [];
  ABC: any[][]
  model: any;
  // dtoModel : DTOSub;
  // dtoMain : DTOMain;
  public dtoModel: MainDTO;
  //modelValues: any = {};
  public TypesWithWeight: any[] = [{
    typeID: '',
    Weight: '' ,
    rate:'',
    total:''
  }];

  A: any;
  B: any;
  C: any;
  errorMessage: string = "";
  duplicateErrorMessage: string = "";
  typesnullErrorMessage: string = "";
  // public modelValues: any[] = [{
  //   companyID: '',
  //   businessID: '',
  //   collectDate: ''
  // }];
  public modelValues: any[] = []

  constructor(public service: MywasteserviceService, private route: ActivatedRoute, private router: Router,private excelService: ExcelService) {

  }
  ngOnInit() {
    this.loadWasteTypes();
    this.loadBusinesses();
    this.max = new Date();
    this.min = new Date(2017, 1, 1);
    this.ismessage = false;

  }
  async loadWasteTypes() {
    var typesResponse = await this.service.GetWasteTypes();
    if (typesResponse.statusCode == 0) {
      this.typesList = typesResponse.data;
    }

  }
  async loadBusinesses() {
    var Headresponse = await this.service.GetHeadOfficesOFBusinessForGOI();
    if (Headresponse.statusCode == 0) {
      this.HeadBusinessesList = Headresponse.data;

    }
  }
  async onChangeBusiness(event) {
    this.companyList = [];
    console.log(event.target.value);
    var response = await this.service.GetBusinessBranchesByIdForGOI(event.target.value);
    if (response.statusCode == 0) {
      this.companyList = response.data;
    }
  }

  async onBusinesSelectionLoadBranches(id: number) {
    this.companyList = [];

    var response = await this.service.GetBusinessBranchesByIdForGOI(id);
    if (response.statusCode == 0) {
      this.companyList = response.data;
    }
  }

  public handleEvent(data: any) {
    //TODO NAbeel
    debugger
    this.collectDate = new Date(data.collectDate);
    // Naming convenstion not correct like companyID consider businessID and branchID consider companyID
    this.businessID = data.companyID;
    this.onBusinesSelectionLoadBranches(this.businessID);
    this.companyID = data.branchID;
    this.tweight = data.tweight;
    this.recycleD = data.recycleD;

  }

  public showHideHandleEvent(IsSegregated: boolean) {
    this.IsSegregated = IsSegregated;
    if (this.IsSegregated == true) {
      this.businessID = null;
      this.companyID = null;
      this.tweight = null;
      this.collectDate = null;
    }
    else {
      this.TypesWithWeightRecycle = [];
    }
  }


  public getRIDHandleEvent(id: number) {

    if (this.IsSegregated == true) {
      this.LoadSegregatedData(id);
    }

  }
  public getListHandleEvent(objet: []) {
debugger
      this.TypesWithWeightRecycle = objet;
  

  }
  exportAsXLSX(): void {
    debugger
    this.excelService.exportAsExcelFile(this.TypesWithWeightRecycle, 'sample');
  }
  async LoadSegregatedData(Id: number) {
    var response = await this.service.GetSegregatedDataByID(Id);
    if (response.statusCode == 0) {
      this.TypesWithWeightRecycle = response.data;
      console.log(this.TypesWithWeightRecycle)
    }
  }



  addNewRow() {

    for (let item of this.TypesWithWeight) {

      if (item.typeID == "" || item.Weight == ""|| item.rate == "") {
        this.ShowErrorMessage();
        this.errorMessage = "Please fill the empty fields first!";
        this.duplicateErrorMessage = "";
        this.typesnullErrorMessage = "";
        return;
      }
    }
    this.errorMessage = "";
    this.TypesWithWeight.push({
      typeID: '',
      Weight: '',
      rate:'',
      total:''
    });
  }

  removeRow(i: number) {
    this.errorMessage = "";
    this.duplicateErrorMessage = "";
    this.typesnullErrorMessage = "";
    this.TypesWithWeight.splice(i, 1);
  }


  findObjectByKey(array, key, value) {
    // for (var i = 0; i < array.length; i++) {
    //     if (array[i][key] === value) {
    //         return array[i];
    //     }
    // }

    return array.filter(item => item.key === value)[0];
    //return null;
  }

  async SubmitForm() {
    this.loading = true;

    if (this.TypesWithWeight === undefined || this.TypesWithWeight.length == 0) {
      this.typesnullErrorMessage = "Please at least enter one type of weight to continue!";
      this.loading = false;
      return;
    }
    this.typesnullErrorMessage = "";

    let types = this.TypesWithWeight.map(({ typeID }) => typeID)

    //Check Duplicate values
    // var names = ['Mike', 'Matt', 'Nancy', 'Adam', 'Jenny', 'Nancy', 'Carl']

    var uniq = types
      .map((typ) => {
        return {
          count: 1,
          typ: typ
        }
      })
      .reduce((a, b) => {
        a[b.typ] = (a[b.typ] || 0) + b.count
        return a
      }, {})

    var duplicates = Object.keys(uniq).filter((a) => uniq[a] > 1)
    // if(duplicates ){
    //   return;
    // }


    console.log(duplicates) // [ 'Nancy' ]

    //End
    if (duplicates === undefined || duplicates.length == 0) {
      this.duplicateErrorMessage = "";
      this.dtoModel = new MainDTO();
      this.dtoModel.RecycleID = this.recycleD;
      this.dtoModel.businessID = this.businessID;
      this.dtoModel.companyID = this.companyID;
      this.dtoModel.collectDate = this.collectDate;
      this.dtoModel.tWeight = this.tweight;
      this.dtoModel.lists = this.TypesWithWeight;

      var formResponse = await this.service.DumpRecycle(this.dtoModel);
      if (formResponse.statusCode == 0) {
        if (formResponse.data == true) {
          this.loading = false;
         // this.pageRefresh();
         this.tweight = null;
         this.companyID = null;
         this.businessID = null;
         this.recycleD = null;
         this.TypesWithWeight = [{
          typeID: '',
          Weight: ''
        }];
         this.child.reloadGrid();
          this.SuccessMessage = "Record Successfully Updated!"
          this.ismessage = false;
          this.isSucccessmessage = true;


        }
        else {
          this.loading = false;
          this.router.navigateByUrl('/pages/driver/GUIForRecycle/add-seggregated-waste-with-types');
          this.isSucccessmessage = false;
          this.ismessage = true;
          this.ShowMissWeightErrorMessage();
          this.MessageError = "Types weight must be equal to Total weight"

        }

      }

    }
    else {
      //  for(let xitem in this.typesList){
      //    console.log(xitem);
      //    var obj = this.findObjectByKey(this.typesList, xitem, 3);
      //  }
      this.loading = false;
      this.errorMessage = "";
      this.typesnullErrorMessage = "";
      this.ShowErrorMessage();
      this.duplicateErrorMessage = "Please remove duplicate types to continue!"
      return;
    }




    //console.log(this.dtoModel)
    // console.log(this.businessID);
    // console.log("jjjjjjjjjjjjjjjjjjjjjjjjjjj")
    // this.TypesWithWeight.push(this.businessID)
    // this.TypesWithWeight.push(this.companyID)
    // this.TypesWithWeight.push(this.collectDate)
    // var myResult = this.TypesWithWeight.concat(this.modelValues);

    //this.dtoModel.businessID = this.modelValues.indexOf[0];
    //this.dtoModel.businessID = this.modelValues.indexOf[1];
    // this.dtoModel.businessID = this.modelValues.indexOf[2];
    //this.dtoMain.mainValues = this.dtoModel;
    // this.dtoMain.TypesList = this.TypesWithWeight;

    // this.dtoModel.list = this.TypesWithWeight;


    //  this.MainArray =  this.TypesWithWeight.concat(this.modelValues);
    //console.log(this.TypesWithWeight);
    //console.log(this.modelValues)

    //var result = this.TypesWithWeight.concat(this.modelValues);

    //this.A = this.modelValues;
    //this.B = this.TypesWithWeight;
    // // // //  this.MainArray.push(this.modelValues);
    // // // //  this.MainArray.push(this.TypesWithWeight)
    // // // //  this.model = this.MainArray;
    // // // //  console.log(this.model);

    // console.log(this.A);
    //console.log(this.B)
    //this.C = this.A + this.B;
    //this.C = this.B;
    // var obj : DTOMain;
    //obj.mainValues.push(this.modelValues);
    //this.dtoMain.TypesList.push(this.TypesWithWeight);
    //console.log(obj)
    // console.log(this.dtoMain)

    // console.log( this.A = this.modelValues.find[1]);
  }
//   pageRefresh() {
//     location.reload();
//  }
 ShowErrorMessage(): void {
  //show box msg
  this.shownTime = true;
  //wait 3 Seconds and hide
  setTimeout(function() {
      this.shownTime = false;
  }.bind(this), 2500);
 }
 ShowMissWeightErrorMessage(): void {
  //show box msg
  this.ismessage = true;
  //wait 3 Seconds and hide
  setTimeout(function() {
      this.ismessage = false;
  }.bind(this), 2500);
 }



  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

}


export class MainDTO {
  RecycleID: number;
  businessID: number;
  companyID: number;
  collectDate: Date;
  tWeight: number;
  lists: Array<subItems>;

}

export class subItems {
  typeID: number;
  Weight: number;

}

