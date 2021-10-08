import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ThemeModule } from '../../../@theme/theme.module';
import { UserListRoutingModule, routedComponents } from './user-list-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../../../common/token.interceptor';
import { NbDialogModule } from '@nebular/theme';
import { UserService } from '../service/user-service';
import { BasicComponent } from './basic/basic.component';
import { GridModule } from '@progress/kendo-angular-grid';
import { RegisteredComponent } from './registered/registered.component';
import { SystemAdminsListComponent } from './system-admins-list/system-admins-list.component';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';

@NgModule({
  imports: [
    ThemeModule,
    UserListRoutingModule,
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
    BasicComponent,
    RegisteredComponent,
    SystemAdminsListComponent, 
  ]
})
export class UserListModule { }
