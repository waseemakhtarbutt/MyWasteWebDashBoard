import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ThemeModule } from '../../@theme/theme.module';
import { EmailRoutingModule, routedComponents } from './email-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../../common/token.interceptor';
import { NbDialogModule } from '@nebular/theme';
import { EmailService } from './service/email-service';
import { GridModule } from '@progress/kendo-angular-grid';

@NgModule({
  imports: [
    ThemeModule,
    EmailRoutingModule,
    Ng2SmartTableModule,
    NbDialogModule.forChild(),
    GridModule,
  ],
  providers: [EmailService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  declarations: [
    ...routedComponents, 
  ]
})
export class EmailModule { }
