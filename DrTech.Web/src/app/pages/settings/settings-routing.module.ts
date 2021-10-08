import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { SettingComponent } from './setting/setting.component';
import { WorkingHoursComponent } from './working-hours/working-hours.component';
import { GpnLevelsComponent } from './gpn-levels/gpn-levels.component';
import { SettingsComponent } from './settings.component';
import { AddEditLevelComponent } from './add-edit-level/add-edit-level.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
const routes: Routes = [{
  path: '',
  component: SettingsComponent,
  children: [
    {
      path: 'setting',
      component: SettingComponent,
    },
    {
      path: 'listGPLevels',
      component: GpnLevelsComponent,
    },
    {
      path: 'addWHours',
      component: AddEditLevelComponent,
    },
    {
      path: 'addWHours/:id',
      component: AddEditLevelComponent,
    },
    {
      path: 'config',
      component: ChangePasswordComponent,
    },
  ],
}];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingsRoutingModule { }
export const routedSettingComponents = [
   SettingsComponent,
   WorkingHoursComponent,
   GpnLevelsComponent,
   SettingComponent,
   AddEditLevelComponent,
   ChangePasswordComponent
];


