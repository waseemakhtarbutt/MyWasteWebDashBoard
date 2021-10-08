import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ThemeModule } from '../../@theme/theme.module';
import { UserRoutingModule, routedComponents } from './user-routing.module';
import { UserService } from './service/user-service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../../common/token.interceptor';
import { NbDialogModule } from '@nebular/theme';
import { GridModule } from '@progress/kendo-angular-grid';
import { AdminUsersListComponent } from './admin-users-list/admin-users-list.component';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
@NgModule({
  imports: [
    ThemeModule,
    UserRoutingModule,
    Ng2SmartTableModule,
    NbDialogModule.forChild(),
    GridModule,
    DateInputsModule
    ],
  providers: [UserService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  declarations: [
    ...routedComponents,
    AdminUsersListComponent
  ]
})
export class UserModule { 
  
}
