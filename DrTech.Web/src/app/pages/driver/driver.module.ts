import { NgModule } from '@angular/core';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { DriverRoutingModule, routedComponents } from './driver-routing.module';
import { TokenInterceptor } from '../../common/token.interceptor';
import { DriverService } from './service/driver.service';
import { ThemeModule } from '../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { CommonService } from '../../common/service/common-service';
import { TasklistComponent } from './tasklist/tasklist.component';
import { NumberDirective } from '../../directives/number-only.directive';
import { DecimalDirective } from '../../directives/decimal-only.directive';

import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
import { RegiftDriverlistComponent } from './regift-driverlist/regift-driverlist.component';
import { RecycleDriverlistComponent } from './recycle-driverlist/recycle-driverlist.component';
import { DeliveredDriverlistComponent } from './delivered-driverlist/delivered-driverlist.component';
import { AlljobsComponent } from './alljobs/alljobs.component';
import { AddGuiForRecyleComponent } from './GUIForRecycle/add-gui-for-recyle/add-gui-for-recyle.component';
import { GuiForRecycleListComponent } from './GUIForRecycle/gui-for-recycle-list/gui-for-recycle-list.component';
import { GuiBarChartComponent } from './GUIForRecycle/gui-bar-chart/gui-bar-chart.component';
import { TopPerformersComponent } from './GUIForRecycle/top-performers/top-performers.component';
import { PiChartComponent } from './GUIForRecycle/pi-chart/pi-chart.component';
import { BarChartComponent } from './GUIForRecycle/bar-chart/bar-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { RecycleBarChartComponent } from './GUIForRecycle/recycle-bar-chart/recycle-bar-chart.component';
import { RecyclePiChartComponent } from './GUIForRecycle/recycle-pi-chart/recycle-pi-chart.component';
import { GOIGraphYearWiseComponent } from './GUIForRecycle/goigraph-year-wise/goigraph-year-wise.component';
import { GOIGraphMonthWiseComponent } from './GUIForRecycle/goigraph-month-wise/goigraph-month-wise.component';
import { RecyleDetailChartComponent } from './GUIForRecycle/recyle-detail-chart/recyle-detail-chart.component';
import { CircularChartComponent } from './GUIForRecycle/circular-chart/circular-chart.component';
import { DailyGreenCreditsGraphComponent } from './GUIForRecycle/daily-green-credits-graph/daily-green-credits-graph.component';
import { DailyWeightGraphComponent } from './GUIForRecycle/daily-weight-graph/daily-weight-graph.component';
import { AddSeggregatedWasteWithTypesComponent } from './GUIForRecycle/add-seggregated-waste-with-types/add-seggregated-waste-with-types.component';
import { SegregatedGridComponent } from './GUIForRecycle/segregated-grid/segregated-grid.component';
import { DesegregatedGridComponent } from './GUIForRecycle/desegregated-grid/desegregated-grid.component';

@NgModule({
  imports: [

    ThemeModule,
    DriverRoutingModule,
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),
    DateInputsModule,
    NgxChartsModule,


  ],
  providers: [DriverService, CommonService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedComponents,
    TasklistComponent,
    RegiftDriverlistComponent,
    RecycleDriverlistComponent,
    DeliveredDriverlistComponent,
    AlljobsComponent,
    NumberDirective,
    DecimalDirective,
    AddGuiForRecyleComponent,
    GuiForRecycleListComponent,
    GuiBarChartComponent,
    TopPerformersComponent,
    PiChartComponent,
    BarChartComponent,
    RecycleBarChartComponent,
    RecyclePiChartComponent,
    GOIGraphYearWiseComponent,
    GOIGraphMonthWiseComponent,
    RecyleDetailChartComponent,
    CircularChartComponent,
    DailyGreenCreditsGraphComponent,
    DailyWeightGraphComponent,
    AddSeggregatedWasteWithTypesComponent,
    SegregatedGridComponent,
    DesegregatedGridComponent
  ],
})
export class DriverModule { }
