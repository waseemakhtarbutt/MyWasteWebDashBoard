import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MemberDetailComponent } from './detail/member-detail.component';
import { MemberListComponent } from './list/member-list.component';
import { MemberComponent } from './member.component';

const routes: Routes = [{
  path: '',
  component: MemberComponent,
  children: [
    {
      path: 'list',
      component: MemberListComponent,
    },
    {
      path: 'list/:id',
      component: MemberListComponent,
    },
    {
      path: 'detail/:id',
      component: MemberDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MemberRoutingModule { }

export const routedComponents = [
  MemberComponent,
  MemberListComponent,
  MemberDetailComponent,
];
