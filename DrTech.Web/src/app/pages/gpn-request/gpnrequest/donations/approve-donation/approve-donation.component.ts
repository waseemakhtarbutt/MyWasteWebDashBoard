import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GpnRequestService } from '../../../service/gpn-request.service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'ngx-approve-donation',
  templateUrl: './approve-donation.component.html',
  styleUrls: ['./approve-donation.component.scss']
})

export class ApproveDonationComponent implements OnInit {

  _viewModel: any = {};
  itemslist: any;
  subitemslist: any;
  show: boolean = false;
  donationFlag = false;

  constructor(public gpnRequestService: GpnRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    var ID = this.route.snapshot.paramMap.get("id");
    if (ID != null) {
      // this.showHide=true;
      var listresponse = await this.gpnRequestService.GetOrgNeedsById(ID);
      this.show = true;
      if (listresponse.statusCode == 0)
     
      this._viewModel = listresponse.data;     
      this._viewModel.title = "Approve Call for Donation";
    }
    else{
      this.show = false;
      this._viewModel.title = "Add Call for Donation";
    }
   
     

    var response = await this.gpnRequestService.GetOrgNeedsItemsList("DonationType");
    if (response.statusCode == 0)
    this.itemslist = response.data;  
  }

  async LoadSubItems(id: number) {
    var response = await this.gpnRequestService.GetOrgNeedsSubitemsList(id);
    if (response.statusCode == 0)
      this.subitemslist = response.data;
  }

  async onSubmit() {
    var formResponse = await this.gpnRequestService.SaveApprovedDonation(this._viewModel);
    if (formResponse.statusCode == 0)
      this.router.navigate(["/pages/gpn/gpnrequest/lstForApproveldonation"]);

  }

  getSelectedOptionText(event: Event) {
    let selectElementText = event.target['options']
    [event.target['options'].selectedIndex].text;
    if (selectElementText == "Other") {
      this.donationFlag = true;
      this.subitemslist = null;
    }
    else {
      this._viewModel.typeDescription = "";
      this._viewModel.subTypeDescription = "";
      this.donationFlag = false;

    }

  }

}
