import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BusinessComponent } from './business.component';
import { BusinessListComponent } from './list/business-list.component';
import { BusinessDetailComponent } from './detail/business-detail.component';
import { AdminEmpListComponent } from './employee/adminemploylist/admin-employ-list.component';
import { AdminEmployeeSuspendedListComponent } from './employee/admin-employee-suspended-list/admin-employee-suspended-list.component';
import { EmployeeProgressComponent } from './employee-progress/employee-progress.component';
import { ComparisonComponent } from './comparison/comparison.component';

const routes: Routes = [{
  path: '',
  component: BusinessComponent ,
  children: [
    {
      path: 'list',
      component: BusinessListComponent,
    },
    {
      path: 'alist',
      component: AdminEmpListComponent,
    },
    {
      path: 'suslistEmployee',
      component: AdminEmployeeSuspendedListComponent,
    },    
    {
      path: 'list/:id',
      component: BusinessListComponent,
    },
    {
      path: 'detail/:id',
      component: BusinessDetailComponent,
    },
    {
      path: 'employee',
      loadChildren: './employee/employee.module#EmployeeModule',
    },
    {
      path: 'employeeProgress',
      component: EmployeeProgressComponent,
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
export class BusinessRoutingModule { }

export const routedComponents = [
  BusinessComponent,
  BusinessListComponent,
  BusinessDetailComponent
];
