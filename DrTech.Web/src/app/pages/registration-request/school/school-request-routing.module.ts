import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SchoolRequestComponent } from './school-request.component';
import { SchoolListComponent } from './list/school-list.component';
import { SchoolDetailComponent } from './detail/school-detail.component';

const routes: Routes = [{
  path: '',
  component: SchoolRequestComponent,
  children: [
    {
      path: 'list',
      component: SchoolListComponent,
    },
    {
      path: 'list/:id',
      component: SchoolListComponent,
    },
    {
      path: 'detail/:id',
      component: SchoolDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SchoolRequestRoutingModule { }

export const routedComponents = [
  SchoolListComponent,
  SchoolRequestComponent,
  SchoolDetailComponent
];
