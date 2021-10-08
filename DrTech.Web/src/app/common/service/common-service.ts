/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, of as observableOf } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BaseService } from '../../common/base.service';
import { ResponseObject } from '../../common/response-object';
import { DropdownDTO } from '../../common/dropdown-dto';

@Injectable()
export class CommonService extends BaseService {

  constructor(public route: ActivatedRoute, public http: HttpClient) { super(http); }

  GetStatusList(): Observable<ResponseObject<Array<DropdownDTO>>> {
    const link = this.baseUrl + 'Common/GetAllStatusesList';
    return this.http.get<ResponseObject<Array<DropdownDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  GetAllStatus(statuses: string[]): Observable<ResponseObject<Array<DropdownDTO>>> {
    const link = this.baseUrl + 'Common/GetAllStatuses';
    return this.http.post<ResponseObject<Array<DropdownDTO>>>(link, statuses)
      .pipe(catchError(this.handleError));
  }

  GetDropdownByType(typeName: any = ""): Observable<ResponseObject<Array<DropdownDTO>>> {
    const link = this.baseUrl + 'Common/GetDropdownByType?TypeName=' + typeName;
    return this.http.get<ResponseObject<Array<DropdownDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  async GetDefaultGreenPoints(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Common/GetDefaultGreenPoints');
  }
}
