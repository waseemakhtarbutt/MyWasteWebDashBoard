
import { Component, OnDestroy, Input } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';
@Component({
  selector: 'ngx-recycle-bar-chart',
  templateUrl: './recycle-bar-chart.component.html',
})
export class RecycleBarChartComponent implements OnDestroy {
  @Input() results: any;
  ScreenSize : any;
  view: any[] = [, 200];
  //showLegend = true;
  showXAxis = true;
  showYAxis = true;
  xAxisLabel = 'Year';
  yAxisLabel = 'Weight';
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

        domain: [
          '#4C78A9','#8AD279','#E35656','#9FCBE9','#F2CF5A','#BAB0AC','#D8B5A5','#84BCB6',
          '#A067D6','#D57C68','#FF9300','#D7D946','#8381D5','#57B266','#868686','#F5B977',
          '#FF8A80',
          '#EA80FC',
          '#8C9EFF',
          '#80D8FF',
          '#A7FFEB',
          '#CCFF90',
          '#FFFF8D',
          '#FF9E80'
        ]
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

    var response = await this.service.GetGOIGraph();

      //empty array which we are going to push our selected items, always define types

    if (response.statusCode == 0) {


      response.data.forEach(i=>{
        this.resultArray.push(
    {
     "name":i.cYear,
     "value":i.rWeight
    });
 });

     this.resultsBar = this.resultArray;


    }


  }



}
