import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';

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
    const id = this.route.snapshot.paramMap.get('id');

    if (id === 'my') {
      const response = await this.service.GetMyBusinessDetail();
      if (response.statusCode === 0) {
        this._viewModel = response.data;
      }
    }
    else {
      const response = await this.service.GetBusinessDetail(id);
      if (response.statusCode === 0) {
        this._viewModel = response.data;
      }
    }

    
  }
}
