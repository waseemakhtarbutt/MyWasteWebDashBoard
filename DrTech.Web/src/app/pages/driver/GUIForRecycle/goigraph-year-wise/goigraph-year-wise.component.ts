import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';
import { PiChart } from '../dto/PiChartsDTO';

// @Component({
//   selector: 'ngx-goigraph-year-wise',
//   templateUrl: './goigraph-year-wise.component.html',
//   styleUrls: ['./goigraph-year-wise.component.scss']
// })
// export class GOIGraphYearWiseComponent implements OnInit {

//   constructor() { }

//   ngOnInit() {
//   }

// }

//////////////////

@Component({
  selector: 'ngx-goigraph-year-wise',
  templateUrl: './goigraph-year-wise.component.html',
})

export class GOIGraphYearWiseComponent implements OnDestroy {
  @Input() results: any;
  view: any[] = [, 100];
  showXAxis = true;
  showYAxis = true;
  xAxisLabel = 'Year';
  yAxisLabel = 'Weight';
  showDataLabel = true;
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
        // domain: [colors.primaryLight, colors.infoLight, colors.successLight, colors.warningLight, colors.dangerLight],
      //  domain: ['#5AA454',   '#C7B42C','#AAAAAA','#A10A28']
        domain: [
          '#4C78A9','#8AD279','#E35656','#9FCBE9','#F2CF5A','#BAB0AC','#D8B5A5','#84BCB6',
          '#8381D5','#57B266','#868686','#F5B977','#A067D6','#D57C68','#FF9300','#D7D946',
          '#8C9EFF',
          '#80D8FF',
          '#A7FFEB',
          '#FF8A80',
          '#EA80FC',
          '#CCFF90',
          '#FFFF8D',
          '#FF9E80'
        ]
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
      // this.viewModel = response.data;
      // console.log(this.viewModel)
     }




   }
   async reloadGrid() {

    var response = await this.service.GetGOIGraphYearWise();


      //empty array which we are going to push our selected items, always define types




    if (response.statusCode == 0) {

      response.data.forEach(i=>{
        this.resultArray.push(
    {
     "name":i.cYear,
     "value":i.gCs
    });
 });
     this.resultsPie = this.resultArray;


    }


  }


}

