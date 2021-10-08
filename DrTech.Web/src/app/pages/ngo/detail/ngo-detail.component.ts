import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';

@Component({
  selector: 'ngx-ngo-detail',
  templateUrl: './ngo-detail.component.html',
  styleUrls: ['./ngo-detail.component.scss'],
})
export class NgoDetailComponent implements OnInit {
  _viewModel: any = {};
  constructor(public service: RegistrationRequestService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id === 'my') {
      const response = await this.service.GetMyNgoDetail();
      if (response.statusCode === 0) {
        this._viewModel = response.data;
      }
    }
    else {
      const response = await this.service.GetNgoDetail(id);
      if (response.statusCode === 0) {
        this._viewModel = response.data;
      }
    }
    
  }
}
