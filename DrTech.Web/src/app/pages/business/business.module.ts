import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { BusinessRoutingModule, routedComponents } from './business-routing.module';
import { TokenInterceptor } from '../../common/token.interceptor';
import { ThemeModule } from '../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { RegistrationRequestService } from '../registration-request/service/registration-request-service';
import { AdminEmpListComponent } from './employee/adminemploylist/admin-employ-list.component';
import { BusinessService } from './service/business-service';
import { AdminEmployeeSuspendedListComponent } from './employee/admin-employee-suspended-list/admin-employee-suspended-list.component';
import { ComparisonComponent } from './comparison/comparison.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ChartsModule } from '@progress/kendo-angular-charts';  
import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
import { EmployeeProgressComponent } from './employee-progress/employee-progress.component';

@NgModule({
  imports: [
    ThemeModule,
    BusinessRoutingModule,
    Ng2SmartTableModule,
    NbDialogModule.forChild(),
    GridModule,
    NgMultiSelectDropDownModule,
    DateInputsModule,
    ChartsModule,
  ],
  providers: [RegistrationRequestService, BusinessService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents, 
    AdminEmpListComponent,
    AdminEmployeeSuspendedListComponent,
    EmployeeProgressComponent,
  
    AdminEmployeeSuspendedListComponent,
    ComparisonComponent
  ],
})
export class BusinessModule { }
