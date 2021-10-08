import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DriverComponent } from './driver.component';
import { ListComponent } from './list/list.component';
import { AddDriverComponent } from './add-driver/add-driver.component';
import { DetailComponent } from './detail/detail.component';
import { Driver } from 'selenium-webdriver/chrome';
import { RegiftRequestAssignComponent } from './assign-driver/regift-request-assign/regift-request-assign.component';
import { RecycleRequestAssignComponent } from './assign-driver/recycle-request-assign/recycle-request-assign.component';
import { UpdateRecycleRequestAssignComponent } from './assign-driver/update-recycle-request-assign/update-recycle-request-assign.component';
import { BinRequestAssignComponent } from './assign-driver/bin-request-assign/bin-request-assign.component';
import { TasklistComponent } from './tasklist/tasklist.component';
import { GuiForRecycleListComponent } from './GUIForRecycle/gui-for-recycle-list/gui-for-recycle-list.component';
import { AddGuiForRecyleComponent } from './GUIForRecycle/add-gui-for-recyle/add-gui-for-recyle.component';
import { TopPerformersComponent } from './GUIForRecycle/top-performers/top-performers.component';
import { AddSeggregatedWasteWithTypesComponent } from './GUIForRecycle/add-seggregated-waste-with-types/add-seggregated-waste-with-types.component';

const routes: Routes = [{
  path: '',
  component: DriverComponent,
  children: [
    {
      path: 'list',
      component: ListComponent,
    },
    {
      path: 'adddriver',
      component: AddDriverComponent,
    },
    {
      path: 'editdriver/:id',
      component: AddDriverComponent,
    },
    {
      path: 'detail/:id',
      component: DetailComponent,
    },
    {
      path: 'assign-driver/regift-assign/:id',
      component: RegiftRequestAssignComponent,
    },
    {
      path: 'assign-driver/recycle-assign/:id',
      component: RecycleRequestAssignComponent,
    },
    {
      path: 'assign-driver/update-recycle-assign/:id',
      component: UpdateRecycleRequestAssignComponent,
    },
    {
      path: 'assign-driver/bin-assign/:id',
      component: BinRequestAssignComponent,
    },
    {
      path: 'tasklist/:id',
      component: TasklistComponent,
    },
    {
      path: 'GUIForRecycle/gui-for-recycle-list',
      component: GuiForRecycleListComponent,
    },
    {
      path: 'GUIForRecycle/add-gui-recycle',
      component: AddGuiForRecyleComponent,
    },
    {
      path: 'GUIForRecycle/amal-top-performer',
      component: TopPerformersComponent,
    },
    {
      path: 'GUIForRecycle/add-seggregated-waste-with-types',
      component: AddSeggregatedWasteWithTypesComponent,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DriverRoutingModule { }
export const routedComponents = [
  ListComponent,
  AddDriverComponent,
  DetailComponent,
  DriverComponent,
  RegiftRequestAssignComponent,
  RecycleRequestAssignComponent,
  UpdateRecycleRequestAssignComponent,
  BinRequestAssignComponent,
];