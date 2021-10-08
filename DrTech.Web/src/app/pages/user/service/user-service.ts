/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BaseService } from '../../../common/base.service';
import { ResponseObject } from '../../../common/response-object';
import { UserDTO } from '../dto';

@Injectable()
export class UserService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  // GetUserList(type: string): Observable<ResponseObject<Array<UserDTO>>> {
  //   const link = this.baseUrl + 'users/GetUserList?type=' + type;
  //   return this.http.get<ResponseObject<Array<UserDTO>>>(link)
  //     .pipe(catchError(this.handleError));
  // }
  GetUserList(model): Observable<ResponseObject<Array<UserDTO>>> {
    const link = this.baseUrl + 'users/GetUserList';
    return this.http.post<ResponseObject<Array<UserDTO>>>(link,model)
      .pipe(catchError(this.handleError));
  }

  async GetUserList1(type: string): Promise<ResponseObject<any>> {
    const link = this.baseUrl + 'users/GetUserList?type=' + type;
    return await this.Get<any>(link);
  }



  GetUserDetail(id:string): Observable<ResponseObject<UserDTO>> {
    const link = this.baseUrl + 'users/GetUserDetail?id=' + id;
    return this.http.get<ResponseObject<UserDTO>>(link)
      .pipe(catchError(this.handleError));
  }
  GetUserDetailAssoList(id:string): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'users/GetUserDetailAssoList?id=' + id;
    return this.http.get<ResponseObject<any>>(link)
      .pipe(catchError(this.handleError));
  }

  async registerSchool(model, file: File): Promise<ResponseObject<boolean>> {
    return await this.UploadWithData<boolean>(file, "Users/SchoolRegistrationForm", model);
  }
  async registerBusiness(model, file: File): Promise<ResponseObject<boolean>> {
    return await this.UploadWithData<boolean>(file, "Users/BusinessRegistrationForm", model);
  }
  async registerNgo(model, file: File): Promise<ResponseObject<boolean>>  {
    return  await this.UploadWithData<boolean>(file, "Users/NgoRegistrationForm", model);
  }
  async GetContactPersonDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('users/GetContactPersonDetail?id=' + id);
  }

//  async GetAdminUserList(): Observable<ResponseObject<Array<UserDTO>>> {
//     const link = this.baseUrl + 'users/GetAdminUsers';
//     return await this.http.get<ResponseObject<Array<UserDTO>>>(link)
//       .pipe(catchError(this.handleError));
//   }

async GetAdminUserList(): Promise<ResponseObject<any>> {
  return await this.Get<any>('users/GetAdminUsers');
}


}
