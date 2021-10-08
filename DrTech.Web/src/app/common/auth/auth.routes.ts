/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Routes, RouterModule } from '@angular/router';

import { NbAuthComponent } from './components/auth.component';
import { NbLoginComponent } from './components/login/login.component';
import { NbLogoutComponent } from './components/logout/logout.component';
import { NbRequestPasswordComponent } from './components/request-password/request-password.component';
import { NbResetPasswordComponent } from './components/reset-password/reset-password.component';
import { NbNGORegisterComponent } from './components/register/ngo/ngo-register.component';
import { NbBusinessRegisterComponent } from './components/register/business/business-register.component';
import { NbSchoolRegisterComponent } from './components/register/school/school-register.component';
import { NbAddSchoolRegisterComponent } from './components/register/add-school/add-school-register.component';
import { NbAddBusinessRegisterComponent } from './components/register/add-business/add-business-register.component';
import { NbAddNgoRegisterComponent } from './components/register/add-ngo/add-ngo-register.component';

export const AUTH_ROUTES: Routes = [
  {
    path: 'auth',
    component: NbAuthComponent,
    children: [
      {
        path: '',
        component: NbLoginComponent,
      },
      {
        path: 'login',
        component: NbLoginComponent,
      },
      {
        path: 'register-school/:id',
        component: NbSchoolRegisterComponent,
      },
      {
        path: 'register-ngo/:id',
        component: NbNGORegisterComponent,
      },
      {
        path: 'register-business/:id',
        component: NbBusinessRegisterComponent,
      },
      {
        path: 'add-school',
        component: NbAddSchoolRegisterComponent,
      },
      {
        path: 'add-ngo',
        component: NbAddNgoRegisterComponent,
      },
      {
        path: 'add-business',
        component: NbAddBusinessRegisterComponent,
      },
      {
        path: 'logout',
        component: NbLogoutComponent,
      },
      {
        path: 'request-password',
        component: NbRequestPasswordComponent,
      },
      {
        path: 'reset-password',
        component: NbResetPasswordComponent,
      },
    ],
  },
];
