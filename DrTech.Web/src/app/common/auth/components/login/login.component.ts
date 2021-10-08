/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { NB_AUTH_OPTIONS, NbAuthSocialLink } from '../../auth.options';
import { getDeepFromObject } from '../../helpers';

import { NbAuthService } from '../../services/auth.service';
import { NbAuthResult } from '../../services/auth-result';

@Component({
  selector: 'nb-login',
  templateUrl: './login.component.html',
 // styleUrls: ['./login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NbLoginComponent {

  redirectDelay: number = 0;
  showMessages: any = {};
  strategy: string = '';

  errors: string[] = [];
  messages: string[] = [];
  user: any = {};
  submitted: boolean = false;
  socialLinks: NbAuthSocialLink[] = [];
  rememberMe = false;
  type: any;

  constructor(protected service: NbAuthService,
              @Inject(NB_AUTH_OPTIONS) protected options = {},
              protected cd: ChangeDetectorRef,
              protected router: Router) {

    this.redirectDelay = this.getConfigValue('forms.login.redirectDelay');
    this.showMessages = this.getConfigValue('forms.login.showMessages');
    this.strategy = this.getConfigValue('forms.login.strategy');
    this.socialLinks = this.getConfigValue('forms.login.socialLinks');
    this.rememberMe = this.getConfigValue('forms.login.rememberMe');

    let role = service.getRole();


    this.redirectHome(role);
  }

  login(): void {
    this.errors = [];
    this.messages = [];
    this.submitted = true;

    this.service.authenticate(this.strategy, this.user).subscribe((result: NbAuthResult) => {
      this.submitted = false;
      if (result.isSuccess() && result.getResponse().body.statusCode === 0) {
        this.messages = result.getMessages();
        this.messages = [result.getResponse().body.statusMessage];

        let pl = result.getToken().getPayload();
        let role = pl.role;

        //Set User Type in localStorage.
        localStorage.setItem('type',pl.type)
        this.type = localStorage.getItem('type');
        console.log( localStorage.getItem('type'));

        this.redirectHome(role);
      } else {
        this.showMessages.error = true;
        this.errors = result.getErrors();
        this.errors = [result.getResponse().body.statusMessage];
      }


      // const redirect = result.getRedirect();
      // if (redirect) {
      //   setTimeout(() => {
      //     return this.router.navigateByUrl(redirect);
      //   }, this.redirectDelay);
      // }
      // this.cd.detectChanges();
    });
  }

  redirectHome(role) {
    if (this.service.isAuthenticated && role === 1)
      this.router.navigate(['/pages/map']);
    else if (this.service.isAuthenticated && role === 2 || role === 7)
      this.router.navigate(['/pages/school/alist']);
    else if (this.service.isAuthenticated && role === 3 || role === 8)
      this.router.navigate(['/pages/ngo/alist']);
    else if (this.service.isAuthenticated && (role === 4 || role === 9) && this.type != "G")
    this.router.navigate(['/pages/business/alist']);
      else if (this.service.isAuthenticated && role === 10)
      this.router.navigate(['/pages/mywaste/schedule']);
      else if (this.service.isAuthenticated && (role === 4 || role === 9) && this.type === "G")
      this.router.navigate(['/pages/driver/GUIForRecycle/gui-for-recycle-list']);
      else if (this.service.isAuthenticated && role === 1 && this.type === "S")
      this.router.navigate(['/pages/driver/GUIForRecycle/add-seggregated-waste-with-types']);
      else if (this.service.isAuthenticated && (role === 11) && this.type === "G")
      this.router.navigate(['/pages/request/recyclelist']);
  }

  getConfigValue(key: string): any {
    return getDeepFromObject(this.options, key, null);
  }
}
