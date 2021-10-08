/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../common/base.service';

@Injectable()
export class DashboardService extends BaseService {
  constructor(public http: HttpClient) { super(http); }
  
  async getDashboardDetail() {
    const result = await this.Get<any>('Dashboard');
    return result;
  }
  async getRDetail(rType) {
    const result = await this.Get<any>('Dashboard/getRDetail?rType=' + rType);
    return result;
  }
}
