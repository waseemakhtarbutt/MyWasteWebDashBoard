import { Component, OnInit, Input } from '@angular/core';
import { MywasteserviceService } from '../../../my-waste/mywasteservice.service';
import { COSMIC_THEME } from '../../../../@theme/styles/theme.cosmic';

@Component({
  selector: 'ngx-segregated-grid',
  templateUrl: './segregated-grid.component.html',
  styleUrls: ['./segregated-grid.component.scss']
})
export class SegregatedGridComponent implements OnInit {
  @Input() Id: number;
  listViewModel: any[] = [];
  public RecycleID :number;
  @Input() typeswithWeight: any[];
  constructor( private service: MywasteserviceService) { }

  ngOnInit() {
    // this.listViewModel = this.dataa;
    // console.log(this.dataa);
  }

  // // @Input('dataa')
  // //   set dataa(data: any) {
  // //      this.listViewModel = data;
  // //      console.log('MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM')
  // //      console.log(this.listViewModel)
  // //   }




  public handlePassIDEvent(data: number) {
    this.RecycleID = data;
    console.log("This is from Segregated data")
    console.log(this.listViewModel)
    this.LoadSegregatedData(this.RecycleID)
  }

  async LoadSegregatedData(Id:number) {
    var response = await this.service.GetSegregatedDataByID(Id);
    if (response.statusCode == 0) {
     this.listViewModel = response.data;
    }
  }
}
