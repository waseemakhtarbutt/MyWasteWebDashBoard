import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GpnRequestService } from '../../../service/gpn-request.service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'ngx-add-business',
  templateUrl: './add-business.component.html',
  styleUrls: ['./add-business.component.scss']
})
export class AddBusinessComponent implements OnInit {
  imageToUpload: File;
  fileToUpload: File;
  businessList: any;
  cityList: any;
  businessFlag = false;
  stgId: any;
  _viewModel: any = {};
  file: File[] = [];
  queryLst: any = [];
  schoolName: string;
  requestType: string;
  headerInfo: String = "Create an Instance for Business";
  downloadAllow: boolean = true;
  uploadTxt: string = "Upload Logo";
  businessTypeList: any;
  fileUrl;
  size: any;
  picSize: boolean = true;
  picType: boolean = true;
  fileSize: boolean = true;
  fileType: boolean = true;
  rowClass: boolean = true;
  rowButtonClass: boolean = true;
  loading: boolean = false;

  constructor(public service: GpnRequestService, private route: ActivatedRoute, private router: Router, private dialogService: NbDialogService, private sanitizer: DomSanitizer) { }
  async ngOnInit() {
    var queryParameter = this.route.snapshot.paramMap.get("id");
    if (queryParameter != null) {
      this.queryLst = queryParameter.split('-');
      var id = this.queryLst[0];
      this.requestType = this.queryLst[1];
    }
    else {
      this.requestType = "New";
      this.dropDownDefaultValues();
    }

    if (this.requestType != "New") {
      var response = await this.service.GetBusinessList(queryParameter);
      if (response.statusCode == 0) {
        this._viewModel = response.data;
        if (this.requestType == 'S') {
          this.stgId = response.data.id;
          this.dropDownDefaultValues();
        }
        else {
          this.stgId = "";
          this.headerInfo = "Edit an Instance for Business";
          this.downloadAllow = (this._viewModel.documentFileName == null ? true : false);
          this.uploadTxt = "Change Logo";
          this.businessFlag = true;
        }

        this._viewModel.stgId = this.stgId;
        this.schoolName = this._viewModel.name;
      }
    }

    var regResponse = await this.service.GetRegBusiness();
    if (regResponse.statusCode == 0)
      this.businessList = regResponse.data;

    // var cityResponse = await this.service.GetCityList();
    // if (cityResponse.statusCode == 0)
    //   this.cityList = cityResponse.data;

    this.service.GetAllCitys().subscribe(result => {
      this.cityList = result.data;
    });


    var businessType = await this.service.GetBusinessType();
    if (businessType.statusCode == 0)
      this.businessTypeList = businessType.data;
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
    this.imageToUpload = event.target.files[0];
    this._viewModel.image = this.imageToUpload.name;

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

  public onChangeObj(id) {
    if (id == 0) { this.businessFlag = false; this._viewModel.name = this.schoolName; }
    //else if(id == -1){ this.schoolFlag = false; this._viewModel.name = this.schoolName;}
    else if (id) { this.businessFlag = true; this._viewModel.name = this.businessList.find(x => x.id == id).name; }
  }

  async onSubmit() {
    this.loading = true;

    //this._viewModel.imageToUpload =  1;
    if (!this.imageToUpload) this._viewModel.imageToUpload = ""; else this._viewModel.imageToUpload = this.imageToUpload;
    if (!this.fileToUpload) this._viewModel.fileToUpload = ""; else this._viewModel.fileToUpload = this.fileToUpload;
    this._viewModel.requestTYpe = this.requestType;

    this.file.push(this.imageToUpload);
    this.file.push(this.fileToUpload);

    var formResponse = await this.service.SaveBusiness(this.file, this._viewModel);
    if (formResponse.statusCode == 0)
      this.router.navigate(["/pages/gpn/gpnrequest/approvedbusiness"]);
    this.loading = false;
  }

  onComplaintAction(dialog: TemplateRef<any>, event: any) {
    //this.updatedStatus = event.data.status;
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });
  }

  backToList(dialog: NbDialogRef<any>) {
    dialog.close();
    this.router.navigate(["/pages/gpn/gpnrequest/list"]);
  }

  dropDownDefaultValues() {
    this._viewModel.parentId = 0;
    // this. _viewModel.cityID = -1; 
    // this._viewModel.orgTypeID = -1;
  }


}
