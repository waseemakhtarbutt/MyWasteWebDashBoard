/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of as observableOf } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BaseService } from '../../../common/base.service';
import { ResponseObject } from '../../../common/response-object';
import { NeedDTO } from '../dto';
import { DropdownDTO, DonationDropdownsViewModel, DropdownViewModelWithTitle } from '../../../common/dropdown-dto';
@Injectable()
export class NgoService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  GetStatusList(): Observable<ResponseObject<Array<DropdownDTO>>> {
    const link = this.baseUrl + 'Common/GetStatusList';
    return this.http.get<ResponseObject<Array<DropdownDTO>>>(link)
      .pipe(catchError(this.handleError));
  }
  
  GetDropdownList(): Observable<ResponseObject<DonationDropdownsViewModel>> {
    const link = this.baseUrl + 'regift/GetRecipientDropdowns';
    return this.http.get<ResponseObject<DonationDropdownsViewModel>>(link)
      .pipe(catchError(this.handleError));
  }

  GetSubTypeDropdown(id): Observable<ResponseObject<DonationDropdownsViewModel>> {
    const link = this.baseUrl + 'Common/GetSubTypeDropdown?id=' + id;
    return this.http.get<ResponseObject<DonationDropdownsViewModel>>(link)
      .pipe(catchError(this.handleError));
  }

  UpdateNeedRequest(model: NeedDTO): Observable<ResponseObject<boolean>> {
    var obj = Object.assign({}, model);
    delete obj.id;
    const link = this.baseUrl + 'Ngo/UpdateNeed?id=' + model.id;
    return this.http.put<ResponseObject<boolean>>(link, obj)
      .pipe(catchError(this.handleError));
  }
  AddNeedRequest(model: NeedDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Ngo/AddNeed';
    return this.http.post<ResponseObject<boolean>>(link, model)
      .pipe(catchError(this.handleError));
  }
  GetAllNeedList(): Observable<ResponseObject<Array<NeedDTO>>> {
    const link = this.baseUrl + 'Ngo/GetNeedList';
    return this.http.get<ResponseObject<Array<NeedDTO>>>(link)
      .pipe(catchError(this.handleError));
  }
  GetMemberListByRole(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'Organization/GetMemberListByRole?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }
  GetMemberListByRoleWithMemberProgress(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'Organization/GetMemberListByRoleWithMemberProgress?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }



  
  async SuspendMember(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('Organization/SuspendMember?id=' + id);
  }
  async ActivateMember(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('Organization/ActivateMember?id=' + id);
  }

  async GetDepartmentList(): Promise<ResponseObject<boolean>> {
    return await this.Get<any>('Organization/GetDepartmentsByRole');
  }

  async GetComparisonGreenPoints(departments, organizations, fromDate, toDate): Promise<ResponseObject<any>> {
    return await this.Get<any>('Organization/GetOrganizationComparison?dpt=' + departments + '&org=' + organizations + '&fd=' + fromDate + '&td=' + toDate);
  }
  async GetBranchesList(): Promise<ResponseObject<boolean>> {
    return await this.Get<any>('Organization/GetBranchesByOrganizationAdmin');
  }
  
}
