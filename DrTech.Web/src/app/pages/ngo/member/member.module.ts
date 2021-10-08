import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { ThemeModule } from '../../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { MemberRoutingModule, routedComponents } from './member-routing.module';
import { RegistrationRequestService } from '../../registration-request/service/registration-request-service';
import { TokenInterceptor } from '../../../common/token.interceptor';
import { AdminMemberSuspendedListComponent } from './admin-member-suspended-list/admin-member-suspended-list.component';

@NgModule({
  imports: [
    ThemeModule,
    MemberRoutingModule,
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),
  ],
  providers: [RegistrationRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
   // AdminMemberSuspendedListComponent, 
  ],
  exports : [...routedComponents],
})
export class MemberModule { }
