import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GPNRequestComponent } from './gpn-request.component';

const routes: Routes = [{
  path: '',
  component: GPNRequestComponent,
  children: [
    {
      path: 'gpnrequest',
      loadChildren: './gpnrequest/gpnrequest.module#GpnrequestModule',
    },
    
  ],
}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GpnRequestRoutingModule { }
export const routedComponents = [
  GPNRequestComponent
];
