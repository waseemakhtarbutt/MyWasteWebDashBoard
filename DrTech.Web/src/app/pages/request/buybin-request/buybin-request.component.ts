import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { RequestService } from '../service/request-service';
import { CommonService } from '../../../common/service/common-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { LocationLinkComponent, UserDetailLinkComponent } from '../../../common/custom-control';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';

@Component({
  selector: 'ngx-buybin-request',
  templateUrl: './buybin-request.component.html',
  styleUrls: ['./buybin-request.component.scss'],
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
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
export class BuyBinRequestComponent implements OnInit {

  showMessage(va) {
    alert(va);
  }

  source: LocalDataSource = new LocalDataSource();
  userId: any;
  statusId: any;
  badge: any;
  points: number = 0;
  listViewModel: any[] = [];
  loading = false;
  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  public gridView: GridDataResult;
  public pageSize = 9;
  public skip = 0;
  // public sort: SortDescriptor[] = [{
  //   field: 'binName',
  //   dir: 'asc'
  // }];
  public multiple = false;
  public allowUnsort = true;

  constructor(public requestService: RequestService, public commonService: CommonService, private dialogService: NbDialogService, private route: ActivatedRoute, private router: Router) {
    this.userId = route.snapshot.paramMap.get("id");
    this.statusId = 1;
  }

  ngOnInit() {
    this.LoadData();
    this.commonService.GetStatusList().subscribe(result => {
      this.statusList = result.data;

    });
  }

  LoadData() {
    this.loading = true;

    this.requestService.GetBuyBinList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;
        this.loadItems();
      }
    });

    this.loading = false
  }

  public onSelect(e) {
    this.router.navigate(["/pages/driver/assign-driver/bin-assign/" + this.gridView.data[e.index % this.pageSize].id]);
  }
  public sortChange(sort: SortDescriptor[]): void {
    //this.sort = sort;
    this.loadItems();
  }
  private loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
      total: this.listViewModel.length
    };
    if (this.listViewModel) {
      this.badge = this.listViewModel.length;
    }
    else
      this.badge = '';
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  searchGrid(search) {

    const predicate = compileFilter(
      {
        logic: "or",
        filters: [
          { field: "idea", operator: "contains", value: search },
          { field: "userName", operator: "contains", value: search },
          { field: "statusDescription", operator: "contains", value: search }

        ]
      });

    if (search) {
      this.gridView = {
        data: this.listViewModel.filter(predicate),
        total: this.listViewModel.length
      };
    }
    else {
      this.gridView = {
        data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
      };
    }
  }

  public filterChanged(event: Event) {
    let selectElementText = event.target['value'];
    this.statusId = selectElementText;
    var datalist = this.requestService.GetBuyBinList(this.statusId).subscribe(result => {
      if (result.statusCode == 0) {
        //this.listViewModel=[];  
        this.skip = 0;
        debugger
        this.listViewModel = result.data;
        this.loadItems();
      }
    });

  }
}
