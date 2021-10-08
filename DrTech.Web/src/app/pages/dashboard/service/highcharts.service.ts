import { Injectable } from '@angular/core';
import * as Highcharts from 'highcharts';

@Injectable()
export class HighchartsService {

  charts = [];
  //defaultOptions = {
  //  chart: {
  //    type: 'pie',
  //  },
  //  title: {
  //    text: 'Solar Employment Growth by Sector, 2010-2016',
  //  },
  //  //subtitle: {
  //  //  text: 'Source: thesolarfoundation.com',
  //  //},
  //  yAxis: {
  //    title: {
  //      text: 'Number of Employees',
  //    },
  //  },
  //  credits: {
  //    enabled: false,
  //  },
  //  legend: {
  //    enabled: true,
  //    reversed: true,
  //  },
  //  plotOptions: {
  //    pie: {
  //      showInLegend: true,
  //      allowPointSelect: true,
  //      cursor: 'pointer',
        
  //      point: {
  //        events: {
  //          click: function () {
  //            console.log('Category: ' + this.name + ', value: ' + this.y);
  //          },
  //        },
  //      },
  //      dataLabels: {
  //        enabled: true,
  //        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
  //        style: {
  //          color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black',
  //        },
  //      },
  //    },
  //  },
  //  series: [
  //    {
  //    name: 'Brands',
  //    colorByPoint: true,
  //    data: [{
  //      name: 'Chrome',
  //      y: 61.41,
  //    }, {
  //      name: 'Internet Explorer',
  //      y: 11.84,
  //    }, {
  //      name: 'Firefox',
  //      y: 10.85,
  //    }, {
  //      name: 'Edge',
  //      y: 4.67,
  //    }, {
  //      name: 'Safari',
  //      y: 4.18,
  //    }, {
  //      name: 'Sogou Explorer',
  //      y: 1.64,
  //    }, {
  //      name: 'Opera',
  //      y: 1.6,
  //    }],
  //  }],
  //};

  constructor() {
  }

  createChart(container, options?: any) {
   // const opts = !!options ? options : this.defaultOptions;
    const e = document.createElement('div');
    //e.className = 'col-md-6';
    //e.style.width = '100%';
    container.appendChild(e);

    if (!!options.chart) {
      options.chart['renderTo'] = e;
    }
    else {
      options.chart = {
        'renderTo': e,
      };
    }

    this.charts.push(new Highcharts.Chart(options));
  }

  removeFirst() {
    this.charts.shift();
  }

  removeLast() {
    this.charts.pop();
  }

  getChartInstances(): number {
    return this.charts.length;
  }

  getCharts() {
    return this.charts;
  }
}
