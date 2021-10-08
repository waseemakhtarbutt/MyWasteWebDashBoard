import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GpnrequestComponent } from './gpnrequest.component';
import { ListComponent } from './list/list.component';
import { SchoolComponent } from './school/school/school.component';
import { AddSchoolComponent } from './school/add-school/add-school.component';
import { AddBusinessComponent } from './business/add-business/add-business.component';
import { ApprovedSchoolComponent } from './school/approved-school/approved-school.component';
import { ApprovedOrganizationComponent } from './organization/approved-organization/approved-organization.component';
import { ApprovedBusinessComponent } from './business/approved-business/approved-business.component';
import { CreateInstanceComponent } from './create-instance/create-instance.component';
import { OrganizationComponent } from './organization/organization.component';
import { BusinessComponent } from './business/business.component';
import { AddOrganizationComponent } from './organization/add-organization/add-organization.component';
import { SuspendedComponent } from './suspended/suspended.component';
import { SuspendedSchoolComponent } from './school/suspended-school/suspended-school.component';
import { SuspendedBusinessComponent } from './business/suspended-business/suspended-business.component';
import { SuspendedOrganizationComponent } from './organization/suspended-organization/suspended-organization.component';
import { ListDonationsComponent } from './donations/list-donations/list-donations.component';
import { AddDonationComponent } from './donations/add-donation/add-donation.component';
import { ListforApprovelDonationsComponent } from './donations/listfor-approvel-donations/listfor-approvel-donations.component';
import { ApproveDonationComponent } from './donations/approve-donation/approve-donation.component';
import { SchoolComparisonComponent } from './school-comparison/school-comparison.component';


const routes: Routes = [{
  path: '',
  component: GpnrequestComponent,
  children: [
    {
      path: 'list',
      component: ListComponent,
    },
    {
      path: 'list/:id',
      component: ListComponent,
    },
    {
      path: 'school',
      component: SchoolComponent,
    }, {
      path: 'schoolcomparison',
      component: SchoolComparisonComponent,
    },
    {
      path: 'addschool/:id',
      component: AddSchoolComponent,
    },
    {
      path: 'addbusiness/:id',
      component: AddBusinessComponent,
    },
    {
      path: 'addorganization/:id',
      component: AddOrganizationComponent,
    },
    {
      path: 'approvedschool',
      component: ApprovedSchoolComponent,
    },
    {
      path: 'approvedorganization',
      component: ApprovedOrganizationComponent,
    },
    {
      path: 'approvedbusiness',
      component: ApprovedBusinessComponent,
    },
    {
      path: 'createinstance',
      component: CreateInstanceComponent,
    },
    {
      path: 'suspended',
      component: SuspendedComponent,
    },
    {
      path: 'donation',
      component: ListDonationsComponent,
    },
    {
      path: 'addDonations',
      component: AddDonationComponent,
    },
    {
      path: 'addDonations/:id',
      component: AddDonationComponent,
    },
    {
      path: 'lstForApproveldonation',
      component: ListforApprovelDonationsComponent,
    },
    {
      path: 'approveDonations/:id',
      component: ApproveDonationComponent,
    },
  
 
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GpnrequestRoutingModule { }

export const routedComponents = [
  GpnrequestComponent,
  ListComponent,
  SchoolComponent,
  ApprovedSchoolComponent,
  ApprovedOrganizationComponent,
  ApprovedBusinessComponent,
  BusinessComponent,
  OrganizationComponent,
  AddSchoolComponent,
  AddOrganizationComponent,
  AddBusinessComponent,
  ApprovedOrganizationComponent,
  ApprovedBusinessComponent,
  CreateInstanceComponent,
  SuspendedComponent,
  SuspendedSchoolComponent,
  SuspendedBusinessComponent,
  SuspendedOrganizationComponent,
  AddDonationComponent,
  ListDonationsComponent,
  ListforApprovelDonationsComponent,
  ApproveDonationComponent,
  SchoolComparisonComponent
];
