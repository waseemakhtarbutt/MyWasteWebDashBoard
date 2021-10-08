import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbDialogModule } from '@nebular/theme';
import { TokenInterceptor } from '../../common/token.interceptor';
import { ThemeModule } from '../../@theme/theme.module';
import { GridModule } from '@progress/kendo-angular-grid';
import { CommonService } from '../../common/service/common-service';
import { DateInputsModule  } from '@progress/kendo-angular-dateinputs';
import { routedSettingComponents, SettingsRoutingModule } from './settings-routing.module';
import { AddEditLevelComponent } from './add-edit-level/add-edit-level.component';
import { AddGreenPointsComponent } from './add-green-points/add-green-points.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { CreateAdComponent } from './create-ad/create-ad.component';
import { CreatemarkitplaceComponent } from './create-markitplace/create-markitplace.component';
import { AddWeightComponent } from './add-weight/add-weight.component';

@NgModule({
  imports: [
    ThemeModule,
    SettingsRoutingModule,
    Ng2SmartTableModule,
    GridModule,
    NbDialogModule.forChild(),
    DateInputsModule,
  ],
  providers: [ CommonService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
  ],
  declarations: [
    ...routedSettingComponents,
    AddEditLevelComponent,
    AddGreenPointsComponent,
    ChangePasswordComponent,
    CreateAdComponent,
    AddWeightComponent,
    CreatemarkitplaceComponent,

  ],
})
export class SettingModule { }
