import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ThemeModule } from '../../@theme/theme.module';
import { RegistrationRequestRoutingModule, routedComponents } from './registration-request-routing.module';
import { RegistrationRequestService } from './service/registration-request-service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../../common/token.interceptor';
import { NbDialogModule } from '@nebular/theme';

@NgModule({
  imports: [
    ThemeModule,
    RegistrationRequestRoutingModule,
    Ng2SmartTableModule,
    NbDialogModule.forChild(),
    
  ],
  providers: [RegistrationRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  declarations: [
    ...routedComponents,
   
  ]
})
export class RegistrationRequestModule { }
