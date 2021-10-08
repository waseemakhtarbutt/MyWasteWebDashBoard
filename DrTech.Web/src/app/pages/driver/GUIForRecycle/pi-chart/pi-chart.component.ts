import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { DriverService } from '../../service/driver.service';
import { PiChart } from '../dto/PiChartsDTO';

// @Component({
//   selector: 'ngx-pi-chart',
//   templateUrl: './pi-chart.component.html',
//   styleUrls: ['./pi-chart.component.scss']
// })
// export class PiChartComponent implements OnInit {

//   constructor() { }

//   ngOnInit() {
//   }

// }

@Component({
  selector: 'ngx-pi-chart',
  template: `
    <ngx-charts-pie-chart
      [scheme]="colorScheme"
      [results]="resultsPie"

      [labels]="showLabels">
    </ngx-charts-pie-chart>
  `,
})

export class PiChartComponent implements OnDestroy {
  @Input() results: any;
  showLabels = true;
  colorScheme: any;
  themeSubscription: any;
  resultsPie : any
  viewModel : PiChart;
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

     }




   }
   async reloadGrid() {

    var response = await this.service.GetPerformerPIChartData();
    console.log(response.data);
    if (response.statusCode == 0) {
      console.log(response.data)
      this.viewModel = response.data;

      this.resultsPie = [
        { name: 'Refuse', value: this.viewModel.refuseCount  },
        { name: 'Reduce', value: this.viewModel.reduceCount  },
        { name: 'Reuse', value: this.viewModel.reuseCount },
        { name: 'Replant', value:this.viewModel.replantCount  },
        { name: 'Recycle', value: this.viewModel.recycleCount  },
        { name: 'Regift', value: this.viewModel.regiftCount },
        { name: 'Report', value: this.viewModel.reportCount },
      ];

    }


  }


}

