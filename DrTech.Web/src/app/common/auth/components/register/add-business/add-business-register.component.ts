/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { NB_AUTH_OPTIONS } from '../../../auth.options';
import { getDeepFromObject } from '../../../helpers';

import { NbAuthService } from '../../../services/auth.service';
import { RegistrationRequestService } from '../../../../../pages/registration-request/service/registration-request-service';

@Component({
  selector: 'nb-add-business-register',
  styleUrls: ['./add-business-register.component.scss'],
  templateUrl: './add-business-register.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NbAddBusinessRegisterComponent {

  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';

  submitted = false;
  errors: string[] = [];
  messages: string[] = [];

  model:any = {};

  constructor(protected service: NbAuthService,
    @Inject(NB_AUTH_OPTIONS) protected options = {},
    protected cd: ChangeDetectorRef,
    protected router: Router,
    protected regService: RegistrationRequestService) {

    this.redirectDelay = this.getConfigValue('forms.register.redirectDelay');
    this.showMessages = this.getConfigValue('forms.register.showMessages');
    this.strategy = this.getConfigValue('forms.register.strategy');
  }

  async Add() {
    this.errors = this.messages = [];
    this.submitted = true;

    const response = await this.regService.addBusiness(this.model.fullName, this.file);
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
  file: any;
  setUploadingFile(event) {
    if (event.srcElement.files != null && event.srcElement.files.length > 0) {
      this.file = event.srcElement.files[0];
    }
  }
}
