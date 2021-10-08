import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentDetailComponent } from './detail/student-detail.component';
import { StudentListComponent } from './list/student-list.component';
import { StudentComponent } from './student.component';

const routes: Routes = [{
  path: '',
  component: StudentComponent,
  children: [
    {
      path: 'list',
      component: StudentListComponent,
    },
    {
      path: 'list/:id',
      component: StudentListComponent,
    },
    {
      path: 'detail/:id',
      component: StudentDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StudentRoutingModule { }

export const routedComponents = [
  StudentComponent,
  StudentListComponent,
  StudentDetailComponent,
];
