import { Component, OnInit, OnDestroy } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';

@Component({
  selector: 'ngx-recyle-detail-chart',
  templateUrl: './recyle-detail-chart.component.html',
  styleUrls: ['./recyle-detail-chart.component.scss']
})
export class RecyleDetailChartComponent implements OnDestroy {

  ScreenSize: any;
  apiresponse: [];
  view: any[] = [, 300];
  legend = true
  //showLegend = true;
  gradient = true;
  showXAxis = true;
  showYAxis = true;
  xAxisLabel = 'Year';
  yAxisLabel = 'Weight';
  showDataLabel = true;
  legendTitle = "legend"
  animations = true;
  roundDomains = true;
  colorScheme: any;
  themeSubscription: any;
  public resultArray: Array<any> = []
  resultsBar: any
  constructor(private theme: NbThemeService, public service: DriverService) {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {
      const colors: any = config.variables;
      this.colorScheme = {
        domain: [ '#4C78A9','#8AD279','#E35656','#9FCBE9','#F2CF5A','#BAB0AC','#D8B5A5','#84BCB6',
         '#8381D5','#57B266','#868686','#F5B977','#A067D6','#D57C68','#FF9300','#D7D946',  colors.primaryLight, colors.infoLight, colors.successLight, colors.warningLight, colors.dangerLight,
          '#808080','#808000','#00FF00','#008000','#00FFFF','#008080','#0000FF','#000080','#FF00FF','#800080'
        ],
      };
    });
  }
  ngOnInit() {
    this.ScreenSize = window.innerWidth;
    // if (this.ScreenSize > 1280) {
    //   this.view = [670, 300]
    // }
    // else {
    //   this.view = [470, 300]
    // }
    this.reloadGrid();
   // this.LoadChartData();
  }

  ngOnDestroy(): void {
    this.themeSubscription.unsubscribe();
  }

     async LoadChartData(){
      var aResponse = await this.service.GetDataForRecycleDetailChartByAdmin();
      console.log(aResponse);
      if(aResponse.statusCode == 0){
        console.log("Detail Graph Data")
        console.log(aResponse.data);
       // this.apiresponse = aResponse.data;


        //this.resultsBar = this.apiresponse;
      }
     }

  async reloadGrid() {
    this.resultArray = []

    var response = await this.service.GetDataForRecycleDetailChartByAdmin();

    //empty array which we are going to push our selected items, always define types

    if (response.statusCode == 0) {

     console.log(response.data)
      // response.data.forEach(i => {
      //   this.resultArray.push(
      //     {
      //       "name": i.cYear,
      //       "value": i.rWeight
      //     });
      // });
      this.resultsBar = response.data;

    //  this.resultsBar = [
    //   {
    //     "name": "Aug",
    //     "series": [
    //       {
    //         "name": "Bottles",
    //         "value": 120,
    //         "extra": {
    //           "code": "de"
    //         }
    //       },
    //       {
    //         "name": "2000",
    //         "value": 36953,
    //         "extra": {
    //           "code": "de"
    //         }
    //       },
    //       {
    //         "name": "1990",
    //         "value": 31476,
    //         "extra": {
    //           "code": "de"
    //         }
    //       }
    //     ]
    //   },
    //   {
    //     "name": "Feb",
    //     "series": [
    //       {
    //         "name": "2010",
    //         "value": 0,
    //         "extra": {
    //           "code": "us"
    //         }
    //       },
    //       {
    //         "name": "2000",
    //         "value": 45986,
    //         "extra": {
    //           "code": "us"
    //         }
    //       },
    //       {
    //         "name": "1990",
    //         "value": 37060,
    //         "extra": {
    //           "code": "us"
    //         }
    //       }
    //     ]
    //   },
    //   {
    //     "name": "March",
    //     "series": [
    //       {
    //         "name": "2010",
    //         "value": 36745,
    //         "extra": {
    //           "code": "fr"
    //         }
    //       },
    //       {
    //         "name": "2000",
    //         "value": 34774,
    //         "extra": {
    //           "code": "fr"
    //         }
    //       },
    //       {
    //         "name": "1990",
    //         "value": 29476,
    //         "extra": {
    //           "code": "fr"
    //         }
    //       }
    //     ]
    //   }

    // ]

     }
  }
}

