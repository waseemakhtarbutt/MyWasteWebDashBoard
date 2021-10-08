import { Injectable } from '@angular/core';
import { BaseService } from '../../../common/base.service';
import { HttpClient } from '@angular/common/http';
import { ResponseObject } from '../../../common/response-object';
import { Observable } from 'rxjs';
import { RegiftDTO, RecycleDTO } from '../../request/dto';
import { catchError } from 'rxjs/operators';
import { DriverDTO } from '../dto/driver.dto';
import { BinDTO } from '../../request/dto/bin-dto';
import { CommentsDTO } from '../../request/dto/comments-dto';
import { DriverRequestDto } from '../dto/driver.dto';

@Injectable({
  providedIn: 'root'
})
export class DriverService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  GetDrivers(model: DriverRequestDto): Observable<ResponseObject<any>> {
    const link = this.baseUrl + 'Driver/GetAllDrivers';
    return this.http.post<ResponseObject<any>>(link,model)
      .pipe(catchError(this.handleError));
  }

  async GetVechileList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Driver/GetVechileList');
  }

  async GetGreenshopList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Greenshop/GetGreenshops');
  }

  async CheckPhoneNumber(phoneNumber:string): Promise<ResponseObject<any>> {
    return await this.Get<any>('Driver/CheckPhoneNumber?phoneNumber='+phoneNumber);
  }

  async SaveDriver(file,model): Promise<ResponseObject<any>> {
    return await this.UploadData<any>( file,'Driver/AddDriverDetails',model);
  }

  GetRegiftDetailById(regiftId: any): Observable<ResponseObject<RegiftDTO>> {
    if (regiftId != null)
    {
      const link = this.baseUrl + 'Regift/GetRegiftDetailById?RegiftID=' + regiftId + '&IsWebAdmin=true';
      return this.http.get<ResponseObject<RegiftDTO>>(link)
        .pipe(catchError(this.handleError));
    }
  }

  Getshift(): Observable<ResponseObject<any>> {

      const link = this.baseUrl + 'Regift/Shift';
      return this.http.get<ResponseObject<any>>(link)
        .pipe(catchError(this.handleError));

  }
 
  GetAllDrivers(model: DriverRequestDto): Observable<ResponseObject<Array<DriverDTO>>> {
    const link = this.baseUrl + 'Driver/GetAllDrivers';
    return this.http.post<ResponseObject<Array<DriverDTO>>>(link,model)
      .pipe(catchError(this.handleError));
  }

  GetRecycleDetailById(recycleId: any): Observable<ResponseObject<RecycleDTO>> {
    if (recycleId != null)
    {
      const link = this.baseUrl + 'Recycle/GetRecycleDetailById?RecycleID=' + recycleId + '&IsWebAdmin=true';
      return this.http.get<ResponseObject<RecycleDTO>>(link)
        .pipe(catchError(this.handleError));
    }
  }

  GetBinDetailById(binId: any): Observable<ResponseObject<BinDTO>> {
    if (binId != null)
    {
      const link = this.baseUrl + 'BuyBin/GetBinDetailById?BinID=' + binId;
      return this.http.get<ResponseObject<BinDTO>>(link)
        .pipe(catchError(this.handleError));
    }
  }

  AssignRegiftToDriver(_regiftModel: RegiftDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Regift/AssignRegiftToDriver';
    return this.http.post<ResponseObject<boolean>>(link, _regiftModel)
      .pipe(catchError(this.handleError));
  }

  SMSRegiftComments(_commentModel: CommentsDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Regift/SMSRegiftComments';
    return this.http.post<ResponseObject<boolean>>(link, _commentModel)
      .pipe(catchError(this.handleError));
  }

  SMSRecycleComments(_commentModel: CommentsDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Recycle/SMSRecycleComments';
    return this.http.post<ResponseObject<boolean>>(link, _commentModel)
      .pipe(catchError(this.handleError));
  }

  SMSBinComments(_commentModel: CommentsDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'BuyBin/SMSBinComments';
    return this.http.post<ResponseObject<boolean>>(link, _commentModel)
      .pipe(catchError(this.handleError));
  }

  AssignRecycleToDriver(_recycleModel: RecycleDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Recycle/AssignRecycleToDriver';
    return this.http.post<ResponseObject<boolean>>(link, _recycleModel)
      .pipe(catchError(this.handleError));
  }

  AssignBinToDriver(_binModel: BinDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'BuyBin/AssignBinToDriver';
    return this.http.post<ResponseObject<boolean>>(link, _binModel)
      .pipe(catchError(this.handleError));
  }

  async GetDriverByID(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('driver/GetDriverByID?ID=' + id);
  }

  async GetAllJobs(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('driver/GetDriverJobsByID?ID=' + id);
  }

  async GetRegift(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('driver/GetRegiftDriverJobsByID?ID=' + id);
  }

  async GetRecycle(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('driver/GetRecycleDriverJobsByID?ID=' + id);
  }

  async GetDelivered(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('driver/GetBinDriverJobsByID?ID=' + id);
  }
  async Getfile(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('driver/GetLicenceFile?id='  + id);
  }
  async SetConfirmation(orderId, isDriver): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('regift/SetConfirmation?orderId=' + orderId + '&isDriver=' + isDriver);
  }

  RejectRegift(_regiftModel: RegiftDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Regift/RejectRegift';
    return this.http.post<ResponseObject<boolean>>(link, _regiftModel)
      .pipe(catchError(this.handleError));
  }

  RejectRecycle(_recycleModel: RecycleDTO): Observable<ResponseObject<boolean>> {
    const link = this.baseUrl + 'Recycle/RejectRecycle';
    return this.http.post<ResponseObject<boolean>>(link, _recycleModel)
      .pipe(catchError(this.handleError));
  }

  async SuspendDriver(id): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('Driver/SuspendDriver?id=' + id);
  }
  async CheckDriverAssignments(id): Promise<ResponseObject<boolean>> {
    return await this.Get<boolean>('Driver/CheckDriverAssignments?Id=' + id);
  }
  async SaveGUIForRecycle(model): Promise<ResponseObject<any>> {
    return await this.Post<any>( 'Company/AddGuiForRecycle',model);
  }
  async GetGUIList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetGUIList');
  }
  async GetGUIListDateRange(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetGUIList');
  }
  async GetPerformerPIChartData(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Users/RsCountForGPN');
  }
  async GetGOIGraph(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Users/GetGOIGraph');
  }
  async GetPerformerBarhartData(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Users/TopGPUsers');
  }
  async DrawChartByGPNComparison(model): Promise<ResponseObject<any>> {
    return await this.Post<any>( 'Users/GetGreenCreditsForGOI',model);
  }
  async GetGOIGraphYearWise(): Promise<ResponseObject<any>> {
    return await this.Get<any>( 'Users/GetGOIGraphYearWise');
  }
  async GetGOIGraphMonthWise(): Promise<ResponseObject<any>> {
    return await this.Get<any>( 'Users/GetGOIGraphMonthWise');
  }
  async GetGOIListForSuperAdmin(BranchID): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetGOIListForSuperAdminByBranchID?BranchID='+ BranchID);
  }
  async GetGOIListForSuperAdminByDate(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('Company/GetGOIListForSuperAdminByBranchDate',model);
  }
  async GetDataForRecycleDetailChartByAdmin(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Users/GetDataForRecycleDetailChartByAdmin');
  }
  async GetCircularGraph(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Users/GetCircularGraph');
  }
  async GetDailyGreenPointsGraph(): Promise<ResponseObject<any>> {
    return await this.Get<any>( 'Users/GetDailyGreenPointsGraph');
  }
  async GetDailyWasteWeightGraph(): Promise<ResponseObject<any>> {
    return await this.Get<any>( 'Users/GetDailyWasteWeightGraph');
  }
}
