import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DriverService } from '../../service/driver.service';
import { CommonService } from '../../../../common/service/common-service';
import { RecycleDTO } from '../../../request/dto';
import { DropdownDTO } from '../../../../common/dropdown-dto';
import { RecycleSubItemDTO } from '../../../request/dto/recycle-subitem-dto';
import { DriverDTO } from '../../dto/driver.dto';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { CommentsDTO } from '../../../request/dto/comments-dto';
import { DriverRequestDto } from '../../dto/driver.dto';

@Component({
  selector: 'ngx-recycle-request-assign',
  templateUrl: './recycle-request-assign.component.html',
  styleUrls: ['../../style/assign-styles.scss'],
})
export class RecycleRequestAssignComponent implements OnInit {
  mindate = new Date();
  recycleId: string = "";
  driverList: Array<DriverDTO> = new Array<DriverDTO>();
  _recycleModel: RecycleDTO = new RecycleDTO();
  public driverRequestDto = new DriverRequestDto();
  listViewModel: any[] = [];
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  isDriver: boolean = true;
  pendingConfirmation: string = "";

  public gridView: GridDataResult;
  public pageSize = 5;
  public skip = 0;
  public min: Date;
  public max: Date;

  constructor(public driverService: DriverService, public CommonService: CommonService, private route: ActivatedRoute, private router: Router, private dialogService: NbDialogService) {
  }

  ngOnInit() {
    this.recycleId = this.route.snapshot.paramMap.get("id");

    this.LoadData();
    this.loadShif();

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
    this.driverService.GetRecycleDetailById(this.recycleId).subscribe(result => {
    if (result.statusCode == 0) {
      this._recycleModel = result.data;

      this._recycleModel.collectDate = new Date(this._recycleModel.collectDate);
      this._recycleModel.collectTime = new Date(this._recycleModel.collectDate);

      if (this._recycleModel.statusName == "Collected" && !this._recycleModel.collectedPendingConfirmation)
        this.pendingConfirmation = "Collected Pending Confirmation";
      else if (this._recycleModel.statusName == "Delivered" && !this._recycleModel.deliveredPendingConfirmation)
        this.pendingConfirmation = "Delivered Pending Confirmation";
      else
        this.pendingConfirmation = this._recycleModel.statusName;

      if (result.data.recycleComments != undefined && result.data.recycleComments.length > 0) {
        this.listViewModel = result.data.recycleComments;
        this.LoadComments();
      }

      this.CalculateGP();
    }
  });
}

  AddItem() {
    var subItem = new RecycleSubItemDTO();
    subItem.weight = 1;

    this._recycleModel.recycleSubItems.push(subItem);

    this.CalculateGP();
  }

  RemoveItem(i) {
    if (this._recycleModel.statusName != 'Declined')
    {
      this._recycleModel.recycleSubItems.splice(i, 1);
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

  AssignRecycleToDriver() {
    var dt = this._recycleModel.collectDate;
    //var tm = this._recycleModel.collectTime;
    this._recycleModel.collectorDate = dt;

    this.driverService.AssignRecycleToDriver(this._recycleModel).subscribe(result => {
        this.router.navigate(['/pages/request/recycle']);
    });
  }

  async RejectRecycle(dialog: NbDialogRef<any>) {
    this.driverService.RejectRecycle(this._recycleModel).subscribe(result => {
      dialog.close();
      this.LoadData();
    });
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
  CalculateGP() {
    this._recycleModel.totalGP = 0;
    for (let i=0; i<this._recycleModel.recycleSubItems.length; i++)
      this._recycleModel.totalGP += (!this._recycleModel.recycleSubItems[i].weight? 0: Math.ceil(this._recycleModel.recycleSubItems[i].weight)) * this._recycleModel.gpv;
  }

  SMSRecycleComments() {
    var commentModel = new CommentsDTO();

    commentModel.phone = this._recycleModel.userPhone;
    commentModel.comments = this._recycleModel.comments;
    commentModel.rId = this._recycleModel.id;

    this.driverService.SMSRecycleComments(commentModel).subscribe(result => {
        this.LoadData();
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
  async SetConfirmation(dialog: NbDialogRef<any>) {
    var response = await this.driverService.SetConfirmation(this._recycleModel.orderID, this.isDriver);

    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();
    }
  }
}
