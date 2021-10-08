import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { ThemeModule } from '../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
import { SchoolRoutingModule, routedComponents } from './school-routing.module';
import { RegistrationRequestService } from '../registration-request/service/registration-request-service';
import { TokenInterceptor } from '../../common/token.interceptor';
import { AgmCoreModule } from '@agm/core';
import { StudentListComponent } from './student/list/student-list.component';
import { StudentModule } from './student/student.module';
import { AdminStudentListComponent } from './student/adminstudentlist/admin-student-list.component';
import { AdminStaffListComponent } from './student/adminstafflist/admin-staff-list.component';
import { ComparisonComponent } from './comparison/comparison.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { ProgressComponent } from './progress/progress.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { BranchesComparisionComponent } from './branches-comparision/branches-comparision.component';

@NgModule({
  imports: [
    ThemeModule,
    SchoolRoutingModule,
    GridModule,
    NbDialogModule.forChild(),
    StudentModule,
    DateInputsModule,
    ChartsModule,
    AgmCoreModule,
    NgMultiSelectDropDownModule.forRoot(),
    NgxChartsModule
  ],
  providers: [RegistrationRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
    AdminStudentListComponent,
    AdminStaffListComponent,
    ComparisonComponent,
    ProgressComponent,
    BranchesComparisionComponent
  ],
  exports: [...routedComponents ],
})
export class SchoolModule { }
