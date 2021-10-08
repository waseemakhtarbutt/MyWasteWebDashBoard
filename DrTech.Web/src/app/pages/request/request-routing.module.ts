import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RequestComponent } from './request.component';
import { ReportRequestComponent } from './report-request/report-request.component';
import { RegiftRequestComponent } from './regift-request/regift-request.component';
import { RegiftRequestApprovalComponent } from './regift-request-approval/regift-request-approval.component';
import { RecycleRequestComponent } from './recycle-request/recycle-request.component';
import { RefuseRequestComponent } from './refuse-request/refuse-request.component';
import { ReplantRequestComponent } from './replant-request/replant-request.component';
import { ReuseRequestComponent } from './reuse-request/reuse-request.component';
import { BuyBinRequestComponent } from './buybin-request/buybin-request.component';
import { ReduceRequestComponent } from './reduce-request/reduce-request.component';
import { DashboardRequestComponent} from './dashboard-request/dashboard-request.component';
import { RecycleallRequestComponent } from './recycleall-request/recycleall-request.component';
import { RecyclelistRequestComponent } from './recyclelist-request/recyclelist-request.component';

const routes: Routes = [{
  path: '',
  component: RequestComponent,
  children: [
    {
      path: 'report',
      component: ReportRequestComponent,
    },
    {
      path: 'report/:id',
      component: ReportRequestComponent,
    },
    {
      path: 'regift',
      component: RegiftRequestComponent,
    },
    {
      path: 'regift/:id',
      component: RegiftRequestComponent,
    },
    {
      path: 'recycle',
      component: RecycleRequestComponent,
    },
    {
      path: 'recycleall',
      component: RecycleallRequestComponent,
    },
    {
      path: 'recyclelist',
      component: RecyclelistRequestComponent,
    },
    {
      path: 'recycle/:id',
      component: RecycleRequestComponent,
    },

    {
      path: 'refuse',
      component: RefuseRequestComponent,
    },
    {
      path: 'refuse/:id',
      component: RefuseRequestComponent,
    },
    {
      path: 'reduce',
      component: ReduceRequestComponent,
    },
    {
      path: 'reduce/:id',
      component: ReduceRequestComponent,
    },
    {
      path: 'replant',
      component: ReplantRequestComponent,
    },
    {
      path: 'replant/:id',
      component: ReplantRequestComponent,
    },
    {
      path: 'reuse',
      component: ReuseRequestComponent,
    },
    {
      path: 'reuse/:id',
      component: ReuseRequestComponent,
    },
    {
      path: 'regift-approval',
      component: RegiftRequestApprovalComponent,
    },
    {
      path: 'buybin',
      component: BuyBinRequestComponent,
    },
    {
      path: 'requestDashboard',
      component: DashboardRequestComponent,
    }  

  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RequestRoutingModule { }

export const routedComponents = [
  RequestComponent,
  ReportRequestComponent,
  RegiftRequestComponent,
  RecycleRequestComponent,
  RegiftRequestApprovalComponent,
  RefuseRequestComponent,
  ReduceRequestComponent,
  ReplantRequestComponent,
  ReuseRequestComponent,
  BuyBinRequestComponent,
  DashboardRequestComponent,
  RecycleallRequestComponent,
  RecyclelistRequestComponent,
];
