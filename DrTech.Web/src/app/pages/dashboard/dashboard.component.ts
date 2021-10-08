import {
  Component,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  ChangeDetectorRef,
  ViewChild,
  OnInit,
} from '@angular/core';
import { NbTokenService } from '../../common/auth';
import { HighchartsService } from './service/highcharts.service';
import { DashboardService } from './service/dashboard-service';


@Component({
  selector: 'ngx-dashboard',
  styleUrls: ['./dashboard.component.scss'],
  templateUrl: 'dashboard.component.html',
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('chartsPie') public chartElPie: ElementRef;
  @ViewChild('chartsBar') public chartElBar: ElementRef;

  roleType: number = 0;
  type: any;


  markers: any[] = [];
  zoom: number = 5;

  lat: number = 31.5581261;
  lng: number = 74.3278866;

  chartsList;

  dashboardViewModel: any = {
    topUsers: []
  };


  constructor(private tokenService: NbTokenService, private hcs: HighchartsService, protected dashboardService: DashboardService, private changeDetectionRef: ChangeDetectorRef) {
    const role = tokenService.getRole();
    if (role === 1)
      this.roleType = 1;
    if (role === 2)
      this.roleType = 2;

  }
  async ngOnInit() {


  }
  public async ngAfterViewInit() {
    const response = await this.dashboardService.getDashboardDetail();

    if (response.statusCode === 0) {
      this.dashboardViewModel = response.data;
      this.addPieChart();
      this.addBarChart();
    }

  }

  addPieChart() {
    const options = {
      chart: {
        type: 'pie',
      },
      title: {
        text: 'Solar Employment Growth by Sector, 2010-2016',
      },
      //subtitle: {
      //  text: 'Source: thesolarfoundation.com',
      //},
      yAxis: {
        title: {
          text: 'Number of Employees',
        },
      },
      credits: {
        enabled: false,
      },
      legend: {
        enabled: true,
        reversed: true,
      },
      plotOptions: {
        pie: {
          showInLegend: true,
          allowPointSelect: true,
          cursor: 'pointer',

          point: {
            events: {
              click: (e:any)=> {
                //console.log(e.point.name);
                this.loadPoints(e.point.name);
              },
            },
          },
          dataLabels: {
            enabled: true,
            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
            //style: {
            //  color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
            //},
          },
        },
      },
      series: [
        {
          name: 'Brands',
          colorByPoint: true,
          data: this.dashboardViewModel.pieChart.data,
        }],
    };

    this.createCustomChart(this.chartElPie.nativeElement, options);
  }
  addBarChart() {
    const options = {
      chart: {
        type: 'column',
      },
      title: {
        text: 'Browser market shares. January, 2018',
      },
      subtitle: {
        text: 'Click the columns to view versions. Source: <a href="http://statcounter.com" target="_blank">statcounter.com</a>',
      },
      xAxis: {
        type: 'category',
      },
      yAxis: {
        title: {
          text: 'Total percent market share',
        },
      },
      legend: {
        enabled: true,
        reversed: true,
      },
      plotOptions: {
        series: {
          borderWidth: 0,
          dataLabels: {
            enabled: true,
            format: '{point.y:.1f}%',
          },
        },
      },

      tooltip: {
        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>',
      },
      credits: {
        enabled: false,
      },
      series: [{
        name: 'Browsers',
        colorByPoint: true,
        data: this.dashboardViewModel.barChart.data,
      }],
    };
    this.createCustomChart(this.chartElBar.nativeElement, options);
  }
  createChart() {
    this.hcs.createChart(this.chartElPie.nativeElement);
  }

  createCustomChart(element: any, myOpts: Object) {
    this.hcs.createChart(element, myOpts);
  }
  makeMarker(element: any, pin) {
    const maker = {
      lat: element.lat,
      lng: element.lng,
      label: element.label,
      draggable: false,
      icon: {
        url: '/assets/images/map/' + pin,
        scaledSize: {
          width: 44,
          height: 66,
        },
      },
      cash: element.cash,
      fileName: element.fileName,
      greenPoints: element.greenPoints,
      description: element.description,
      plantCount: element.plantCount,
    };
    return maker;
  }
  async loadPoints(marker: string) {

    if (marker != null)
      marker = marker.toLowerCase();


    const response = await this.dashboardService.getRDetail(marker);

    this.markers = [];

    if (response.statusCode === 0 && Array.isArray(response.data) && response.data.length > 0) {
      response.data.forEach(element => {
        this.markers.push(this.makeMarker(element, marker + '.png'));
      });



    }
  }
}

