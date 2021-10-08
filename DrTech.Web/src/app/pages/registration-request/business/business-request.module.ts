import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { BusinessRequestRoutingModule, routedComponents } from './business-request-routing.module';
import { TokenInterceptor } from '../../../common/token.interceptor';
import { RegistrationRequestService } from '../service/registration-request-service';
import { ThemeModule } from '../../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';

@NgModule({
  imports: [
    ThemeModule,
    BusinessRequestRoutingModule,
    Ng2SmartTableModule,
    NbDialogModule.forChild(),
    GridModule
  ],
  providers: [RegistrationRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  declarations: [
    ...routedComponents, 
  ]
})
export class BusinessRequestModule { }
