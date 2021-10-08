import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { GpnRequestRoutingModule, routedComponents } from './gpn-request-routing.module';
import { TokenInterceptor } from '../../common/token.interceptor';
import { GpnRequestService } from './service/gpn-request.service';
import { ThemeModule } from '../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { NgxChartsModule } from '@swimlane/ngx-charts';
//import { NumberDirective } from '../../directives/number-only.directive';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';


@NgModule({
  imports: [
    ThemeModule,    
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),
    CommonModule,
    GpnRequestRoutingModule,
    NgxChartsModule,
    DateInputsModule
  ],
  providers: [GpnRequestService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
    //NumberDirective,
 
  ]
 
  
})
export class GpnRequestModule { }
