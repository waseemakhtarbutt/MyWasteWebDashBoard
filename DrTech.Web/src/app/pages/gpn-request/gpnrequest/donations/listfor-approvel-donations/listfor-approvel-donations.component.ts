import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GpnRequestService } from '../../../service/gpn-request.service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
@Component({
  selector: 'ngx-listfor-approvel-donations',
  templateUrl: './listfor-approvel-donations.component.html',
  styleUrls: ['./listfor-approvel-donations.component.scss']
})
export class ListforApprovelDonationsComponent implements OnInit {
  @Output() messageEvent = new EventEmitter<string>();
  listViewModel: any[] = [];
  organizationBadge : any ;
  loading=false;
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public sort: SortDescriptor[] = [{
    field: 'typeName',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public gpnRequestService: GpnRequestService,private dialogService: NbDialogService) { }
  async ngOnInit() {
    this.loading=true;
    var response = await this.gpnRequestService.GetNGONeedListInActive();
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
      this.loadItems();
      this.badgeCount();
  }
  this.loading=false;
}

badgeCount() {
  if(!!this.listViewModel){
    this.organizationBadge = (this.listViewModel.length == 0 ? '' : this.listViewModel.length);
 }
 else
 this.organizationBadge = '';
  this.messageEvent.emit(this.organizationBadge)
}

public pageChange(event: PageChangeEvent): void {
  this.skip = event.skip;
  this.loadItems();
}
public sortChange(sort: SortDescriptor[]): void {
  this.sort = sort;
  this.loadItems();
}

private loadItems(): void {
  this.gridView = {
      data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
      total: this.listViewModel.length
  };
}

searchGrid(search) {

  const predicate = compileFilter(
    { logic: "or",
     filters: [
       { field: "description", operator: "contains", value: search },
      ]});

      if(search)
      {
        this.gridView = {
          data: this.listViewModel.filter(predicate),
           total: this.listViewModel.length
       };
      }
      else
      {
        this.gridView = {
          data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
           total: this.listViewModel.length
       };
      }
}
}
