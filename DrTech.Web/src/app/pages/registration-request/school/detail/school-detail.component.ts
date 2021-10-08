import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { RegistrationRequestService } from '../../service/registration-request-service';
import { Location } from '@angular/common';

@Component({
  selector: 'ngx-school-detail',
  templateUrl: './school-detail.component.html',
  styleUrls: ['./school-detail.component.scss'],
})
export class SchoolDetailComponent implements OnInit {

  _viewModel: any = {};
  isRegistered: boolean = false;
  constructor(public service: RegistrationRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    var id = this.route.snapshot.paramMap.get("id");
    var response = await this.service.GetSchoolDetail(id);
    if (response.statusCode == 0) {
      this._viewModel = response.data;
    }
  }

  async btnUpdateStatusOnClick(status, id) {
    var response = await this.service.UpdateSchoolStatus(id, status);
    if (response.statusCode == 0) {
      this.router.navigate(["/pages/registration/school/list"]);
    }
  }
}
