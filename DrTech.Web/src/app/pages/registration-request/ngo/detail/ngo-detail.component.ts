import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { RegistrationRequestService } from '../../service/registration-request-service';

@Component({
  selector: 'ngx-ngo-detail',
  templateUrl: './ngo-detail.component.html',
  styleUrls: ['./ngo-detail.component.scss'],
})
export class NgoDetailComponent implements OnInit {

  _viewModel: any = {};
  isRegistered: boolean = false;
  constructor(public service: RegistrationRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    var id = this.route.snapshot.paramMap.get("id");
    var response = await this.service.GetNgoDetail(id);
    if (response.statusCode == 0) {
      this._viewModel = response.data;
    }
  }

  async btnUpdateStatusOnClick(status, id) {
    var response = await this.service.UpdateNgoStatus(id, status);
    if (response.statusCode == 0) {
      this.router.navigate(["/pages/registration/ngo/list"]);
    }
  }
}
