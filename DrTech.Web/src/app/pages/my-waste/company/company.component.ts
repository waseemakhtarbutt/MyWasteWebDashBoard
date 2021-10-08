import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { MywasteserviceService } from '../mywasteservice.service';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  selector: 'ngx-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.scss']
})
export class CompanyComponent implements OnInit {
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
  headerInfo: String = "Add Company";
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


  listViewModel: any[] = [];
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  source: LocalDataSource = new LocalDataSource();
  updatedStatus: number = -1;
  points: number = 10;
  userId: any;
  statusId: any;
  companyFullName: string = ""
  public CompanyId: number = 0
  public SuspensionMessage: string = "Do you realy want to suspend this company?"
  IsDisabled: boolean = true;
  public ID
  public sort: SortDescriptor[] = [{
    field: 'companyName',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public service: MywasteserviceService, private route: ActivatedRoute, private router: Router, private dialogService: NbDialogService) { }
  async ngOnInit() {


    // this.ID = this.route.snapshot.paramMap.get("id");
    this.reloadGrid()

    // var cityResponse = await this.service.GetAllCitys();
    //   this.cityList = cityResponse.data;
    // // setInterval(() => {
    // //   this.reloadGrid();
    // // }, 5000);
    this.service.GetAllCitys().subscribe(result => {
      this.cityList = result.data;
      debugger
    })
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
    this.gridView = {
      data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize), this.sort),
      total: this.listViewModel.length
    };

  }

  //   private loadProducts(): void {
  //     this.gridView = {
  //         data: orderBy(this.listViewModel, this.sort),
  //         total: this.listViewModel.length
  //     };
  // }
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


  async onSubmit() {
    this.loading = true;

    //this._viewModel.imageToUpload =  1;
    if (!this.imageToUpload) this._viewModel.imageToUpload = ""; else this._viewModel.imageToUpload = this.imageToUpload;
    if (!this.fileToUpload) this._viewModel.fileToUpload = ""; else this._viewModel.fileToUpload = this.fileToUpload;
    this._viewModel.requestTYpe = this.requestType;

    this.file.push(this.imageToUpload);
    // this.file.push(this.fileToUpload);

    var formResponse = await this.service.SaveCompany(this.file, this._viewModel);
    if (formResponse.statusCode == 0)
      this.headerInfo = "Add Company";
    this._viewModel = {};
    this.loading = false;
    this.router.navigate(["/pages/mywaste/Newcompany"]);
    this.reloadGrid()
  }
  async reloadGrid() {
    this.loading = true;

    var response = await this.service.GetCompanyList();
    console.log(response.data);
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();

    }
    this.loading = false

  }

  onComplaintAction(dialog: TemplateRef<any>, event: any, id: any) {
    this.companyFullName = this.listViewModel.find(x => x.id == id).companyName;

    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

    this.CompanyId = id;
  }


  async SuspendCompany(dialog: NbDialogRef<any>) {

    var response = await this.service.SuspendCompany(this.CompanyId);

    if (response.statusCode == 0) {
      dialog.close();
      this.reloadGrid()
    }
  }
  redirectNew() {

    this._viewModel = {};
    this.headerInfo = "Add Company";
    this.router.navigate(["/pages/mywaste/Newcompany"]);
  }

  searchGrid(search) {

    const predicate = compileFilter(
      {
        logic: "or",
        filters: [
          { field: "companyName", operator: "contains", value: search },
          { field: "phone", operator: "contains", value: search },
          { field: "contactPerson", operator: "contains", value: search },
          { field: "email", operator: "contains", value: search },
          { field: "address", operator: "contains", value: search },
          { field: "createdDate", operator: "contains", value: search }
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

  async onEdit(Id: number) {
    this.headerInfo = "Edit Company";

    console.log("GetCompanyByID Service Called")
    var listresponse = await this.service.GetCompanyByID(Id);
    if (listresponse.statusCode == 0)
      this._viewModel = listresponse.data;
    //this.router.navigate(["/pages/mywaste/editcompany/"+Id]);
  }


}
