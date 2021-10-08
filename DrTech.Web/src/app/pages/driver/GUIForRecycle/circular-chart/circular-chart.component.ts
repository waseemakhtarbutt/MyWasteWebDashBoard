import { Component, OnDestroy, Input } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';

@Component({
  selector: 'ngx-circular-chart',
  templateUrl: './circular-chart.component.html',
  styleUrls: ['./circular-chart.component.scss']
})

////////////////////////////////////////////////////////////////////////////

export class CircularChartComponent implements OnDestroy {
  @Input() results: any;
  ScreenSize : any;
  view: any[] = [, 200];
  //showLegend = true;
  showXAxis = true;
  showYAxis = true;
  xAxisLabel = 'Year';
  yAxisLabel = 'Weight';
  labels = true;
  showDataLabel = true;
  legend : true;
  colorScheme: any;
  themeSubscription: any;
  public resultArray:Array<any>=[]
  resultsBar : any
  constructor(private theme: NbThemeService,public service : DriverService) {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {
      const colors: any = config.variables;
      this.colorScheme = {
        // domain: [colors.primaryLight, colors.infoLight, colors.successLight, colors.warningLight, colors.dangerLight],
      //  domain: [colors.primaryLight, colors.infoLight, colors.successLight, colors.warningLight, colors.dangerLight],
      domain: [ '#4C78A9','#8AD279','#E35656','#9FCBE9','#F2CF5A','#BAB0AC','#D8B5A5','#84BCB6',
       '#8381D5','#57B167','#868686','#F4B977','#A068D7','#D47C68','#FF9400','#D7D946',  colors.primaryLight, colors.infoLight, colors.successLight, colors.warningLight, colors.dangerLight,
      '#808080','#808000','#00FF00','#008000','#00FFFF','#008080','#0000FF','#000080','#FF00FF','#800080'
    ],
        // domain: [
        //   '#FF8A80',
        //   '#EA80FC',
        //   '#8C9EFF',
        //   '#80D8FF',
        //   '#A7FFEB',
        //   '#CCFF90',
        //   '#FFFF8D',
        //   '#FF9E80'
        // ]
      };
    });
  }
  ngOnInit(){
    this.ScreenSize = window.innerWidth;
    // if(this.ScreenSize > 1280){
    //   this.view =[670, 300]
    // }
    // else{
    //   this.view =[470, 300]
    // }
 this.reloadGrid();
  }

  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }
  async reloadGrid() {
    this.resultArray = []

    var response = await this.service.GetCircularGraph();

      //empty array which we are going to push our selected items, always define types

    if (response.statusCode == 0) {


      response.data.forEach(i=>{
        this.resultArray.push(
    {
     "name":i.name,
     "value":i.value
    });
 });

     this.resultsBar = this.resultArray;


    }


  }



}

