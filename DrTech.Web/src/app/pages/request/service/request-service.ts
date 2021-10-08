/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of as observableOf } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BaseService } from '../../../common/base.service';
import { ResponseObject } from '../../../common/response-object';
import { ComplaintDTO, ReuseDTO, RecycleDTO, RegiftDTO, RefuseDTO } from '../dto';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { ReduceDTO } from '../../registration-request/dto/reduce-dto';
import { ReportDTO } from '../dto/report-dto';
import { ReplantDTO } from '../../registration-request/dto';
import { RecycleRequest } from '../dto/recycleRequest-dto';


@Injectable()
export class RequestService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  GetReportList(statusId: any = ""): Observable<ResponseObject<Array<ComplaintDTO>>> {
    if (statusId == null)
      statusId = 0;
    debugger
    const link = this.baseUrl + 'Report/GetReportsListByStatus?StatusID=' + statusId;
    return this.http.get<ResponseObject<Array<ComplaintDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  updateReportStatus(id: string, status: number, points: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Report/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      StatusId: status,
      GreenPoints: points
    })
      .pipe(catchError(this.handleError));
  }

  GetRegiftList(statusId: any = ""): Observable<ResponseObject<Array<ReuseDTO>>> {

    if (statusId == null)
      statusId = 0;

    const link = this.baseUrl + 'Regift/GetRegiftsListByStatus?StatusID=' + statusId;
    return this.http.get<ResponseObject<Array<ReuseDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  GetBuyBinList(statusId: any = ""): Observable<ResponseObject<Array<ReuseDTO>>> {
    if (statusId == null)
      statusId = 0;

    const link = this.baseUrl + 'BuyBin/GetBinsListByStatus?StatusID=' + statusId;
    return this.http.get<ResponseObject<Array<ReuseDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  updateReduceStatus(id: string, status: number, points: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Reduce/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      StatusId: status,
      GreenPoints: points
    })
      .pipe(catchError(this.handleError));
  }

  GetReduceList(statusId: any = ""): Observable<ResponseObject<Array<ReduceDTO>>> {
    if (statusId == null)
      statusId = 0;

    const link = this.baseUrl + 'Reduce/GetReducesListByStatus?StatusID=' + statusId;
    return this.http.get<ResponseObject<Array<ReduceDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  updateReuseStatus(id: string, status: number, points: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Reuse/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      StatusId: status,
      GreenPoints: points
    })
      .pipe(catchError(this.handleError));
  }

  GetReuseList(statusId: any = ""): Observable<ResponseObject<Array<ReuseDTO>>> {
    if (statusId == null)
      statusId = 0;

    const link = this.baseUrl + 'Reuse/GetReusesListByStatus?StatusID=' + statusId;
    return this.http.get<ResponseObject<Array<ReuseDTO>>>(link)
      .pipe(catchError(this.handleError));
  }

  updateRefuseStatus(id: string, status: number, points: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Refuse/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      StatusId: status,
      GreenPoints: points
    })
      .pipe(catchError(this.handleError));
  }

  updateRefuseStatusById(refuseModel: RefuseDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Refuse/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, refuseModel)
      .pipe(catchError(this.handleError));
  }
  updateReuseStatusById(reuseModel: ReuseDTO): Observable<ResponseObject<boolean>> {
    console.log(reuseModel)
    const link = this.baseUrl + 'Reuse/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, reuseModel)
      .pipe(catchError(this.handleError));
  }
  updateReplantStatusById(reuseModel: ReplantDTO): Observable<ResponseObject<boolean>> {
    console.log(reuseModel)
    const link = this.baseUrl + 'Replant/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, reuseModel)
      .pipe(catchError(this.handleError));
  }
  updateReduceStatusById(reuseModel: ReduceDTO): Observable<ResponseObject<boolean>> {
    console.log(reuseModel)
    const link = this.baseUrl + 'Reduce/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, reuseModel)
      .pipe(catchError(this.handleError));
  }
  updateReportStatusById(reuseModel: ReportDTO): Observable<ResponseObject<boolean>> {
    console.log(reuseModel)
    const link = this.baseUrl + 'Report/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, reuseModel)
      .pipe(catchError(this.handleError));
  }


  GetRefuseList(model): Observable<ResponseObject<Array<RefuseDTO>>> {
    if (model.statusId == null)
      model.statusId = 0;

    if (model.statusId == "")
      model.statusId = 0;

    const link = this.baseUrl + 'Refuse/GetRefusesListByStatus';
    return this.http.post<ResponseObject<Array<RefuseDTO>>>(link,model)
      .pipe(catchError(this.handleError));
  }
  // GetRefuseById(Id: any = ""): Observable<ResponseObject<RefuseDTO>> {
  //   if (Id == null)
  //   Id = 0;

  //   const link = this.baseUrl + 'Refuse/GetRefusesById?Id=' + Id;
  //   return this.http.get<ResponseObject<RefuseDTO>>(link)
  //     .pipe(catchError(this.handleError));
  // }

  async GetRefuseById(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Refuse/GetRefusesById?Id=' + id);
  }
  async GetReuseById(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Reuse/GetReusesById?Id=' + id);
  }
  async GetReplantById(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Replant/GetReplantById?Id=' + id);
  }
  async GetReduceById(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Reduce/GetReduceById?Id=' + id);
  }
  async GetReportById(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Report/GetReportById?Id=' + id);
  }
  GetRegiftPenddingApprovalList(): Observable<ResponseObject<Array<ReuseDTO>>> {
    const link = this.baseUrl + 'Regift/GetAllPendingForApproval';
    return this.http.get<ResponseObject<Array<ReuseDTO>>>(link)
      .pipe(catchError(this.handleError));
  }
  GetReplantList(statusId: any = ""): Observable<ResponseObject<Array<any>>> {
    if (statusId == null)
      statusId = 0;

    const link = this.baseUrl + 'Replant/GetReplantsListByStatus?StatusID=' + statusId;
    return this.http.get<ResponseObject<Array<any>>>(link)
      .pipe(catchError(this.handleError));
  }
  updateReplantStatus(id: string, status: number, points: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Replant/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      StatusId: status,
      GreenPoints: points
    })
      .pipe(catchError(this.handleError));
  }
  updateRegiftStatus(id: string, status: number, selectedFile: File, points: number): Observable<ResponseObject<boolean>> {

    const uploadData = new FormData();
    uploadData.append('file', selectedFile, selectedFile.name);
    uploadData.append('Id', id);
    uploadData.append('StatusId', status + "");
    uploadData.append('GreenPoints', points + "");

    //this.http.post('my-backend.com/file-upload', uploadData)
    //  .subscribe(event => {
    //    console.log(event); // handle event here
    //  });

    const link = this.baseUrl + 'Regift/UpdateStatus';


    var headerObject = new HttpHeaders();

    return this.http.post<ResponseObject<boolean>>(link, uploadData, { headers: null })
      .pipe(catchError(this.handleError));
  }
  UpdateRegiftApprovalStatus(id: string, status: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Regift/UpdateApprovalStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      Status: status
    })
      .pipe(catchError(this.handleError));
  }

  GetRecycleList(model): Observable<ResponseObject<Array<RecycleDTO>>> {
debugger;
    if (model.statusId == null)
      model.statusId = 0;
  //     const params = new HttpParams()
  //  .set('statusId', statusId)
  //  .set('startDate', startDate)
  //  .set('endDate', endDate);

    const link = this.baseUrl + 'Recycle/GetRecyclesListByStatus';
    return this.http.post<ResponseObject<Array<RecycleDTO>>>(link,model)
      .pipe(catchError(this.handleError));
  }
  GetRecycleListExcel(model): Observable<ResponseObject<Array<RecycleDTO>>> {
    debugger;
        if (model.statusId == null)
          model.statusId = 0;
      //     const params = new HttpParams()
      //  .set('statusId', statusId)
      //  .set('startDate', startDate)
      //  .set('endDate', endDate);
    
        const link = this.baseUrl + 'Recycle/GetRecyclesListByStatusExcel';
        return this.http.post<ResponseObject<Array<RecycleDTO>>>(link,model)
          .pipe(catchError(this.handleError));
      }
  GetRecycleAllList(model: RecycleRequest): Observable<ResponseObject<Array<RecycleDTO>>> {

    if (model.statusId == null)
      model.statusId = 0;

    const link = this.baseUrl + 'Recycle/GetRecyclesAllListByStatus';
    return this.http.post<ResponseObject<Array<RecycleDTO>>>(link,model)
      .pipe(catchError(this.handleError));
  }
  GetRecycleAllListExcel(model: RecycleRequest): Observable<ResponseObject<Array<RecycleDTO>>> {

    if (model.statusId == null)
      model.statusId = 0;

    const link = this.baseUrl + 'Recycle/GetRecyclesAllListByStatus';
    return this.http.post<ResponseObject<Array<RecycleDTO>>>(link,model)
      .pipe(catchError(this.handleError));
  }
  updateRecycleStatus(id: string, status: number, weight: number): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Recycle/UpdateStatus';
    return this.http.post<ResponseObject<boolean>>(link, {
      Id: id,
      StatusId: status,
      Weight: weight
    })
      .pipe(catchError(this.handleError));
  }
  async GetAllRefuseList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Refuse/GetAll');
  }
  async GetAllReuseList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Reuse/GetAll');
  }
  async GetAllReduceList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Reduce/GetAll');
  }
  async GetAllReportList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Report/GetAll');
  }
  async GetAllReplantList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Replant/GetAll');
  }
  async GetArsRequest(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Common/GetArsRequestCount');
  }


}
