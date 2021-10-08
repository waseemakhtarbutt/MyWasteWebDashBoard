import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyWasteRoutingModule, routedComponents } from './my-waste-routing.module';
import { ScheduleComponent } from './schedule/schedule.component';
import { NbDialogModule } from '@nebular/theme';
import { GridModule } from '@progress/kendo-angular-grid';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ThemeModule } from '../../@theme/theme.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../../common/token.interceptor';
import { MywasteserviceService } from './mywasteservice.service';
import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
import {ExcelService} from '../../common/service/excel.service';


@NgModule({

  imports: [
    ThemeModule,
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),

    CommonModule,
    MyWasteRoutingModule,
    DateInputsModule,

  ],
  providers: [MywasteserviceService,ExcelService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true,  },
  ],
  declarations: [...routedComponents]
})


export class MyWasteModule { }
