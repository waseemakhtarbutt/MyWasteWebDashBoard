import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { RegistrationRequestService } from '../../service/registration-request-service';

@Component({
  selector: 'ngx-business-detail',
  templateUrl: './business-detail.component.html',
  styleUrls: ['./business-detail.component.scss'],
})
export class BusinessDetailComponent implements OnInit {

  _viewModel: any = {};
  isRegistered: boolean = false;
  constructor(public service: RegistrationRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    var id = this.route.snapshot.paramMap.get("id");
    var response = await this.service.GetBusinessDetail(id);
    if (response.statusCode == 0) {
      this._viewModel = response.data;
    }
  }

  async btnUpdateStatusOnClick(status, id) {
    var response = await this.service.UpdateBusinessStatus(id, status);
    if (response.statusCode == 0) {
      this.router.navigate(["/pages/registration/business/list"]);
    }
  }
}
