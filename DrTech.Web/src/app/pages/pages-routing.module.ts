import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from './pages.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';
import { GmapsComponent } from './maps/gmaps/gmaps.component';
import { StudentMapComponent } from './maps/studentmap/studentmap.component';
import { DashboardComponent } from './dashboard/dashboard.component';


const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'dashboard',
      component: DashboardComponent,
    },
    {
      path: 'map',
      component: GmapsComponent,
    },
    {
      path: 'request',
      loadChildren: './request/request.module#RequestModule',
    },
    {
      path: 'approval',
      loadChildren: './approval/approval.module#ApprovalModule',
    },
    {
      path: 'ngo',
      loadChildren: './ngo/ngo.module#NgoModule',
    }, {
      path: 'maps',
      component: GmapsComponent,
    },
    {
      path: 'smap',
      component: StudentMapComponent,
    }, {
      path: 'email',
      loadChildren: './email/email.module#EmailModule',
    },
    {
      path: 'user',
      loadChildren: './user/user.module#UserModule',
    },
    {
      path:'driver',
      loadChildren: './driver/driver.module#DriverModule'
    },

    {
      path: 'tables',
      loadChildren: './tables/tables.module#TablesModule',
    },
    {
      path: 'registration',
      loadChildren: './registration-request/registration-request.module#RegistrationRequestModule',
    },
    {
      path: 'gpn',
      loadChildren: './gpn-request/gpn-request.module#GpnRequestModule',
    },
    {
      path: 'driver',
      loadChildren: './driver/driver.module#DriverModule',
     
    },
    {
      path:'settings',
      loadChildren: './settings/setting.module#SettingModule'
    },
    {     
      path: 'school',
      loadChildren: './school/school.module#SchoolModule',
    },
    {
      path: 'business',
      loadChildren: './business/business.module#BusinessModule',
    },
    {
      path: 'mywaste',
      loadChildren: './my-waste/my-waste.module#MyWasteModule',
    },
    {
      path: '',
      redirectTo: 'maps',
      pathMatch: 'full',
    }, {
      path: '**',
      component: NotFoundComponent,
    }
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
