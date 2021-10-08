import { Component, OnInit } from '@angular/core';
import {SchoolService} from '../../service/school.service'
import { Router } from '@angular/router';
import { NbTokenService } from '../../../../common/auth';

import 'hammerjs';

@Component({
   selector: 'ngx-school-comparison',
   templateUrl: './school-comparison.component.html',
   styleUrls: ['./school-comparison.component.scss']
})
export class SchoolComparisonComponent implements OnInit {
  subAdminFromDate: Date;
  subAdminToDate: Date;
  schoolAdminFromDate: Date;
  schoolAdminToDate: Date;
  fromFormattedDate: string;
  toFormattedDate: string;
  clas: string;
  comparisonList: Array<ComparisonData> = new Array<ComparisonData>();
  chartData: Array<ChartData> = new Array<ChartData>();
  monthList: string[];
  sectionList: Array<string> = new Array<string>();
  schoolList: Array<School> = new Array<School>();

  constructor(public schoolService: SchoolService, private router: Router, private tokenService: NbTokenService) { }
  dropdownList = [];
  selectedSections = [];
  selectedSchools = [];
  dropdownSettings = {};
  dynamicdropdown = [];
  ddlSectionList:any;
  ddlSchoolList:any;
  ddlClassList:any;
  IsSearched:boolean;
  roleId: number;

  async ngOnInit() {
    this.roleId = this.tokenService.getRole();

    this.IsSearched = false;
    this.monthList = [];
    var d = new Date();
    this.subAdminFromDate = this.subAdminToDate = this.schoolAdminFromDate = this.schoolAdminToDate = new Date();

    if (this.roleId == 7) { // Sub School Admin
      var response = await this.schoolService.GetClassList();

      if(response.statusCode == 0)
        this.ddlClassList = response.data;
    }
    else if (this.roleId == 2) { // School Admin
      var response = await this.schoolService.GetBranchesList();

      this.dynamicdropdown = [];

      if(response.statusCode == 0)
        this.ddlSchoolList = response.data;

      let number = 0;

      this.ddlSchoolList.forEach(element => {
          this.dynamicdropdown.push({item_id:element.schoolId, item_text: element.schoolName})
      });
      
      this.dropdownList = this.dynamicdropdown;
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

  async onChange(deviceValue) {
    var response = await this.schoolService.GetSectionList(deviceValue);

    this.dynamicdropdown = [];

    if(response.statusCode == 0)
      this.ddlSectionList = response.data;
    let number = 0;
      this.ddlSectionList.forEach(element => {
        this.dynamicdropdown.push({item_id:element.section, item_text: element.section})
    });
    
    this.dropdownList = this.dynamicdropdown;
  }

  onItemSelect(item: any) {
    console.log(item);
  }

  onSelectAll(items: any) {
    console.log(items);
  }

  async GetComparisonData() {
    var result = await this.SectionWiseSum();

    if (result)
    {
      this.PrepareChartData(result);
      this.IsSearched = true;
    }
  }
  async SectionWiseSum() {        
    var response;

    if (this.roleId == 7) {
      this.fromFormattedDate = new Date((this.subAdminFromDate.getMonth()+1) + " " + this.subAdminFromDate.getDate() +", " + this.subAdminFromDate.getFullYear() + " " + this.subAdminFromDate.getHours() + ":" + this.subAdminFromDate.getMinutes() + ":" + this.subAdminFromDate.getSeconds()).toLocaleString('ur-PK');
      this.toFormattedDate = new Date((this.subAdminToDate.getMonth()+1) + " " + this.subAdminToDate.getDate() +", " + this.subAdminToDate.getFullYear() + " " + this.subAdminToDate.getHours() + ":" + this.subAdminToDate.getMinutes() + ":" + this.subAdminToDate.getSeconds()).toLocaleString('ur-PK');

      response = await this.schoolService.GetComparisonGreenPoints(this.clas, this.selectedSections, '', this.fromFormattedDate, this.toFormattedDate);
    }
    else if (this.roleId == 2) {
      this.fromFormattedDate = new Date((this.schoolAdminFromDate.getMonth()+1) + " " + this.schoolAdminFromDate.getDate() +", " + this.schoolAdminFromDate.getFullYear() + " " + this.schoolAdminFromDate.getHours() + ":" + this.schoolAdminFromDate.getMinutes() + ":" + this.schoolAdminFromDate.getSeconds()).toLocaleString('ur-PK');
      this.toFormattedDate = new Date((this.schoolAdminToDate.getMonth()+1) + " " + this.schoolAdminToDate.getDate() +", " + this.schoolAdminToDate.getFullYear() + " " + this.schoolAdminToDate.getHours() + ":" + this.schoolAdminToDate.getMinutes() + ":" + this.schoolAdminToDate.getSeconds()).toLocaleString('ur-PK');
      
      var schoolIds = [];

      this.selectedSchools.forEach(element => {
        schoolIds.push(element.item_id);
      });

      response = await this.schoolService.GetComparisonGreenPoints('', '', schoolIds, this.fromFormattedDate, this.toFormattedDate);
    }

    this.comparisonList = [];
    
    if (response.data == null)
      return false;

    response.data.forEach(element => {

      element.refuses.forEach(elrefuses => {
        let row = { section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elrefuses.key.year + '_' + elrefuses.key.month, 
          gp:elrefuses.gp,
          type: 'refuse',
          dt: new Date(elrefuses.key.year, elrefuses.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.reduces.forEach(elreduces => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elreduces.key.year + '_' + elreduces.key.month, 
          gp:elreduces.gp,
          type: 'reduce',
          dt: new Date(elreduces.key.year, elreduces.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.reuses.forEach(elreuses => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elreuses.key.year + '_' + elreuses.key.month, 
          gp:elreuses.gp,
          type: 'reuse',
          dt: new Date(elreuses.key.year, elreuses.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.replants.forEach(elreplants => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elreplants.key.year + '_' + elreplants.key.month, 
          gp:elreplants.gp,
          type: 'replant',
          dt: new Date(elreplants.key.year, elreplants.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.recycles.forEach(elrecycles => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elrecycles.key.year + '_' + elrecycles.key.month, 
          gp:elrecycles.gp,
          type: 'recycle',
          dt: new Date(elrecycles.key.year, elrecycles.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.regifts.forEach(elregifts => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elregifts.key.year + '_' + elregifts.key.month, 
          gp:elregifts.gp,
          type: 'regift',
          dt: new Date(elregifts.key.year, elregifts.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.reports.forEach(elreports => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elreports.key.year + '_' + elreports.key.month, 
          gp:elreports.gp,
          type: 'report',
          dt: new Date(elreports.key.year, elreports.key.month-1)
        }

        this.comparisonList.push(row);
      });

      element.buyBins.forEach(elbuybins => {
        let row = {section:element.section, schoolId:element.schoolId, schoolName:element.schoolName, 
          month:elbuybins.key.year + '_' + elbuybins.key.month, 
          gp:elbuybins.gp,
          type: 'buybin',
          dt: new Date(elbuybins.key.year, elbuybins.key.month-1)
        }

        this.comparisonList.push(row);
      });
    });
    
    var result = [];

    if (this.roleId == 7) // Sub School Admin
    {
      this.comparisonList.reduce(function(res, value) {
      if (!res[value.month+value.section]) {
        res[value.month+value.section] = { month: value.month, section: value.section, schoolId: value.schoolId, schoolName: value.schoolName, gp: 0, dt: value.dt };
        result.push(res[value.month+value.section])
      }
      res[value.month+value.section].gp += value.gp;
      return res;
      }, {});
    }
    else if (this.roleId == 2) // School Admin
    {
      this.comparisonList.reduce(function(res, value) {
        if (!res[value.month+value.schoolId]) {
          res[value.month+value.schoolId] = { month: value.month, schoolId: value.schoolId, schoolName: value.schoolName, gp: 0, dt: value.dt };
          result.push(res[value.month+value.schoolId])
        }
        res[value.month+value.schoolId].gp += value.gp;
        return res;
        }, {});
    }

    
    return result;
  }

  PrepareChartData(result) {
    result = result.slice().sort(function (a, b) {
      return a["dt"] < b["dt"] ? -1 : 1;
    });

    this.monthList = [];
    this.sectionList = [];
    this.schoolList = [];

    this.chartData = Array<ChartData>();

    result.forEach(element => {
      let found = this.monthList.find(e => e == element.month);
      if (found==undefined)
        this.monthList.push(element.month);

      if (this.roleId == 7) // Sub School Admin
      {
        let section = this.sectionList.find(e => e == element.section);
        if (section==undefined)
          this.sectionList.push(element.section);
      }
      else if (this.roleId == 2) // School Admin 
      {
        let school = this.schoolList.find(e => e.schoolId == element.schoolId);
        if (school==undefined)
        {
          let row = { schoolId:element.schoolId, schoolName:element.schoolName } 

          this.schoolList.push(row);
        }
      }
    });

    this.chartData = Array<ChartData>();

    if (this.roleId == 7) // Sub School Admin
    {
      this.sectionList.forEach(section => {
        var cData = new ChartData();
        var greenPoints = Array<number>();
  
        this.monthList.forEach(month => {
            var flag = false;
            result.forEach(element => {
              if (element.month == month && element.section == section)
              {
                greenPoints.push(element.gp);
                  flag = true;
              }
            });
  
            if (!flag)
            greenPoints.push(0);
          });
  
        cData.category = section;
        cData.gpList=greenPoints;
                  
        this.chartData.push(cData);
      });
    }
    else if (this.roleId == 2) // School Admin 
    {
      this.schoolList.forEach(school => {
        var cData = new ChartData();
        var greenPoints = Array<number>();
  
        this.monthList.forEach(month => {
            var flag = false;
            result.forEach(element => {
              if (element.month == month && element.schoolId == school.schoolId)
              {
                greenPoints.push(element.gp);
                  flag = true;
              }
            });
  
            if (!flag)
            greenPoints.push(0);
          });
  
        cData.category = school.schoolName;
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
    
  }
}

export class ChartData {
  category: any;
  gpList: any;
  month: any;
}

export class ComparisonData {
  section: string;
  schoolId: string;
  schoolName: string;
  month: string;
  gp: number;
  type: string;
  dt: Date;
}

export class School {
  schoolId: string;
  schoolName: string;
}




// import { Component, OnInit,NgModule , ViewChild, ElementRef} from '@angular/core';
// import { BrowserModule } from '@angular/platform-browser';
// import { NgxChartsModule } from '@swimlane/ngx-charts';
// import 'chartjs-plugin-annotation';
// import { multi } from './data';
// @Component({
//   selector: 'ngx-school-comparison',
//   templateUrl: './school-comparison.component.html',
//   styleUrls: ['./school-comparison.component.scss']
// })
// export class SchoolComparisonComponent implements OnInit {
//   @ViewChild('myCanvas')
//   public canvas: ElementRef;
//   public context: CanvasRenderingContext2D;
//   public chartType: string = 'line';
//   public chartData: any[];
//   public chartLabels: any[];
//   public chartColors: any[];
//   public chartOptions: any;
  
//   constructor() {
   
//   }
//   ngOnInit() {
//     this.chartData = [{
//       data: [200],
//       label: 'School',
    
//     },
//     {
//       data: [500],
//       label: 'SchoolA',
      
//     }];
//     this.chartLabels = ['Jan', 'Feb', 'Mar', 'Apr', 'May','June','July','Aug','Sept','Oct','Nov','Dec'];
//     this.chartOptions = {
//       scales: {
//         yAxes: [{
//           ticks: {
//             beginAtZero: true,
//             stepSize: 50
//           }
//         }]
//       },
//       annotation: {
//          drawTime: 'beforeDatasetsDraw',
//          annotations: [{
//             type: 'box',
//             id: 'a-box-1',
//             yScaleID: 'y-axis-0',
//             yMin: 0,
//             yMax: 0,
//          }]
//       }
//     }
//   };
  
// }
