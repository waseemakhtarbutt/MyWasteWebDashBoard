/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NB_AUTH_OPTIONS, NbAuthSocialLink } from '../../../auth.options';
import { getDeepFromObject } from '../../../helpers';

import { NbAuthService } from '../../../services/auth.service';
import { NbAuthResult } from '../../../services/auth-result';
import { UserService } from '../../../../../pages/user/service/user-service';
import { RegistrationRequestService } from '../../../../../pages/registration-request/service/registration-request-service';
import { DropdownDTO } from '../../../../dropdown-dto';


@Component({
  selector: 'nb-business-register',
  styleUrls: ['./business-register.component.scss'],
  templateUrl: './business-register.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NbBusinessRegisterComponent implements OnInit {
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
  user: any = {};
  socialLinks: NbAuthSocialLink[] = [];
  typeList: Array<DropdownDTO> = new Array<DropdownDTO>();

  constructor(protected service: NbAuthService,
    @Inject(NB_AUTH_OPTIONS) protected options = {},
    protected cd: ChangeDetectorRef,
    protected router: Router,
    private route: ActivatedRoute,
    protected userService: UserService,
    protected regService: RegistrationRequestService) {
    

    this.redirectDelay = this.getConfigValue('forms.register.redirectDelay');
    this.showMessages = this.getConfigValue('forms.register.showMessages');
    this.strategy = this.getConfigValue('forms.register.strategy');
    this.socialLinks = this.getConfigValue('forms.login.socialLinks');

    if (navigator) {
      navigator.geolocation.getCurrentPosition(pos => {
        this.lng = +pos.coords.longitude;
        this.lat = +pos.coords.latitude;

        this.user.longitude = this.lng;
        this.user.latitude = this.lat;

        this.markers = [];
        this.markers.push({
          lat: this.lat,
          lng: this.lng,
          draggable: true
        });
      });
    }
  }
  async ngOnInit() {
    var id = this.route.snapshot.paramMap.get("id");
    this.user.ContactPersonId = id;
    var response = await this.userService.GetContactPersonDetail(id);

    if (response.statusCode == 0)
      this.user.contactPerson = response.data;

    var response2 = await this.regService.getBusinessDropdown();

    if (response2.statusCode == 0)
      this.typeList = response2.data;

    this.cd.detectChanges();
  }
  markerDragEnd(m: any, $event: MouseEvent) {
    this.user.longitude = m.lng;
    this.user.latitude = m.lat;
  }
  async register(){
    this.errors = this.messages = [];
    this.submitted = true;

    var response =await this.userService.registerBusiness(this.user,this.file);

    this.cd.detectChanges();

    if (response.data) {
      this.messages = [response.statusMessage];
    }
    else {
      this.errors = [response.statusMessage];
    }

    this.submitted = false;
    this.cd.detectChanges();

    if (response.data)
      this.router.navigate(["/auth/login"]);
  }
  file: any;
  setUploadingFile(event) {
    if (event.srcElement.files != null && event.srcElement.files.length > 0) {
      this.file = event.srcElement.files[0];
    }
  }
  getConfigValue(key: string): any {
    return getDeepFromObject(this.options, key, null);
  }
}
