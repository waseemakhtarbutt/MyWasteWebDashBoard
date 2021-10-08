import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SchoolComponent } from './school.component';
import { SchoolDetailComponent } from './detail/school-detail.component';
import { SchoolListComponent } from './list/school-list.component';
import { AdminListComponent } from './student/adminlist/adminlist.component';
import { SchoolEditComponent } from './edit/school-edit.component';
import { ComparisonComponent} from './comparison/comparison.component'
import {ProgressComponent} from './progress/progress.component'
import { AdminSuspendedListComponent } from './student/admin-suspended-list/admin-suspended-list.component';
import { BranchesComparisionComponent } from './branches-comparision/branches-comparision.component';
import { StudentsComponent } from './students/students.component';

const routes: Routes = [{
  path: '',
  component: SchoolComponent,
  children: [
    {
      path: 'list',
      component: SchoolListComponent,
    },
    {
      path: 'alist',
      component: AdminListComponent,
    },
    {
      path: 'aSuspendedSlist',
      component: AdminSuspendedListComponent,
    },
    {
      path: 'detail/:id',
      component: SchoolDetailComponent,
    },
    {
      path: 'edit/:id',
      component: SchoolEditComponent,
    },
    {
      path: 'student',
      loadChildren: './student/student.module#StudentModule',
    },
    {
      path: 'comparison',
     component: ComparisonComponent,
    },
    {
      path:'progress',
      component: ProgressComponent,
    },
    {
      path: 'branchcomparison',
     component: BranchesComparisionComponent,
    },
    {
      path: 'students',
     component: StudentsComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SchoolRoutingModule { }

export const routedComponents = [
  SchoolListComponent,
  SchoolComponent,
  SchoolDetailComponent,
  SchoolEditComponent,
  AdminListComponent,
  ComparisonComponent,
  ProgressComponent
];
