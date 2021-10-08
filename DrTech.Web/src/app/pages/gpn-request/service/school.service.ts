import { Injectable } from '@angular/core';
import { BaseService } from '../../../common/base.service';
import { HttpClient } from '@angular/common/http';
import { ResponseObject } from '../../../common/response-object';
import { Observable } from 'rxjs';
import { RegiftDTO, RecycleDTO } from '../../request/dto';
import { catchError } from 'rxjs/operators';
import { BinDTO } from '../../request/dto/bin-dto';

@Injectable({
  providedIn: 'root'
})
export class SchoolService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  GetStudentListByRole(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'School/GetStudentListByRole?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }
  GetStudentListByRoleWithGreenPointsProgress(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'School/GetStudentListByRoleWithPointsProgress?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }

  GetStaffListByRole(IsSuspended): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'School/GetStaffListByRole?IsSuspended=' + IsSuspended;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }

  async SuspendChild(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('School/SuspendChild?id=' + id);
  }
  async ActivateStudent(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('School/ActivateStudent?Id=' + id);
  }

  async ActivateSchoolStaff(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('School/ActivateSchoolStaff?Id=' + id);
  }

  async SuspendStaff(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('School/SuspendStaff?id=' + id);
  }

  async GetSectionList(className): Promise<ResponseObject<boolean>> {
    return await this.Get<any>('School/GetSectionByClass?Class=' + className);
  }

  async GetBranchesList(): Promise<ResponseObject<boolean>> {
    return await this.Get<any>('School/GetBranchesBySchoolAdmin');
  }

  async GetClassList(): Promise<ResponseObject<boolean>> {
     return await this.Get<any>('School/GetClassBySchool');
   }

  async GetComparisonGreenPoints(clas, sections, schools, fromDate, toDate): Promise<ResponseObject<any>> {
    return await this.Get<any>('School/GetSchoolComparison?clas=' + clas + '&sct=' + sections + '&sch=' + schools + '&fd=' + fromDate + '&td=' + toDate);
  }
}
