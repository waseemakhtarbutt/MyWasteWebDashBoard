import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
//import { RequestService } from '../../../request/service/request-service';
import { DriverService } from '../../service/driver.service';
import { CommonService } from '../../../../common/service/common-service';
import { RegiftDTO, RegiftSubItemDTO } from '../../../request/dto';
import { DropdownDTO } from '../../../../common/dropdown-dto';
import { DriverDTO } from '../../dto/driver.dto';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { PageChangeEvent, GridDataResult } from '@progress/kendo-angular-grid';
import { CommentsDTO } from '../../../request/dto/comments-dto';
import { DriverRequestDto } from '../../dto/driver.dto';

@Component({
  selector: 'ngx-regift-request-assign',
  templateUrl: './regift-request-assign.component.html',
  styleUrls: ['../../style/assign-styles.scss'],
})
export class RegiftRequestAssignComponent implements OnInit {
   mindate = new Date();
  regiftId: string = "";
  typeName: string = "";
  typeList: Array<DropdownDTO> = new Array<DropdownDTO>();
  driverList: Array<DriverDTO> = new Array<DriverDTO>();
  public driverRequestDto = new DriverRequestDto();
  _regiftModel: RegiftDTO = new RegiftDTO();
  isDriver: boolean = true;
  listViewModel: any[] = [];
  pendingConfirmation: string = "";
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();

  public gridView: GridDataResult;
  public pageSize = 5;
  public skip = 0;

  public min: Date;
  public max: Date;

  now: Date = new Date();
  minDate: any;
  maxDate: any;

  constructor(public driverService: DriverService, public CommonService: CommonService, private route: ActivatedRoute, private router: Router, private dialogService: NbDialogService) {
    this.typeName = "DonationType";

  }


  ngOnInit() {
    this.regiftId = this.route.snapshot.paramMap.get("id");

    this.LoadData();
    this.loadShif();
    //If you want to disable past dates from current date, you can set mindate to current date.

    this.minDate = { day: this.now.getDate(), month: this.now.getMonth() + 1, year: this.now.getFullYear(),  };
    console.log(this.minDate)

    this.CommonService.GetDropdownByType(this.typeName).subscribe(result => {
      this.typeList = result.data;
    });

    this.driverService.GetAllDrivers(this.driverRequestDto).subscribe(result => {
      var driver = new DriverDTO();
      driver.fullName = 'Select a Driver';
      driver.id = -1;
      result.data.splice(0, 0, driver);
      this.driverList = result.data;
    });

    var statuses = ["Declined", "Pending"];

    this.CommonService.GetAllStatus(statuses).subscribe(result => {
      var status = new DropdownDTO();

      status.value = -1;
      status.description = 'Select Status';

      result.data.splice(0, 0, status);

      this.statusList = result.data;
    });
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.LoadComments();
  }

  private LoadComments(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
    };
  }

  LoadData() {
    this.driverService.GetRegiftDetailById(this.regiftId).subscribe(result => {
      if (result.statusCode == 0) {
        this._regiftModel = result.data;

        this._regiftModel.pickDate = new Date(this._regiftModel.pickDate);
        this._regiftModel.pickTime = new Date(this._regiftModel.pickDate);

        if (this._regiftModel.statusName == "Collected" && !this._regiftModel.collectedPendingConfirmation)
          this.pendingConfirmation = "Collected Pending Confirmation";
        else if (this._regiftModel.statusName == "Delivered" && !this._regiftModel.deliveredPendingConfirmation)
          this.pendingConfirmation = "Delivered Pending Confirmation";
        else
          this.pendingConfirmation = this._regiftModel.statusName;

        if (result.data.regiftComments != undefined && result.data.regiftComments.length > 0) {
          this.listViewModel = result.data.regiftComments;
          this.LoadComments();
        }

        this.CalculateGP();
      }
    });
  }

  loadShif(){
    this.driverService.Getshift().subscribe(result => {
      if (result.statusCode == 0) {
       new Date(2000, 2, 10, 10, 0)
      //  this.min =  new Date(2000, 2, 10, result.data.startTimeHOurs, result.data.startTimeMinuts);
      //  this.max = new Date(2000, 2, 10, result.data.endTimeHOurs, result.data.endTimeMinuts);
       this.min =  new Date(2000, 2, 10, result.data.startTimeHours, result.data.startTimeMinutes);
       this.max = new Date(2000, 2, 10, result.data.endTimeHours, result.data.endTimeMinutes);

      }
    });
  }

  AddItem() {
    var subItem = new RegiftSubItemDTO();
    subItem.qty = 1;
    subItem.typeID = this.typeList[0].id;

    this._regiftModel.regiftSubItems.push(subItem);

    this.CalculateGP();
  }

  RemoveItem(i) {
    if (this._regiftModel.statusName != 'Declined')
    {
      this._regiftModel.regiftSubItems.splice(i, 1);
      this.CalculateGP();
    }
  }

  AssignRegiftToDriver() {
    var dt = this._regiftModel.pickDate;
    var tm = this._regiftModel.pickTime;
    this._regiftModel.pickupDate = new Date((dt.getMonth()+1) + " " + dt.getDate() +", " + dt.getFullYear() + " " + tm.getHours() + ":" + tm.getMinutes() + ":" + tm.getSeconds()).toLocaleString('ur-PK');

    this.driverService.AssignRegiftToDriver(this._regiftModel).subscribe(result => {
        this.router.navigate(['/pages/request/regift']);
    });
  }

  SMSRegiftComments() {
    var commentModel = new CommentsDTO();

    commentModel.phone = this._regiftModel.userPhone;
    commentModel.comments = this._regiftModel.comments;
    commentModel.rId = this._regiftModel.id;

    this.driverService.SMSRegiftComments(commentModel).subscribe(result => {
        this.LoadData();
    });
  }

  async RejectRegift(dialog: NbDialogRef<any>) {
    this.driverService.RejectRegift(this._regiftModel).subscribe(result => {
      dialog.close();
      this.LoadData();
    });
  }

  CalculateGP() {
    this._regiftModel.totalGP = 0;
    for (let i=0; i<this._regiftModel.regiftSubItems.length; i++)
      this._regiftModel.totalGP += (!this._regiftModel.regiftSubItems[i].qty? 0: this._regiftModel.regiftSubItems[i].qty) * this._regiftModel.gpv;
  }

  QtyInc(subitem) {
    if (subitem.qty < 999 && this._regiftModel.statusName != 'Declined' && this._regiftModel.statusName != 'Delivered')
    {
      subitem.qty++;
      this.CalculateGP();
    }
  }

  QtyDec(subitem) {
    if (subitem.qty > 1 && this._regiftModel.statusName != 'Declined' && this._regiftModel.statusName != 'Delivered')
    {
      subitem.qty--;
      this.CalculateGP();
    }
  }

  onComplaintAction(dialog: TemplateRef<any>, event: any, isDrv: any) {
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

      this.isDriver = isDrv;
  }
  onRejectAction(dialog: TemplateRef<any>, event: any) {
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });
  }
  async SetConfirmation(dialog: NbDialogRef<any>) {
    var response = await this.driverService.SetConfirmation(this._regiftModel.orderID, this.isDriver);

    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();
    }
  }
}
