import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { NgoRoutingModule, routedComponents } from './ngo-routing.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { NgoService } from './service/ngo-service';
import { ThemeModule } from '../../@theme/theme.module';
import { RegistrationRequestService } from '../registration-request/service/registration-request-service';
import { TokenInterceptor } from '../../common/token.interceptor';
import { AdminMemberListComponent } from './member/adminmemberlist/admin-member-list.component';
import { AdminMemberSuspendedListComponent } from './member/admin-member-suspended-list/admin-member-suspended-list.component';
import { MemberProgressComponent } from './member-progress/member-progress.component';
import { ChartsModule } from '@progress/kendo-angular-charts';  
import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
import { ComparisonComponent } from './comparison/comparison.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';

@NgModule({
  imports: [
    ThemeModule,
    NgoRoutingModule,
    Ng2SmartTableModule,
    NbDialogModule.forChild(),
    GridModule,
    DateInputsModule,
    ChartsModule,
    NgMultiSelectDropDownModule
  ],
  providers: [RegistrationRequestService, NgoService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents, 
    AdminMemberListComponent,
    AdminMemberSuspendedListComponent,
    MemberProgressComponent,
    ComparisonComponent
  ],
})
export class NgoModule { }
