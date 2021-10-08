import { NgModule } from '@angular/core';
import { PagesComponent } from './pages.component';
import { PagesRoutingModule } from './pages-routing.module';
import { ThemeModule } from '../@theme/theme.module';
import { MiscellaneousModule } from './miscellaneous/miscellaneous.module';
import { GmapsComponent } from './maps/gmaps/gmaps.component';
import { StudentMapComponent } from './maps/studentmap/studentmap.component';
import { AgmCoreModule } from '@agm/core';
import { MapService } from './maps/service/map-service';
import { LocationLinkComponent ,UserDetailLinkComponent } from '../common/custom-control';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../common/token.interceptor';
import { NbTokenService } from '../common/auth';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ChartModule } from 'angular-highcharts';
import { HighchartsChartModule } from 'highcharts-angular';
import { HighchartsService } from './dashboard/service/highcharts.service';
import { DashboardService } from './dashboard/service/dashboard-service';
const PAGES_COMPONENTS = [
  PagesComponent,
  GmapsComponent,
  StudentMapComponent,
  DashboardComponent,
];

@NgModule({
  imports: [
    PagesRoutingModule,
    ThemeModule,
    MiscellaneousModule,
    Ng2SmartTableModule,    
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyCbQp4NlQjZNR6U0lrCCfjFLWlMfaXsAMk',
      libraries: ['places'],
    }),
    ChartModule,
    HighchartsChartModule,
  ],
  providers: [NbTokenService, MapService, { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }, HighchartsService, DashboardService],
  declarations: [
    ...PAGES_COMPONENTS, LocationLinkComponent, UserDetailLinkComponent, 
  ],
  entryComponents: [LocationLinkComponent, UserDetailLinkComponent],
})
export class PagesModule {}
