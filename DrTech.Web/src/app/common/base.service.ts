import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

import { CommonFunction } from './common-function';
import { map } from 'rxjs/operators';
import { ResponseObject } from './response-object';
import { strict } from 'assert';


@Injectable()
export class BaseService {
  protected baseUrl = '';
  constructor(public http: HttpClient) {
    this.baseUrl = CommonFunction.GetCurrentURLOrigin();
  }

  protected async Post<TReponse>(path: string, options: any = null, showError: boolean = true) {
    try {

      const response = await this.http.post(this.baseUrl + path, options).pipe(map((response: any) => response as ResponseObject<TReponse>)).toPromise();

      //if (!response.isSuccess) {

      //  this.showError(response);
      //  //this.removeToken(response);
      //}
      return response;
    }
    catch (ex) {
      //this.showError(ex.error as ResponseObject<TReponse>);
      const result = new ResponseObject<TReponse>();
      result.statusMessage = ex.message;
      result.statusCode = 1;
      return result;
    }
    finally {
      //this.blockUI.stop();
    }
  }

  protected async Get<TReponse>(path: string, showError: boolean = true) {
    try {
      // let fullpath = "";
      // if(path == "http://localhost:64331/api/school/GetStgSchoolList" || "http://localhost:64331/api/business/GetStgBusinessList" || "http://localhost:64331/api/business/GetStgOrganizationList" || "http://localhost:64331/api/school/GetStgSchoolByID?id")
      // fullpath=  path
      // else
      //  fullpath = this.baseUrl + path

   
      const result = await this.http.get(this.baseUrl + path).pipe(map((response: any) => response as ResponseObject<TReponse>)).toPromise();
      const response = result as ResponseObject<TReponse>;
      
      return response;
    }
    catch (ex) {
      //this.showError(ex.error as ResponseObject<TReponse>);
      const result = new ResponseObject<TReponse>();
      result.statusMessage = ex.message;
      result.statusCode = 1;
      return result;
    }
    finally {
      // this.blockUI.stop();
    }
  }

  protected async UploadWithData<TReponse>(file: File, path, objectClass) {
    try {

      const formData = new FormData();
      formData.append('file', file);

      if (objectClass != null && objectClass !== undefined) {
        for (const [key, value] of Object.entries(objectClass)) {
          formData.append(key, value + '');
        }
      }

      const httpUploadOptions = {
        headers: null,
      };

      const result = await this.http.post(this.baseUrl + path, formData, httpUploadOptions).pipe(map((response: any) => response)).toPromise();
      const response = result as ResponseObject<TReponse>;

      return response;
    }
    catch (ex) {
      const result = new ResponseObject<TReponse>();
      return result;
    }
    finally {
      // this.blockUI.stop();
    }

  }

  protected async UploadData<TReponse>(file: File[], path, objectClass) {
    try {

      const formData = new FormData();
      formData.append('file', file[0]);
      formData.append('file', file[1]);

      if (objectClass != null && objectClass !== undefined) {
        for (const [key, value] of Object.entries(objectClass)) {
          formData.append(key, value + '');
        }
      }

      const httpUploadOptions = {
        headers: null,
      };

      const result = await this.http.post(this.baseUrl + path, formData, httpUploadOptions).pipe(map((response: any) => response)).toPromise();
      const response = result as ResponseObject<TReponse>;

      return response;
    }
    catch (ex) {
      const result = new ResponseObject<TReponse>();
      return result;
    }
    finally {
      // this.blockUI.stop();
    }

  }


  protected handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occured:', error.error.message);
    } else {
      console.error(
        'Backend returned code ${error.status}, ' +
        'body was ${error.error}');
    }
    return throwError(
      'Something bad happend; please try again later.');
  }
  
}
