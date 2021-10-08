import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';
import { PiChart } from '../dto/PiChartsDTO';

@Component({
  selector: 'ngx-bar-chart',
  template: `
    <ngx-charts-bar-horizontal
      [scheme]="colorScheme"
      [results]="resultsPie"

      >
    </ngx-charts-bar-horizontal>
  `,
})

export class BarChartComponent implements OnDestroy {
  @Input() results: any;
  showLabels = true;
  colorScheme: any;
  themeSubscription: any;
  resultsPie : any
  viewModel : PiChart;
  public resultArray:Array<any>=[]
  constructor(private theme: NbThemeService,public service : DriverService) {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {
      const colors: any = config.variables;
      this.colorScheme = {
        domain: [colors.primaryLight, colors.infoLight, colors.successLight, colors.warningLight, colors.dangerLight],
      };
    });

  }
  async ngOnInit() {
    this.reloadGrid()

  }
  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }
   async Results() :Promise<void>{
     var response = await this.service.GetPerformerPIChartData();
    //console.log(response.data)
     if(response.statusCode == 0){
       console.log("request complete")
        console.log(response.data[0])
      // this.viewModel = response.data;
      // console.log(this.viewModel)
     }




   }
   async reloadGrid() {

    var response = await this.service.GetPerformerBarhartData();


      //empty array which we are going to push our selected items, always define types




    if (response.statusCode == 0) {
      console.log(response.data)

      response.data.forEach(i=>{
        this.resultArray.push(
    {
     "name":i.fullName,
     "value":i.gPs
    });
 });
     this.resultsPie = this.resultArray;


    }


  }


}




