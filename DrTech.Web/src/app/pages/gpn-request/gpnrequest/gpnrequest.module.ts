import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule, NbTabsetModule,NbSpinnerModule} from '@nebular/theme';
import { GpnrequestRoutingModule, routedComponents } from './gpnrequest-routing.module';
import { TokenInterceptor } from '../../../common/token.interceptor';
import { GpnRequestService } from '../service/gpn-request.service';
import { ThemeModule } from '../../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { NbTableModule } from '@nebular/theme/components/cdk/table';
import { SuspendedSchoolComponent } from './school/suspended-school/suspended-school.component';
import { SuspendedBusinessComponent } from './business/suspended-business/suspended-business.component';
import { SuspendedOrganizationComponent } from './organization/suspended-organization/suspended-organization.component';
import { ListDonationsComponent } from './donations/list-donations/list-donations.component';
import { AddDonationComponent } from './donations/add-donation/add-donation.component';
import { ApproveDonationComponent } from './donations/approve-donation/approve-donation.component';
import { ListforApprovelDonationsComponent } from './donations/listfor-approvel-donations/listfor-approvel-donations.component';
import { SchoolComparisonComponent } from './school-comparison/school-comparison.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ChartsModule } from '@progress/kendo-angular-charts';
import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
@NgModule({
  imports: [
    ThemeModule,
    GpnrequestRoutingModule,
    Ng2SmartTableModule,
    GridModule,
    ChartsModule,
    NbDialogModule.forChild(),
    NbTableModule,
    NbSpinnerModule,
    NgxChartsModule,
    NgMultiSelectDropDownModule,
    DateInputsModule
  ],
 
  providers: [GpnRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
    ListDonationsComponent,
    AddDonationComponent,
    ApproveDonationComponent,
    ListforApprovelDonationsComponent,
    SchoolComparisonComponent,
  ],
})
export class GpnrequestModule { }


