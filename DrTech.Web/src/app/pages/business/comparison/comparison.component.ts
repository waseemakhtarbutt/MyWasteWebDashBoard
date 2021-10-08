import { Component, OnInit } from '@angular/core';
import {BusinessService} from '../service/business-service'
import { Router } from '@angular/router';
import { NbTokenService } from '../../../common/auth';
import 'hammerjs';
import { elementDef } from '@angular/core/src/view';

@Component({
  selector: 'ngx-comparison',
  templateUrl: './comparison.component.html',
  styleUrls: ['./comparison.component.scss']
})
export class ComparisonComponent implements OnInit {
  subAdminFromDate: Date;
  subAdminToDate: Date;
  businessAdminFromDate: Date;
  businessAdminToDate: Date;
  fromFormattedDate: string;
  toFormattedDate: string;
  comparisonList: Array<ComparisonData> = new Array<ComparisonData>();
  chartData: Array<ChartData> = new Array<ChartData>();
  monthList: string[];
  departmentList: Array<string> = new Array<string>();
  businessList: Array<Business> = new Array<Business>();

  constructor(public businessService: BusinessService, private router: Router, private tokenService: NbTokenService) { }
   selectedDepartments = [];
   selectedBusinesses = [];
   dropdownSettings = {};
   dynamicdropdown = [];
   ddlDepartmentList:any;
   ddlBusinessList:any;
   IsSearched:boolean;
   roleId: number;

  async ngOnInit() {
    this.roleId = this.tokenService.getRole();

    this.IsSearched = false;
    this.monthList = [];
    var d = new Date();
    this.subAdminFromDate = this.subAdminToDate = this.businessAdminFromDate = this.businessAdminToDate = new Date();

    if (this.roleId == 9) { // Sub Business Admin
      var response = await this.businessService.GetDepartmentList();

      this.dynamicdropdown = [];

      if(response.statusCode == 0)
        this.ddlDepartmentList = response.data;
        let number = 0;

        this.ddlDepartmentList.forEach(element => {
          this.dynamicdropdown.push({item_id:element.department, item_text: element.department})
      });
      
      this.ddlDepartmentList = this.dynamicdropdown;
    }
    else if (this.roleId == 4) { // Business Admin
      var response = await this.businessService.GetBranchesList();

      this.dynamicdropdown = [];

      if(response.statusCode == 0)
        this.ddlBusinessList = response.data;

      let number = 0;

      this.ddlBusinessList.forEach(element => {
          this.dynamicdropdown.push({item_id:element.businessId, item_text: element.businessName})
      });
      
      this.ddlBusinessList = this.dynamicdropdown;
    }
    
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      limitSelection: 3,
      allowSearchFilter: true,   
      enableCheckAll: false,
    };
  }

  onItemSelect(item: any) {
    console.log(item);
  }

  onSelectAll(items: any) {
    console.log(items);
  }

  async GetComparisonData() {
    var result = await this.BusinessWiseSum();

    if (result)
    {
      this.PrepareChartData(result);
      this.IsSearched = true;
    }
  }

  async BusinessWiseSum() {        
    var response;

    if (this.roleId == 9) {
      this.fromFormattedDate = new Date((this.subAdminFromDate.getMonth()+1) + " " + this.subAdminFromDate.getDate() +", " + this.subAdminFromDate.getFullYear() + " " + this.subAdminFromDate.getHours() + ":" + this.subAdminFromDate.getMinutes() + ":" + this.subAdminFromDate.getSeconds()).toLocaleString('ur-PK');
      this.toFormattedDate = new Date((this.subAdminToDate.getMonth()+1) + " " + this.subAdminToDate.getDate() +", " + this.subAdminToDate.getFullYear() + " " + this.subAdminToDate.getHours() + ":" + this.subAdminToDate.getMinutes() + ":" + this.subAdminToDate.getSeconds()).toLocaleString('ur-PK');

      response = await this.businessService.GetComparisonGreenPoints(this.selectedDepartments, '', this.fromFormattedDate, this.toFormattedDate);
    }
    else if (this.roleId == 4) {
      this.fromFormattedDate = new Date((this.businessAdminFromDate.getMonth()+1) + " " + this.businessAdminFromDate.getDate() +", " + this.businessAdminFromDate.getFullYear() + " " + this.businessAdminFromDate.getHours() + ":" + this.businessAdminFromDate.getMinutes() + ":" + this.businessAdminFromDate.getSeconds()).toLocaleString('ur-PK');
      this.toFormattedDate = new Date((this.businessAdminToDate.getMonth()+1) + " " + this.businessAdminToDate.getDate() +", " + this.businessAdminToDate.getFullYear() + " " + this.businessAdminToDate.getHours() + ":" + this.businessAdminToDate.getMinutes() + ":" + this.businessAdminToDate.getSeconds()).toLocaleString('ur-PK');
      
      var businessIds = [];

      this.selectedBusinesses.forEach(element => {
        businessIds.push(element.item_id);
      });

      response = await this.businessService.GetComparisonGreenPoints('', businessIds, this.fromFormattedDate, this.toFormattedDate);
    }

    this.comparisonList = [];
    
    if (response.data == null)
      return false;

    response.data.forEach(element => {

      element.refuses.forEach(elrefuses => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elrefuses.key.year + '_' + elrefuses.key.month, 
          gp:elrefuses.gp,
          type: 'refuse',
          dt: new Date(elrefuses.key.year, elrefuses.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.reduces.forEach(elreduces => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elreduces.key.year + '_' + elreduces.key.month, 
          gp:elreduces.gp,
          type: 'reduce',
          dt: new Date(elreduces.key.year, elreduces.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.reuses.forEach(elreuses => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elreuses.key.year + '_' + elreuses.key.month, 
          gp:elreuses.gp,
          type: 'reuse',
          dt: new Date(elreuses.key.year, elreuses.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.replants.forEach(elreplants => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elreplants.key.year + '_' + elreplants.key.month, 
          gp:elreplants.gp,
          type: 'replant',
          dt: new Date(elreplants.key.year, elreplants.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.recycles.forEach(elrecycles => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elrecycles.key.year + '_' + elrecycles.key.month, 
          gp:elrecycles.gp,
          type: 'recycle',
          dt: new Date(elrecycles.key.year, elrecycles.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.regifts.forEach(elregifts => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elregifts.key.year + '_' + elregifts.key.month, 
          gp:elregifts.gp,
          type: 'regift',
          dt: new Date(elregifts.key.year, elregifts.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.reports.forEach(elreports => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elreports.key.year + '_' + elreports.key.month, 
          gp:elreports.gp,
          type: 'report',
          dt: new Date(elreports.key.year, elreports.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.buyBins.forEach(elbuybins => {
        let row = { department:element.department, businessId:element.businessId, businessName:element.businessName, 
          month:elbuybins.key.year + '_' + elbuybins.key.month, 
          gp:elbuybins.gp,
          type: 'buybin',
          dt: new Date(elbuybins.key.year, elbuybins.key.month-1)
        }

        this.comparisonList.push(row);
      });
    });
    
    var result = [];

    if (this.roleId == 9) // Sub Business Admin
    {
      this.comparisonList.reduce(function(res, value) {
      if (!res[value.month+value.department]) {
        res[value.month+value.department] = { month: value.month, department: value.department, businessId: value.businessId, businessName: value.businessName, gp: 0, dt: value.dt };
        result.push(res[value.month+value.department])
      }
      res[value.month+value.department].gp += value.gp;
      return res;
      }, {});
    }
    else if (this.roleId == 4) // Business Admin
    {
      this.comparisonList.reduce(function(res, value) {
        if (!res[value.month+value.businessId]) {
          res[value.month+value.businessId] = { month: value.month, businessId: value.businessId, businessName: value.businessName, gp: 0, dt: value.dt };
          result.push(res[value.month+value.businessId])
        }
        res[value.month+value.businessId].gp += value.gp;
        return res;
        }, {});
    }

    debugger;
    return result;
  }

  PrepareChartData(result) {
    result = result.slice().sort(function (a, b) {
      return a["dt"] < b["dt"] ? -1 : 1;
    });

    this.monthList = [];
    this.departmentList = [];
    this.businessList = [];

    this.chartData = Array<ChartData>();

    result.forEach(element => {
      let found = this.monthList.find(e => e == element.month);
      if (found==undefined)
        this.monthList.push(element.month);

      if (this.roleId == 9) // Sub Business Admin
      {
        let department = this.departmentList.find(e => e == element.department);
        if (department==undefined)
          this.departmentList.push(element.department);
      }
      else if (this.roleId == 4) // Business Admin 
      {
        let business = this.businessList.find(e => e.businessId == element.businessId);
        if (business==undefined)
        {
          let row = { businessId:element.businessId, businessName:element.businessName } 

          this.businessList.push(row);
        }
      }
    });

    this.chartData = Array<ChartData>();

    if (this.roleId == 9) // Sub Business Admin
    {
      this.departmentList.forEach(department => {
        var cData = new ChartData();
        var greenPoints = Array<number>();
  
        this.monthList.forEach(month => {
            var flag = false;
            result.forEach(element => {
              if (element.month == month && element.department == department)
              {
                greenPoints.push(element.gp);
                  flag = true;
              }
            });
  
            if (!flag)
              greenPoints.push(0);
          });
  
        cData.category = department;
        cData.gpList=greenPoints;
                  
        this.chartData.push(cData);
      });
    }
    else if (this.roleId == 4) // Business Admin 
    {
      this.businessList.forEach(business => {
        var cData = new ChartData();
        var greenPoints = Array<number>();
  
        this.monthList.forEach(month => {
            var flag = false;
            result.forEach(element => {
              if (element.month == month && element.businessId == business.businessId)
              {
                greenPoints.push(element.gp);
                  flag = true;
              }
            });
  
            if (!flag)
            greenPoints.push(0);
          });
  
        cData.category = business.businessName;
        cData.gpList=greenPoints;
                  
        this.chartData.push(cData);
      });
    }

    for (var i = 0; i < this.monthList.length; i++) {
      switch(this.monthList[i].toString().split('_')[1]) {
        case "1": this.monthList[i] = "January " + this.monthList[i].toString().split('_')[0];  break;
        case "2": this.monthList[i] = "February " + this.monthList[i].toString().split('_')[0];  break;
        case "3": this.monthList[i] = "March " + this.monthList[i].toString().split('_')[0];  break;
        case "4": this.monthList[i] = "April " + this.monthList[i].toString().split('_')[0];  break;
        case "5": this.monthList[i] = "May " + this.monthList[i].toString().split('_')[0];  break;
        case "6": this.monthList[i] = "June " + this.monthList[i].toString().split('_')[0];  break;
        case "7": this.monthList[i] = "July " + this.monthList[i].toString().split('_')[0];  break;
        case "8": this.monthList[i] = "August " + this.monthList[i].toString().split('_')[0];  break;
        case "9": this.monthList[i] = "September " + this.monthList[i].toString().split('_')[0];  break;
        case "10": this.monthList[i] = "October " + this.monthList[i].toString().split('_')[0];  break;
        case "11": this.monthList[i] = "November " + this.monthList[i].toString().split('_')[0];  break;
        case "12": this.monthList[i] = "December " + this.monthList[i].toString().split('_')[0];  break;
      }
    }
    debugger;
  }
}

export class ChartData {
  category: any;
  gpList: any;
  month: any;
}

export class ComparisonData {
  department: string;
  businessId: string;
  businessName: string;
  month: string;
  gp: number;
  type: string;
  dt: Date;
}

export class Business {
  businessId: string;
  businessName: string;
}

