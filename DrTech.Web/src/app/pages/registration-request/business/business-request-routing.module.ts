import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BusinessRequestComponent } from './business-request.component';
import { BusinessListComponent } from './list/business-list.component';
import { BusinessDetailComponent } from './detail/business-detail.component';

const routes: Routes = [{
  path: '',
  component: BusinessRequestComponent,
  children: [
    {
      path: 'list',
      component: BusinessListComponent,
    },
    {
      path: 'list/:id',
      component: BusinessListComponent,
    },
    {
      path: 'detail/:id',
      component: BusinessDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BusinessRequestRoutingModule { }

export const routedComponents = [
  BusinessRequestComponent,
  BusinessListComponent,
  BusinessDetailComponent
];
