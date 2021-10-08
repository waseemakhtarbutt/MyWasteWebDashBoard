import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmployeeDetailComponent } from './detail/employee-detail.component';
import { EmployeeListComponent } from './list/employee-list.component';
import { EmployeeComponent } from './employee.component';
import { AdminEmployeeSuspendedListComponent } from './admin-employee-suspended-list/admin-employee-suspended-list.component';

const routes: Routes = [{
  path: '',
  component: EmployeeComponent,
  children: [
    {
      path: 'list',
      component: EmployeeListComponent,
    },   
   
    {
      path: 'list/:id',
      component: EmployeeListComponent,
    },
    {
      path: 'detail/:id',
      component: EmployeeDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EmployeeRoutingModule { }

export const routedComponents = [
  EmployeeComponent,
  EmployeeListComponent,
  EmployeeDetailComponent,
];
