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
import { EmailDTO } from '../dto';


@Injectable()
export class EmailService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  GetSentEmailList(): Observable<ResponseObject<Array<EmailDTO>>> {
    const link = this.baseUrl + 'email/GetSentEmailList';
    return this.http.get<ResponseObject<Array<EmailDTO>>>(link)
      .pipe(catchError(this.handleError));
  }
  async GetEmailById(id): Promise<ResponseObject<EmailDTO>> {
    const link = this.baseUrl + 'email/GetSentEmailList';
    return await this.Get<EmailDTO>('email/GetEmailById?Id=' + id);
  }
  GetNotSentEmailList(): Observable<ResponseObject<Array<EmailDTO>>> {
    const link = this.baseUrl + 'email/GetNotSentEmailList';
    return this.http.get<ResponseObject<Array<EmailDTO>>>(link)
      .pipe(catchError(this.handleError));
  }
  SendEmail(model: EmailDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'email/SendEmail';
    return this.http.post<ResponseObject<boolean>>(link,model)
      .pipe(catchError(this.handleError));
  }
  

  //GetUserDetail(id: string): Observable<ResponseObject<EmailDTO>> {
  //  const link = this.baseUrl + 'users/GetUserDetail?id=' + id;
  //  return this.http.get<ResponseObject<EmailDTO>>(link)
  //    .pipe(catchError(this.handleError));
  //}
}
