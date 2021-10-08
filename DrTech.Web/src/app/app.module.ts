/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { APP_BASE_HREF } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CoreModule } from './@core/core.module';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ThemeModule } from './@theme/theme.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NbAuthModule } from './common/auth/auth.module';
import { NbPasswordAuthStrategy,NbAuthJWTToken } from './common/auth';
import { AuthGuard } from './common/auth-guard.service';
import { TokenInterceptor } from './common/token.interceptor';
import { CommonFunction } from './common/common-function';
import { UserService } from './pages/user/service/user-service';
import { NbSelectModule } from '@nebular/theme';
// import { NumberDirective } from './directives/number-only.directive';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { UserRightsComponent } from './user-rights/user-rights.component';


 //var link = "https://mywaste.azurewebsites.net/api/";
  var link = 'http://localhost:64331/api/';

@NgModule({
  declarations: [AppComponent, UserRightsComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    ThemeModule.forRoot(),
    CoreModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NbSelectModule,
    NbAuthModule.forRoot({
      strategies: [
        NbPasswordAuthStrategy.setup({
          name: 'email',
          token: {
            class: NbAuthJWTToken,
          },
          baseEndpoint: link + 'Users/',
              login: {
                endpoint: 'login' ,
                redirect: {
                  success: '/',
                  failure: null,
                }
              },
              logout: {
                method:null,
                redirect: {
                  success: '/',
                  failure: '/',
                },
              },
              register: {
                endpoint: 'RegistrationForm',
              },
        }),
      ],
      forms: {
        redirectDelay: 0,
        showMessages: {
          success: true,
        },
      },
    }),
  ],
  bootstrap: [AppComponent],
  providers: [
    UserService,
    AuthGuard,
    { provide: APP_BASE_HREF, useValue: '/' },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true},
  ],
})
export class AppModule {
}
