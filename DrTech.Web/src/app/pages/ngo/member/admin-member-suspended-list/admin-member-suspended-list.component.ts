import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgoService } from '../../service/ngo-service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, CompositeFilterDescriptor, filterBy, SortDescriptor, orderBy } from '@progress/kendo-data-query';

@Component({
  selector: 'ngx-admin-member-suspended-list',
  templateUrl: './admin-member-suspended-list.component.html',
  styleUrls: ['./admin-member-suspended-list.component.scss']
})
export class AdminMemberSuspendedListComponent implements OnInit {
  listViewModel: any[] = [];
  loading=false;
  public memberId = 0;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public filter: CompositeFilterDescriptor;
  public memberName:string=""
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public ngoService: NgoService, protected route: ActivatedRoute, protected router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading = true;

    this.ngoService.GetMemberListByRole(true).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;

        this.loadItems();
      }
    });

    this.loading = false
  }

  private loadItems(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
        total: this.listViewModel.length
    };
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
}
  async SuspendMember(dialog: NbDialogRef<any>) {
    var response = await this.ngoService.ActivateMember(this.memberId);

    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();
    }
  }

  onComplaintAction(dialog: TemplateRef<any>, event: any, id: any) {
    this.memberName = this.listViewModel.find(x=>x.id==id).name;
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

      this.memberId = id;
  }

  searchGrid(search) {
    const predicate = compileFilter(
      { logic: "or",
       filters: [
        { field: "name", operator: "contains", value: search },
        { field: "orgname", operator: "contains", value: search },
        { field: "department", operator: "contains", value: search},
        { field: "role", operator: "contains", value: search},
        { field: "location", operator: "contains", value: search},
        { field: "contactno", operator: "contains", value: search},
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
  public filterChange(filter: CompositeFilterDescriptor): void {
    this.filter = filter;
    this.gridView = {
        data: filterBy(this.listViewModel, filter),
        total: this.listViewModel.length
    };
  }
}
