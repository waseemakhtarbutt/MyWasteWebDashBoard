import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {  NgoListComponent } from './list/ngo-list.component';
import { NgoDetailComponent } from './detail/ngo-detail.component';
import { NgoNeedComponent } from './need-list/need-list.component';
import { NgoComponent } from './ngo.component';
import {  AdminMemberListComponent } from './member/adminmemberlist/admin-member-list.component';
import { AdminOrgListComponent } from './adminorglist/admin-org-list.component';
import { AdminMemberSuspendedListComponent } from './member/admin-member-suspended-list/admin-member-suspended-list.component';
import { MemberProgressComponent } from './member-progress/member-progress.component';
import { ComparisonComponent } from './comparison/comparison.component';

const routes: Routes = [{
  path: '',
  component: NgoComponent,
  children: [
    {
      path: 'need',
      component: NgoNeedComponent,
    },
    {
      path: 'list',
      component: NgoListComponent,
    },
    {
      path: 'alist',
      component: AdminMemberListComponent,
    },
    {
      path: 'asuspendMlist',
      component: AdminMemberSuspendedListComponent,
    },
    {
      path: 'detail/:id',
      component: NgoDetailComponent,
    },
    {
      path: 'member',
      loadChildren: './member/member.module#MemberModule',
    },
    {
      path: 'memberProgressFor',
      component: MemberProgressComponent,
    },
    {
      path: 'comparison',
     component: ComparisonComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NgoRoutingModule { }

export const routedComponents = [
  NgoComponent,
  NgoListComponent,
  NgoDetailComponent,
  NgoNeedComponent,
  AdminOrgListComponent,
  AdminMemberSuspendedListComponent,
  MemberProgressComponent
];
