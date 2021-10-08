import { Injectable } from '@angular/core';
import { BaseService } from '../../../common/base.service';
import { HttpClient } from '@angular/common/http';
import { ResponseObject } from '../../../common/response-object';
import { CommonService } from '../../../common/service/common-service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SchoolRequestDto,OrganizationRequestDto, BusinessRequestDto } from '../gpnrequest/dto/dto';

@Injectable({
  providedIn: 'root'
})
export class GpnRequestService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  async GetStgSchoolList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/GetStgSchoolList');
  }
  async GetStgBusinessList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('business/GetStgBusinessList');
  }
  async GetStgOrganizationList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetStgOrganizationList');
  }
  async GetOrgNeedsList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('NGONeed/GetNGONeedListByUserID');
  }
  async GetNGONeedListInActive(): Promise<ResponseObject<any>> {
    return await this.Get<any>('NGONeed/GetNGONeedListInActive');
  }



  async GetOrgNeedsById(Id): Promise<ResponseObject<any>> {
    return await this.Get<any>('NGONeed/GetNGONeedById?Id=' + Id);
  }
  async SaveDonation(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('NGONeed/AddNGODonation', model);
  }
  async SaveApprovedDonation(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('NGONeed/ApproveCallForNGONeed', model);
  }


  async GetOrgNeedsItemsList(type: string): Promise<ResponseObject<any>> {
    return await this.Get<any>('NGONeed/GetDropdownByType?TypeName=' + type);
  }
  async GetOrgNeedsSubitemsList(Id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Common/GetDropdownByParentID?ParentID=' + Id);
  }

  async GetSchoolBranch(queryParameter): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/GetStgSchoolByID?param=' + queryParameter);
  }

  async GetRegSchool(): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/GetRegSchoolDropdown');
  }

  async GetCityList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('common/GetDropdownByType?TypeName=City');
  }
  GetAllCitys(): Observable<ResponseObject<Array<any>>> {
    const link = this.baseUrl + 'MyWaste/GetAllCity';
    return this.http.get<ResponseObject<Array<any>>>(link)
      .pipe(catchError(this.handleError));
  }
  async GetAreasByID(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('MyWaste/GetAreaByID?ID=' + id);
  }

  async SaveSchool(file, model): Promise<ResponseObject<any>> {
    return await this.UploadData<any>(file, 'school/AddSchool', model);
  }

  async InactiveStgSchool(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/InactiveStgSchool?ID=' + id);
  }
  async InactiveStgBusiness(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('business/InactiveStgBusiness?ID=' + id);
  }
  async InactiveStgOrg(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/InactiveStgOrg?ID=' + id);
  }

  async SuspendOrg(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/SuspendOrg?ID=' + id);
  }

  async SuspendBusiness(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('business/SuspendBusiness?ID=' + id);
  }

  async SuspendSchool(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/SuspendSchool?ID=' + id);
  }

  async GetApprovedSchoolsList(model: SchoolRequestDto): Promise<ResponseObject<any>> {
    return await this.Post<any>('school/GetSchoolList',model);
  }

  async GetApprovedOrganizationsList(model: OrganizationRequestDto): Promise<ResponseObject<any>> {
    return await this.Post<any>('Organization/GetOrganizationList',model);
  }
  async GetApprovedBusinessList(model: BusinessRequestDto): Promise<ResponseObject<any>> {
    return await this.Post<any>('Business/GetBusinessList',model);
  }

  async GetSuspendedSchoolsList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/GetSuspendedSchoolList');
  }
  async GetSuspendedBusinessList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Business/GetSuspendedBusinessList');
  }
  async GetSuspendedOrganizationsList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Organization/GetSuspendedOrganizationList');
  }

  async GetBusinessList(queryParameter): Promise<ResponseObject<any>> {
    return await this.Get<any>('business/GetStgBusinessByID?param=' + queryParameter);
  }

  async GetBusinessType(): Promise<ResponseObject<any>> {
    return await this.Get<any>('common/GetDropdownByType?TypeName=Business');
  }

  async SaveBusiness(file, model): Promise<ResponseObject<any>> {
    return await this.UploadData<any>(file, 'business/AddBusiness', model);
  }

  async GetRegBusiness(): Promise<ResponseObject<any>> {
    return await this.Get<any>('business/GetRegBusinessDropdown');
  }

  async GetRegOrganization(): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetRegOrganizationDropdown');
  }

  async GetOrganizationList(queryParameter): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetStgOrganizationByID?param=' + queryParameter);
  }
  async GetOrganizationType(): Promise<ResponseObject<any>> {
    return await this.Get<any>('common/GetDropdownByType?TypeName=Organization');
  }

  async SaveOrganization(file, model): Promise<ResponseObject<any>> {
    return await this.UploadData<any>(file, 'organization/AddOrganization', model);
  }

  async ActivateInstanceSchool(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('school/ActivateInstance?ID=' + id);
  }

  async ActivateInstanceBusiness(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('business/ActivateInstance?ID=' + id);
  }
  async ActivateInstanceOrg(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/ActivateInstance?ID=' + id);
  }

}
