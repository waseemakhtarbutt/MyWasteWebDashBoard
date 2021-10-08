import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { ThemeModule } from '../../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { EmployeeRoutingModule, routedComponents } from './employee-routing.module';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';
import { TokenInterceptor } from '../../../common/token.interceptor';
import { AdminEmployeeSuspendedListComponent } from './admin-employee-suspended-list/admin-employee-suspended-list.component';

@NgModule({
  imports: [
    ThemeModule,
    EmployeeRoutingModule,
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),
  ],
  providers: [RegistrationRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
    //AdminEmployeeSuspendedListComponent, 
  ],
  exports : [...routedComponents],
})
export class EmployeeModule { }
