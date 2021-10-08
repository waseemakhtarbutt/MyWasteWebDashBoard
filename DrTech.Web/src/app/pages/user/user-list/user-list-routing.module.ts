import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UserListComponent } from './user-list.component';
import { BasicComponent } from './basic/basic.component';
import { RegisteredComponent } from './registered/registered.component';
import { AdminUsersListComponent } from '../admin-users-list/admin-users-list.component';
import { SystemAdminsListComponent } from './system-admins-list/system-admins-list.component';

const routes: Routes = [{
  path: '',
  component: UserListComponent,
  children: [
    {
      path: 'basic',
      component: BasicComponent,
    },
    {
      path: 'registered',
      component: RegisteredComponent,
    },
    {
      path: 'systemusers',
      component: SystemAdminsListComponent,
    }
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserListRoutingModule { }

export const routedComponents = [
  UserListComponent,
  BasicComponent,
  RegisteredComponent,
  SystemAdminsListComponent
];
