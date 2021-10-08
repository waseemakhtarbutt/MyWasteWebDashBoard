import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
import { ApprovalRoutingModule, routedComponents } from './approval-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../../common/token.interceptor';
import { RequestService } from '../request/service/request-service';
import { GridModule } from '@progress/kendo-angular-grid';
import { NbDialogModule } from '@nebular/theme';


@NgModule({
  imports: [
    ThemeModule,
    ApprovalRoutingModule,
    GridModule,
    NbDialogModule.forChild(),
  ],
  providers: [RequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents, 
  ],
})
export class ApprovalModule { }
