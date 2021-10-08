import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../../../settings/settings.service';
import { MywasteserviceService } from '../../../my-waste/mywasteservice.service';
import { DriverService } from '../../service/driver.service';
import { NbTokenService } from '../../../../common/auth';
@Component({
  selector: 'ngx-add-gui-for-recyle',
  templateUrl: './add-gui-for-recyle.component.html',
  styleUrls: ['./add-gui-for-recyle.component.scss']
})
export class AddGuiForRecyleComponent implements OnInit {

  _viewModel: any = {};
  itemslist: any;
  subitemslist: any;
  show: boolean = false;
  donationFlag = false;
  companyList : any;
  typesList :any;
  HeadBusinessesList :any;
  file: File[] = [];
SuccessMessage :string =""
ShowHide :boolean = false;
default: any;
ShowDefault:boolean = false;
LoggedInAdminInfo :string = "";
IsRequired :boolean = true;
public max: Date;
  constructor(private tokenService: NbTokenService,public driverservice: DriverService,public service: MywasteserviceService,  public settingsService: SettingsService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    //Show hide dropdown basis on admins
    let role = this.tokenService.getRole();
    if (role === 1){
      this.ShowHide = true;
      this.ShowDefault =false;
      this.LoggedInAdminInfo = "You are LoggedIn as a DrTech Admin!"
    }
    else if(role == 4){
      this.ShowHide = false;
      this.ShowDefault =false;
      this.LoggedInAdminInfo = "You are LoggedIn as a Business Admin!"
    }
    else{
      this.ShowHide = false;
      this.ShowDefault =false;

    }

    var response = await this.service.GetCompanyByAdminRole();
    if(response.statusCode == 0){
      if (role === 1){
        this.companyList = []
      }
     else if (role === 9){
       this.IsRequired = false;
        this.ShowDefault =true;
        this.default = response.data[0];
        this._viewModel.companyID1 = Object.assign(this.default.id);
        this.LoggedInAdminInfo = "You are LoggedIn as a Location Admin!"
      }
      else{
        this.companyList = response.data;
      }

      //console.log(this.companyList)
     }

     var typesResponse = await this.service.GetWasteTypes();
    if(typesResponse.statusCode == 0){
      this.typesList = typesResponse.data;
      //console.log(this.companyList)
     }

     var Headresponse = await this.service.GetHeadOfficesOFBusinessForGOI();
    if(Headresponse.statusCode == 0){
        this.HeadBusinessesList = Headresponse.data;

     }
    this.max = new Date();
  }
  async onSubmit() {
    // console.log("Hi Ihsan yes I am here")
    // console.log(this._viewModel)
    // var collectdate = new Date(this._viewModel.collectdate)
    // collectdate.setMonth(collectdate.getMonth()+1,1);
    // this._viewModel.collectdate = collectdate;
    var formResponse = await this.driverservice.SaveGUIForRecycle(this._viewModel);

    if (formResponse.statusCode == 0)
    {
      this.SuccessMessage = "Successfull!"
      this._viewModel = {} ;
    }



  }
  async onChangeBusiness(event) {
    this.companyList =[];
    console.log(event.target.value);
    var response = await this.service.GetBusinessBranchesByIdForGOI(event.target.value);
    if(response.statusCode == 0){
      this.companyList = response.data;
      //console.log(this.companyList)
     }
}
  // getSelectedOptionText(event: Event) {
  //   let selectElementText = event.target['options']
  //   [event.target['options'].selectedIndex].text;
  //   if (selectElementText == "Other") {
  //     this.donationFlag = true;
  //     this.subitemslist = null;
  //   }
  //   else {
  //     this._viewModel.typeDescription = "";
  //     this._viewModel.subTypeDescription = "";
  //     this.donationFlag = false;

  //   }

 // }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }
}

