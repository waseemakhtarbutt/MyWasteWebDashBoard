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

@Injectable()
export class BusinessService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  GetEmployListByRole(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'Business/GetEmployListByRole?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }
  GetEmployListByRoleWithEmployeeProgress(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'Business/GetEmployListByRoleWithEmployeeProgress?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }
  
  async SuspendEmploy(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('Business/SuspendEmploy?id=' + id);
  }
  async ActivateEmployee(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('Business/ActivateEmployee?Id=' + id);
  }

  async GetDepartmentList(): Promise<ResponseObject<boolean>> {
    return await this.Get<any>('Business/GetDepartmentsByRole');
  }

  async GetComparisonGreenPoints(departments, businesses, fromDate, toDate): Promise<ResponseObject<any>> {
    return await this.Get<any>('Business/GetBusinessComparison?dpt=' + departments + '&bus=' + businesses + '&fd=' + fromDate + '&td=' + toDate);
  }
  async GetBranchesList(): Promise<ResponseObject<boolean>> {
    return await this.Get<any>('Business/GetBranchesByBusinessAdmin');
  }
}
