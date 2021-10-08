import { Component, OnInit, Inject, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';
import { NbAuthService, NB_AUTH_OPTIONS } from '../../../common/auth';
import { getDeepFromObject } from '../../../common/auth/helpers';

@Component({
  selector: 'ngx-school-edit',
  templateUrl: './school-edit.component.html',
  styleUrls: ['./school-edit.component.scss'],
})
export class SchoolEditComponent implements OnInit {

  _viewModel: any = {};
  zoom: number = 8;
  // initial center position for the map
  lat: number = 0;
  lng: number = 0;

  markers: any[] = [];

  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';

  submitted = false;
  errors: string[] = [];
  messages: string[] = [];
  user: any = { };


  constructor(private route: ActivatedRoute,
    protected service: NbAuthService,
    @Inject(NB_AUTH_OPTIONS) protected options = {},
    protected cd: ChangeDetectorRef,
    protected router: Router,
    protected regService: RegistrationRequestService) {


    this.redirectDelay = this.getConfigValue('forms.register.redirectDelay');
    this.showMessages = this.getConfigValue('forms.register.showMessages');
    this.strategy = this.getConfigValue('forms.register.strategy');

  }

  async ngOnInit() {
    //const id = this.route.snapshot.paramMap.get('id');
    const response = await this.regService.GetSchoolDetailForEdit();
    if (response.statusCode === 0) {
      this.user = response.data;

      this.markers = [];
      this.markers.push({
        lat: this.user.latitude,
        lng: this.user.longitude,
        draggable: true,
      });
    }
  }

  markerDragEnd(m: any, $event: MouseEvent) {
    this.user.longitude = m.lng;
    this.user.latitude = m.lat;
  }

  async Edit() {
    this.errors = this.messages = [];
    this.submitted = true;

    const response = await this.regService.updateSchool(this.user);
    if (response.data) {
      this.messages = [response.statusMessage];
    }
    else {
      this.errors = [response.statusMessage];
    }

    this.submitted = false;
    this.cd.detectChanges();

    if (response.data)
      this.router.navigate(['/auth/login']);
  }
  getConfigValue(key: string): any {
    return getDeepFromObject(this.options, key, null);
  }
}
