import { Component, OnInit,OnDestroy,Input } from '@angular/core';
import { MywasteserviceService } from '../../../my-waste/mywasteservice.service';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';
import { empty } from 'rxjs';

@Component({
  selector: 'ngx-recycle-pi-chart',
  templateUrl: './recycle-pi-chart.component.html',
  styleUrls: ['./recycle-pi-chart.component.scss']
})
export class RecyclePiChartComponent implements OnDestroy {
  companyList : any;
  _viewModel: any = {};
  showXAxis = true;
  showYAxis = true;
  xAxisLabel = 'Designation';
  yAxisLabel = 'Count';
  showDataLabel = true;
  colorScheme: any;
  themeSubscription: any;
  public resultArray:Array<any>=[]
  resultsBarHorizontal : any
  companyList1 : any;
  companyList2 : any;
  default :any;
  defaultThird :any;
  defaultSecond : any;
  public innerWidth: any;
  ScreenSize:any;
 // view: any[] = [520, 300];
  view: any[] = [, 360];
  constructor(public service: MywasteserviceService,private theme: NbThemeService,public driverservice : DriverService) {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {
      const colors: any = config.variables;
      this.colorScheme = {
        domain: [ '#4C78A9','#8AD279','#E35656','#9FCBE9','#F2CF5A','#BAB0AC','#D8B5A5','#84BCB6', '#8381D5','#57B266','#868686','#F5B977','#A067D6','#D57C68','#FF9300','#D7D946', colors.successLight,colors.dangerLight,colors.primaryLight, colors.infoLight, colors.warningLight],
      };
    });
   }

  async ngOnInit() {
    this.innerWidth = window.innerWidth;
    this.ScreenSize = window.innerWidth;
    // if(this.ScreenSize > 1280){
    //   this.view =[670, 300]
    // }
    // else{
    //   this.view =[470, 300]
    // }

    var response = await this.service.GetBusinessList();
    if(response.statusCode == 0){
      // this.default = response.data[0];
      // this._viewModel.companyID1 = Object.assign(this.default.id);

      this.companyList = response.data;

      // this.defaultSecond = response.data[1];
      // this._viewModel.companyID2 = Object.assign(this.defaultSecond.id);
      this.companyList1 = response.data;
      // this.defaultThird = response.data[2];
      // this._viewModel.companyID3 = Object.assign(this.defaultThird.id);
      this.companyList2 = response.data;
     }

     this.onSubmit()

  }
  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }

//   async onSubmit() {
//     var response = await this.driverservice.DrawChartByGPNComparison(this._viewModel);
//    // console.log(".................................")
//     //console.log(response.data)

//     if (response.statusCode == 0) {
//      // console.log(response.data)

//       response.data.forEach(i=>{
//         this.resultArray.push(
//     {
//      "name":i.name,
//      "value":i.gPs
//     });
//  });
//      this.resultsBarHorizontal = this.resultArray;


//     }


//   }

  async onSubmit() {
    this.resultArray = [];

    //this.resultsBarHorizontal = [];
    this.resultArray =[];
    var response = await this.driverservice.DrawChartByGPNComparison(this._viewModel);

      //empty array which we are going to push our selected items, always define types

    if (response.statusCode == 0) {


      response.data.forEach(i=>{
        this.resultArray.push(
    {
     "name":i.name,
     "value":i.gPs
    });
 });
     this.resultsBarHorizontal = this.resultArray;


    }


  }


}
