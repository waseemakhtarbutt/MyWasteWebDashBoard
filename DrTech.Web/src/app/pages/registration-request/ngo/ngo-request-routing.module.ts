import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NGORequestComponent } from './ngo-request.component';
import { NGOListComponent } from './list/ngo-list.component';
import { NgoDetailComponent } from './detail/ngo-detail.component';

const routes: Routes = [{
  path: '',
  component: NGORequestComponent,
  children: [
    {
      path: 'list',
      component: NGOListComponent,
    },
    {
      path: 'list/:id',
      component: NGOListComponent,
    },
    {
      path: 'detail/:id',
      component: NgoDetailComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NGORequestRoutingModule { }

export const routedComponents = [
  NGORequestComponent,
  NGOListComponent,
  NgoDetailComponent
];
