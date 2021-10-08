import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationRequestService } from '../../../registration-request/service/registration-request-service';

@Component({
  selector: 'ngx-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss'],
})
export class MemberDetailComponent implements OnInit {

  _viewModel: any = {};
  constructor(public service: RegistrationRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    const response = await this.service.GetMemberDetail(id);
    if (response.statusCode === 0) {
      this._viewModel = response.data;
    }
  }
  get isVerified() {
    return this._viewModel.verified === 'YES';
  }

  async btnUpdateStatusOnClick(status, id) {

    const response = await this.service.updateMemberStatus(status, id);
    if (response.statusCode === 0) {
      this.router.navigate(['pages/school/Member/list']);
    }
  }
}
