/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../common/base.service';
import { ResponseObject } from '../../../common/response-object';
import { DropdownDTO } from '../../../common/dropdown-dto';

@Injectable()
export class RegistrationRequestService extends BaseService {

  constructor(public http: HttpClient) { super(http); }

  async GetSchoolList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('schools/GetSchoolList');
  }
  async GetStudentList(id): Promise<ResponseObject<any>> {
    if (id === null || id === undefined)
      id = '';
    return await this.Get<any>('schools/GetStudentList?id=' + id);
  }
  async GetStudentDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('schools/GetStudentDetail?id=' + id);
  }
  async updateStudentStatus(status, id): Promise<ResponseObject<any>> {
    return await this.Post<any>('schools/UpdateStudentStatus', {
      id: id,
      status: status,
    });
  }


  async GetEmployeeList(id): Promise<ResponseObject<any>> {
    if (id === null || id === undefined)
      id = '';
    return await this.Get<any>('organization/GetEmployeeList?id=' + id);
  }
  async GetEmployeeDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetEmployeeDetail?id=' + id);
  }
  async updateEmployeeStatus(status, id): Promise<ResponseObject<any>> {
    return await this.Post<any>('organization/updateEmployeeStatus', {
      id: id,
      status: status,
    });
  }

  async GetMemberList(id): Promise<ResponseObject<any>> {
    if (id === null || id === undefined)
      id = '';
    return await this.Get<any>('ngo/GetMemberList?id=' + id);
  }
  async GetMemberDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('ngo/GetMemberDetail?id=' + id);
  }
  async updateMemberStatus(status, id): Promise<ResponseObject<any>> {
    return await this.Post<any>('ngo/updateMemberStatus', {
      id: id,
      status: status,
    });
  }


  async GetSchoolApprovalList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('schools/GetSchoolApprovalList');
  }
  async GetBusinessList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetBusinessList');
  }
  async GetNgoList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('ngo/GetNgoList');
  }
  async GetNgoApprovalList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('ngo/GetNgoApprovalList');
  }
  async GetBusinessApprovalList(): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetBusinessApprovalList');
  }

  async GetBusinessDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetBusinessDetail?id=' + id);
  }
  async GetMyBusinessDetail(): Promise<ResponseObject<any>> {
    return await this.Get<any>('organization/GetMyBusinessDetail');
  }
  async GetSchoolDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('schools/GetSchoolDetail?id=' + id);
  }
  async GetSchoolDetailForEdit(): Promise<ResponseObject<any>> {
    return await this.Get<any>('schools/GetSchoolDetailForEdit');
  }
  async GetMySchoolDetail(): Promise<ResponseObject<any>> {
    return await this.Get<any>('schools/GetMySchoolDetail');
  }
  async GetNgoDetail(id): Promise<ResponseObject<any>> {
    return await this.Get<any>('ngo/GetNgoDetail?id=' + id);
  }
  async GetMyNgoDetail(): Promise<ResponseObject<any>> {
    return await this.Get<any>('ngo/GetMyNgoDetail');
  }

  async UpdateBusinessStatus(id, status): Promise<ResponseObject<boolean>> {
    const model = {
      status: status,
      id: id,
    };
    return await this.Post<boolean>('organization/UpdateBusinessStatus', model);
  }
  async UpdateSchoolStatus(id, status): Promise<ResponseObject<boolean>> {
    const model = {
      status: status,
      id: id,
    };
    return await this.Post<boolean>('schools/UpdateSchoolStatus', model);
  }
  async UpdateNgoStatus(id, status): Promise<ResponseObject<boolean>> {
    const model = {
      status: status,
      id: id,
    };
    return await this.Post<boolean>('ngo/UpdateNgoStatus', model);
  }
  async getSchoolDropdown(): Promise<ResponseObject<Array<DropdownDTO>>> {
    return await this.Get<Array<DropdownDTO>>('schools/GetSchoolDropdown');
  }
  async getBusinessDropdown(): Promise<ResponseObject<Array<DropdownDTO>>> {
    return await this.Get<Array<DropdownDTO>>('organization/GetBusinessDropdown');
  }
  async getNgoDropdown(): Promise<ResponseObject<Array<DropdownDTO>>> {
    return await this.Get<Array<DropdownDTO>>('ngo/GetNgoDropdown');
  }

  async addSchool(model, file: File): Promise<ResponseObject<boolean>> {
    return await this.UploadWithData<boolean>(file, 'schools/AddSchool', { name: model });
  }
  async updateSchool(model): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('schools/updateSchool', model);
  }
  async updateSchoolApprovalStatus(id, status): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('schools/updateSchoolApprovalStatus', {
      id: id,
      status: status,
    });
  }
  async updateNgoApprovalStatus(id, status): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('ngo/updateNgoApprovalStatus', {
      id: id,
      status: status,
    });
  }
  async updateBusinessApprovalStatus(id, status): Promise<ResponseObject<boolean>> {
    return await this.Post<boolean>('organization/updateBusinessApprovalStatus', {
      id: id,
      status: status,
    });
  }
  async addBusiness(model, file:File): Promise<ResponseObject<boolean>> {
    return await this.UploadWithData<boolean>(file,'organization/AddBusiness', { name: model });
  }
  async addNgo(model, file: File): Promise<ResponseObject<boolean>> {
    return await this.UploadWithData<boolean>(file, 'ngo/AddNgo', { name: model });
  }
  
}
