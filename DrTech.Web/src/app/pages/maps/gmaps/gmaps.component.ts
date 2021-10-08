import { Component, OnInit } from '@angular/core';
import { MapService } from '../service/map-service';
import { isArray } from 'util';
import { MapMarkerDTO } from './dto/map-marker-dto';

@Component({
  selector: 'ngx-gmaps',
  styleUrls: ['./gmaps.component.scss'],
  templateUrl: 'gmaps.component.html',
})
export class GmapsComponent implements OnInit {
  markers: marker[] = [];
  // google maps zoom level
  //zoom: number = 13.5;
  zoom: number = 10;
  markerType = ['Refuse', 'Reduce', 'Reuse', 'Regift', 'Report', 'Recycle', 'Replant', 'Bin'];
  // initial center position for the map
  lat: number = 31.5581261;
  lng: number = 74.3278866;
  checkedMarkerList = [];

  constructor(protected mapService: MapService) { }
  ngOnInit() { }

  makeMarker(element: MapMarkerDTO) {
    const maker: marker = {
      lat: element.latitude,
      lng: element.longitude,
      label: element.label,
      draggable: false,
      icon: {
       // url: '/assets/images/map/' + element.pinImage,
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
      name: element.name,
    };
    return maker;
  }

  loadPoints(marker: string) {

    if (marker != null)
      marker = marker.toLowerCase();

      this.mapService.GetMapPoints(marker).subscribe(
        response => {
          if (response.statusCode == 0) {
            response.data.forEach(element => {
              this.markers.push(this.makeMarker(element));
            });
          }
        });

    // if (marker == 'refuse') {
    //   this.mapService.GetRefusePoints().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }
    // if (marker == 'reduce') {
    //   this.mapService.GetReducePoints().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }

    // if (marker == 'reuse') {
    //   this.mapService.GetReusePoints().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }
    // if (marker == 'regift') {
    //   this.mapService.GetRegiftPoints().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }
    // if (marker == 'report') {
    //   this.mapService.GetUserReportsPoint().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }
    // if (marker == 'recycle') {
    //   this.mapService.GetRecyclePoints().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }
    // if (marker == 'replant') {
    //   this.mapService.GetReplantPoint().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }
    // if (marker == 'bin') {
    //   this.mapService.GetBinPoint().subscribe(
    //     response => {
    //       if (response.statusCode == 0) {
    //         response.data.forEach(element => {
    //           this.markers.push(this.makeMarker(element));
    //         });
    //       }
    //     });
    // }

  }


  chkMarkerTypeOnChange(event, marker) {
    if (event != null && event.returnValue == true) {
      const ms = this.markers.filter(p => {
        if (p.label.toLocaleLowerCase() == marker.toLocaleLowerCase())
          return true;
      });
      if (!isArray(ms) || ms.length <= 0) {
        this.loadPoints(marker);
      }
    }
    else {
      const ff = this;
      this.markers.map(function (provider, index) {
        return provider.label.indexOf(marker.toLocaleLowerCase()) >= 0 ? index : -1;
      }).filter(function (index) {
        return index >= 0;
      }).reverse().forEach(function (index) {
        ff.markers.splice(index, 1);
      });
    }
  }
}

// just an interface for type safety.
interface marker {
  lat: number;
  lng: number;
  label?: string;
  draggable: boolean;
  icon?: any;
  cash?: number;
  greenPoints?: number;
  fileName?: string;
  plantCount?: number;
  description?: string;
  name?: string;
}

