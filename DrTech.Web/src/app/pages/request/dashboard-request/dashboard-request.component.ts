import { Component, OnInit, Output, EventEmitter, TemplateRef, OnDestroy } from '@angular/core';
import { RequestService } from '../service/request-service';
import { CommonService } from '../../../common/service/common-service';
import { Router } from '@angular/router';
import { NbDialogService } from '@nebular/theme';
@Component({
  selector: 'ngx-dashboard-request',
  templateUrl: './dashboard-request.component.html',
  styleUrls: ['./dashboard-request.component.scss']
})
export class DashboardRequestComponent implements OnInit,OnDestroy {

  Refuse: boolean = true;
  Reduse: boolean = true;
  Reuse: boolean = true;
  Replant: boolean = true;
  Recycle: boolean = true;
  Regift: boolean = true;
  Report: boolean = true;
  GPN: boolean = true;
  Users: boolean = true;
  bin: boolean = true;
  ArsRequestlist: any[] = [];
  public timspan : number = 100000;
  constructor(public requestService: RequestService, public commonService: CommonService, private router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.timspan = 100000;

    var GetallArsRequest = await this.requestService.GetArsRequest();
    if (GetallArsRequest.statusCode == 0) {

      this.ArsRequestlist = GetallArsRequest.data;
    }


    if(this.timspan == 100000)
    {
      setInterval(() => {
        this.LoadDAta();
      }, this.timspan);
    }
    else{
      return;
    }


  }

  ngOnDestroy() {
    this.timspan = 0;

  }
  public async LoadDAta() {
    var GetallArsRequest = await this.requestService.GetArsRequest();
    if (GetallArsRequest.statusCode == 0) {

      this.ArsRequestlist = GetallArsRequest.data;
    }

  }
  public onrequest(request) {

    switch (request) {
      case "Refuse":
        this.Reduse = true;
        this.Refuse = false;
        this.Reuse = true;
        this.Replant = true;
        this.Recycle = true;
        this.Regift = true;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Reduse":
        this.Reduse = false;
        this.Refuse = true;
        this.Reuse = true;
        this.Replant = true;
        this.Recycle = true;
        this.Regift = true;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Reuse":
        this.Reduse = true;
        this.Refuse = true;
        this.Reuse = false;
        this.Replant = true;
        this.Recycle = true;
        this.Regift = true;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Replant":
        this.Reduse = true;
        this.Refuse = true;
        this.Reuse = true;
        this.Replant = false;
        this.Recycle = true;
        this.Regift = true;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Recycle":
        this.Reduse = true;
        this.Refuse = true;
        this.Reuse = true;
        this.Replant = true;
        this.Recycle = false;
        this.Regift = true;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Regift":
        this.Reduse = true;
        this.Refuse = true;
        this.Reuse = true;
        this.Replant = true;
        this.Recycle = true;
        this.Regift = false;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Report":
        this.Reduse = true;
        this.Refuse = true;
        this.Reuse = true;
        this.Replant = true;
        this.Recycle = true;
        this.Regift = true;
        this.Report = false;
        this.GPN = true;
        this.Users = true;
        this.bin = true;
        break;
      case "Bin":
        this.Reduse = true;
        this.Refuse = true;
        this.Reuse = true;
        this.Replant = true;
        this.Recycle = true;
        this.Regift = true;
        this.Report = true;
        this.GPN = true;
        this.Users = true;
        this.bin = false;
        break;
      default:
        confirm("Sorry, that color is not in the system yet!");
    }


  }
}
