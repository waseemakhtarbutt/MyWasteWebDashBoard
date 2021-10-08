import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DriverService } from '../../service/driver.service';
import { CommonService } from '../../../../common/service/common-service';
import { RecycleDTO } from '../../../request/dto';
import { DropdownDTO } from '../../../../common/dropdown-dto';
import { RecycleSubItemDTO } from '../../../request/dto/recycle-subitem-dto';
import { DriverDTO } from '../../dto/driver.dto';
import { BinDTO } from '../../../request/dto/bin-dto';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { CommentsDTO } from '../../../request/dto/comments-dto';
import { DriverRequestDto } from '../../dto/driver.dto';
@Component({
  selector: 'ngx-bin-request-assign',
  templateUrl: './bin-request-assign.component.html',
  styleUrls: ['../../style/assign-styles.scss'],
})
export class BinRequestAssignComponent implements OnInit {
  mindate = new Date();
  binId: string = "";
  driverList: Array<DriverDTO> = new Array<DriverDTO>();
  _binModel: BinDTO = new BinDTO();
  listViewModel: any[] = [];
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  public driverRequestDto = new DriverRequestDto();
  public gridView: GridDataResult;
  public pageSize = 5;
  public skip = 0;
  public min: Date;
  public max: Date;
  constructor(public driverService: DriverService, public CommonService: CommonService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {
    this.binId = this.route.snapshot.paramMap.get("id");

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

  LoadData() {
    this.driverService.GetBinDetailById(this.binId).subscribe(result => {
      if (result.statusCode == 0) {
        this._binModel = result.data;

        this._binModel.deliverDate = new Date(this._binModel.deliverDate);
        this._binModel.deliverTime = new Date(this._binModel.deliverDate);

        if (result.data.buyBinComments != undefined && result.data.buyBinComments.length > 0) {
          this.listViewModel = result.data.buyBinComments;
          this.LoadComments();
        }

        this.Calculate();
    }
    });
  }

  SMSBinComments() {
    var commentModel = new CommentsDTO();

    commentModel.phone = this._binModel.userPhone;
    commentModel.comments = this._binModel.comments;
    commentModel.rId = this._binModel.id;

    this.driverService.SMSBinComments(commentModel).subscribe(result => {
        this.LoadData();
    });
  }

  private LoadComments(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
    };
  }

  AssignBinToDriver() {
    var dt = this._binModel.deliverDate;
    var tm = this._binModel.deliverTime;
    this._binModel.deliveryDate = new Date((dt.getMonth()+1) + " " + dt.getDate() +", " + dt.getFullYear() + " " + tm.getHours() + ":" + tm.getMinutes() + ":" + tm.getSeconds()).toLocaleString('ur-PK');

    this.driverService.AssignBinToDriver(this._binModel).subscribe(result => {
        this.router.navigate(['/pages/request/buybin']);
    });
  }

  Calculate() {
    this._binModel.totalGP = 0;
    this._binModel.totalPrice = 0;
    for (let i=0; i<this._binModel.binSubItems.length; i++) {
      this._binModel.totalGP += (!this._binModel.binSubItems[i].qty? 0: this._binModel.binSubItems[i].qty) * this._binModel.gpv;
      this._binModel.totalPrice += (!this._binModel.binSubItems[i].qty? 0: this._binModel.binSubItems[i].qty) * this._binModel.price;
    }
  }

  QtyInc(subitem) {
    if (subitem.qty < 999 && this._binModel.statusName != 'Declined' && this._binModel.statusName != 'Delivered')
    {
      subitem.qty++;
      this.Calculate();
    }
  }

  QtyDec(subitem) {
    if (subitem.qty > 1 && this._binModel.statusName != 'Declined' && this._binModel.statusName != 'Delivered')
    {
      subitem.qty--;
      this.Calculate();
    }
  }

  loadShif(){
    this.driverService.Getshift().subscribe(result => {
      if (result.statusCode == 0) {
       new Date(2000, 2, 10, 10, 0)
       this.min =  new Date(2000, 2, 10, result.data.startTimeHours, result.data.startTimeMinutes);
       this.max = new Date(2000, 2, 10, result.data.endTimeHours, result.data.endTimeMinutes);
       console.log(this.min, this.max);
      }
    });
  }
}
