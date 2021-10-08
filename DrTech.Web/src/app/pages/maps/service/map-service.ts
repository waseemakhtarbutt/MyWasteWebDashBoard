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
import { MapMarkerDTO } from '../gmaps/dto/map-marker-dto';



@Injectable()
export class MapService extends BaseService {
  url: string = this.baseUrl + "Map/";
  constructor(public http: HttpClient) { super(http); }

  GetMapPoints(rType: string): Observable<ResponseObject<Array<MapMarkerDTO>>> {
    const link = this.url + 'getuserMappoints?Type='+ rType;
    return this.http.get<ResponseObject<Array<MapMarkerDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  GetStudentMapPoints(rType: string): Observable<ResponseObject<Array<MapMarkerDTO>>> {
    const link = this.url + 'GetStudentStaffRsByRole?type='+ rType;
    return this.http.get<ResponseObject<Array<MapMarkerDTO>>>(link)
      .pipe(catchError(this.handleError));
  }
}
