import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { ThemeModule } from '../../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { StudentRoutingModule, routedComponents } from './student-routing.module';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';
import { TokenInterceptor } from '../../../common/token.interceptor';
import { AdminSuspendedListComponent } from './admin-suspended-list/admin-suspended-list.component';
import { AdminSuspendedStaffListComponent } from './admin-suspended-staff-list/admin-suspended-staff-list.component';
import { AdminSuspendedStudentListComponent } from './admin-suspended-student-list/admin-suspended-student-list.component';

@NgModule({
  imports: [
    ThemeModule,
    StudentRoutingModule,
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),
  ],
  providers: [RegistrationRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
    AdminSuspendedListComponent,
    AdminSuspendedStaffListComponent,
    AdminSuspendedStudentListComponent, 
  ],
  exports : [...routedComponents],
})
export class StudentModule { }
