import { Injectable } from '@angular/core';
import { BaseService } from '../../common/base.service';
import { HttpClient } from '@angular/common/http';
import { ResponseObject } from '../../common/response-object';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DriverDTO } from '../driver/dto/driver.dto';

@Injectable({
  providedIn: 'root'
})
export class MywasteserviceService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

    GetAllDrivers(): Observable<ResponseObject<Array<any>>> {
    const link = this.baseUrl + 'Driver/GetAllDriver';
    return this.http.get<ResponseObject<Array<DriverDTO>>>(link)
      .pipe(catchError(this.handleError));
  }


  // GetAllAreas(): Observable<ResponseObject<Array<any>>> {
  //   const link = this.baseUrl + 'MyWaste/GetAllArea';
  //   return this.http.get<ResponseObject<Array<any>>>(link)
  //     .pipe(catchError(this.handleError));
  // }

  GetAllCitys(): Observable<ResponseObject<Array<any>>> {
    const link = this.baseUrl + 'MyWaste/GetAllCity';
    return this.http.get<ResponseObject<Array<any>>>(link)
      .pipe(catchError(this.handleError));
  }
  async GetAreasByID(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('MyWaste/GetAreaByID?ID=' + id);
  }
  async SaveSchedule(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('MyWaste/AddWasteSchedule', model);
  }

  async ImportExcel(file, model): Promise<ResponseObject<any>> {
    return await this.UploadWithData<any>(file, 'MyWaste/ImportExcel', model);
  }
  async SaveCompany(file, model): Promise<ResponseObject<any>> {
    return await this.UploadData<any>(file, 'Company/AddCompanyInformation', model);
  }
  async GetCityList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('common/GetDropdownByType?TypeName=City');
  }
  async GetCompanyList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetAll');
  }
  async GetCompanyByID(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetCompanyByID?ID=' + id);
  }
  async SuspendCompany(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/SuspendCompany?ID=' + id);
  }
  async GetCompanyByAdminRole(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetAllByAdminRole');
  }
  async GetBusinessList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Business/GetBusinessListForGOI');
  }
  async GetHeadOfficesOFBusinessForGOI(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Business/GetHeadOfficesOFBusinessForGOI');
  }

  async GetBusinessBranchesByIdForGOI(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Business/GetBusinessBranchesByIdForGOI?ID=' + id);
  }
  async GetBusinessBranchesGOIByIdList(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('Business/GetBusinessBranchesByIdForGOI?ID=' + id);
  }
  async GetWasteTypes(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Company/GetWasteTypes');
  }
  async GetLoggedInAdminBusinessName(): Promise<ResponseObject<any>> {
    return await this.Get<any>('Users/GetLoggedInAdminBusinessName');
  }
  async DumpRecycle(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('Company/DumpSegregatedRecycle', model);
  }
  async GetDesegregatedList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('MyWaste/GetDesegregatedList');
  }

  GetDesegregatedByID(id): Promise<ResponseObject<any>> {
    return this.Get<any>('MyWaste/GetDesegregatedByID?ID=' + id);
  }

  IsDataSegregated(id): Promise<ResponseObject<any>> {
    return this.Get<any>('MyWaste/IsDataSegregated?RecycleID=' + id);
  }
  GetSegregatedDataByID(id): Promise<ResponseObject<any>> {
    return this.Get<any>('MyWaste/GetSegregatedDataByID?RecycleID=' + id);
  }
 
  async GetDesegregatedListBetweenTwoDates(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('MyWaste/GetDesegregatedListBetweenTwoDates', model);
  }
 
  async GetSegregatedDataBetweenTwoDates(model): Promise<ResponseObject<any>> {
    return await this.Post<any>('MyWaste/GetSegregatedDataBetweenTwoDates', model);
  }
}
