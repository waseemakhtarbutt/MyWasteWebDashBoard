import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ScheduleComponent } from './schedule/schedule.component';
import { CompanyComponent } from './company/company.component';
import { MyWasteComponent } from './mywaste.component';

const routes: Routes = [{
  path: '',
  component: MyWasteComponent,
  children: [
    {
      path: 'schedule',
      component: ScheduleComponent,
    },
    {
      path: 'Newcompany',
      component: CompanyComponent,
    },
     {
      path: 'editcompany/:id',
      component: CompanyComponent,
    },
  ],


}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyWasteRoutingModule { }
export const routedComponents = [
  MyWasteComponent,
  ScheduleComponent,
  CompanyComponent
];

