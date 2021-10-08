import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';
import { PiChart } from '../dto/PiChartsDTO';

@Component({
  selector: 'ngx-daily-green-credits-graph',
  templateUrl: './daily-green-credits-graph.component.html',
  styleUrls: ['./daily-green-credits-graph.component.scss']
})
// export class DailyGreenCreditsGraphComponent implements OnInit {

//   constructor() { }

//   ngOnInit() {
//   }

// }

export class DailyGreenCreditsGraphComponent implements OnDestroy {
  @Input() results: any;
  view: any[] = [, 100];
  showDataLabel = true;
  innerRadius: "100%"
  showXAxis = true;
  showYAxis = true;
  xAxisLabel = 'Year';
  yAxisLabel = 'Weight';
  showLabels = true;
  colorScheme: any;
  themeSubscription: any;
  //legend: true;
  //legendTitle: "Green Points";
  resultsSet : any
  viewModel : PiChart;
  public resultArray:Array<any>=[]
  constructor(private theme: NbThemeService,public service : DriverService) {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {
      const colors: any = config.variables;
      this.colorScheme = {
        // domain: [colors.infoLight, colors.primaryLight, colors.successLight, colors.dangerLight, colors.warningLight],
        domain: [
          '#4C78A9','#8AD279','#E35656','#9FCBE9','#F2CF5A','#BAB0AC','#D8B5A5','#84BCB6',
          '#8381D5','#57B266','#868686','#F5B977','#A067D6','#D57C68','#FF9300','#D7D946',
          '#A7FFEB',
          '#CCFF90',
          '#FF8A80',
          '#EA80FC',
          '#8C9EFF',
          '#80D8FF',
          '#808080','#808000','#00FF00','#008000','#00FFFF','#008080','#0000FF','#000080','#FF00FF','#800080'
        ]
      };
    });

  }
  async ngOnInit() {
    this.loadGraph()

  }
  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }
   async Results() :Promise<void>{
     var response = await this.service.GetGOIGraphMonthWise();
    //console.log(response.data)
     if(response.statusCode == 0){
      // this.viewModel = response.data;
      // console.log(this.viewModel)
     }
   }
   async loadGraph() {

    var response = await this.service.GetDailyGreenPointsGraph();
      //empty array which we are going to push our selected items, always define types
    if (response.statusCode == 0) {
      console.log(response.data)
      response.data.forEach(i=>{
        this.resultArray.push(
    {
     "name":i.cDayName,
     "value":i.gCs
    });
 });
     this.resultsSet = this.resultArray;
     console.log(this.resultsSet)
    }
  }


}
