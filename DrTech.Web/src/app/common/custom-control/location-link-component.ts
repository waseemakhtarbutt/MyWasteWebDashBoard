import { Component, Input, OnInit } from '@angular/core';
import { ViewCell } from 'ng2-smart-table';

@Component({
  template: `
    <span><a href="https://www.google.com/maps/place/{{rowData.latitude}},{{rowData.longitude}}/@{{rowData.latitude}},{{rowData.longitude}},18z" target="_blank" style="text-decoration:none;"><i class="nb-location"></i></a> </span>
  `,
  styles: [`
    i.nb-location
    { 
      color: #a4abb3 !important;
      font-size:30px;
    }
    i.nb-location:hover
    { 
      background-color: #91C747 !important;
      border-radius: 50px;
      padding:3px 3px 3px 3px;
      color: #ffffff !important;
    }
  `],
})
export class LocationLinkComponent implements ViewCell, OnInit {
  @Input() type: string | number;  
  @Input() value: string | number;   // This hold the cell value
  @Input() rowData: any;  // This holds the entire row object

  ngOnInit() {
    //console.log(this.rowData);
  }
}
