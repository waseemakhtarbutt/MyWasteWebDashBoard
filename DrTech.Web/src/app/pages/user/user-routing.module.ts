import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UserComponent } from './user.component';
import { UserDetailComponent } from './detail/user-detail.component';
import { AdminUsersListComponent } from './admin-users-list/admin-users-list.component';

const routes: Routes = [{
  path: '',
  component: UserComponent,
  children: [
    {
      path: 'list',
      loadChildren: './user-list/user-list.module#UserListModule',
    },
    {
      path: 'detail/:id',
      component: UserDetailComponent,
    },
    // {
    //   path: 'adminsList',
    //   component: AdminUsersListComponent,
    // },

  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule { }

export const routedComponents = [
  UserComponent,
  UserDetailComponent
];
