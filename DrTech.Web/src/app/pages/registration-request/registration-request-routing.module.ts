import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegistrationRequestComponent } from './registration-request.component';


const routes: Routes = [{
  path: '',
  component: RegistrationRequestComponent,
  children: [
    {
      path: 'school',
      loadChildren: './school/school-request.module#SchoolRequestModule',
    },
    {
      path: 'ngo',
      loadChildren: './ngo/ngo-request.module#NGORequestModule',
    },
    {
      path: 'business',
      loadChildren: './business/business-request.module#BusinessRequestModule',
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RegistrationRequestRoutingModule { }

export const routedComponents = [
  RegistrationRequestComponent
];
