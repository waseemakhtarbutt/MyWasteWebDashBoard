import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ApprovalComponent } from './approval.component';
import { SchoolApprovalListComponent } from './school/school-approval-list.component';
import { NgoApprovalListComponent } from './ngo/ngo-approval-list.component';
import { BusinessApprovalListComponent } from './business/business-approval-list.component';

const routes: Routes = [{
  path: '',
  component: ApprovalComponent,
  children: [
    {
      path: 'school',
      component: SchoolApprovalListComponent,
    },
    {
      path: 'ngo',
      component: NgoApprovalListComponent,
    },
    {
      path: 'business',
      component: BusinessApprovalListComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApprovalRoutingModule { }

export const routedComponents = [
  ApprovalComponent,
  NgoApprovalListComponent,
  SchoolApprovalListComponent,
  BusinessApprovalListComponent,
];
