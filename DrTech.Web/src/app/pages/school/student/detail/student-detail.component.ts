import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationRequestService } from '../../../registration-request/service/registration-request-service';

@Component({
  selector: 'ngx-student-detail',
  templateUrl: './student-detail.component.html',
  styleUrls: ['./student-detail.component.scss'],
})
export class StudentDetailComponent implements OnInit {

  _viewModel: any = {};
  constructor(public service: RegistrationRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    const response = await this.service.GetStudentDetail(id);
    if (response.statusCode === 0) {
      this._viewModel = response.data;
    }
  }
  get isVerified() {
    return this._viewModel.verified === 'YES';
  }

  async btnUpdateStatusOnClick(status, id) {

    const response = await this.service.updateStudentStatus(status, id);
    if (response.statusCode === 0) {
      this.router.navigate(['pages/school/student/list']);
    }
  }
}
